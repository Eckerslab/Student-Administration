using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Helper;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;
using System.Collections.ObjectModel;

namespace StudentAdministration.UI.ViewModel;

public partial class DetailsPageViewModel : ObservableObject, IDetailsPageViewModel
{
    [ObservableProperty]
    private ObservableCollection<CourseGradeBindingModel>? courseGradeDisplay;

    [ObservableProperty]
    private string? gradePointAverage;

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private StudentBindingModel? student;

    [ObservableProperty]
    private string? studentFullName;

    private Guid studentId;
    private readonly ILogger<DetailsPageViewModel> logger;
    private readonly IStudentService studentService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DetailsPageViewModel"/> class.
    /// </summary>
    /// <param name="studentService">
    /// The service responsible for handling student-related operations.
    /// </param>
    /// <param name="logger">
    /// The logger instance used for logging diagnostic and error information.
    /// </param>
    public DetailsPageViewModel(IStudentService studentService, ILogger<DetailsPageViewModel> logger)
    {
        this.studentService = studentService;
        this.logger = logger;
    }

    /// <summary>
    /// Asynchronously initializes the details page view model by loading the student data, 
    /// calculating the grade point average, and preparing the course grade display.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        try
        {
            this.IsBusy = true;
            this.Student = await this.studentService.GetByIdAsync(this.studentId);
            this.IsBusy = false;
            this.StudentFullName = this.Student!.FirstName + " " + this.Student.LastName;
            this.CourseGradeDisplay = [];
            List<int> listOfGrades = [];
            foreach (GradeBindingModel grade in this.Student.Grades!)
            {
                listOfGrades.Add(grade.Note);
                this.CourseGradeDisplay.Add(new CourseGradeBindingModel(grade.Course!.Name, grade.Note.ToString()));
            }

            this.GradePointAverage = listOfGrades.Count == 0 ? "N/A" : listOfGrades.Average().ToString("F2") + "%";
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage, AppResources.OkayTitle);
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue(NavigationPassedParameterHelper.SelectedStudent, out object? value);
        StudentListModel studentBindingModel = value as StudentListModel ?? new StudentListModel();
        this.studentId = studentBindingModel.Id;
    }

    [RelayCommand]
    public async Task CloseDetailsPopupAsync()
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage, AppResources.OkayTitle);
        }
    }
}