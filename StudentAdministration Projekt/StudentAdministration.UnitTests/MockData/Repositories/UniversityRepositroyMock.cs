using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationDatabase.SampleData;
using StudentAdministrationTests.MockData.DataSource;

namespace StudentAdministrationTests.MockData.Repository;

/// <summary>
///     Represents a mock implementation of the <see cref="IUniversityRepository" /> interface for testing purposes.
/// </summary>
/// <remarks>
///     This class provides a predefined list of sample universities for testing, initialized using
///     <see cref="SampleUniversities.GetUniversities" />. It is intended for use in unit tests to simulate
///     repository behavior without requiring a database connection.
/// </remarks>
internal class UniversityRepositroyMock : IUniversityRepository
{
    private readonly List<University> universities;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UniversityRepositroyMock" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor initializes the repository with a predefined list of sample universities
    ///     retrieved from <see cref="SampleUniversities.GetUniversities" />.
    /// </remarks>
    public UniversityRepositroyMock()
    {
        this.universities = SampleUniversitiesMock.GetUniversities().ToList();
    }

    /// <summary>
    ///     Adds a new <see cref="University" /> entity to the repository.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="University" /> entity to add. Must not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="value" /> is <c>null</c>.
    /// </exception>
    public Task AddAsync(University value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        this.universities.Add(value);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Deletes a university entity from the repository asynchronously based on the specified unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the university entity to be deleted.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous delete operation.
    /// </returns>
    /// <remarks>
    ///     If a university entity with the specified <paramref name="id" /> exists in the repository, it will be removed.
    ///     If no such entity exists, the method completes without making any changes.
    /// </remarks>
    public Task DeleteAsync(Guid id)
    {
        University? university = this.universities.FirstOrDefault(u => u.Id == id);
        if (university != null)
        {
            this.universities.Remove(university);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Retrieves all universities from the repository.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="StudentAdministrationDatabase.Models.University" /> objects.
    /// </returns>
    public Task<List<University>> GetAllAsync()
    {
        return Task.FromResult(this.universities);
    }

    /// <summary>
    ///     Retrieves a university entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the university to retrieve.</param>
    /// <returns>
    ///     A <see cref="University" /> object that matches the specified identifier.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if no university with the specified identifier is found.
    /// </exception>
    /// <remarks>
    ///     This method searches the in-memory list of universities for a match based on the provided identifier.
    /// </remarks>
    public async Task<University> GetByIdAsync(Guid id)
    {
        return await Task.FromResult(this.universities.FirstOrDefault(u => u.Id == id)) ?? throw new InvalidOperationException();
    }
}