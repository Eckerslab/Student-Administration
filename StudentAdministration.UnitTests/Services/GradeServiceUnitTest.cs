using FluentAssertions;
using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Services;
using StudentAdministrationTests.MockData.Repository;
using StudentAdministrationTests.MockData.Services;

namespace StudentAdministrationTests.Services;

public class GradeServiceUnitTest
{
    // --- SaveGradeAsync / DeleteGradeAsync: the credit-recompute choke points ---
    // Sample data: student "Martin Schmid" (StudentMartin) has credits 15 from two passing grades:
    //   course "Programmieren 1" (6 credits, value 2) and course "Programmieren 2" (9 credits, value 1).
    private static readonly Guid StudentMartin = Guid.Parse("96d71862-bb38-4e1f-a3fd-12f43582d35e");
    private static readonly Guid StudentJonas = Guid.Parse("e9fdd9ab-2c76-4778-8992-80e35f3a4a03");
    private static readonly Guid CourseProgrammieren1 = Guid.Parse("8ab3add5-1510-4f2e-8bff-151f3bc33883"); // 6 credits
    private static readonly Guid GradeMartinProgrammieren1 = Guid.Parse("59bcd467-0261-4a04-abe9-3910cb092694");

    private static readonly Guid
        CourseModerneDatenbanken = Guid.Parse("725e835b-733d-43c0-86f8-69392bcb70f0"); // 9 credits

    private readonly GradeRepositoryMock gradeRepositoryMock = new();
    private readonly GradeService gradeService;
    private readonly StudentRepositoryMock studentRepositoryMock = new();
    private readonly StudentServiceMock studentServiceMock = new();

    public GradeServiceUnitTest()
    {
        gradeService = new GradeService(
            gradeRepositoryMock,
            studentRepositoryMock,
            new GradeCalculationService());
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
        var grades = await gradeRepositoryMock.GetAllAsync();
        var countBefore = grades.Count;
        //--Act
        await gradeService.AddAsync(grade);
        var gradesAfter = await gradeRepositoryMock.GetAllAsync();
        var countAfter = gradesAfter.Count;
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
        var grade = (await gradeRepositoryMock.GetAllAsync()).First();

        var gradesBefore = await gradeRepositoryMock.GetAllAsync();
        var countBefore = gradesBefore.Count;

        //--Act
        await gradeService.DeleteAsync(grade.Id);
        var gradesAfter = await gradeRepositoryMock.GetAllAsync();
        var countAfter = gradesAfter.Count;

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
        var existingGrades = await gradeRepositoryMock.GetAllAsync();

        //--Act
        var allGrades = await gradeService.GetAllAsync();

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
        var gradeId = Guid.NewGuid();
        Grade grade = new()
        {
            Id = gradeId,
            CourseId = Guid.NewGuid(),
            StudentId = Guid.NewGuid(),
            Value = 5
        };
        await gradeRepositoryMock.AddAsync(grade);

        //--Act
        var retrievedGrade = await gradeService.GetByIdAsync(gradeId);

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
        var studentId = (await gradeRepositoryMock.GetAllAsync()).First().StudentId;
        var student = await studentServiceMock.GetByIdAsync(studentId);
        //--Act
        var studentGrades = await gradeService.GetByStudentIdAsync(studentId);

        //--Assert
        studentGrades.Should().NotBeEmpty();
        for (var i = 0; i < studentGrades.Count; i++) studentGrades[i].Id.Should().Be(student.Grades![i].Id);
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
        var gradeId = Guid.NewGuid();
        Grade originalGrade = new()
        {
            Id = gradeId,
            CourseId = Guid.NewGuid(),
            StudentId = Guid.NewGuid(),
            Value = 5
        };
        await gradeRepositoryMock.AddAsync(originalGrade);

        Grade updatedGrade = new()
        {
            Id = gradeId,
            CourseId = Guid.NewGuid(), // New CourseId
            StudentId = originalGrade.StudentId, // Same StudentId
            Value = 6 // New Value
        };

        //--Act
        await gradeService.UpdateAsync(updatedGrade);
        var retrievedUpdatedGrade = await gradeRepositoryMock.GetByIdAsync(gradeId);

        //--Assert
        retrievedUpdatedGrade.Should().NotBeNull();
        retrievedUpdatedGrade.Id.Should().Be(gradeId);
        retrievedUpdatedGrade.CourseId.Should().Be(updatedGrade.CourseId);
        retrievedUpdatedGrade.StudentId.Should().Be(updatedGrade.StudentId);
        retrievedUpdatedGrade.Value.Should().Be(updatedGrade.Value);
    }

    [Fact]
    public async Task SaveGradeAsync_UpdatesExistingGrade_RecomputesCredits()
    {
        //--Arrange: Programmieren 1 (6 credits) currently passes; make it fail so only Programmieren 2 (9) counts.
        var gradeCountBefore = (await gradeRepositoryMock.GetAllAsync()).Count;

        //--Act
        await gradeService.SaveGradeAsync(StudentMartin, CourseProgrammieren1, GradingScale.FirstFailingValue);

        //--Assert: no new grade was created and credits were recomputed to 9.
        (await gradeRepositoryMock.GetAllAsync()).Count.Should().Be(gradeCountBefore);
        (await studentRepositoryMock.GetByIdAsync(StudentMartin)).Credits.Should().Be(9);
    }

    [Fact]
    public async Task SaveGradeAsync_CreatesNewGrade_WhenNoneExists_AndRecomputesCredits()
    {
        //--Arrange: Jonas has no grades and 0 credits.
        var gradeCountBefore = (await gradeRepositoryMock.GetAllAsync()).Count;
        (await studentRepositoryMock.GetByIdAsync(StudentJonas)).Credits.Should().Be(0);

        //--Act: award a passing grade for a 9-credit course.
        await gradeService.SaveGradeAsync(StudentJonas, CourseModerneDatenbanken, 1);

        //--Assert
        (await gradeRepositoryMock.GetAllAsync()).Count.Should().Be(gradeCountBefore + 1);
        (await studentRepositoryMock.GetByIdAsync(StudentJonas)).Credits.Should().Be(9);
    }

    [Fact]
    public async Task DeleteGradeAsync_RevertsContributedCredits()
    {
        //--Arrange: Martin starts with 15 credits (6 + 9).
        (await studentRepositoryMock.GetByIdAsync(StudentMartin)).Credits.Should().Be(15);

        //--Act: delete the Programmieren 1 grade (6 credits).
        await gradeService.DeleteGradeAsync(GradeMartinProgrammieren1);

        //--Assert: credits drop to the remaining 9.
        (await studentRepositoryMock.GetByIdAsync(StudentMartin)).Credits.Should().Be(9);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(GradingScale.MaxValue + 1)]
    public async Task SaveGradeAsync_GradeValueOutOfRange_ThrowsAndDoesNotPersist(int invalidValue)
    {
        //--Arrange
        var gradeCountBefore = (await gradeRepositoryMock.GetAllAsync()).Count;

        //--Act
        var act = () => gradeService.SaveGradeAsync(StudentJonas, CourseModerneDatenbanken, invalidValue);

        //--Assert
        await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
        (await gradeRepositoryMock.GetAllAsync()).Count.Should().Be(gradeCountBefore);
    }
}