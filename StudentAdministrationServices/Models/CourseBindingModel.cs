using CommunityToolkit.Mvvm.ComponentModel;

namespace StudentAdministrationServices.Models;

public partial class CourseBindingModel : ObservableObject
{
    [ObservableProperty] private int credit;

    [ObservableProperty] private Guid id;

    [ObservableProperty] private string? name;
}