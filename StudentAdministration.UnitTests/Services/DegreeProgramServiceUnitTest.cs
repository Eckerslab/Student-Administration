using FluentAssertions;
using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services;
using StudentAdministrationTests.MockData.Repository;

namespace StudentAdministrationTests.Services;

/// <summary>
///     Provides unit tests for the <see cref="StudentAdministrationServices.Services.DegreeProgramService" /> class.
/// </summary>
/// <remarks>
///     This class contains test methods to verify the behavior and correctness of the
///     <see cref="StudentAdministrationServices.Services.DegreeProgramService" /> methods, ensuring that they function
///     as expected under various scenarios.
/// </remarks>
public class DegreeProgramServiceUnitTest
{
    private readonly DegreeProgramRepositoryMock degreeProgramRepositoryMock = new();
    private readonly DegreeProgramService degreeProgramService;

    public DegreeProgramServiceUnitTest()
    {
        degreeProgramService = new DegreeProgramService(degreeProgramRepositoryMock);
    }

    /// <summary>
    ///     Verifies that the GetAllAsync method of DegreeProgramService returns a list of degree programs with correct data
    ///     when valid data is provided.
    /// </summary>
    /// <remarks>
    ///     This test ensures that the number of returned degree programs and their properties match the
    ///     data retrieved from the repository. It is intended to validate correct mapping and retrieval behavior for valid
    ///     input scenarios.
    /// </remarks>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Fact]
    public async Task GetAllAsync_ValidData_CorrectOutput()
    {
        // Arrange
        var degreePrograms = await degreeProgramRepositoryMock.GetAllAsync();
        // Act
        var result = await degreeProgramService.GetAllAsync();
        // Assert
        result.Count.Should().Be(degreePrograms.Count);
        for (var i = 0; i < result.Count; i++)
        {
            result[i].Id.Should().Be(degreePrograms[i].Id);
            result[i].Name.Should().Be(degreePrograms[i].Name);
        }
    }

    /// <summary>
    ///     Tests the <see cref="DegreeProgramService.GetAllDegreeProgramListModels" /> method to ensure it retrieves
    ///     a list of degree program models correctly when provided with valid data.
    /// </summary>
    /// <remarks>
    ///     This unit test verifies that the number of degree programs retrieved matches the expected count
    ///     and that the properties of each degree program are correctly mapped.
    /// </remarks>
    /// <returns>
    ///     A task representing the asynchronous operation of the test.
    /// </returns>
    [Fact]
    public async Task GetAllDegreeProgramListModels_ValidData_CorrectOutput()
    {
        // Arrange
        var degreePrograms = await degreeProgramRepositoryMock.GetAllAsync();

        // Act
        var result = await degreeProgramService.GetAllDegreeProgramListModels();

        // Assert
        result.Count.Should().Be(degreePrograms.Count);
        for (var i = 0; i < result.Count; i++)
        {
            result[i].Id.Should().Be(degreePrograms[i].Id);
            result[i].Name.Should().Be(degreePrograms[i].Name);
        }
    }

    /// <summary>
    ///     Tests the <see cref="DegreeProgramService.GetByIdAsync(Guid)" /> method to ensure it retrieves
    ///     the correct degree program when provided with valid data.
    /// </summary>
    /// <remarks>
    ///     This test verifies that the <see cref="DegreeProgramService.GetByIdAsync(Guid)" /> method
    ///     correctly maps the retrieved <see cref="DegreeProgram" /> entity to a
    ///     <see cref="DegreeProgramBindingModel" /> and that all properties match the expected values.
    /// </remarks>
    /// <returns>
    ///     A task representing the asynchronous operation of the test.
    /// </returns>
    [Fact]
    public async Task GetByIdAsync_ValidData_CorrectOutput()
    {
        // Arrange
        var degreePrograms = await degreeProgramRepositoryMock.GetAllAsync();
        var expectedDegreeProgram = degreePrograms[0];
        // Act
        var result = await degreeProgramService.GetByIdAsync(expectedDegreeProgram.Id);
        // Assert
        result.Id.Should().Be(expectedDegreeProgram.Id);
        result.Name.Should().Be(expectedDegreeProgram.Name);
    }
}