using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Helper;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using StudentAdministration.UI.Views;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;
using Syncfusion.Maui.DataGrid;

namespace StudentAdministration.UI.ViewModel;

public partial class MainPageViewModel : ObservableObject, IMainPageViewModel
{
    private readonly IDegreeProgramService degreeProgramService;

    private readonly ILogger<MainPageViewModel> logger;
    private readonly IStudentService studentService;

    [ObservableProperty] private string? addEditPopupTitle;

    [ObservableProperty] private ObservableCollection<string?> collectionOfCourseNames = null!;

    [ObservableProperty] private ObservableCollection<CourseGradeBindingModel> courseGradeDisplays = [];

    [ObservableProperty] private List<DegreeProgramBindingModel>? degreeProgramBindingModels;

    [ObservableProperty] private ObservableCollection<DegreeProgramListModel>? degreeProgramListModels;

    [ObservableProperty] private string? errorHeader;

    [ObservableProperty] private string? errorText;

    [ObservableProperty] private DegreeProgramListModel? filterDegreeProgrammName;

    [ObservableProperty] private string? filterFirstName;

    [ObservableProperty] private string? filterId;

    [ObservableProperty] private string? filterLastName;

    [ObservableProperty] private string? gradePointAverage;

    [ObservableProperty] private bool isAddEditPopupVisible;

    [ObservableProperty] private bool isAddGradePopupVisible;

    [ObservableProperty] private bool isBusy;

    [ObservableProperty] private bool isDetailsPopupOpen;

    [ObservableProperty] private bool isErrorPopupOpen;

    [ObservableProperty] private bool isFilterPopupVisible;

    [ObservableProperty] private bool isInvalidGrade;

    [ObservableProperty] private bool isNoResultsPopupOpen;

    [ObservableProperty] private bool isRadialMenuVisible;

    [ObservableProperty] private DegreeProgramBindingModel? newDegreeProgramm;

    [ObservableProperty] private string? newEmail;

    [ObservableProperty] private string? newFirstName;

    [ObservableProperty] private string? newLastName;

    [ObservableProperty] private string? selectedCourse;

    [ObservableProperty] private DegreeProgramListModel? selectedDegreeProgrammName;

    [ObservableProperty] private int? selectedGrade;

    [ObservableProperty] private StudentListModel? selectedStudent;

    [ObservableProperty] private string? studentCountText;

    [ObservableProperty] private string? studentFullName;

    [ObservableProperty] private ObservableCollection<StudentListModel>? studentListBindingModels;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MainPageViewModel" /> class.
    /// </summary>
    /// <param name="studentService">
    ///     The service responsible for handling student-related operations.
    /// </param>
    /// <param name="degreeProgramService">
    ///     The service responsible for handling degree program-related operations.
    /// </param>
    public MainPageViewModel(IStudentService studentService, IDegreeProgramService degreeProgramService,
        ILogger<MainPageViewModel> logger)
    {
        this.degreeProgramService = degreeProgramService;
        this.studentService = studentService;
        this.logger = logger;
    }

    public bool IsNewStudent { get; set; }

    /// <summary>
    ///     Initializes the page and prepares it for user interaction.
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
            StudentListBindingModels = await studentService.GetAllStudentListModels();
            DegreeProgramListModels = await degreeProgramService.GetAllDegreeProgramListModels();
            StudentCountText = $"{AppResources.CountTitle} : {StudentListBindingModels?.Count}";

            var hasActiveFilter = FilterId != null
                                  || !string.IsNullOrEmpty(FilterLastName)
                                  || !string.IsNullOrEmpty(FilterFirstName)
                                  || FilterDegreeProgrammName != null;

            if (hasActiveFilter)
                await ApplyFilterAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, AppResources.ItemLoadError);
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.ItemLoadError,
                AppResources.OkayTitle);
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    ///     Navigates to the <see cref="AddGradePage" /> to add a grade for the selected student.
    /// </summary>
    /// <remarks>
    ///     This method sets the <see cref="IsBusy" /> property to true during the operation and hides the radial menu.
    ///     It passes the selected student's ID as a parameter to the navigation.
    /// </remarks>
    /// <exception cref="Exception">Thrown if an error occurs during navigation.</exception>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [RelayCommand]
    public async Task AddGradeAsync()
    {
        try
        {
            IsBusy = true;
            IsRadialMenuVisible = false;
            Dictionary<string, object> passedParameters = new()
            {
                {
                    NavigationPassedParameterHelper.StudentID, SelectedStudent?.Id ?? Guid.Empty
                }
            };
            await Shell.Current.GoToAsync(nameof(AddGradePage), passedParameters);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage,
                AppResources.OkayTitle);
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    ///     Displays the "Add Student" popup, allowing the user to add a new student to the system.
    /// </summary>
    /// <remarks>
    ///     This method sets the state to indicate that a new student is being added.
    ///     It updates the popup title to "Add Student," makes the add/edit popup visible,
    ///     and hides the radial menu. The method completes asynchronously.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public async Task AddStudentAsync()
    {
        try
        {
            IsBusy = true;
            IsNewStudent = true;
            AddEditPopupTitle = "Add Student";
            IsRadialMenuVisible = false;
            Dictionary<string, object> passedParameters = new()
            {
                {
                    NavigationPassedParameterHelper.IsNewStudent, IsNewStudent
                }
            };
            await Shell.Current.GoToAsync(nameof(AddEditStudentPage), passedParameters);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, AppResources.NavigationErrorMessage);
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage,
                AppResources.OkayTitle);
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    ///     Applies the currently selected filter to the student data.
    /// </summary>
    /// <remarks>
    ///     This method processes the filter criteria set by the user and updates the
    ///     displayed student data accordingly. It completes asynchronously.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public async Task ApplyFilterAsync()
    {
        try
        {
            IsBusy = true;
            IsFilterPopupVisible = false;
            var allStudents = await studentService.GetAllStudentListModels();
            var filteredStudents = allStudents;
            if (!string.IsNullOrWhiteSpace(FilterId))
            {
                FilterId = FilterId.Trim('_');
                filteredStudents = new ObservableCollection<StudentListModel>(
                    filteredStudents.Where(s => s.StudentNumber.ToString().Contains(FilterId.Trim())));
            }

            if (!string.IsNullOrEmpty(FilterLastName))
                filteredStudents = new ObservableCollection<StudentListModel>(
                    filteredStudents.Where(s => s.LastName!.Contains(FilterLastName!)));

            if (!string.IsNullOrEmpty(FilterFirstName))
                filteredStudents = new ObservableCollection<StudentListModel>(
                    filteredStudents.Where(s => s.FirstName!.Contains(FilterFirstName!)));

            if (FilterDegreeProgrammName != null)
                filteredStudents = new ObservableCollection<StudentListModel>(
                    filteredStudents.Where(s => s.DegreeProgramListModel.Name == FilterDegreeProgrammName.Name));

            if (filteredStudents.Count == 0)
            {
                // Reuse the already-loaded list instead of hitting the database again.
                filteredStudents = allStudents;
                await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NoStudentsFoundTitle,
                    AppResources.OkayTitle);
            }

            StudentListBindingModels = filteredStudents;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, AppResources.FilterErrorMessage);
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.FilterErrorMessage,
                AppResources.OkayTitle);
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    ///     Cancels the currently applied filter and resets the filter state.
    /// </summary>
    /// <remarks>
    ///     This method is used to clear any active filters applied to the student list or other data.
    ///     It ensures that the original unfiltered data is displayed. The method completes asynchronously.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public async Task CancelFilterAsync()
    {
        try
        {
            IsBusy = true;
            EmptyFilter();
            StudentListBindingModels = await studentService.GetAllStudentListModels();
            IsFilterPopupVisible = false;
            IsNoResultsPopupOpen = false;
            IsBusy = false;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, AppResources.ItemLoadError);
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.ItemLoadError,
                AppResources.OkayTitle);
        }
    }

    /// <summary>
    ///     Deletes the currently selected student from the list of students and sample data.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [RelayCommand]
    public async Task DeleteTaskAsync()
    {
        try
        {
            IsBusy = true;
            await studentService.DeleteAsync(SelectedStudent!.Id);
            EmptyFilter();
            await InitializeAsync();
        }
        catch (KeyNotFoundException exception)
        {
            logger.LogError(exception,
                AppResources.StudentNotFoundMessage + SelectedStudent!.Id);

            await Shell.Current.DisplayAlert(
                AppResources.ErrorTitle,
                AppResources.StudentNotFoundMessage,
                AppResources.OkayTitle);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                AppResources.StudentFailedToDeleteMessage, SelectedStudent!.Id);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.StudentFailedToDeleteMessage,
                AppResources.OkayTitle);
        }
        finally
        {
            IsRadialMenuVisible = false;
            IsBusy = false;
        }
    }

    /// <summary>
    ///     Initiates the process to edit the currently selected student task asynchronously.
    /// </summary>
    /// <remarks>
    ///     This method is triggered by the associated command and is intended to handle the logic
    ///     for editing a task related to the selected student. Ensure that <see cref="SelectedStudent" />
    ///     is set before invoking this method.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [RelayCommand]
    public async Task EditStudentAsync()
    {
        try
        {
            if (SelectedStudent == null) return;

            IsNewStudent = false;
            Dictionary<string, object> passedParameters = new()
            {
                {
                    NavigationPassedParameterHelper.SelectedStudent, SelectedStudent!
                },
                {
                    NavigationPassedParameterHelper.IsNewStudent, IsNewStudent
                }
            };
            await Shell.Current.GoToAsync(nameof(AddEditStudentPage), passedParameters);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                AppResources.NavigationErrorMessage, SelectedStudent!.Id);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage,
                AppResources.OkayTitle);
        }
        finally
        {
            IsAddEditPopupVisible = true;
            IsRadialMenuVisible = false;
        }
    }

    /// <summary>
    ///     Navigates to the <see cref="DetailsPage" /> and opens a popup displaying details of the selected student.
    /// </summary>
    /// <remarks>
    ///     This method sets the <c>IsRadialMenuVisible</c> property to <c>false</c> before navigating.
    ///     It passes the currently selected student as a parameter to the <see cref="DetailsPage" />.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [RelayCommand]
    public async Task OpenDetailsPopupAsync()
    {
        try
        {
            IsRadialMenuVisible = false;
            Dictionary<string, object> passedParameters = new()
            {
                {
                    NavigationPassedParameterHelper.SelectedStudent, SelectedStudent!
                }
            };
            await Shell.Current.GoToAsync(nameof(DetailsPage), passedParameters);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                AppResources.GeneralErrorMessage, SelectedStudent!.Id);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.GeneralErrorMessage,
                AppResources.OkayTitle);
        }
    }

    /// <summary>
    ///     Handles the long press event on an item in the list.
    /// </summary>
    /// <param name="dataGridCellRightTappedEventArgs">
    ///     The event arguments containing details about the long-pressed item.
    ///     If the long-pressed item is of type <see cref="StudentBindingModel" />, it will be set as the selected student.
    /// </param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [RelayCommand]
    public Task RightClickStudentAsync(DataGridCellRightTappedEventArgs dataGridCellRightTappedEventArgs)
    {
        if (dataGridCellRightTappedEventArgs.RowData is not StudentListModel student) return Task.CompletedTask;

        SelectedStudent = student;
        IsRadialMenuVisible = true;

        return Task.CompletedTask;
    }

    /// <summary>
    ///     Displays the filter popup in the UI.
    /// </summary>
    /// <remarks>
    ///     This method sets the visibility of the filter popup to true, allowing the user to apply filters
    ///     to the displayed data. It is typically invoked when the user requests to filter the student list.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public Task ShowFilterPopupAsync()
    {
        IsFilterPopupVisible = true;
        return Task.CompletedTask;
    }

    /// <summary>
    ///     Clears all filter criteria by resetting the filter-related properties to their default values.
    /// </summary>
    /// <remarks>
    ///     This method resets properties such as <c>FilterId</c>, <c>FilterDegreeProgrammName</c>,
    ///     <c>FilterFirstName</c>, and <c>FilterLastName</c>.
    /// </remarks>
    private void EmptyFilter()
    {
        FilterId = null;
        FilterDegreeProgrammName = null;
        FilterFirstName = string.Empty;
        FilterLastName = string.Empty;
    }
}