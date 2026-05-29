using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationDatabase.SampleData;
using StudentAdministrationTests.MockData.DataSource;

namespace StudentAdministrationTests.MockData.Repository;

/// <summary>
///     Represents a repository for managing <see cref="Course" /> entities in the context of unit tests.
/// </summary>
/// <remarks>
///     This class provides methods to perform CRUD operations on <see cref="Course" /> entities,
///     including adding, deleting, retrieving, and listing courses. It is primarily used for testing purposes
///     and operates on a predefined set of sample data.
/// </remarks>
internal class CourseRepositoryMock : ICourseRepository
{
    private readonly List<Course> courses;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CourseRepositoryMock" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor initializes the repository with a predefined list of sample courses
    ///     retrieved from <see cref="SampleCourses.GetCourses" />.
    /// </remarks>
    public CourseRepositoryMock()
    {
        this.courses = SampleCoursesMock.GetCourses();
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="Course" /> entity to the repository.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Course" /> entity to be added. Must not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="value" /> is <c>null</c>.
    /// </exception>
    public Task AddAsync(Course value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        this.courses.Add(value);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Deletes a course from the repository asynchronously based on the provided unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the course to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the provided <paramref name="id" /> is an empty GUID or if no course with the specified ID is found.
    /// </exception>
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("The provided ID cannot be an empty GUID.", nameof(id));
        }

        Course? course = this.courses.SingleOrDefault(x => x.Id == id);
        if (course == null)
        {
            throw new ArgumentException("Entry not found!");
        }

        this.courses.Remove(course);
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Retrieves all <see cref="Course" /> entities from the repository.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of <see cref="Course" />
    ///     entities.
    /// </returns>
    public async Task<List<Course>> GetAllAsync()
    {
        return await Task.FromResult(this.courses);
    }

    /// <summary>
    ///     Retrieves a <see cref="Course" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the course to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="Course" /> entity.</returns>
    /// <exception cref="ArgumentException">Thrown when no course with the specified identifier is found.</exception>
    public Task<Course> GetByIdAsync(Guid id)
    {
        Course? course = this.courses.SingleOrDefault(x => x.Id == id);
        if (course == null)
        {
            throw new ArgumentException("Entry not found!");
        }

        return Task.FromResult(course);
    }
}