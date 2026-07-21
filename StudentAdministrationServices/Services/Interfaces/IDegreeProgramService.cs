using System.Collections.ObjectModel;
using StudentAdministrationServices.Models;

namespace StudentAdministrationServices.Services.Interfaces;

/// <summary>
///     Interface for degree program service operations.
/// </summary>
public interface IDegreeProgramService : IReceiveService<DegreeProgramBindingModel>
{
    Task<ObservableCollection<DegreeProgramListModel>> GetAllDegreeProgramListModels();
}