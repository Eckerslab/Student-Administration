using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Represents a repository for managing <see cref="Student" /> entities, providing data access operations
///     such as adding, deleting, and retrieving students by their unique identifiers.
/// </summary>
public class StudentRepository : IStudentRepository
{
    private readonly StudentAdministrationDbContext dbContext;

    // Der DbContext wird hier automatisch vom DI-Container (MauiProgram) übergeben
    public StudentRepository(StudentAdministrationDbContext dbContext)
    {
        this.dbContext = dbContext;
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
    public async Task AddAsync(Student student)
    {
        this.dbContext.Students.Add(student);
        await this.dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously deletes a <see cref="Student" /> entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Student" /> entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="id" /> is an empty GUID or the student is not found.</exception>
    public async Task DeleteAsync(Guid id)
    {
        Student? student = this.dbContext.Students.SingleOrDefault(x => x.Id == id);
        if (student is null)
        {
            throw new KeyNotFoundException("Entry not found!");
        }

        this.dbContext.Remove(student);

        await this.dbContext.SaveChangesAsync();
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
        return await this.dbContext.Students.Include(s => s.DegreeProgram).Include(s => s.University).Include(s => s.Grades)!.ThenInclude(g => g.Course).ToListAsync();
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
        Student? student = this.dbContext.Students.SingleOrDefault(x => x.Id == id);
        if (student is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        return Task.FromResult(student);
    }

    public async Task UpdateAsync(Student value)
    {
        Student? tracked = await this.dbContext.Students.FindAsync(value.Id);
        if (tracked is null)
        {
            throw new KeyNotFoundException();
        }

        this.dbContext.Entry(tracked).CurrentValues.SetValues(value);
        await this.dbContext.SaveChangesAsync();
    }
}