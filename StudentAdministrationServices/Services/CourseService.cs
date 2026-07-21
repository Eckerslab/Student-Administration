using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationServices.Mapping;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministrationServices.Services;

/// <summary>
///     Provides services for managing courses in the student administration system.
/// </summary>
public class CourseService : ICourseService
{
    private readonly ICourseRepository courseRepository;

    /// <summary>
    ///     CourseService constructor.
    /// </summary>
    /// <param name="courseRepository"></param>
    public CourseService(ICourseRepository courseRepository)
    {
        this.courseRepository = courseRepository;
    }

    /// <summary>
    ///     Retrieves a list of courses associated with a specific degree program.
    /// </summary>
    /// <param name="id">The unique identifier of the degree program.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="CourseBindingModel" /> objects representing the courses.
    /// </returns>
    public async Task<List<CourseBindingModel>> GetCoursesByDegreeProgramID(Guid id)
    {
        // Filtering happens in the database instead of loading every course and filtering in memory.
        var courses = await courseRepository.GetByDegreeProgramIdAsync(id);
        return courses.Select(EntityMappings.ToBindingModel).ToList();
    }

    /// <summary>
    ///     Retrieves all courses from the repository and converts them into binding models.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains a list of <see cref="CourseBindingModel" /> representing all courses.
    /// </returns>
    public async Task<List<CourseBindingModel>> GetAllAsync()
    {
        var courses = await courseRepository.GetAllAsync();
        return courses.Select(EntityMappings.ToBindingModel).ToList();
    }

    /// <summary>
    ///     Retrieves a course by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the course to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     a <see cref="CourseBindingModel" /> representing the course with the specified identifier.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the provided <paramref name="id" /> is invalid.</exception>
    public async Task<CourseBindingModel> GetByIdAsync(Guid id)
    {
        var course = await courseRepository.GetByIdAsync(id);
        return course.ToBindingModel();
    }
}