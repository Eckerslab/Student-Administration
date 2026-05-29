using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Represents a repository for managing <see cref="Grade" /> entities,
///     providing data access operations specific to grades in the student administration system.
/// </summary>
/// <remarks>
///     This repository implements the <see cref="IGradeRepository" /> interface,
///     inheriting common data access operations from <see cref="IBaseRepository{T}" />.
/// </remarks>
public class GradeRepository : IGradeRepository
{
    private readonly StudentAdministrationDbContext dbContext;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GradeRepository" /> class.
    /// </summary>
    /// <param name="dbContext">
    ///     The <see cref="StudentAdministrationDbContext" /> instance used to interact with the database.
    ///     Must not be <c>null</c>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if the <paramref name="dbContext" /> is <c>null</c>.
    /// </exception>
    public GradeRepository(StudentAdministrationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="Grade" /> entity to the repository.
    /// </summary>
    /// <param name="value">The <see cref="Grade" /> entity to be added. Must not be <c>null</c>.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="value" /> is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the operation cannot be completed due to repository constraints or state.
    /// </exception>
    public async Task AddAsync(Grade value)
    {
        await this.dbContext.Grades.AddAsync(value);
        await this.dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously deletes a <see cref="Grade" /> entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Grade" /> entity to be deleted.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the <paramref name="id" /> is an empty <see cref="Guid" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the operation cannot be completed due to repository constraints or state.
    /// </exception>
    public Task DeleteAsync(Guid id)
    {
        Grade? grade = this.dbContext.Grades.SingleOrDefault(x => x.Id == id);
        if (grade is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        this.dbContext.Grades.Remove(grade);
        this.dbContext.SaveChangesAsync();
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Retrieves all <see cref="Grade" /> entities from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="Grade" /> entities.
    /// </returns>
    /// <remarks>
    ///     This method queries the database context to fetch all records from the <see cref="DbSet{T}" />
    ///     of <see cref="Grade" />.
    /// </remarks>
    public async Task<List<Grade>> GetAllAsync()
    {
        return await this.dbContext.Grades.ToListAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves a <see cref="Grade" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Grade" /> entity to retrieve.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation,
    ///     containing the <see cref="Grade" /> entity if found, or <c>null</c> if no entity with the specified identifier
    ///     exists.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="id" /> is an empty GUID.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the operation cannot be completed due to repository constraints or state.
    /// </exception>
    public Task<Grade> GetByIdAsync(Guid id)
    {
        Grade? grade = this.dbContext.Grades.SingleOrDefault(x => x.Id == id);
        if (grade is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        return Task.FromResult(grade);
    }

    /// <summary>
    ///     Asynchronously retrieves a <see cref="Grade" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Grade" /> entity to retrieve.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation,
    ///     containing the <see cref="Grade" /> entity if found, or <c>null</c> if no entity with the specified identifier
    ///     exists.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="id" /> is an empty GUID.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the operation cannot be completed due to repository constraints or state.
    /// </exception>
    public Task<List<Grade>> GetByStudentIdAsync(Guid id)
    {
        List<Grade> grade = this.dbContext.Grades.Where(x => x.StudentId == id).ToList();
        if (grade is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        return Task.FromResult(grade);
    }

    /// <summary>
    ///     Updates an existing <see cref="Grade" /> entity in the database with new values.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Grade" /> entity containing updated values. The entity must have a valid <see cref="Grade.Id" />.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous update operation.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown when the <see cref="Grade" /> entity with the specified <see cref="Grade.Id" /> is not found in the
    ///     database.
    /// </exception>
    public async Task UpdateAsync(Grade value)
    {
        Grade? tracked = await this.dbContext.Grades.FindAsync(value.Id);
        if (tracked is null)
        {
            throw new KeyNotFoundException();
        }

        this.dbContext.Entry(tracked).CurrentValues.SetValues(value);
        await this.dbContext.SaveChangesAsync();
    }
}