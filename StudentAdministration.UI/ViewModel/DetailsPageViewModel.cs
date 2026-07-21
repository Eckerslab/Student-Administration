using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Helper;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministration.UI.ViewModel;

public partial class DetailsPageViewModel : ObservableObject, IDetailsPageViewModel
{
    private readonly IGradeCalculationService gradeCalculationService;
    private readonly ILogger<DetailsPageViewModel> logger;
    private readonly IStudentService studentService;

    [ObservableProperty] private ObservableCollection<CourseGradeBindingModel>? courseGradeDisplay;

    [ObservableProperty] private string? gradePointAverage;

    [ObservableProperty] private bool isBusy;

    [ObservableProperty] private StudentBindingModel? student;

    [ObservableProperty] private string? studentFullName;

    private Guid studentId;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DetailsPageViewModel" /> class.
    /// </summary>
    /// <param name="studentService">
    ///     The service responsible for handling student-related operations.
    /// </param>
    /// <param name="gradeCalculationService">
    ///     The service responsible for grading calculations such as the average grade.
    /// </param>
    /// <param name="logger">
    ///     The logger instance used for logging diagnostic and error information.
    /// </param>
    public DetailsPageViewModel(IStudentService studentService, IGradeCalculationService gradeCalculationService,
        ILogger<DetailsPageViewModel> logger)
    {
        this.studentService = studentService;
        this.gradeCalculationService = gradeCalculationService;
        this.logger = logger;
    }

    /// <summary>
    ///     Asynchronously initializes the details page view model by loading the student data,
    ///     calculating the grade point average, and preparing the course grade display.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
            Student = await studentService.GetByIdAsync(studentId);
            StudentFullName = Student!.FirstName + " " + Student.LastName;
            CourseGradeDisplay = [];
            List<int> listOfGrades = [];
            foreach (var grade in Student.Grades!)
            {
                listOfGrades.Add(grade.Note);
                CourseGradeDisplay.Add(new CourseGradeBindingModel(grade.Course!.Name, grade.Note.ToString()));
            }

            var averageGrade = gradeCalculationService.CalculateAverageGrade(listOfGrades);
            GradePointAverage = averageGrade is null ? "N/A" : averageGrade.Value.ToString("F2");
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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue(NavigationPassedParameterHelper.SelectedStudent, out var value);
        var studentBindingModel = value as StudentListModel ?? new StudentListModel();
        studentId = studentBindingModel.Id;
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
            logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage,
                AppResources.OkayTitle);
        }
    }
}