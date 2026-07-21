using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Provides data access operations for <see cref="DegreeProgram" /> entities.
/// </summary>
/// <remarks>
///     Each operation uses a short-lived <see cref="StudentAdministrationDbContext" /> obtained from an
///     <see cref="IDbContextFactory{TContext}" /> to avoid a long-lived, shared (and non-thread-safe) context.
/// </remarks>
public class DegreeProgramRepository : IDegreeProgramRepository
{
    private readonly IDbContextFactory<StudentAdministrationDbContext> contextFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DegreeProgramRepository" /> class.
    /// </summary>
    /// <param name="contextFactory">
    ///     The factory used to create short-lived <see cref="StudentAdministrationDbContext" /> instances.
    /// </param>
    public DegreeProgramRepository(IDbContextFactory<StudentAdministrationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    /// <summary>
    ///     Adds a new <see cref="DegreeProgram" /> entity to the database asynchronously.
    /// </summary>
    /// <param name="value">The <see cref="DegreeProgram" /> entity to be added to the database.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddAsync(DegreeProgram value)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        dbContext.DegreePrograms.Add(value);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Deletes a degree program entity from the database based on the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the degree program to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no degree program with the specified identifier is found.</exception>
    public async Task DeleteAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var degreeProgramToDelete = await dbContext.DegreePrograms.SingleOrDefaultAsync(x => x.Id == id);
        if (degreeProgramToDelete is null) throw new KeyNotFoundException("Entry not found!");

        dbContext.DegreePrograms.Remove(degreeProgramToDelete);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves all degree programs from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="DegreeProgram" /> entities.
    /// </returns>
    public async Task<List<DegreeProgram>> GetAllAsync()
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.DegreePrograms.AsNoTracking().ToListAsync();
    }

    /// <summary>
    ///     Retrieves a <see cref="DegreeProgram" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="DegreeProgram" /> to retrieve.</param>
    /// <returns>A task whose result contains the <see cref="DegreeProgram" /> entity if found.</returns>
    /// <exception cref="ArgumentException">Thrown when no matching entity is found.</exception>
    public async Task<DegreeProgram> GetByIdAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var degreeProgram = await dbContext.DegreePrograms.SingleOrDefaultAsync(x => x.Id == id);
        if (degreeProgram is null) throw new ArgumentException("Entry not found!");

        return degreeProgram;
    }
}