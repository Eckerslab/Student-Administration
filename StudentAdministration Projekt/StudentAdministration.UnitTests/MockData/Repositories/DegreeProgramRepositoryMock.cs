using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationTests.MockData.DataSource;

namespace StudentAdministrationTests.MockData.Repository;

internal class DegreeProgramRepositoryMock : IDegreeProgramRepository
{
    private readonly List<DegreeProgram> degreePrograms;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DegreeProgramRepositoryMock" /> class.
    /// </summary>
    /// <remarks>
    ///     This constructor sets up an internal list to manage <see cref="DegreeProgram" /> entities.
    /// </remarks>
    public DegreeProgramRepositoryMock()
    {
        this.degreePrograms = new List<DegreeProgram>();
        this.degreePrograms = SampleDegreeProgramMock.GetDegreePrograms();
    }

    /// <summary>
    ///     Asynchronously adds a new <see cref="DegreeProgram" /> to the repository.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="DegreeProgram" /> instance to add. Must not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when the <paramref name="value" /> is <c>null</c>.
    /// </exception>
    public Task AddAsync(DegreeProgram value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "The degree program cannot be null.");
        }

        this.degreePrograms.Add(value);
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Deletes a <see cref="DegreeProgram" /> entity with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="DegreeProgram" /> to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the provided <paramref name="id" /> is an empty GUID or if no matching entry is found.
    /// </exception>
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("The provided ID cannot be an empty GUID.", nameof(id));
        }

        DegreeProgram? degreeProgram = this.degreePrograms.SingleOrDefault(x => x.Id == id);
        if (degreeProgram == null)
        {
            throw new ArgumentException("Entry not found!");
        }

        this.degreePrograms.Remove(degreeProgram);
        await Task.CompletedTask;
    }

    /// <summary>
    ///     Retrieves all <see cref="DegreeProgram" /> entities managed by the repository.
    /// </summary>
    /// <remarks>
    ///     This method asynchronously returns a list of all <see cref="DegreeProgram" /> objects
    ///     currently stored in the repository.
    /// </remarks>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains a list of
    ///     <see cref="DegreeProgram" /> objects.
    /// </returns>
    public async Task<List<DegreeProgram>> GetAllAsync()
    {
        return await Task.FromResult(this.degreePrograms);
    }

    /// <summary>
    ///     Retrieves a <see cref="DegreeProgram" /> entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the <see cref="DegreeProgram" /> to retrieve.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the
    ///     <see cref="DegreeProgram" /> entity corresponding to the specified <paramref name="id" />.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the provided <paramref name="id" /> is an empty GUID or if no matching
    ///     <see cref="DegreeProgram" /> entity is found.
    /// </exception>
    public Task<DegreeProgram> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("The provided ID is invalid.");
        }

        DegreeProgram? degreeProgram = this.degreePrograms.SingleOrDefault(dp => dp.Id == id);
        if (degreeProgram == null)
        {
            throw new ArgumentException("Entry not found!");
        }

        return Task.FromResult(degreeProgram);
    }
}