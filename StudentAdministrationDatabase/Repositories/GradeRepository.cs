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
///     Each operation uses a short-lived <see cref="StudentAdministrationDbContext" /> obtained from an
///     <see cref="IDbContextFactory{TContext}" /> to avoid a long-lived, shared (and non-thread-safe) context.
/// </remarks>
public class GradeRepository : IGradeRepository
{
    private readonly IDbContextFactory<StudentAdministrationDbContext> contextFactory;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GradeRepository" /> class.
    /// </summary>
    /// <param name="contextFactory">
    ///     The factory used to create short-lived <see cref="StudentAdministrationDbContext" /> instances.
    /// </param>
    public GradeRepository(IDbContextFactory<StudentAdministrationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="Grade" /> entity to the repository.
    /// </summary>
    /// <param name="value">The <see cref="Grade" /> entity to be added. Must not be <c>null</c>.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task AddAsync(Grade value)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        await dbContext.Grades.AddAsync(value);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously deletes a <see cref="Grade" /> entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Grade" /> entity to be deleted.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task DeleteAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var grade = await dbContext.Grades.SingleAsync(x => x.Id == id);
        dbContext.Grades.Remove(grade);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves all <see cref="Grade" /> entities from the database.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="Grade" /> entities.
    /// </returns>
    public async Task<List<Grade>> GetAllAsync()
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.Grades.AsNoTracking().ToListAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves a <see cref="Grade" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Grade" /> entity to retrieve.</param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation, containing the matching
    ///     <see cref="Grade" /> entity.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if no grade with the specified identifier exists.</exception>
    public async Task<Grade> GetByIdAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var grade = await dbContext.Grades.SingleOrDefaultAsync(x => x.Id == id);
        if (grade is null) throw new ArgumentException("Entry not found!");

        return grade;
    }

    /// <summary>
    ///     Asynchronously retrieves all <see cref="Grade" /> entities for a student, including their courses.
    /// </summary>
    /// <param name="id">The unique identifier of the student whose grades are to be retrieved.</param>
    /// <returns>
    ///     A <see cref="Task" /> whose result is the list of the student's grades. The list is empty when the
    ///     student has no grades. Each grade has its <see cref="Grade.Course" /> navigation eagerly loaded.
    /// </returns>
    public async Task<List<Grade>> GetByStudentIdAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.Grades
            .AsNoTracking()
            .Include(x => x.Course)
            .Where(x => x.StudentId == id)
            .ToListAsync();
    }

    /// <inheritdoc cref="IGradeRepository.GetGradeCreditsByStudentIdAsync" />
    public async Task<List<StudentGradeCredit>> GetGradeCreditsByStudentIdAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.Grades
            .AsNoTracking()
            .Where(x => x.StudentId == id)
            .Select(x => new StudentGradeCredit(x.Value, x.Course.Credit))
            .ToListAsync();
    }

    /// <summary>
    ///     Updates an existing <see cref="Grade" /> entity in the database with new values.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Grade" /> entity containing updated values. The entity must have a valid <see cref="Grade.Id" />.
    /// </param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown when the <see cref="Grade" /> entity with the specified <see cref="Grade.Id" /> is not found in the
    ///     database.
    /// </exception>
    public async Task UpdateAsync(Grade value)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var tracked = await dbContext.Grades.FindAsync(value.Id);
        if (tracked is null) throw new KeyNotFoundException();

        dbContext.Entry(tracked).CurrentValues.SetValues(value);
        await dbContext.SaveChangesAsync();
    }
}