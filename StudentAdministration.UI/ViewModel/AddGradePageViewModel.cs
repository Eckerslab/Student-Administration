using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Helper;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministration.UI.ViewModel;

public partial class AddGradePageViewModel : ObservableObject, IAddGradePageViewModel
{
    private readonly ICourseService courseService;
    private readonly IGradeService gradeService;
    private readonly ILogger<AddGradePageViewModel> logger;
    private readonly IStudentService studentService;

    private GradeBindingModel? existingGrade;

    [ObservableProperty] private bool isBusy;

    [ObservableProperty] private bool isInvalidGrade;

    [ObservableProperty] private List<CourseBindingModel>? listOfCourses;

    [ObservableProperty] private CourseBindingModel? selectedCourse;

    [ObservableProperty] private int? selectedGrade;

    [ObservableProperty] private StudentBindingModel? student;

    [ObservableProperty] private string? studentFullName;

    private Guid studentId;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AddGradePageViewModel" /> class.
    /// </summary>
    /// <param name="studentService">
    ///     The service responsible for handling student-related operations.
    /// </param>
    /// <param name="courseService">
    ///     The service responsible for handling course-related operations.
    /// </param>
    /// <param name="gradeService">
    ///     The service responsible for handling grade-related operations.
    /// </param>
    /// <param name="logger">
    ///     The logger instance for logging diagnostic information.
    /// </param>
    public AddGradePageViewModel(IStudentService studentService, ICourseService courseService,
        IGradeService gradeService, ILogger<AddGradePageViewModel> logger)
    {
        this.studentService = studentService;
        this.courseService = courseService;
        this.gradeService = gradeService;
        this.logger = logger;
    }

    /// <summary>
    ///     Lowest selectable grade value, taken from the central <see cref="GradingScale" /> so the UI can never
    ///     drift from the domain rule.
    /// </summary>
    public double GradeMinimum => GradingScale.MinValue;

    /// <summary>
    ///     Highest selectable grade value, taken from the central <see cref="GradingScale" />.
    /// </summary>
    public double GradeMaximum => GradingScale.MaxValue;

    /// <summary>
    ///     Asynchronously initializes the view model by loading the necessary data for the Add Grade page.
    /// </summary>
    /// <remarks>
    ///     This method retrieves the student details, the list of courses associated with the student's degree program,
    ///     and the student's grades. It also sets up the initial state of the view model, such as the selected course
    ///     and grade, and constructs the student's full name.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
            IsInvalidGrade = false;
            SelectedGrade = default!;
            Student = await studentService.GetByIdAsync(studentId);
            ListOfCourses = await courseService.GetCoursesByDegreeProgramID(Student.DegreeProgram!.Id);
            SelectedCourse = ListOfCourses[0];
            Student.Grades = await gradeService.GetByStudentIdAsync(Student.Id);

            IsInvalidGrade = false;
            StudentFullName = $"{Student.FirstName} {Student.LastName}";
            IsBusy = false;
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
    ///     Initializes a new instance of the <see cref="AddGradePageViewModel" /> class.
    /// </summary>
    /// <param name="query"></param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue(NavigationPassedParameterHelper.StudentID, out var studentGuid);
        studentId = studentGuid as Guid? ?? new Guid();
    }

    [RelayCommand]
    public async Task CancelGradeAsync()
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
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    ///     Saves the grade for a student asynchronously.
    /// </summary>
    /// <remarks>
    ///     This method is responsible for persisting the grade information for a student.
    ///     It completes asynchronously and ensures that the grade data is saved properly.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public async Task SaveGradeAsync()
    {
        try
        {
            IsBusy = true;
            if (SelectedGrade == null || SelectedCourse == null || Student == null)
            {
                IsInvalidGrade = true;
                return;
            }

            // The grade service decides whether this is a create or an update and keeps the
            // student's credits consistent, so no business logic is needed here in the view model.
            await gradeService.SaveGradeAsync(Student.Id, SelectedCourse.Id, SelectedGrade.Value);
            await CancelGradeAsync();
        }
        catch (ArgumentOutOfRangeException exception)
        {
            // The selected grade was outside the valid grading scale.
            logger.LogError(exception, AppResources.GradeErrorTitle);
            IsInvalidGrade = true;
        }
        catch (DbUpdateException exception)
        {
            logger.LogError(exception, AppResources.SaveErrorMessage);
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.DuplicateEntryMessage,
                AppResources.OkayTitle);
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
    ///     Handles the event when the selected course changes asynchronously.
    /// </summary>
    /// <remarks>
    ///     This method is triggered whenever the selected course is updated.
    ///     It is intended to handle any logic or state updates required as a result of the course selection change.
    ///     The implementation details are not provided in the current context.
    /// </remarks>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous operation.
    /// </returns>
    [RelayCommand]
    public Task SelectedCourseChangedAsync()
    {
        // Pre-fill the picker with the student's existing grade for the selected course (if any),
        // otherwise clear the selection so a fresh grade can be entered.
        existingGrade = Student?.Grades?.FirstOrDefault(x => x.Course!.Id == SelectedCourse!.Id);
        SelectedGrade = existingGrade?.Note;
        return Task.CompletedTask;
    }
}