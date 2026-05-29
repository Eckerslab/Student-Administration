using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Degree Program Repository Interface.
/// </summary>
public class DegreeProgramRepository : IDegreeProgramRepository
{
    private readonly StudentAdministrationDbContext dbContext;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DegreeProgramRepository" /> class.
    /// </summary>
    /// <param name="dbContext">
    ///     The <see cref="StudentAdministrationDbContext" /> instance used to interact with the database.
    /// </param>
    public DegreeProgramRepository(StudentAdministrationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    ///     Adds a new <see cref="DegreeProgram" /> entity to the database asynchronously.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="DegreeProgram" /> entity to be added to the database.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation of adding the entity to the database.
    /// </returns>
    public async Task AddAsync(DegreeProgram value)
    {
        this.dbContext.DegreePrograms.Add(value);
        await this.dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Deletes a degree program entity from the database based on the specified identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the degree program to be deleted.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous delete operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when no degree program with the specified identifier is found in the database.
    /// </exception>
    public async Task DeleteAsync(Guid id)
    {
        DegreeProgram? degreeProgramToDelete = this.dbContext.DegreePrograms.SingleOrDefault(x => x.Id == id);
        if (degreeProgramToDelete is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        this.dbContext.DegreePrograms.Remove(degreeProgramToDelete);
        await this.dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves all degree programs from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="DegreeProgram" /> entities.
    /// </returns>
    /// <remarks>
    ///     This method queries the database context to fetch all records of degree programs.
    /// </remarks>
    public async Task<List<DegreeProgram>> GetAllAsync()
    {
        return await this.dbContext.DegreePrograms.ToListAsync();
    }

    /// <summary>
    ///     Retrieves a <see cref="DegreeProgram" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="DegreeProgram" /> to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the <see cref="DegreeProgram" /> entity
    ///     if found.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when no <see cref="DegreeProgram" /> entity with the specified <paramref name="id" /> is found.
    /// </exception>
    public Task<DegreeProgram> GetByIdAsync(Guid id)
    {
        DegreeProgram? degreeProgram = this.dbContext.DegreePrograms.SingleOrDefault(x => x.Id == id);
        if (degreeProgram is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        return Task.FromResult(degreeProgram);
    }
}