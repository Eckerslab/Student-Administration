using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationServices.Mapping;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministrationServices.Services;

/// <summary>
///     Grade service implementation.
/// </summary>
/// <remarks>
///     This service is the single entry point for grade writes. <see cref="SaveGradeAsync" /> and
///     <see cref="DeleteGradeAsync" /> keep a student's cached <see cref="Student.Credits" /> consistent by
///     recomputing it authoritatively from the student's passing grades after every change, so credits can
///     never drift and are always correctly reverted on deletion. The credit and passing rules themselves
///     live in <see cref="IGradeCalculationService" />.
/// </remarks>
public class GradeService : IGradeService
{
    private readonly IGradeCalculationService gradeCalculationService;
    private readonly IGradeRepository gradeRepository;
    private readonly IStudentRepository studentRepository;

    /// <summary>
    ///     GradeService constructor.
    /// </summary>
    public GradeService(
        IGradeRepository gradeRepository,
        IStudentRepository studentRepository,
        IGradeCalculationService gradeCalculationService)
    {
        this.gradeRepository = gradeRepository;
        this.studentRepository = studentRepository;
        this.gradeCalculationService = gradeCalculationService;
    }

    /// <summary>
    ///     Adds or updates a student's grade for a course and recomputes the student's credits.
    /// </summary>
    /// <remarks>
    ///     Enforces the "one grade per student per course" rule: if the student already has a grade for the
    ///     course it is updated, otherwise a new grade is created. In both cases the student's earned credits
    ///     are recalculated from all of their passing grades and persisted, so the credit total stays correct
    ///     regardless of whether the grade newly passes, newly fails, or is unchanged.
    /// </remarks>
    /// <param name="studentId">The student the grade belongs to.</param>
    /// <param name="courseId">The course the grade is awarded for.</param>
    /// <param name="gradeValue">The grade value.</param>
    public async Task SaveGradeAsync(Guid studentId, Guid courseId, int gradeValue)
    {
        // Enforce the valid grade range here in the business layer: EF Core does not evaluate the
        // [Range] data annotation on save, so this is the single authoritative guard.
        if (!GradingScale.IsValidValue(gradeValue))
            throw new ArgumentOutOfRangeException(
                nameof(gradeValue),
                gradeValue,
                $"Grade must be within [{GradingScale.MinValue}, {GradingScale.MaxValue}].");

        var studentGrades = await gradeRepository.GetByStudentIdAsync(studentId);
        var existingGrade = studentGrades.FirstOrDefault(grade => grade.CourseId == courseId);

        if (existingGrade is null)
        {
            await gradeRepository.AddAsync(new Grade
            {
                Id = Guid.NewGuid(),
                StudentId = studentId,
                CourseId = courseId,
                Value = gradeValue
            });
        }
        else
        {
            existingGrade.Value = gradeValue;
            await gradeRepository.UpdateAsync(existingGrade);
        }

        await RecalculateCreditsAsync(studentId);
    }

    /// <summary>
    ///     Deletes a grade and recomputes the owning student's credits so any credits it contributed are reverted.
    /// </summary>
    /// <param name="gradeId">The unique identifier of the grade to delete.</param>
    public async Task DeleteGradeAsync(Guid gradeId)
    {
        var grade = await gradeRepository.GetByIdAsync(gradeId);
        var studentId = grade.StudentId;

        await gradeRepository.DeleteAsync(gradeId);
        await RecalculateCreditsAsync(studentId);
    }

    /// <summary>
    ///     Retrieves the grades of a specific student as binding models.
    /// </summary>
    /// <param name="id">The unique identifier of the student.</param>
    /// <returns>The student's grades as <see cref="GradeBindingModel" /> instances.</returns>
    public async Task<List<GradeBindingModel>> GetByStudentIdAsync(Guid id)
    {
        var grades = await gradeRepository.GetByStudentIdAsync(id);
        return grades.Select(EntityMappings.ToBindingModel).ToList();
    }

    /// <summary>
    ///     Retrieves all grades from the repository asynchronously.
    /// </summary>
    public async Task<List<Grade>> GetAllAsync()
    {
        return await gradeRepository.GetAllAsync();
    }

    /// <summary>
    ///     Retrieves a grade by its unique identifier.
    /// </summary>
    public async Task<Grade> GetByIdAsync(Guid id)
    {
        return await gradeRepository.GetByIdAsync(id);
    }

    /// <summary>
    ///     Asynchronously adds a new grade to the repository <b>without</b> recomputing credits.
    /// </summary>
    /// <remarks>
    ///     Low-level persistence primitive. Prefer <see cref="SaveGradeAsync" /> for user-facing operations so
    ///     that the student's credits stay consistent.
    /// </remarks>
    public async Task AddAsync(Grade value)
    {
        await gradeRepository.AddAsync(value);
    }

    /// <summary>
    ///     Asynchronously deletes a grade by its unique identifier <b>without</b> recomputing credits.
    /// </summary>
    /// <remarks>
    ///     Low-level persistence primitive. Prefer <see cref="DeleteGradeAsync" /> for user-facing operations so
    ///     that the student's credits stay consistent.
    /// </remarks>
    public async Task DeleteAsync(Guid id)
    {
        await gradeRepository.DeleteAsync(id).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously updates a grade <b>without</b> recomputing credits.
    /// </summary>
    /// <remarks>
    ///     Low-level persistence primitive. Prefer <see cref="SaveGradeAsync" /> for user-facing operations so
    ///     that the student's credits stay consistent.
    /// </remarks>
    public async Task UpdateAsync(Grade value)
    {
        await gradeRepository.UpdateAsync(value);
    }

    /// <summary>
    ///     Recomputes a student's earned credits from all of their passing grades and persists the result.
    /// </summary>
    private async Task RecalculateCreditsAsync(Guid studentId)
    {
        var gradeCredits = await gradeRepository.GetGradeCreditsByStudentIdAsync(studentId);
        var student = await studentRepository.GetByIdAsync(studentId);

        student.Credits = gradeCalculationService.CalculateEarnedCredits(gradeCredits);
        await studentRepository.UpdateAsync(student);
    }
}