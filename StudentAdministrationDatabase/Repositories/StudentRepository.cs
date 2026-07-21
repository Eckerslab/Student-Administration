using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Represents a repository for managing <see cref="Student" /> entities, providing data access operations
///     such as adding, deleting, and retrieving students by their unique identifiers.
/// </summary>
/// <remarks>
///     Each operation uses a short-lived <see cref="StudentAdministrationDbContext" /> obtained from an
///     <see cref="IDbContextFactory{TContext}" /> to avoid a long-lived, shared (and non-thread-safe) context.
/// </remarks>
public class StudentRepository : IStudentRepository
{
    private readonly IDbContextFactory<StudentAdministrationDbContext> contextFactory;

    public StudentRepository(IDbContextFactory<StudentAdministrationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="Student" /> entity and persists the changes to the database.
    /// </summary>
    /// <param name="student">The <see cref="Student" /> entity to be added. This parameter must not be <c>null</c>.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task AddAsync(Student student)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        dbContext.Students.Add(student);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously deletes a <see cref="Student" /> entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Student" /> entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the student is not found.</exception>
    public async Task DeleteAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var student = await dbContext.Students.SingleOrDefaultAsync(x => x.Id == id);
        if (student is null) throw new KeyNotFoundException("Entry not found!");

        dbContext.Remove(student);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves all <see cref="Student" /> entities including all related data.
    /// </summary>
    /// <remarks>
    ///     Eagerly loads degree program, university and grades (with their courses). This is the full graph;
    ///     for list/summary scenarios prefer <see cref="GetAllForListAsync" /> to avoid over-fetching.
    /// </remarks>
    public async Task<List<Student>> GetAllAsync()
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.Students
            .AsNoTracking()
            .Include(s => s.DegreeProgram)
            .Include(s => s.University)
            .Include(s => s.Grades)!
            .ThenInclude(g => g.Course)
            .ToListAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves all students with only their degree program loaded.
    /// </summary>
    /// <remarks>
    ///     Intended for list/summary views: it deliberately omits university and grades to avoid the Cartesian
    ///     explosion and over-fetching that <see cref="GetAllAsync" /> incurs.
    /// </remarks>
    public async Task<List<Student>> GetAllForListAsync()
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.Students
            .AsNoTracking()
            .Include(s => s.DegreeProgram)
            .ToListAsync();
    }

    /// <summary>
    ///     Gets a <see cref="Student" /> entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Student" /> entity to retrieve.</param>
    /// <returns>A <see cref="Task" /> containing the <see cref="Student" /> entity.</returns>
    public async Task<Student> GetByIdAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var student = await dbContext.Students
            .Include(s => s.DegreeProgram)
            .Include(s => s.University)
            .Include(s => s.Grades)!
            .ThenInclude(g => g.Course)
            .SingleAsync(s => s.Id == id);
        return student;
    }

    /// <summary>
    ///     Returns the next free student number, i.e. one greater than the current maximum
    ///     (or a fixed starting value when there are no students yet).
    /// </summary>
    /// <remarks>
    ///     Replaces random generation, which could produce duplicate student numbers.
    /// </remarks>
    public async Task<int> GetNextStudentNumberAsync()
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var max = await dbContext.Students
            .AsNoTracking()
            .Select(s => (int?)s.StudentNumber)
            .MaxAsync() ?? 9_999_999;
        return max + 1;
    }

    public async Task UpdateAsync(Student value)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var tracked = await dbContext.Students.FindAsync(value.Id);
        if (tracked is null) throw new KeyNotFoundException();

        dbContext.Entry(tracked).CurrentValues.SetValues(value);
        await dbContext.SaveChangesAsync();
    }
}