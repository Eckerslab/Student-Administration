using CommunityToolkit.Mvvm.ComponentModel;

namespace StudentAdministrationServices.Models;

public partial class CourseGradeBindingModel : ObservableObject
{
    [ObservableProperty]
    private string? course;

    [ObservableProperty]
    private string? grade;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CourseGradeBindingModel" /> class with the specified course name and
    ///     grade.
    /// </summary>
    /// <param name="course">The name of the course associated with the grade. Can be <c>null</c>.</param>
    /// <param name="grade">The grade achieved in the course. Can be <c>null</c>.</param>
    public CourseGradeBindingModel(string? course, string? grade)
    {
        this.Course = course;
        this.Grade = grade;
    }
}