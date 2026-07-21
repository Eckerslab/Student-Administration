using CommunityToolkit.Mvvm.ComponentModel;

namespace StudentAdministrationServices.Models;

public partial class UniversityBindingModel : ObservableObject
{
    [ObservableProperty] private Guid id;

    [ObservableProperty] private string name = string.Empty;
}