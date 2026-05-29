using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationDatabase.SampleData;
using StudentAdministrationTests.MockData.DataSource;

namespace StudentAdministrationTests.MockData.Repository;

/// <summary>
///     Represents a repository for managing <see cref="Grade" /> entities in the context of unit tests.
/// </summary>
/// <remarks>
///     This class provides methods for performing CRUD operations on <see cref="Grade" /> entities,
///     such as adding, retrieving, updating, and deleting grades. It is designed to work with a predefined
///     list of sample grades for testing purposes.
/// </remarks>
internal class GradeRepositoryMock : IGradeRepository
{
    private readonly List<Grade> grades;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GradeRepositoryMock" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor initializes the repository with a predefined list of sample grades
    ///     by invoking the <see cref="SampleGrades.GetGrades" /> method.
    /// </remarks>
    public GradeRepositoryMock()
    {
        this.grades = SampleGradeMock.GetGrades();
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="Grade" /> entity to the repository.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Grade" /> entity to add. Must not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="value" /> is <c>null</c>.
    /// </exception>
    public Task AddAsync(Grade value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        this.grades.Add(value);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Deletes a grade entry from the repository based on the specified unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the grade to be deleted. Must not be an empty GUID.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous delete operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the provided <paramref name="id" /> is an empty GUID or if no grade entry
    ///     with the specified <paramref name="id" /> exists in the repository.
    /// </exception>
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("The provided ID cannot be an empty GUID.", nameof(id));
        }

        Grade? grade = this.grades.SingleOrDefault(x => x.Id == id);
        if (grade == null)
        {
            throw new ArgumentException("Entry not found!");
        }

        this.grades.Remove(grade);
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Retrieves all grades from the repository.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of <see cref="Grade" /> objects.
    /// </returns>
    public async Task<List<Grade>> GetAllAsync()
    {
        return await Task.FromResult(this.grades);
    }

    /// <summary>
    ///     Retrieves a <see cref="Grade" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the grade to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="Grade" /> entity.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the provided <paramref name="id" /> is <see cref="Guid.Empty" />
    ///     or when no grade with the specified <paramref name="id" /> is found.
    /// </exception>
    public Task<Grade> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("The provided ID is invalid.");
        }

        Grade? grade = this.grades.SingleOrDefault(g => g.Id == id);
        if (grade == null)
        {
            throw new ArgumentException("Grade not found.");
        }

        return Task.FromResult(grade);
    }

    /// <summary>
    ///     Asynchronously retrieves a list of <see cref="Grade" /> entities associated with a specific student.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the student whose grades are to be retrieved. Must not be <see cref="Guid.Empty" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> representing the asynchronous operation, with a result of a list of
    ///     <see cref="Grade" /> entities.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the <paramref name="id" /> is <see cref="Guid.Empty" />.
    /// </exception>
    public Task<List<Grade>> GetByStudentIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Student ID cannot be empty.", nameof(id));
        }

        List<Grade> result = this.grades.Where(grade => grade.StudentId == id).ToList();
        return Task.FromResult(result);
    }

    /// <summary>
    ///     Asynchronously updates an existing <see cref="Grade" /> entity in the repository.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Grade" /> entity containing updated information. Must not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="value" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown when no existing grade is found with the same <see cref="Grade.Id" /> as the provided
    ///     <paramref name="value" />.
    /// </exception>
    public Task UpdateAsync(Grade value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "The grade entity must not be null.");
        }

        Grade? existingGrade = this.grades.SingleOrDefault(x => x.Id == value.Id);
        if (existingGrade == null)
        {
            throw new KeyNotFoundException($"No grade found with ID {value.Id}.");
        }

        existingGrade.Course = value.Course;
        existingGrade.CourseId = value.CourseId;
        existingGrade.Student = value.Student;
        existingGrade.StudentId = value.StudentId;
        existingGrade.Value = value.Value;

        return Task.CompletedTask;
    }
}