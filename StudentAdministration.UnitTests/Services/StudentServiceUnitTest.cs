using FluentAssertions;
using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Models.BaseModels;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services;
using StudentAdministrationTests.MockData.Repository;
using StudentAdministrationTests.MockData.Services;

namespace StudentAdministrationTests.Services;

/// <summary>
///     Provides unit tests for the <see cref="StudentService" /> class, ensuring its methods
///     function as expected under various scenarios.
/// </summary>
/// <remarks>
///     This class includes tests for adding, deleting, retrieving, and updating students,
///     as well as verifying the behavior of the <see cref="StudentService" /> methods
///     with mock dependencies.
/// </remarks>
public class StudentServiceUnitTest
{
    private readonly StudentRepositoryMock studentRepositoryMock;
    private readonly StudentService studentService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="StudentServiceUnitTest" /> class.
    ///     Sets up mock dependencies and initializes the <see cref="StudentService" /> instance
    ///     for unit testing purposes.
    /// </summary>
    public StudentServiceUnitTest()
    {
        studentRepositoryMock = new StudentRepositoryMock();
        studentService = new StudentService(studentRepositoryMock);
    }

    [Theory]
    [ClassData(typeof(UpdateStudentDataClass))]
    public async Task AddAsync_WithValidStudentBindingModel_AddsStudent(StudentBindingModel newStudentValue)
    {
        // Arrange
        var initialStudents = await InitializeStudentsAsync();
        var initialCount = initialStudents.Count;
        StudentBindingModel newStudent = new()
        {
            Id = Guid.NewGuid(),
            FirstName = newStudentValue.FirstName,
            LastName = newStudentValue.LastName,
            Email = newStudentValue.Email,
            Credits = newStudentValue.Credits,
            StudentNumber = newStudentValue.StudentNumber,
            DegreeProgram = new DegreeProgramBindingModel
            {
                Id = newStudentValue.DegreeProgram!.Id,
                Name = newStudentValue.DegreeProgram.Name
            },
            University = new UniversityBindingModel
            {
                Id = newStudentValue.University!.Id,
                Name = newStudentValue.University.Name
            }
        };

        // Act
        await studentService.AddAsync(newStudent);
        var updatedStudents = await InitializeStudentsAsync();

        // Assert
        updatedStudents.Should().NotBeNull();
        updatedStudents.Count.Should().Be(initialCount + 1);
        var addedStudent = updatedStudents.FirstOrDefault(s => s.Id == newStudent.Id);
        addedStudent.Should().NotBeNull();
        addedStudent.FirstName.Should().Be(newStudent.FirstName);
        addedStudent.LastName.Should().Be(newStudent.LastName);
        addedStudent.Email.Should().Be(newStudent.Email);
        addedStudent.Credits.Should().Be(newStudent.Credits);
        addedStudent.StudentNumber.Should().Be(newStudent.StudentNumber);
        addedStudent.DegreeProgramId.Should().Be(newStudent.DegreeProgram.Id);
        addedStudent.UniversityId.Should().Be(newStudent.University.Id);
    }

    /// <summary>
    ///     Tests the <see cref="StudentService.DeleteAsync(Guid)" /> method to ensure that a student is removed
    ///     when a valid student ID is provided.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the student count decreases by one and that the deleted student
    ///     is no longer present in the repository.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous test operation.
    /// </returns>
    [Fact]
    public async Task DeleteAsync_WithValidId_RemovesStudent()
    {
        // Arrange
        var students = await InitializeStudentsAsync();
        var studentIdToDelete = students.First().Id;
        var initialCount = students.Count;

        // Act
        await studentService.DeleteAsync(studentIdToDelete);
        var remainingStudents = await InitializeStudentsAsync();

        // Assert
        remainingStudents.Should().NotBeNull();
        remainingStudents.Count.Should().Be(initialCount - 1);
        remainingStudents.Should().NotContain(s => s.Id == studentIdToDelete);
    }

    /// <summary>
    ///     Tests the <see cref="StudentService.GetAllAsync" /> method to ensure it retrieves all students correctly.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the <see cref="StudentService" /> correctly interacts with its dependencies,
    ///     including <see cref="StudentRepositoryMock" />, <see cref="DegreeProgramServiceMock" />,
    ///     <see cref="UniversityServiceMock" />, and <see cref="GradeServiceMock" />, to return a non-null list of students.
    /// </remarks>
    /// <seealso cref="StudentService" />
    /// <seealso cref="StudentRepositoryMock" />
    /// <seealso cref="DegreeProgramServiceMock" />
    /// <seealso cref="UniversityServiceMock" />
    /// <seealso cref="GradeServiceMock" />
    [Fact]
    public async Task GetAllAsync_WithMockData_ReturnsAllStudents()
    {
        // -- Act
        var result = await studentService.GetAllAsync();

        // -- Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Count.Should().Be(5);

        Assert.All(result, student =>
        {
            student.Id.Should().NotBe(Guid.Empty);
            student.FirstName.Should().NotBeNull();
            student.LastName.Should().NotBeNull();
            student.DegreeProgram.Should().NotBeNull();
            student.University.Should().NotBeNull();
        });
    }

    /// <summary>
    ///     Tests the <see cref="StudentService.GetAllStudentListModels" /> method to ensure it retrieves
    ///     a collection of <see cref="StudentListModel" /> objects when valid student data is provided.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the method:
    ///     - Returns a non-null result.
    ///     - Returns a non-empty collection.
    ///     - Matches the count of the input student data.
    ///     - Maps all properties of each <see cref="Student" /> to the corresponding <see cref="StudentListModel" />
    ///     correctly.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task GetAllStudentListModelsAsync_WithValidStudents_ReturnsStudent()
    {
        // -- Arrange
        var students = await InitializeStudentsAsync();
        var studentListModels = students.Select(ConvertToListModel).ToList();

        // -- Act
        var result = await studentService.GetAllStudentListModels();

        // -- Assert
        result.Should().NotBeNull();
        Assert.NotEmpty(result);
        Assert.Equal(students.Count, result.Count);

        for (var i = 0; i < result.Count; i++)
        {
            Assert.Equal(studentListModels[i].Id, result[i].Id);
            Assert.Equal(studentListModels[i].FirstName, result[i].FirstName);
            Assert.Equal(studentListModels[i].LastName, result[i].LastName);
            Assert.Equal(studentListModels[i].Email, result[i].Email);
            Assert.Equal(studentListModels[i].Credits, result[i].Credits);
            Assert.Equal(studentListModels[i].StudentNumber, result[i].StudentNumber);
            Assert.NotNull(result[i].DegreeProgramListModel);
            Assert.Equal(studentListModels[i].DegreeProgramListModel.Id, result[i].DegreeProgramListModel.Id);
            Assert.Equal(studentListModels[i].DegreeProgramListModel.Name, result[i].DegreeProgramListModel.Name);
        }

        return;

        // <summary>
        //     Converts a Student entity to a StudentBindingModel.
        // </summary>
        // <param name="student"></param>
        // <returns></returns>
        StudentListModel ConvertToListModel(Student student)
        {
            return new StudentListModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Credits = student.Credits,
                StudentNumber = student.StudentNumber,
                DegreeProgramListModel = new DegreeProgramListModel
                {
                    Id = student.DegreeProgram.Id,
                    Name = student.DegreeProgram.Name!
                }
            };
        }
    }

    /// <summary>
    ///     Tests the <see cref="StudentService.GetByIdAsync(Guid)" /> method to ensure it retrieves a
    ///     <see cref="StudentBindingModel" /> when provided with a valid student ID.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the method:
    ///     <list type="bullet">
    ///         <item>Returns a non-null <see cref="StudentBindingModel" />.</item>
    ///         <item>Returns a <see cref="StudentBindingModel" /> with a matching <see cref="BaseKeyModel.Id" />.</item>
    ///         <item>Ensures all required properties of the <see cref="StudentBindingModel" /> are populated.</item>
    ///     </list>
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous unit test operation.
    /// </returns>
    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsStudent()
    {
        // Arrange
        var validId = (await InitializeStudentsAsync()).First().Id;
        // Act
        var result = await studentService.GetByIdAsync(validId);
        // Assert
        result.Should().NotBeNull();
        Assert.Equal(validId, result.Id);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotNull(result.FirstName);
        Assert.NotNull(result.LastName);
        Assert.NotNull(result.DegreeProgram);
        Assert.NotNull(result.University);
    }

    [Theory]
    [ClassData(typeof(UpdateStudentDataClass))]
    public async Task UpdateAsync_WithValidStudent_UpdatesStudent(StudentBindingModel updateData)
    {
        // Arrange
        var students = await InitializeStudentsAsync();
        var studentToUpdate = students.First();
        StudentBindingModel updatedStudentBindingModel = new()
        {
            Id = studentToUpdate.Id,
            FirstName = updateData.FirstName,
            LastName = updateData.LastName,
            Email = updateData.Email,
            Credits = updateData.Credits,
            StudentNumber = updateData.StudentNumber,
            DegreeProgram = new DegreeProgramBindingModel
            {
                Id = updateData.DegreeProgram!.Id,
                Name = updateData.DegreeProgram.Name
            },
            University = new UniversityBindingModel
            {
                Id = updateData.University!.Id,
                Name = updateData.University.Name
            }
        };

        // Act
        await studentService.UpdateAsync(updatedStudentBindingModel);
        var updatedStudent = await studentRepositoryMock.GetByIdAsync(studentToUpdate.Id);

        // Assert
        updatedStudent.Should().NotBeNull();
        updatedStudent.Id.Should().Be(studentToUpdate.Id);
        updatedStudent.FirstName.Should().Be(updatedStudentBindingModel.FirstName);
        updatedStudent.LastName.Should().Be(updatedStudentBindingModel.LastName);
        updatedStudent.Email.Should().Be(updatedStudentBindingModel.Email);
        updatedStudent.Credits.Should().Be(updatedStudentBindingModel.Credits);
        updatedStudent.StudentNumber.Should().Be(updatedStudentBindingModel.StudentNumber);
        updatedStudent.DegreeProgramId.Should().Be(updatedStudentBindingModel.DegreeProgram!.Id);
        updatedStudent.UniversityId.Should().Be(updatedStudentBindingModel.University!.Id);
    }

    private async Task<List<Student>> InitializeStudentsAsync()
    {
        return await studentRepositoryMock.GetAllAsync();
    }
}

/// <summary>
///     Provides a collection of test data for updating student information.
/// </summary>
/// <remarks>
///     This class inherits from <see cref="Xunit.TheoryData{T}" /> and is used to supply
///     multiple <see cref="StudentAdministrationServices.Models.StudentBindingModel" /> instances
///     as test data for parameterized unit tests.
/// </remarks>
public class UpdateStudentDataClass : TheoryData<StudentBindingModel>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UpdateStudentDataClass" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor populates the test data collection with predefined instances of
    ///     <see cref="StudentAdministrationServices.Models.StudentBindingModel" />. Each instance represents
    ///     a student with specific attributes such as first name, last name, email, credits, and student number.
    /// </remarks>
    public UpdateStudentDataClass()
    {
        Add(new StudentBindingModel
        {
            FirstName = "Max",
            LastName = "Mustermann",
            Email = "max@example.com",
            Credits = 60,
            StudentNumber = 100001,
            DegreeProgram = new DegreeProgramBindingModel
            {
                Id = Guid.NewGuid(),
                Name = "Informatik"
            },
            University = new UniversityBindingModel
            {
                Id = Guid.NewGuid(),
                Name = "Musteruniversität"
            }
        });

        Add(new StudentBindingModel
        {
            FirstName = "Anna",
            LastName = "Schmidt",
            Email = "anna.neu@example.com",
            Credits = 120,
            StudentNumber = 100002,
            DegreeProgram = new DegreeProgramBindingModel
            {
                Id = Guid.NewGuid(),
                Name = "Informatik"
            },
            University = new UniversityBindingModel
            {
                Id = Guid.NewGuid(),
                Name = "Musteruniversität"
            }
        });

        Add(new StudentBindingModel
        {
            FirstName = "Lisa",
            LastName = "Bauer",
            Email = "lisa@example.com",
            Credits = 0,
            StudentNumber = 100003,
            DegreeProgram = new DegreeProgramBindingModel
            {
                Id = Guid.NewGuid(),
                Name = "Informatik"
            },
            University = new UniversityBindingModel
            {
                Id = Guid.NewGuid(),
                Name = "Musteruniversität"
            }
        });

        Add(new StudentBindingModel
        {
            FirstName = "Tom",
            LastName = "Weber",
            Email = "tom@example.com",
            Credits = 200,
            StudentNumber = 100004,
            DegreeProgram = new DegreeProgramBindingModel
            {
                Id = Guid.NewGuid(),
                Name = "Informatik"
            },
            University = new UniversityBindingModel
            {
                Id = Guid.NewGuid(),
                Name = "Musteruniversität"
            }
        });
    }
}