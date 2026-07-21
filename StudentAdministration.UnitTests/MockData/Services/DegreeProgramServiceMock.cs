using System.Collections.ObjectModel;
using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;
using StudentAdministrationTests.MockData.Repository;

namespace StudentAdministrationTests.MockData.Services;

internal class DegreeProgramServiceMock : IDegreeProgramService
{
    private readonly DegreeProgramRepositoryMock degreeProgramRepositoryMock = new();

    public async Task<ObservableCollection<DegreeProgramListModel>> GetAllDegreeProgramListModels()
    {
        var degreePrograms = await degreeProgramRepositoryMock.GetAllAsync();
        ObservableCollection<DegreeProgramListModel> listModels = new();
        foreach (var degreeProgram in degreePrograms)
        {
            DegreeProgramListModel listModel = new()
            {
                Id = degreeProgram.Id,
                Name = degreeProgram.Name!
            };
            listModels.Add(listModel);
        }

        return listModels;
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
        var degreePrograms = await degreeProgramRepositoryMock.GetAllAsync();
        List<DegreeProgramBindingModel> bindingModels = new();
        foreach (var degreeProgram in degreePrograms)
        {
            var bindingModel = ConvertTo(degreeProgram);

            bindingModels.Add(bindingModel);
        }

        return bindingModels;
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
        var degreeProgram = await degreeProgramRepositoryMock.GetByIdAsync(id);
        var degreeProgramBindingModel = ConvertTo(degreeProgram);

        return degreeProgramBindingModel;
    }

    /// <summary>
    ///     Converts a <see cref="DegreeProgram" /> entity to a <see cref="DegreeProgramBindingModel" />.
    /// </summary>
    /// <param name="degreeProgram">
    ///     The <see cref="DegreeProgram" /> entity to be converted.
    /// </param>
    /// <returns>
    ///     A <see cref="DegreeProgramBindingModel" /> representing the converted degree program.
    /// </returns>
    private DegreeProgramBindingModel ConvertTo(DegreeProgram degreeProgram)
    {
        return new DegreeProgramBindingModel
        {
            Id = degreeProgram.Id,
            Name = degreeProgram.Name!
        };
    }
}