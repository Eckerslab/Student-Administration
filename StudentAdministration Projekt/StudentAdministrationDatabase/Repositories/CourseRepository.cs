using Microsoft.EntityFrameworkCore;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;

namespace StudentAdministrationDatabase.Repositories;

/// <summary>
///     Provides repository operations for managing <see cref="StudentAdministrationDatabase.Models.Course" /> entities
///     in the student administration database.
/// </summary>
/// <remarks>
///     This class implements <see cref="StudentAdministrationDatabase.Repositories.Interfaces.ICourseRepository" />
///     and provides methods for adding, deleting, and retrieving course entities asynchronously.
/// </remarks>
public class CourseRepository : ICourseRepository
{
    private readonly StudentAdministrationDbContext dbContext;

    public CourseRepository(StudentAdministrationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="Course" /> entity to the database.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Course" /> entity to be added to the database.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This method adds the specified <see cref="Course" /> entity to the database context
    ///     and saves the changes asynchronously.
    /// </remarks>
    public async Task AddAsync(Course value)
    {
        this.dbContext.Courses.Add(value);
        await this.dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Deletes a course entity from the database by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the course to be deleted.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous delete operation.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the course with the specified <paramref name="id" /> is not found in the database.
    /// </exception>
    public async Task DeleteAsync(Guid id)
    {
        Course? courseToDelete = this.dbContext.Courses.SingleOrDefault(x => x.Id == id);
        if (courseToDelete is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        this.dbContext.Courses.Remove(courseToDelete);
        await this.dbContext.SaveChangesAsync();
    }

    /// <summary>
    ///     Asynchronously retrieves all courses from the data store.n>
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of all courses. The list will be
    ///     empty if no courses are found.
    /// </returns>
    public async Task<List<Course>> GetAllAsync()
    {
        return await this.dbContext.Courses.ToListAsync();
    }

    /// <summary>
    ///     Retrieves a <see cref="Course" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the <see cref="Course" /> to retrieve.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the <see cref="Course" /> entity
    ///     associated with the specified identifier.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when no <see cref="Course" /> entity with the specified identifier is found.
    /// </exception>
    public Task<Course> GetByIdAsync(Guid id)
    {
        Course? course = this.dbContext.Courses.SingleOrDefault(x => x.Id == id);
        if (course is null)
        {
            throw new ArgumentException("Entry not found!");
        }

        return Task.FromResult(course);
    }
}