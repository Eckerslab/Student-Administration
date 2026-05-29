using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using StudentAdministrationServices.Models;

namespace StudentAdministration.UI.ViewModel.Interfaces;

public interface IDetailsPageViewModel : IQueryAttributable, IInitialize, ICoreInterface
{
    /// <summary>
    ///     gets the command that closes the details popup asynchronously.
    /// </summary>
    IAsyncRelayCommand CloseDetailsPopupCommand { get; }

    /// <summary>
    ///     Gets or sets the collection of course grades to be displayed on the details page.
    /// </summary>
    /// <remarks>
    ///     Each item in the collection represents a course and its corresponding grade.
    ///     This property is typically used as the data source for UI components, such as a data grid.
    /// </remarks>
    ObservableCollection<CourseGradeBindingModel>? CourseGradeDisplay { get; set; }

    /// <summary>
    ///     Gets or sets the Grade Point Average (GPA) of the student.
    /// </summary>
    /// <remarks>
    ///     This property represents the calculated GPA based on the student's course grades.
    ///     It is displayed in the details page of the student.
    /// </remarks>
    string? GradePointAverage { get; set; }

    /// <summary>
    /// </summary>
    StudentBindingModel Student { get; set; }

    /// <summary>
    ///     Gets or sets the full name of the student.
    /// </summary>
    /// <remarks>
    ///     This property is used for displaying the student's full name in the UI.
    ///     It is bound to the corresponding label in the <c>DetailsPage.xaml</c>.
    /// </remarks>

    string? StudentFullName { get; set; }
}