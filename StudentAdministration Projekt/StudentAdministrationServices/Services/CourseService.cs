using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;
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
    ///     <see cref="StudentAdministrationServices.Models.CourseBindingModel" /> objects representing the courses.
    /// </returns>
    public async Task<List<CourseBindingModel>> GetCoursesByDegreeProgramID(Guid id)
    {
        List<CourseBindingModel> courseBindingModels = new();
        List<Course> allCourses = await this.courseRepository.GetAllAsync();
        List<Course> convertedCourses = allCourses.Where(course => course.DegreeProgramId == id).ToList();
        foreach (Course course in convertedCourses)
        {
            CourseBindingModel bindingModel = ConvertTo(course);
            courseBindingModels.Add(bindingModel);
        }

        return courseBindingModels;
    }

    /// <summary>
    ///     Retrieves all courses from the repository and converts them into binding models.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains a list of <see cref="CourseBindingModel" /> representing all courses.
    /// </returns>
    /// <remarks>
    ///     This method fetches all courses from the underlying data source using the repository,
    ///     converts them into binding models, and returns the resulting list.
    /// </remarks>
    public async Task<List<CourseBindingModel>> GetAllAsync()
    {
        List<Course> courses = await this.courseRepository.GetAllAsync();
        List<CourseBindingModel> bindingModels = [];
        foreach (Course course in courses)
        {
            CourseBindingModel bindingModel = ConvertTo(course);
            bindingModels.Add(bindingModel);
        }

        return bindingModels;
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
    /// <exception cref="InvalidOperationException">Thrown if no course is found with the specified <paramref name="id" />.</exception>
    public async Task<CourseBindingModel> GetByIdAsync(Guid id)
    {
        Course course = await this.courseRepository.GetByIdAsync(id);
        return ConvertTo(course);
    }

    /// <summary>
    ///     Converts a <see cref="Course" /> object to a <see cref="CourseBindingModel" /> object.
    /// </summary>
    /// <param name="course">
    ///     The <see cref="Course" /> object to be converted.
    /// </param>
    /// <returns>
    ///     A <see cref="CourseBindingModel" /> object containing the converted data from the provided <see cref="Course" />.
    /// </returns>
    private static CourseBindingModel ConvertTo(Course course)
    {
        return new CourseBindingModel
               {
                   Id = course.Id,
                   Name = course.Name,
                   Credit = course.Credit
               };
    }
}