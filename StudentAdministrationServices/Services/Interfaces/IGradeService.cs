using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;

namespace StudentAdministrationServices.Services.Interfaces;

/// <summary>
///     Interface for grade service operations.
/// </summary>
/// <remarks>
///     Deliberately exposes only the safe, credit-consistent write operations (<see cref="SaveGradeAsync" /> and
///     <see cref="DeleteGradeAsync" />) plus read access. The low-level create/update/delete primitives are
///     intentionally <b>not</b> part of this contract so callers cannot bypass the credit recomputation.
/// </remarks>
public interface IGradeService : IReceiveService<Grade>
{
    Task<List<GradeBindingModel>> GetByStudentIdAsync(Guid id);

    /// <summary>
    ///     Adds or updates a student's grade for a course and keeps the student's credits consistent.
    /// </summary>
    /// <param name="studentId">The student the grade belongs to.</param>
    /// <param name="courseId">The course the grade is awarded for.</param>
    /// <param name="gradeValue">The grade value.</param>
    Task SaveGradeAsync(Guid studentId, Guid courseId, int gradeValue);

    /// <summary>
    ///     Deletes a grade and reverts its contribution to the owning student's credits.
    /// </summary>
    /// <param name="gradeId">The unique identifier of the grade to delete.</param>
    Task DeleteGradeAsync(Guid gradeId);
}