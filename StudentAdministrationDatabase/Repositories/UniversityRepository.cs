using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Provides data access operations for managing <see cref="University" /> entities in the database.
/// </summary>
/// <remarks>
///     Each operation uses a short-lived <see cref="StudentAdministrationDbContext" /> obtained from an
///     <see cref="IDbContextFactory{TContext}" /> to avoid a long-lived, shared (and non-thread-safe) context.
/// </remarks>
public class UniversityRepository : IUniversityRepository
{
    private readonly IDbContextFactory<StudentAdministrationDbContext> contextFactory;

    public UniversityRepository(IDbContextFactory<StudentAdministrationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="University" /> entity to the database.
    /// </summary>
    /// <param name="value">The <see cref="University" /> entity to be added.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task AddAsync(University value)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        await dbContext.Universities.AddAsync(value);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Deletes a <see cref="University" /> entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="University" /> to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no matching entity is found in the database.</exception>
    public async Task DeleteAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var universityToDelete = await dbContext.Universities.SingleOrDefaultAsync(x => x.Id == id);
        if (universityToDelete is null) throw new KeyNotFoundException("Entry not found!");

        dbContext.Universities.Remove(universityToDelete);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves all <see cref="University" /> entities from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="University" /> entities.
    /// </returns>
    public async Task<List<University>> GetAllAsync()
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.Universities.AsNoTracking().ToListAsync();
    }

    /// <summary>
    ///     Retrieves a <see cref="University" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="University" /> to retrieve.</param>
    /// <returns>A task whose result contains the matching <see cref="University" /> entity.</returns>
    /// <exception cref="ArgumentException">Thrown when no matching entity is found.</exception>
    public async Task<University> GetByIdAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var university = await dbContext.Universities.SingleOrDefaultAsync(x => x.Id == id);
        if (university is null) throw new ArgumentException("Entry not found!");

        return university;
    }
}