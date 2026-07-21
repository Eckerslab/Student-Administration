using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Helper;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministration.UI.ViewModel;

public partial class AddEditStudentPageViewModel : ObservableObject, IAddEditStudentPageViewModel
{
    private readonly IDegreeProgramService degreeProgramService;
    private readonly ILogger<AddEditStudentPageViewModel> logger;

    private readonly IStudentService studentService;
    private readonly IUniversityService universityService;

    [ObservableProperty] private string? addEditTitle;

    [ObservableProperty] private bool degreeProgramError;

    [ObservableProperty] private ObservableCollection<DegreeProgramListModel> degreeProgrammBindingModels = [];

    [ObservableProperty] private bool emailError;

    [ObservableProperty] private string? emailErrorText;

    [ObservableProperty] private bool firstNameError;

    [ObservableProperty] private bool isBusy;

    private bool isNewStudent;

    [ObservableProperty] private bool lastNameError;

    private DegreeProgramBindingModel? newDegreeProgramm;

    [ObservableProperty] private string? newEmail;

    [ObservableProperty] private string? newFirstName;

    [ObservableProperty] private string? newLastName;

    [ObservableProperty] private DegreeProgramListModel? newSelectedDegreeProgramm;

    private StudentListModel? selectedStudent;

    /// <summary>
    ///     Initializes a new instance of the AddEditStudentPageViewModel class with the specified services for degree
    ///     programs, universities, and students.
    /// </summary>
    /// <param name="degreeProgramService">The service used to retrieve and manage degree program information.</param>
    /// <param name="universityService">The service used to access and manage university data.</param>
    /// <param name="studentService">The service used to handle student-related operations.</param>
    public AddEditStudentPageViewModel(IDegreeProgramService degreeProgramService, IUniversityService universityService,
        IStudentService studentService, ILogger<AddEditStudentPageViewModel> logger)
    {
        this.degreeProgramService = degreeProgramService;
        this.universityService = universityService;
        this.studentService = studentService;
        this.logger = logger;
    }

    /// <summary>
    ///     Initializes the view model with degree program data and sets up properties for adding or editing a student.
    /// </summary>
    /// <remarks>
    ///     Call this method before displaying the view to ensure that all necessary data is loaded and
    ///     the view model is in a consistent state. This method prepares the view model for either adding a new student or
    ///     editing an existing one, depending on the current context.
    /// </remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    public async Task InitializeAsync()
    {
        try
        {
            IsBusy = true;
            DegreeProgrammBindingModels = await degreeProgramService.GetAllDegreeProgramListModels();
            if (!isNewStudent)
            {
                AddEditTitle = AppResources.EditTitle;
                NewFirstName = selectedStudent?.FirstName;
                NewLastName = selectedStudent?.LastName;
                NewEmail = selectedStudent?.Email;
                NewSelectedDegreeProgramm =
                    DegreeProgrammBindingModels.FirstOrDefault(d => d.Id == selectedStudent!.DegreeProgramListModel.Id);
            }
            else
            {
                AddEditTitle = AppResources.AddStudentTitle;
                NewSelectedDegreeProgramm = DegreeProgrammBindingModels.First();
            }

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
    ///     Applies query attributes to the view model, initializing its state based on the provided query parameters.
    /// </summary>
    /// <param name="query">
    ///     A dictionary containing query parameters. Expected keys include:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>
    ///                 <see cref="NavigationPassedParameterHelper.IsNewStudent" />
    ///             </term>
    ///             <description>A boolean value indicating whether a new student is being added.</description>
    ///         </item>
    ///         <item>
    ///             <term>
    ///                 <see cref="NavigationPassedParameterHelper.SelectedStudent" />
    ///             </term>
    ///             <description>A <see cref="StudentBindingModel" /> object representing the selected student to edit.</description>
    ///         </item>
    ///     </list>
    /// </param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        query.TryGetValue(NavigationPassedParameterHelper.IsNewStudent, out var newStudent);
        isNewStudent = newStudent is not null && (bool)newStudent;
        if (isNewStudent) return;

        query.TryGetValue(NavigationPassedParameterHelper.SelectedStudent, out var student);
        selectedStudent = student as StudentListModel;
    }

    /// <summary>
    ///     Cancels the current add/edit student operation, resets the input fields,
    ///     and navigates back to the previous page.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [RelayCommand]
    public async Task CancelAddEditStudentAsync()
    {
        try
        {
            NewSelectedDegreeProgramm = default!;
            NewEmail = string.Empty;
            NewFirstName = string.Empty;
            NewLastName = string.Empty;
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
    ///     Saves the changes made to a student's information or adds a new student to the system.
    /// </summary>
    /// <remarks>
    ///     This method validates the input fields to ensure all required fields are filled.
    ///     If the operation is for a new student, it creates and adds a new student to the collection.
    ///     If editing an existing student, it updates the student's details.
    /// </remarks>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    [RelayCommand]
    public async Task SaveAddEditStudentAsync()
    {
        try
        {
            IsBusy = true;
            if (!ValidateFields())
                // Stay on the page so the user can correct the highlighted fields.
                return;

            var degreePrograms = await degreeProgramService.GetAllAsync();

            newDegreeProgramm = degreePrograms.First(d => d.Name == NewSelectedDegreeProgramm!.Name);
            if (isNewStudent)
            {
                var universities = await universityService.GetAllAsync();
                var university = universities.First();

                // Unique, collision-free student number from the service instead of a random value.
                var studentNumber = await studentService.GetNextStudentNumberAsync();
                await studentService.AddAsync(new StudentBindingModel
                {
                    FirstName = NewFirstName!,
                    LastName = NewLastName!,
                    Email = NewEmail!,
                    DegreeProgram = newDegreeProgramm!,
                    StudentNumber = studentNumber,
                    Id = Guid.NewGuid(),
                    Grades = [],
                    University = university
                });
            }
            else
            {
                await UpdatedSelectedStudent();
            }

            // Navigate back exactly once, and only after a successful save.
            await CancelAddEditStudentAsync();
        }
        catch (DbUpdateException exception)
        {
            // e.g. the unique student-number index was violated.
            logger.LogError(exception, AppResources.SaveErrorMessage);
            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.DuplicateEntryMessage,
                AppResources.OkayTitle);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                AppResources.SaveErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.SaveErrorMessage,
                AppResources.OkayTitle);
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    ///     Validates the input fields individually and updates the per-field error flags.
    /// </summary>
    /// <returns><c>true</c> when all fields are valid; otherwise <c>false</c>.</returns>
    private bool ValidateFields()
    {
        FirstNameError = string.IsNullOrWhiteSpace(NewFirstName);
        LastNameError = string.IsNullOrWhiteSpace(NewLastName);
        DegreeProgramError = NewSelectedDegreeProgramm is null;

        if (string.IsNullOrWhiteSpace(NewEmail))
        {
            EmailError = true;
            EmailErrorText = AppResources.FillFieldsTitle;
        }
        else if (!new EmailAddressAttribute().IsValid(NewEmail))
        {
            EmailError = true;
            EmailErrorText = AppResources.InvalidEmailMessage;
        }
        else
        {
            EmailError = false;
        }

        return !(FirstNameError || LastNameError || EmailError || DegreeProgramError);
    }

    /// <summary>
    ///     Updates the selected student's details with the current values from the selection model.
    /// </summary>
    /// <remarks>
    ///     This method synchronizes the selected student's information with the latest values from the
    ///     selection model, including name, email, and degree program. It retrieves the existing student record and applies
    ///     the updated values. This method is intended for internal use and does not persist changes to the data
    ///     store.
    /// </remarks>
    private async Task UpdatedSelectedStudent()
    {
        try
        {
            IsBusy = true;
            var updateStudent = await studentService.GetByIdAsync(selectedStudent!.Id);
            updateStudent.FirstName = NewFirstName!;
            updateStudent.LastName = NewLastName!;
            updateStudent.Email = NewEmail!;
            // Only when the degree program actually changes do we reassign it and reset the grades;
            // otherwise the existing values are kept as-is.
            if (selectedStudent.DegreeProgramListModel.Id != NewSelectedDegreeProgramm!.Id)
            {
                updateStudent.DegreeProgram = await degreeProgramService.GetByIdAsync(NewSelectedDegreeProgramm.Id);
                updateStudent.Grades = [];
            }

            await studentService.UpdateAsync(updateStudent);
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                AppResources.SaveErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.SaveErrorMessage,
                AppResources.OkayTitle);
        }
        finally
        {
            IsBusy = false;
        }
    }
}