using System.Collections.ObjectModel;
using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;
using StudentAdministrationTests.MockData.Repository;

namespace StudentAdministrationTests.MockData.Services;

internal class StudentServiceMock : IStudentService
{
    private readonly DegreeProgramServiceMock degreeProgramServiceMock = new();
    private readonly StudentRepositoryMock studentRepositoryMock = new();
    private readonly UniversityServiceMock universityServiceMock = new();

    /// <summary>
    ///     Adds a new student.
    /// </summary>
    /// <param name="value">The student binding model to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided <paramref name="value" /> is null.</exception>
    public async Task AddAsync(StudentBindingModel value)
    {
        Student student = this.ConvertTo(value);
        await this.studentRepositoryMock.AddAsync(student);
    }

    /// <summary>
    ///     Asynchronously deletes a student by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task DeleteAsync(Guid id)
    {
        await this.studentRepositoryMock.DeleteAsync(id).ConfigureAwait(false);
    }

    /// <summary>
    ///     Retrieves all students along with their associated degree programs and universities.
    /// </summary>
    /// <remarks>
    ///     This method fetches all student entities from the repository, converts them into binding models,
    ///     and enriches them with related degree program and university data.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="StudentBindingModel" /> objects representing the students.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if a degree program or university associated with a student cannot be found.
    /// </exception>
    public async Task<List<StudentBindingModel>> GetAllAsync()
    {
        List<Student> studentEntities = await this.studentRepositoryMock.GetAllAsync();
        List<DegreeProgramBindingModel> degreePrograms = await this.degreeProgramServiceMock.GetAllAsync();
        List<StudentBindingModel> bindingModels = new();

        foreach (Student student in studentEntities)
        {
            StudentBindingModel studentBindingModel = ConvertTo(student);
            studentBindingModel.DegreeProgram = degreePrograms.First(dp => dp.Id == student.DegreeProgramId);
            Guid universityId = student.UniversityId;
            studentBindingModel.University = await this.universityServiceMock.GetByIdAsync(universityId);
            bindingModels.Add(studentBindingModel);
        }

        return bindingModels;
    }

    /// <summary>
    ///     Retrieves a student entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student to retrieve.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains the
    ///     <see cref="Student" /> entity corresponding to the specified identifier, or <c>null</c> if no such entity exists.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the provided <paramref name="id" /> is invalid.</exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the student entity cannot be retrieved due to an unexpected
    ///     issue.
    /// </exception>
    public async Task<StudentBindingModel> GetByIdAsync(Guid id)
    {
        Student student = await this.studentRepositoryMock.GetByIdAsync(id);
        StudentBindingModel bindingModel = ConvertTo(student);
        return bindingModel;
    }

    /// <summary>
    ///     Retrieves a list of student list models, which contain summarized information about students.
    /// </summary>
    /// <remarks>
    ///     This method fetches all student binding models and maps them to a list of <see cref="StudentListModel" />.
    ///     Each <see cref="StudentListModel" /> includes details such as the student's ID, first name, last name, email,
    ///     student number, and the name of their degree program.
    /// </remarks>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="StudentListModel" />.
    /// </returns>
    /// <exception cref="Exception">
    ///     An exception might be thrown if there is an issue retrieving the student data or mapping it to the list models.
    /// </exception>
    public async Task<ObservableCollection<StudentListModel>> GetAllStudentListModels()
    {
        List<StudentBindingModel> studentBindingModel = await this.GetAllAsync();
        ObservableCollection<StudentListModel> studentListModels = new();
        foreach (StudentBindingModel bindingModel in studentBindingModel)
        {
            StudentListModel listModel = this.ConvertToListModel(bindingModel);
            studentListModels.Add(listModel);
        }

        return studentListModels;
    }

    /// <summary>
    ///     Updates the student record with the values provided in the specified binding model.
    /// </summary>
    /// <remarks>
    ///     The method removes the existing student record and adds a new one with the updated values.
    ///     Ensure that the student identifier corresponds to an existing record before calling this method.
    /// </remarks>
    /// <param name="student">
    ///     A binding model containing the updated student information. The student must have a valid
    ///     identifier.
    /// </param>
    /// <returns>A task that represents the asynchronous edit operation.</returns>
    public async Task UpdateAsync(StudentBindingModel student)
    {
        Student studentEntity = this.ConvertTo(student);
        await this.studentRepositoryMock.UpdateAsync(studentEntity);
    }

    /// <summary>
    ///     Converts a Student entity to a StudentBindingModel.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    public StudentListModel ConvertToListModel(StudentBindingModel student)
    {
        return new StudentListModel
               {
                   Id = student.Id,
                   FirstName = student.FirstName,
                   LastName = student.LastName,
                   Email = student.Email,
                   Credits = student.Credits,
                   StudentNumber = student.StudentNumber,
                   DegreeProgramListModel = new DegreeProgramListModel
                                            {
                                                Id = student.DegreeProgram!.Id,
                                                Name = student.DegreeProgram.Name
                                            }
               };
    }

    /// <summary>
    ///     Converts a StudentBindingModel to a Student entity.
    /// </summary>
    /// <param name="bindingModel"></param>
    /// <returns></returns>
    private Student ConvertTo(StudentBindingModel bindingModel)
    {
        Student student = new()
                          {
                              Id = bindingModel.Id,
                              FirstName = bindingModel.FirstName,
                              LastName = bindingModel.LastName,
                              Email = bindingModel.Email,
                              Credits = bindingModel.Credits,
                              StudentNumber = bindingModel.StudentNumber,
                              UniversityId = bindingModel.University!.Id,
                              DegreeProgramId = bindingModel.DegreeProgram!.Id
                          };
        return student;
    }

    /// <summary>
    ///     Converts a Student entity to a StudentBindingModel.
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    private static StudentBindingModel ConvertTo(Student student)
    {
        return new StudentBindingModel
               {
                   Id = student.Id,
                   FirstName = student.FirstName,
                   LastName = student.LastName,
                   Email = student.Email,
                   Credits = student.Credits,
                   StudentNumber = student.StudentNumber,
                   DegreeProgram =
                       new DegreeProgramBindingModel
                       {
                           Id = student.DegreeProgram.Id,
                           Name = student.DegreeProgram.Name ?? string.Empty
                       },
                   University =
                       new UniversityBindingModel
                       {
                           Id = student.University.Id,
                           Name = student.University.Name!
                       },
                   Grades = student.Grades?.Select(g => new GradeBindingModel
                                                        {
                                                            Id = g.Id,
                                                            Note = g.Value,
                                                            Course = new CourseBindingModel
                                                                     {
                                                                         Id = g.Course.Id,
                                                                         Name = g.Course.Name,
                                                                         Credit = g.Course.Credit
                                                                     }
                                                        }).
                                    ToList() ?? []
               };
    }
}