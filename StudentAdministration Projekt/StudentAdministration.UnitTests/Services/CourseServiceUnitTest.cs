using FluentAssertions;
using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services;
using StudentAdministrationTests.MockData.Repository;

namespace StudentAdministrationTests.Services;

/// <summary>
///     Provides unit tests for the <see cref="CourseService" /> class, ensuring its methods function as expected.
/// </summary>
/// <remarks>
///     This class contains tests for various methods of the <see cref="CourseService" /> class, including:
///     <list type="bullet">
///         <item>
///             <description>
///                 Retrieving all courses and verifying their mapping to <see cref="CourseBindingModel" />
///                 instances.
///             </description>
///         </item>
///         <item>
///             <description>Retrieving a course by its ID and validating the returned data.</description>
///         </item>
///         <item>
///             <description>Handling invalid course IDs by throwing appropriate exceptions.</description>
///         </item>
///         <item>
///             <description>Retrieving courses associated with a specific degree program ID.</description>
///         </item>
///     </list>
///     Mock data and repositories are used to simulate the behavior of the underlying data layer.
/// </remarks>
public class CourseServiceUnitTest
{
    private readonly CourseRepositoryMock courseRepositoryMock = new();
    private readonly CourseService courseService;
    private readonly DegreeProgramRepositoryMock degreeProgramRepositoryMock = new();

    public CourseServiceUnitTest()
    {
        this.courseService = new CourseService(this.courseRepositoryMock);
    }

    /// <summary>
    ///     Tests the <see cref="CourseService.GetAllAsync" /> method to ensure it retrieves all courses from the repository
    ///     and maps them correctly to <see cref="CourseBindingModel" /> instances.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the number of retrieved courses matches the expected count and that each course's
    ///     properties (Id, Name, and Credit) are correctly mapped.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation of the test.
    /// </returns>
    [Fact]
    public async Task GetAllAsync_ReturnsAllCourses()
    {
        // Arrange
        List<Course> courses = await this.courseRepositoryMock.GetAllAsync();
        int expectedCoursesCount = courses.Count;

        // Act
        List<CourseBindingModel> result = await this.courseService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(expectedCoursesCount);
        foreach (CourseBindingModel course in result)
        {
            courses.Should().Contain(c => c.Id == course.Id && c.Name == course.Name && c.Credit == course.Credit);
        }
    }

    /// <summary>
    ///     Tests the <see cref="CourseService.GetByIdAsync(Guid)" /> method to ensure it retrieves the correct course
    ///     based on the provided course ID.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the returned <see cref="CourseBindingModel" /> matches the expected course data,
    ///     including ID, name, and credit values.
    /// </remarks>
    /// <returns>A task that represents the asynchronous operation of the test.</returns>
    /// +
    [Fact]
    public async Task GetByIdAsync_ReturnsCorrectCourse()
    {
        // Arrange
        List<Course> courses = await this.courseRepositoryMock.GetAllAsync();
        Guid courseId = courses.First().Id;

        // Act
        CourseBindingModel result = await this.courseService.GetByIdAsync(courseId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(courseId);
        result.Name.Should().Be(courses.First().Name);
        result.Credit.Should().Be(courses.First().Credit);
    }

    /// <summary>
    ///     Tests the <see cref="CourseService.GetCoursesByDegreeProgramID(Guid)" /> method to ensure it retrieves
    ///     the correct courses associated with a specific degree program ID.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the method:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>Returns a non-null list of courses.</description>
    ///         </item>
    ///         <item>
    ///             <description>Returns the correct number of courses associated with the given degree program ID.</description>
    ///         </item>
    ///         <item>
    ///             <description>Ensures each course in the result matches the expected properties (ID, name, and credit).</description>
    ///         </item>
    ///     </list>
    ///     Mock data is used to simulate the behavior of the underlying data layer.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation of the test.</returns>
    [Fact]
    public async Task GetCoursesByDegreeProgramID_ReturnsCorrectCourses()
    {
        // Arrange
        DegreeProgram degreeProgram = (await this.degreeProgramRepositoryMock.GetAllAsync()).First();
        Guid degreeProgramId = degreeProgram.Id;
        int expectedCoursesCount = degreeProgram.Courses.Count;

        // Act
        List<CourseBindingModel> result = await this.courseService.GetCoursesByDegreeProgramID(degreeProgramId);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(expectedCoursesCount);
        foreach (CourseBindingModel course in result)
        {
            degreeProgram.Courses.Should().Contain(c => c.Id == course.Id && c.Name == course.Name && c.Credit == course.Credit);
        }
    }
}