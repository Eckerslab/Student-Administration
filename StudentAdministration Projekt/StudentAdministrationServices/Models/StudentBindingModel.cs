using CommunityToolkit.Mvvm.ComponentModel;

namespace StudentAdministrationServices.Models;

public partial class StudentBindingModel : ObservableObject
{
    [ObservableProperty]
    private int credits;

    [ObservableProperty]
    private DegreeProgramBindingModel? degreeProgram;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private List<GradeBindingModel>? grades = [];

    [ObservableProperty]
    private Guid id;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private int studentNumber;

    [ObservableProperty]
    private UniversityBindingModel? university;
}