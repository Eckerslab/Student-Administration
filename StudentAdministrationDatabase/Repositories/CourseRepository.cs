using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Provides repository operations for managing <see cref="Course" /> entities
///     in the student administration database.
/// </summary>
/// <remarks>
///     Each operation uses a short-lived <see cref="StudentAdministrationDbContext" /> obtained from an
///     <see cref="IDbContextFactory{TContext}" /> to avoid a long-lived, shared (and non-thread-safe) context.
/// </remarks>
public class CourseRepository : ICourseRepository
{
    private readonly IDbContextFactory<StudentAdministrationDbContext> contextFactory;

    public CourseRepository(IDbContextFactory<StudentAdministrationDbContext> contextFactory)
    {
        this.contextFactory = contextFactory;
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="Course" /> entity to the database.
    /// </summary>
    /// <param name="value">The <see cref="Course" /> entity to be added to the database.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task AddAsync(Course value)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        dbContext.Courses.Add(value);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Deletes a course entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the course to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the course is not found in the database.</exception>
    public async Task DeleteAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var courseToDelete = await dbContext.Courses.SingleOrDefaultAsync(x => x.Id == id);
        if (courseToDelete is null) throw new KeyNotFoundException("Entry not found!");

        dbContext.Courses.Remove(courseToDelete);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves all courses from the data store.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of all courses. The list
    ///     will be empty if no courses are found.
    /// </returns>
    public async Task<List<Course>> GetAllAsync()
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.Courses.AsNoTracking().ToListAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves all courses that belong to the specified degree program.
    /// </summary>
    /// <param name="degreeProgramId">The unique identifier of the degree program.</param>
    /// <returns>A task whose result is the list of matching courses. The filter runs in the database.</returns>
    public async Task<List<Course>> GetByDegreeProgramIdAsync(Guid degreeProgramId)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        return await dbContext.Courses
            .AsNoTracking()
            .Where(course => course.DegreeProgramId == degreeProgramId)
            .ToListAsync();
    }

    /// <summary>
    ///     Retrieves a <see cref="Course" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="Course" /> to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation, containing the <see cref="Course" /> entity.</returns>
    /// <exception cref="ArgumentException">Thrown when no course with the specified identifier is found.</exception>
    public async Task<Course> GetByIdAsync(Guid id)
    {
        await using var dbContext = await contextFactory.CreateDbContextAsync();
        var course = await dbContext.Courses.SingleOrDefaultAsync(x => x.Id == id);
        if (course is null) throw new ArgumentException("Entry not found!");

        return course;
    }
}