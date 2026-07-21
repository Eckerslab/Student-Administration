using CommunityToolkit.Mvvm.ComponentModel;

namespace StudentAdministrationServices.Models;

public partial class StudentListModel : ObservableObject
{
    [ObservableProperty] private int credits;

    [ObservableProperty] private DegreeProgramListModel degreeProgramListModel = new();

    [ObservableProperty] private string? email;

    [ObservableProperty] private string? firstName;

    [ObservableProperty] private Guid id;

    [ObservableProperty] private string? lastName;

    [ObservableProperty] private int studentNumber;
}