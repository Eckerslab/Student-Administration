using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    private readonly IStudentService studentService;

    private GradeBindingModel? existingGrade;

    [ObservableProperty]
    private bool isBusy;

    private bool isExistingNote;

    [ObservableProperty]
    private bool isInvalidGrade;

    [ObservableProperty]
    private List<CourseBindingModel>? listOfCourses;

    [ObservableProperty]
    private CourseBindingModel? selectedCourse;

    [ObservableProperty]
    private int? selectedGrade;

    [ObservableProperty]
    private StudentBindingModel? student;

    [ObservableProperty]
    private string? studentFullName;

    private Guid studentId;
    private readonly ILogger<AddGradePageViewModel> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddGradePageViewModel"/> class.
    /// </summary>
    /// <param name="studentService">
    /// The service responsible for handling student-related operations.
    /// </param>
    /// <param name="courseService">
    /// The service responsible for handling course-related operations.
    /// </param>
    /// <param name="gradeService">
    /// The service responsible for handling grade-related operations.
    /// </param>
    /// <param name="logger">
    /// The logger instance for logging diagnostic information.
    /// </param>
    public AddGradePageViewModel(IStudentService studentService, ICourseService courseService, IGradeService gradeService, ILogger<AddGradePageViewModel> logger)
    {
        this.studentService = studentService;
        this.courseService = courseService;
        this.gradeService = gradeService;
        this.logger = logger;
    }

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
            this.IsBusy = true;
            this.IsInvalidGrade = false;
            this.SelectedGrade = default!;
            this.Student = await this.studentService.GetByIdAsync(this.studentId);
            this.ListOfCourses = await this.courseService.GetCoursesByDegreeProgramID(this.Student.DegreeProgram!.Id);
            this.SelectedCourse = this.ListOfCourses[0];
            this.Student.Grades = await this.gradeService.GetByStudentIdAsync(this.Student.Id);

            this.IsInvalidGrade = false;
            this.StudentFullName = $"{this.Student.FirstName} {this.Student.LastName}";
            this.IsBusy = false;
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage, AppResources.OkayTitle);
        }
        finally
        {
            this.IsBusy = false;
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AddGradePageViewModel" /> class.
    /// </summary>
    /// <param name="query"></param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue(NavigationPassedParameterHelper.StudentID, out object? studentGuid);
        this.studentId = studentGuid as Guid? ?? new Guid();
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
            this.logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage, AppResources.OkayTitle);
        }
        finally
        {
            this.IsBusy = true;
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
            this.IsBusy = true;
            if (this.SelectedGrade == null || this.SelectedCourse == null)
            {
                this.IsInvalidGrade = true;
                return;
            }

            if (!this.isExistingNote)
            {
                if (this.SelectedGrade! < 5)
                {
                    UpdateCredits(true);
                }

                Guid guid = Guid.NewGuid();
                await this.gradeService.AddAsync(new Grade
                                                 {
                                                     CourseId = this.SelectedCourse.Id,
                                                     Value = (int)this.SelectedGrade,
                                                     Id = guid,
                                                     StudentId = this.Student!.Id
                                                 });
                this.Student!.Grades!.Add(new GradeBindingModel
                                          {
                                              Course = this.SelectedCourse,
                                              Note = (int)this.SelectedGrade,
                                              Id = guid
                                          });
            }
            else
            {
                if (this.SelectedGrade > 4 && this.existingGrade!.Note < 5)
                {
                    UpdateCredits(false);
                }

                if (this.SelectedGrade < 5 && this.existingGrade!.Note > 4)
                {
                    UpdateCredits(true);
                }

                this.Student!.Grades!.Remove(this.existingGrade!);
                GradeBindingModel gradeToUpdate = new()
                                                  {
                                                      Note = this.SelectedGrade.Value,
                                                      Course = this.SelectedCourse,
                                                      Id = this.existingGrade!.Id
                                                  };
                this.Student.Grades.Add(gradeToUpdate);
                await this.gradeService.UpdateAsync(new Grade
                                                    {
                                                        CourseId = this.SelectedCourse.Id,
                                                        Value = (int)this.SelectedGrade,
                                                        Id = gradeToUpdate.Id,
                                                        StudentId = this.Student.Id
                                                    });
            }

            await this.studentService.UpdateAsync(this.Student!);
            this.IsBusy = false;
            await this.CancelGradeAsync();
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage, AppResources.OkayTitle);
        }
        finally
        {
            this.IsBusy = true;
        }

        void UpdateCredits(bool addCredits)
        {
            if (addCredits)
            {
                this.Student!.Credits = this.Student.Credits + this.SelectedCourse.Credit;
            }
            else
            {
                this.Student!.Credits = this.Student.Credits - this.SelectedCourse.Credit;
            }
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
        if (this.Student!.Grades!.Count == 0)
        {
            this.isExistingNote = false;
            return Task.CompletedTask;
        }

        this.existingGrade = this.Student!.Grades.FirstOrDefault(x => x.Course!.Id == this.SelectedCourse!.Id);
        if (this.existingGrade == null)
        {
            this.isExistingNote = false;
            this.SelectedGrade = null!;
            return Task.CompletedTask;
        }

        this.SelectedGrade = this.existingGrade.Note;

        this.isExistingNote = true;
        return Task.CompletedTask;
    }
}