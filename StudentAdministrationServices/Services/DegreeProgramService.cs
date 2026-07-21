using System.Collections.ObjectModel;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationServices.Mapping;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministrationServices.Services;

/// <summary>
///     DegreeProgramService handles operations related to degree programs within the student administration system.
/// </summary>
public class DegreeProgramService : IDegreeProgramService
{
    private readonly IDegreeProgramRepository degreeProgramRepository;

    /// <summary>
    ///     DegreeProgramService constructor.
    /// </summary>
    /// <param name="degreeProgramRepository">The degree program repository.</param>
    public DegreeProgramService(IDegreeProgramRepository degreeProgramRepository)
    {
        this.degreeProgramRepository = degreeProgramRepository;
    }

    public async Task<ObservableCollection<DegreeProgramListModel>> GetAllDegreeProgramListModels()
    {
        var degreePrograms = await degreeProgramRepository.GetAllAsync();
        return new ObservableCollection<DegreeProgramListModel>(degreePrograms.Select(EntityMappings.ToListModel));
    }

    /// <summary>
    ///     Retrieves all degree programs as a list of <see cref="DegreeProgramBindingModel" />.
    /// </summary>
    /// <remarks>
    ///     This method fetches all degree programs from the repository and converts them into binding models
    ///     for use within the application.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="DegreeProgramBindingModel" /> representing the degree programs.
    /// </returns>
    /// <exception cref="Exception">
    ///     An exception might be thrown if the repository operation fails or if there are issues during the conversion
    ///     process.
    /// </exception>
    public async Task<List<DegreeProgramBindingModel>> GetAllAsync()
    {
        var degreePrograms = await degreeProgramRepository.GetAllAsync();
        return degreePrograms.Select(EntityMappings.ToBindingModel).ToList();
    }

    /// <summary>
    ///     Retrieves a degree program by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the degree program to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     a <see cref="DegreeProgramBindingModel" /> representing the degree program with the specified identifier.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the provided <paramref name="id" /> is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if no degree program is found with the specified <paramref name="id" />.</exception>
    /// <exception cref="Exception">Thrown if an unexpected error occurs during the operation.</exception>
    public async Task<DegreeProgramBindingModel> GetByIdAsync(Guid id)
    {
        var degreeProgram = await degreeProgramRepository.GetByIdAsync(id);
        return degreeProgram.ToBindingModel();
    }
}