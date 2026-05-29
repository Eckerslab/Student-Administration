using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Provides data access operations for managing <see cref="University" /> entities in the database.
///     Implements the <see cref="IUniversityRepository" /> interface to define specific repository behavior for
///     universities.
/// </summary>
public class UniversityRepository : IUniversityRepository
{
    private readonly StudentAdministrationDbContext dbContext;

    public UniversityRepository(StudentAdministrationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="University" /> entity to the database.
    /// </summary>
    /// <param name="value">The <see cref="University" /> entity to be added.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="value" /> is <c>null</c>.</exception>
    public async Task AddAsync(University value)
    {
        await this.dbContext.Universities.AddAsync(value);
        await this.dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Deletes a <see cref="University" /> entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="University" /> to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when no <see cref="University" /> entity with the specified <paramref name="id" /> is found in the database.
    /// </exception>
    public async Task DeleteAsync(Guid id)
    {
        University? universityToDelete = await this.dbContext.Universities.SingleOrDefaultAsync(x => x.Id == id);
        if (universityToDelete is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        this.dbContext.Universities.Remove(universityToDelete);
        await this.dbContext.SaveChangesAsync();
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
        // Lädt alle Universitäten aus der Datenbank
        return await this.dbContext.Universities.ToListAsync();
    }

    /// <summary>
    ///     Retrieves a <see cref="University" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the <see cref="University" /> to retrieve.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the <see cref="University" /> entity
    ///     with the specified identifier.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when no <see cref="University" /> entity with the specified identifier is found.
    /// </exception>
    public async Task<University> GetByIdAsync(Guid id)
    {
        University? university = await this.dbContext.Universities.SingleOrDefaultAsync(x => x.Id == id);
        if (university is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        return university;
    }
}