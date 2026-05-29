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

public partial class AddEditStudentPageViewModel : ObservableObject, IAddEditStudentPageViewModel
{
    private readonly IDegreeProgramService degreeProgramService;

    private readonly IStudentService studentService;
    private readonly IUniversityService universityService;

    [ObservableProperty]
    private string? addEditTitle;

    [ObservableProperty]
    private ObservableCollection<DegreeProgramListModel> degreeProgrammBindingModels = [];

    [ObservableProperty]
    private bool fieldsEmptyCheck;

    [ObservableProperty]
    private bool isBusy;

    private bool isNewStudent;

    private DegreeProgramBindingModel? newDegreeProgramm;

    [ObservableProperty]
    private string? newEmail;

    [ObservableProperty]
    private string? newFirstName;

    [ObservableProperty]
    private string? newLastName;

    [ObservableProperty]
    private DegreeProgramListModel? newSelectedDegreeProgramm;

    private StudentListModel? selectedStudent;
    private readonly ILogger<AddEditStudentPageViewModel> logger;

    /// <summary>
    ///     Initializes a new instance of the AddEditStudentPageViewModel class with the specified services for degree
    ///     programs, universities, and students.
    /// </summary>
    /// <param name="degreeProgramService">The service used to retrieve and manage degree program information.</param>
    /// <param name="universityService">The service used to access and manage university data.</param>
    /// <param name="studentService">The service used to handle student-related operations.</param>
    public AddEditStudentPageViewModel(IDegreeProgramService degreeProgramService, IUniversityService universityService, IStudentService studentService, ILogger<AddEditStudentPageViewModel> logger)
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
            this.IsBusy = true;
            this.DegreeProgrammBindingModels = await this.degreeProgramService.GetAllDegreeProgramListModels();
            if (!this.isNewStudent)
            {
                this.AddEditTitle = AppResources.EditTitle;
                this.NewFirstName = this.selectedStudent?.FirstName;
                this.NewLastName = this.selectedStudent?.LastName;
                this.NewEmail = this.selectedStudent?.Email;
                this.NewSelectedDegreeProgramm = this.DegreeProgrammBindingModels.FirstOrDefault(d => d.Id == this.selectedStudent!.DegreeProgramListModel.Id);
            }
            else
            {
                this.AddEditTitle = AppResources.AddStudentTitle;
                this.NewSelectedDegreeProgramm = this.DegreeProgrammBindingModels.First();
            }

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
        query.TryGetValue(NavigationPassedParameterHelper.IsNewStudent, out object? newStudent);
        this.isNewStudent = newStudent is not null && (bool)newStudent;
        if (this.isNewStudent)
        {
            return;
        }

        query.TryGetValue(NavigationPassedParameterHelper.SelectedStudent, out object? student);
        this.selectedStudent = student as StudentListModel;
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
            this.NewSelectedDegreeProgramm = default!;
            this.NewEmail = string.Empty;
            this.NewFirstName = string.Empty;
            this.NewLastName = string.Empty;
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
            this.IsBusy = false;
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
            this.IsBusy = true;
            Random random = new();
            this.FieldsEmptyCheck = AreAddEditFieldsEmpty();
            if (this.FieldsEmptyCheck)
            {
                return;
            }

            List<DegreeProgramBindingModel> degreePrograms = await this.degreeProgramService.GetAllAsync();

            this.newDegreeProgramm = degreePrograms.First(d => d.Name == this.NewSelectedDegreeProgramm!.Name);
            if (this.isNewStudent)
            {
                Guid uniGuid = new("cdc54724-c8c4-44de-a7aa-24104fb482dd");
                UniversityBindingModel university = await this.universityService.GetByIdAsync(uniGuid);
                await this.studentService.AddAsync(new StudentBindingModel
                                                   {
                                                       FirstName = this.NewFirstName!,
                                                       LastName = this.NewLastName!,
                                                       Email = this.NewEmail!,
                                                       DegreeProgram = this.newDegreeProgramm!,
                                                       StudentNumber = random.Next(10000000, 99999999),
                                                       Id = Guid.NewGuid(),
                                                       Grades = [],
                                                       University = university
                                                   });
            }
            else
            {
                await this.UpdatedSelectedStudent();
                await this.CancelAddEditStudentAsync();
            }
        }

        catch (Exception exception)
        {
            this.logger.LogError(exception,
                AppResources.SaveErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.SaveErrorMessage, AppResources.OkayTitle);
        }
        finally
        {
            this.IsBusy = false;
        }

        await this.CancelAddEditStudentAsync();

        bool AreAddEditFieldsEmpty()
        {
            return this.NewSelectedDegreeProgramm == null || string.IsNullOrEmpty(this.NewEmail) || string.IsNullOrEmpty(this.NewFirstName) || string.IsNullOrEmpty(this.NewLastName);
        }
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
            this.IsBusy = true;
            StudentBindingModel updateStudent = await this.studentService.GetByIdAsync(this.selectedStudent!.Id);
            updateStudent.FirstName = this.NewFirstName!;
            updateStudent.LastName = this.NewLastName!;
            updateStudent.Email = this.NewEmail!;
            if (this.selectedStudent.DegreeProgramListModel.Id != this.NewSelectedDegreeProgramm!.Id)
            {
                updateStudent.DegreeProgram = await this.degreeProgramService.GetByIdAsync(this.NewSelectedDegreeProgramm.Id);
                updateStudent.Grades = [];
            }
            else
            {
                updateStudent.DegreeProgram = updateStudent.DegreeProgram;
                updateStudent.Grades = updateStudent.Grades;
            }

            await this.studentService.UpdateAsync(updateStudent);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception,
                AppResources.SaveErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.SaveErrorMessage, AppResources.OkayTitle);
        }
        finally
        {
            this.IsBusy = false;
        }
    }
}