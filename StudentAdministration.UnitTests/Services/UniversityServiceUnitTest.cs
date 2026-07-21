using FluentAssertions;
using StudentAdministrationServices.Services;
using StudentAdministrationTests.MockData.Repository;

namespace StudentAdministrationTests.Services;

public class UniversityServiceUnitTest
{
    private readonly UniversityRepositroyMock universityRepositroyMock = new();
    private readonly UniversityService universityService;

    public UniversityServiceUnitTest()
    {
        universityService = new UniversityService(universityRepositroyMock);
    }

    /// <summary>
    ///     Unit test for verifying the behavior of the <see cref="UniversityService.GetAllAsync" /> method
    ///     when provided with valid input.
    /// </summary>
    /// <remarks>
    ///     This test ensures that the <see cref="UniversityService.GetAllAsync" /> method correctly retrieves
    ///     and maps all university entities from the repository to the binding models.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation of the unit test.
    /// </returns>
    [Fact]
    public async Task GetAllAsync_ValidInput_CorrectOutcome()
    {
        // Arrange
        var expectedUniversities = await universityRepositroyMock.GetAllAsync();

        // Act
        var actualUniversities = await universityService.GetAllAsync();
        // Assert
        actualUniversities.Should().NotBeNull();
        for (var i = 0; i < expectedUniversities.Count; i++)
        {
            actualUniversities[i].Id.Should().Be(expectedUniversities[i].Id);
            actualUniversities[i].Name.Should().Be(expectedUniversities[i].Name);
        }
    }

    /// <summary>
    ///     Tests the <see cref="UniversityService.GetByIdAsync(Guid)" /> method to ensure it retrieves the correct university
    ///     when provided with a valid university ID.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the <see cref="UniversityService.GetByIdAsync(Guid)" /> method correctly maps the
    ///     <see cref="StudentAdministrationDatabase.Models.University" /> entity to a
    ///     <see cref="StudentAdministrationServices.Models.UniversityBindingModel" /> and returns the expected result.
    /// </remarks>
    /// <returns>
    ///     A task representing the asynchronous test operation.
    /// </returns>
    [Fact]
    public async Task GetByIdAsync_ValidInput_CorrectOutcome()
    {
        // Arrange
        var expectedUniversity = (await universityRepositroyMock.GetAllAsync()).First();
        // Act
        var actualUniversity = await universityService.GetByIdAsync(expectedUniversity.Id);
        // Assert
        actualUniversity.Should().NotBeNull();
        actualUniversity.Id.Should().Be(expectedUniversity.Id);
        actualUniversity.Name.Should().Be(expectedUniversity.Name);
    }
}