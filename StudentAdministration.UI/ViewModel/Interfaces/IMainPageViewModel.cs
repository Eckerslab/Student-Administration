using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using StudentAdministrationServices.Models;
using Syncfusion.Maui.DataGrid;

namespace StudentAdministration.UI.ViewModel.Interfaces;

/// <summary>
///     Represents the view model interface for the main page in the Student Administration UI.
/// </summary>
public interface IMainPageViewModel : IInitialize, ICoreInterface
{
    /// <summary>
    ///     Gets the command to add a grade to a student's record asynchronously.
    /// </summary>
    /// <remarks>
    ///     This command is bound to the "Add Grade" functionality in the UI and is executed asynchronously.
    /// </remarks>
    /// <value>
    ///     An <see cref="IAsyncRelayCommand" /> instance representing the asynchronous operation for adding a grade.
    /// </value>
    /// FO
    IAsyncRelayCommand AddGradeCommand { get; }

    /// <summary>
    ///     Gets the command that triggers the addition of a new student.
    /// </summary>
    /// <remarks>
    ///     When executed, this command opens a popup for adding a new student,
    ///     sets the popup title to "Add Student," and hides the radial menu.
    /// </remarks>
    IAsyncRelayCommand AddStudentCommand { get; }

    /// <summary>
    ///     Gets the command that applies the current filter criteria to the student list.
    /// </summary>
    /// <remarks>
    ///     This command is typically bound to a UI element, such as a button, and is used to filter
    ///     the displayed list of students based on the specified filter criteria, including ID,
    ///     first name, last name, email, and degree program.
    /// </remarks>
    IAsyncRelayCommand ApplyFilterCommand { get; }

    /// <summary>
    ///     Gets the command to cancel the currently applied filter and reset the filter state.
    /// </summary>
    /// <remarks>
    ///     This command is used to clear any active filters applied to the student list or other data.
    ///     It ensures that the original unfiltered data is displayed and resets the filter popup visibility.
    /// </remarks>
    /// <value>
    ///     An <see cref="IAsyncRelayCommand" /> that represents the asynchronous operation to cancel the filter.
    /// </value>
    IAsyncRelayCommand CancelFilterCommand { get; }

    /// <summary>
    ///     Gets or sets the collection of degree program binding models associated with the entity.
    /// </summary>
    List<DegreeProgramBindingModel> DegreeProgramBindingModels { get; set; }

    ObservableCollection<DegreeProgramListModel> DegreeProgramListModels { get; set; }

    /// <summary>
    ///     Gets the command that executes the deletion of a task.
    /// </summary>
    /// <remarks>
    ///     This command is typically bound to the "Delete" option in the radial menu
    ///     on the main page of the Student Administration UI.
    /// </remarks>
    IAsyncRelayCommand DeleteTaskCommand { get; }

    /// <summary>
    ///     Gets the command that is executed to edit a task.
    ///     This command is typically bound to the "Edit" option in the radial menu
    ///     and is triggered when the user selects the edit action.
    /// </summary>
    IAsyncRelayCommand EditStudentCommand { get; }

    /// <summary>
    ///     Gets or sets the header text for error messages displayed to the user.
    /// </summary>
    string ErrorHeader { get; set; }

    /// <summary>
    ///     Gets or sets the error message associated with the current operation or state.
    /// </summary>
    string ErrorText { get; set; }

    /// <summary>
    ///     F
    ///     Gets or sets the name of the degree program used to filter the list of students.
    /// </summary>
    /// <remarks>
    ///     This property is bound to the selected item in the degree program filter UI component.
    ///     Updating this property triggers the filtering logic to display students associated with the selected degree
    ///     program.
    /// </remarks>
    DegreeProgramListModel FilterDegreeProgrammName { get; set; }

    /// <summary>
    ///     Gets the filter text for the first name used in the filtering functionality
    ///     of the student list on the main page.
    /// </summary>
    /// <remarks>
    ///     This property is typically bound to the user input field in the UI, allowing
    ///     users to filter students by their first name.
    /// </remarks>
    string? FilterFirstName { get; set; }

    /// <summary>
    ///     Gets the filter ID used for filtering students in the main page view model.
    /// </summary>
    /// <remarks>
    ///     This property is typically bound to the user interface to allow filtering
    ///     of student data by their unique identifier.
    /// </remarks>
    string? FilterId { get; set; }

    /// <summary>
    ///     Gets the filter value for the last name used in the search or filtering functionality.
    /// </summary>
    /// <remarks>
    ///     This property is typically bound to the user interface to allow users to input or view
    ///     the last name filter criteria. It is used to refine the displayed list of students
    ///     based on their last names.
    /// </remarks>
    string? FilterLastName { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the error popup is currently open.
    /// </summary>
    bool IsErrorPopupOpen { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the filter popup is currently visible.
    /// </summary>
    bool IsFilterPopupVisible { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the "No Results" popup is open.
    /// </summary>
    /// <remarks>
    ///     This property is bound to the <see cref="Syncfusion.Maui.Popup.SfPopup.IsOpen" />
    ///     and
    ///     <see>
    ///         <cref>IsVisible</cref>
    ///     </see>
    ///     properties in the UI.
    ///     It determines the visibility of the "No Results" popup displayed when no results are found.
    /// </remarks>
    bool IsNoResultsPopupOpen { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the radial menu is visible on the main page.
    /// </summary>
    /// <value>
    ///     <c>true</c> if the radial menu is visible; otherwise, <c>false</c>.
    /// </value>
    bool IsRadialMenuVisible { get; set; }

    /// <summary>
    ///     Gets the command that asynchronously opens the details popup.
    /// </summary>
    /// <remarks>
    ///     Use this command to trigger the display of additional information in a popup dialog. The
    ///     command executes asynchronously and is typically bound to a UI element such as a button.
    /// </remarks>
    IAsyncRelayCommand OpenDetailsPopupCommand { get; }

    /// <summary>
    ///     Gets the command that handles the long-press gesture on a list item.
    /// </summary>
    /// <remarks>
    ///     This command is triggered when a long-press gesture is detected on an item in the list.
    ///     It processes the <see cref="Syncfusion.Maui.ListView.ItemLongPressEventArgs" />
    ///     to perform the associated task, such as interacting with the selected <see cref="StudentBindingModel" />.
    /// </remarks>
    IAsyncRelayCommand<DataGridCellRightTappedEventArgs> RightClickStudentCommand { get; }

    /// <summary>
    ///     Gets or sets the currently selected student in the main page view model.
    /// </summary>
    /// <value>
    ///     The <see cref="StudentBindingModel" /> object representing the selected student.
    /// </value>
    StudentListModel? SelectedStudent { get; set; }

    /// <summary>
    ///     Gets the command to display the filter popup in the main page UI.
    /// </summary>
    /// <remarks>
    ///     This command is typically bound to a UI element, such as a button,
    ///     to trigger the visibility of the filter popup. It is used to allow
    ///     users to apply filters to the displayed data.
    /// </remarks>
    IAsyncRelayCommand ShowFilterPopupCommand { get; }

    string StudentCountText { get; set; }

    /// <summary>
    ///     Gets or sets the full name of the currently selected student.
    /// </summary>
    /// <remarks>
    ///     This property is used to display or manipulate the full name of the student
    ///     that is currently selected in the main page view model.
    /// </remarks>
    string? StudentFullName { get; set; }

    /// <summary>
    ///     Gets or sets the collection of student list models used for data binding in the user interface.
    /// </summary>
    /// <remarks>
    ///     This property is typically used to provide a dynamic list of students that updates
    ///     automatically when the collection changes. It is suitable for scenarios where UI elements need to reflect
    ///     changes in the underlying data, such as adding or removing students.
    /// </remarks>
    ObservableCollection<StudentListModel> StudentListBindingModels { get; set; }
}