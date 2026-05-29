using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationTests.MockData.DataSource;

namespace StudentAdministrationTests.MockData.Repository;

/// <summary>
///     Represents a repository for managing <see cref="Student" /> entities in a test environment.
///     Implements the <see cref="IStudentRepository" /> interface to provide CRUD operations.
/// </summary>
internal class StudentRepositoryMock : IStudentRepository
{
    private readonly List<Student> students;

    public StudentRepositoryMock()
    {
        this.students = SampleStudentMock.GetStudents();
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="Student" /> entity to the repository and persists the changes to the database.
    /// </summary>
    /// <param name="student">
    ///     The <see cref="Student" /> entity to be added. This parameter must not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="student" /> parameter is <c>null</c>.
    /// </exception>
    public Task AddAsync(Student student)
    {
        this.students.Add(student);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Asynchronously deletes a <see cref="Student" /> entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Student" /> entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="id" /> is an empty GUID or the student is not found.</exception>
    public async Task DeleteAsync(Guid id)
    {
        this.students.Remove(this.students.SingleOrDefault(x => x.Id == id) ?? throw new ArgumentException("Entry not found!"));
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Asynchronously retrieves all <see cref="Student" /> entities from the database.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> representing the asynchronous operation,
    ///     with a result of a <see cref="List{T}" /> containing all <see cref="Student" /> entities.
    /// </returns>
    public async Task<List<Student>> GetAllAsync()
    {
        return await Task.FromResult(this.students);
    }

    /// <summary>
    ///     Gets a <see cref="Student" /> entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Student" /> entity to retrieve.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation,
    ///     containing the <see cref="Student" /> entity if found, or <c>null</c> if no entity with the specified identifier
    ///     exists.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="id" /> is an empty GUID or the student is not found.</exception>
    public Task<Student> GetByIdAsync(Guid id)
    {
        return Task.FromResult(this.students.SingleOrDefault(x => x.Id == id) ?? throw new ArgumentException("Entry not found!"));
    }

    public async Task UpdateAsync(Student value)
    {
        this.students.Remove(this.students.SingleOrDefault(x => x.Id == value.Id) ?? throw new ArgumentException("Entry not found!"));
        this.students.Add(value);
        await Task.CompletedTask;
    }
}