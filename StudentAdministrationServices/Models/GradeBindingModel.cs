using CommunityToolkit.Mvvm.ComponentModel;
using StudentAdministrationDatabase.Models;

namespace StudentAdministrationServices.Models;

public partial class GradeBindingModel : ObservableObject
{
    [ObservableProperty] private CourseBindingModel? course;

    [ObservableProperty] private Guid id;

    [ObservableProperty] private int note;

    /// <summary>
    ///     Gets a value indicating whether this grade is a passing result,
    ///     as defined by the centralized <see cref="GradingScale" />.
    /// </summary>
    public bool IsPassing => GradingScale.IsPassing(Note);
}