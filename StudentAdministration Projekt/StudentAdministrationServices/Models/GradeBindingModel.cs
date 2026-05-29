using CommunityToolkit.Mvvm.ComponentModel;

namespace StudentAdministrationServices.Models;

public partial class GradeBindingModel : ObservableObject
{
    [ObservableProperty]
    private CourseBindingModel? course;

    [ObservableProperty]
    private Guid id;

    [ObservableProperty]
    private int note;
}