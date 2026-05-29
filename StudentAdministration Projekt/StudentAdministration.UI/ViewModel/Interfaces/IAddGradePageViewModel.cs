using CommunityToolkit.Mvvm.Input;
using StudentAdministrationServices.Models;

namespace StudentAdministration.UI.ViewModel.Interfaces;

public interface IAddGradePageViewModel : IQueryAttributable, IInitialize, ICoreInterface
{
    /// <summary>
    ///     Gets the command that cancels the grade addition process.
    /// </summary>
    /// <remarks>
    ///     This command is typically bound to a UI element, such as a button, to allow users to cancel the operation.
    /// </remarks>
    IAsyncRelayCommand CancelGradeCommand { get; }

    /// <summary>
    ///     Gets or sets a value indicating whether the grade entered is invalid.
    /// </summary>
    /// <remarks>
    ///     This property is used to determine if there are validation errors related to the grade input.
    ///     It is typically bound to the <c>HasError</c> property of UI elements to display error messages.
    /// </remarks>
    bool IsInvalidGrade { get; set; }

    /// <summary>
    ///     Gets or sets the collection of course names available for selection.
    /// </summary>
    /// <remarks>
    ///     This property is bound to the items source of the course selection control in the UI.
    ///     It allows dynamic updates to the list of courses displayed to the user.
    /// </remarks>
    List<CourseBindingModel?> ListOfCourses { get; set; }

    /// <summary>
    ///     Gets the command that saves the entered grade for the selected course.
    /// </summary>
    IAsyncRelayCommand SaveGradeCommand { get; }

    /// <summary>
    ///     Gets or sets the currently selected course name.
    /// </summary>
    CourseBindingModel SelectedCourse { get; set; }

    /// <summary>
    ///     Gets the command that handles changes to the selected course.
    /// </summary>
    IAsyncRelayCommand SelectedCourseChangedCommand { get; }

    /// <summary>
    ///     Gets or sets the selected grade value.
    /// </summary>
    int? SelectedGrade { get; set; }

    /// <summary>
    ///     Gets or sets the student for whom the grade is being added.
    /// </summary>
    StudentBindingModel Student { get; set; }

    /// <summary>
    ///     Gets or sets the full name of the student.
    /// </summary>
    string StudentFullName { get; set; }
}