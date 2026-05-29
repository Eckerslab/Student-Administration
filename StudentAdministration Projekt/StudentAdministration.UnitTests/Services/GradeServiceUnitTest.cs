using FluentAssertions;
using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services;
using StudentAdministrationTests.MockData.Repository;
using StudentAdministrationTests.MockData.Services;

namespace StudentAdministrationTests.Services;

public class GradeServiceUnitTest
{
    private readonly CourseServiceMock courseServiceMock = new();
    private readonly GradeRepositoryMock gradeRepositoryMock = new();
    private readonly GradeService gradeService;
    private readonly StudentServiceMock studentServiceMock = new();

    public GradeServiceUnitTest()
    {
        this.gradeService = new GradeService(this.gradeRepositoryMock, this.courseServiceMock);
    }

    /// <summary>
    ///     Verifies that adding a valid <see cref="Grade" /> object to the repository
    ///     results in the correct outcome.
    /// </summary>
    /// <remarks>
    ///     This test ensures that the count of grades in the repository increases by one
    ///     and that the newly added grade is present in the repository.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation of the test.
    /// </returns>
    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(0)]
    public async Task AddAsync_ValidInput_CorrectOutcome(int value)
    {
        //--Arrange
        Grade grade = new()
                      {
                          CourseId = Guid.NewGuid(),
                          StudentId = Guid.NewGuid(),
                          Value = value,
                          Id = Guid.NewGuid()
                      };
        List<Grade> grades = await this.gradeRepositoryMock.GetAllAsync();
        int countBefore = grades.Count;
        //--Act
        await this.gradeService.AddAsync(grade);
        List<Grade> gradesAfter = await this.gradeRepositoryMock.GetAllAsync();
        int countAfter = gradesAfter.Count;
        //--Assert
        countAfter.Should().Be(countBefore + 1);
        gradesAfter.Should().ContainEquivalentOf(grade);
    }

    /// <summary>
    ///     Validates the deletion of a grade using a valid identifier.
    /// </summary>
    /// <remarks>
    ///     This test ensures that when a valid grade identifier is provided, the grade is successfully removed
    ///     from the repository, and the count of grades decreases by one.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation of the test.
    /// </returns>
    [Fact]
    public async Task DeleteAsync_ValidInput_CorrectOutcome()
    {
        //--Arrange
        Grade grade = (await this.gradeRepositoryMock.GetAllAsync()).First();

        List<Grade> gradesBefore = await this.gradeRepositoryMock.GetAllAsync();
        int countBefore = gradesBefore.Count;

        //--Act
        await this.gradeService.DeleteAsync(grade.Id);
        List<Grade> gradesAfter = await this.gradeRepositoryMock.GetAllAsync();
        int countAfter = gradesAfter.Count;

        //--Assert
        countAfter.Should().Be(countBefore - 1);
        gradesAfter.Should().NotContainEquivalentOf(grade);
    }

    /// <summary>
    ///     Validates that the <see cref="GradeService.GetAllAsync" /> method retrieves all grades correctly.
    /// </summary>
    /// <remarks>
    ///     This unit test ensures that the number of grades retrieved matches the expected count
    ///     and verifies the correctness of the <see cref="GradeService.GetAllAsync" /> implementation.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation of the unit test.
    /// </returns>
    [Fact]
    public async Task GetAllAsync_ValidInput_CorrectOutcome()
    {
        //--Arrange
        List<Grade> existingGrades = await this.gradeRepositoryMock.GetAllAsync();

        //--Act
        List<Grade> allGrades = await this.gradeService.GetAllAsync();

        //--Assert
        allGrades.Count.Should().Be(existingGrades.Count);
    }

    /// <summary>
    ///     Validates that the <see cref="GradeService.GetByIdAsync(Guid)" /> method correctly retrieves a
    ///     <see cref="Grade" /> entity when provided with a valid identifier.
    /// </summary>
    /// <remarks>
    ///     This test ensures that the retrieved <see cref="Grade" /> entity matches the expected values,
    ///     including its <see cref="Grade.Id" />, <see cref="Grade.CourseId" />, <see cref="Grade.StudentId" />,
    ///     and <see cref="Grade.Value" />.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous unit test operation.
    /// </returns>
    [Fact]
    public async Task GetByIdAsync_ValidInput_CorrectOutcome()
    {
        //--Arrange
        Guid gradeId = Guid.NewGuid();
        Grade grade = new()
                      {
                          Id = gradeId,
                          CourseId = Guid.NewGuid(),
                          StudentId = Guid.NewGuid(),
                          Value = 5
                      };
        await this.gradeRepositoryMock.AddAsync(grade);

        //--Act
        Grade retrievedGrade = await this.gradeService.GetByIdAsync(gradeId);

        //--Assert
        retrievedGrade.Should().NotBeNull();
        retrievedGrade.Id.Should().Be(gradeId);
        retrievedGrade.CourseId.Should().Be(grade.CourseId);
        retrievedGrade.StudentId.Should().Be(grade.StudentId);
        retrievedGrade.Value.Should().Be(grade.Value);
    }

    /// <summary>
    ///     Tests the <see cref="GradeService.GetByStudentIdAsync(Guid)" /> method to ensure it retrieves the correct grades
    ///     for a given student when provided with a valid student ID.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the grades returned by the service match the expected grades associated with the student.
    /// </remarks>
    /// <returns>
    ///     A task representing the asynchronous operation of the test.
    /// </returns>
    [Fact]
    public async Task GetByStudentIdAsync_ValidInput_CorrectOutcome()
    {
        //--Arrange
        Guid studentId = (await this.gradeRepositoryMock.GetAllAsync()).First().StudentId;
        StudentBindingModel student = await this.studentServiceMock.GetByIdAsync(studentId);
        //--Act
        List<GradeBindingModel> studentGrades = await this.gradeService.GetByStudentIdAsync(studentId);

        //--Assert
        for (int i = 0; i < studentGrades.Count; i++)
        {
            studentGrades[i].Id.Should().Be(student.Grades![i].Id);
        }
    }

    /// <summary>
    ///     Unit test for verifying the behavior of the <see cref="GradeService.UpdateAsync" /> method
    ///     when provided with valid input.
    /// </summary>
    /// <remarks>
    ///     This test ensures that the grade is correctly updated in the repository, including its
    ///     properties such as <see cref="Grade.CourseId" />, <see cref="Grade.StudentId" />, and
    ///     <see cref="Grade.Value" />.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation of the unit test.
    /// </returns>
    [Fact]
    public async Task UpdateAsync_ValidInput_CorrectOutcome()
    {
        //--Arrange
        Guid gradeId = Guid.NewGuid();
        Grade originalGrade = new()
                              {
                                  Id = gradeId,
                                  CourseId = Guid.NewGuid(),
                                  StudentId = Guid.NewGuid(),
                                  Value = 5
                              };
        await this.gradeRepositoryMock.AddAsync(originalGrade);

        Grade updatedGrade = new()
                             {
                                 Id = gradeId,
                                 CourseId = Guid.NewGuid(),           // New CourseId
                                 StudentId = originalGrade.StudentId, // Same StudentId
                                 Value = 6                            // New Value
                             };

        //--Act
        await this.gradeService.UpdateAsync(updatedGrade);
        Grade retrievedUpdatedGrade = await this.gradeRepositoryMock.GetByIdAsync(gradeId);

        //--Assert
        retrievedUpdatedGrade.Should().NotBeNull();
        retrievedUpdatedGrade.Id.Should().Be(gradeId);
        retrievedUpdatedGrade.CourseId.Should().Be(updatedGrade.CourseId);
        retrievedUpdatedGrade.StudentId.Should().Be(updatedGrade.StudentId);
        retrievedUpdatedGrade.Value.Should().Be(updatedGrade.Value);
    }
}