using StudentAdministrationDatabase.Models;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministrationServices.Services;

/// <summary>
///     Provides services for managing university entities.
/// </summary>
public class UniversityService : IUniversityService
{
    private readonly IUniversityRepository universityRepository;

    /// <summary>
    ///     UniversityService constructor.
    /// </summary>
    /// <param name="universityRepository"></param>
    public UniversityService(IUniversityRepository universityRepository)
    {
        this.universityRepository = universityRepository;
    }

    /// <summary>
    ///     Retrieves all university entities and maps them to binding models.
    /// </summary>
    /// <remarks>
    ///     This method fetches all universities from the repository, converts them into
    ///     <see cref="UniversityBindingModel" /> instances, and returns the resulting list.
    /// </remarks>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains a list of
    ///     <see cref="UniversityBindingModel" /> objects representing the universities.
    /// </returns>
    public async Task<List<UniversityBindingModel>> GetAllAsync()
    {
        List<University> universities = await this.universityRepository.GetAllAsync();
        List<UniversityBindingModel> bindingModels = [];
        foreach (University university in universities)
        {
            UniversityBindingModel bindingModel = new()
                                                  {
                                                      Id = university.Id,
                                                      Name = university.Name!
                                                  };
            bindingModels.Add(bindingModel);
        }

        return bindingModels;
    }

    /// <summary>
    ///     Retrieves a university entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the university to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains
    ///     a <see cref="UniversityBindingModel" /> representing the university entity.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the provided <paramref name="id" /> is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the university entity is not found.</exception>
    public async Task<UniversityBindingModel> GetByIdAsync(Guid id)
    {
        University university = await this.universityRepository.GetByIdAsync(id);

        {
            return new UniversityBindingModel
                   {
                       Id = university.Id,
                       Name = university.Name!
                   };
        }
    }

    /// <summary>
    ///     Converts a <see cref="UniversityBindingModel" /> instance to a <see cref="University" /> entity.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="UniversityBindingModel" /> instance to convert.
    /// </param>
    /// <returns>
    ///     A <see cref="University" /> entity representing the converted data from the provided binding model.
    /// </returns>
    private University ConvertTo(UniversityBindingModel value)
    {
        return new University
               {
                   Id = value.Id,
                   Name = value.Name
               };
    }
}