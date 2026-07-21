using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using StudentAdministrationServices.Models;

namespace StudentAdministration.UI.ViewModel.Interfaces;

public interface IAddEditStudentPageViewModel : IQueryAttributable, IInitialize, ICoreInterface
{
    string AddEditTitle { get; set; }
    IAsyncRelayCommand CancelAddEditStudentCommand { get; }
    ObservableCollection<DegreeProgramListModel> DegreeProgrammBindingModels { get; set; }
    bool FirstNameError { get; set; }
    bool LastNameError { get; set; }
    bool EmailError { get; set; }
    bool DegreeProgramError { get; set; }
    string? EmailErrorText { get; set; }
    string NewEmail { get; }
    string NewFirstName { get; }
    string NewLastName { get; }
    DegreeProgramListModel NewSelectedDegreeProgramm { get; set; }
    IAsyncRelayCommand SaveAddEditStudentCommand { get; }
}