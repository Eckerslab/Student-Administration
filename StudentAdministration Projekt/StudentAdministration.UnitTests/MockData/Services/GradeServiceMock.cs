using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;
using StudentAdministrationServices.Services.Interfaces;
using StudentAdministrationTests.MockData.Repository;

namespace StudentAdministrationTests.MockData.Services;

internal class GradeServiceMock : IGradeService
{
    private readonly CourseServiceMock courseServiceMock = new();
    private readonly GradeRepositoryMock gradeRepositoryMock = new();

    /// <summary>
    ///     Asynchronously adds a new grade to the repository.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Grade" /> object to be added. Must not be <c>null</c>.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    public async Task AddAsync(Grade value)
    {
        await this.gradeRepositoryMock.AddAsync(value);
    }

    /// <summary>
    ///     Asynchronously deletes a grade from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">
    ///     The unique identifier of the grade to be deleted.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    public async Task DeleteAsync(Guid id)
    {
        await this.gradeRepositoryMock.DeleteAsync(id).ConfigureAwait(false);
    }

    /// <summary>
    ///     Retrieves a list of grades associated with a specific student.
    /// </summary>
    /// <param name="id">The unique identifier of the student.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of
    ///     <see cref="GradeBindingModel" /> objects representing the grades of the student.
    /// </returns>
    public async Task<List<GradeBindingModel>> GetByStudentIdAsync(Guid id)
    {
        List<Grade> listOfGrades = await this.gradeRepositoryMock.GetByStudentIdAsync(id);
        return await this.ConvertToGradeBindingModel(listOfGrades);
    }

    /// <summary>
    ///     Retrieves all grades from the repository asynchronously.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of all grades.
    /// </returns>
    public async Task<List<Grade>> GetAllAsync()
    {
        return await this.gradeRepositoryMock.GetAllAsync();
    }

    /// <summary>
    ///     Retrieves a grade by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the grade to retrieve.</param>
    /// <returns>
    ///     A task representing the asynchronous operation. The task result contains the grade
    ///     associated with the specified identifier, or <c>null</c> if no grade is found.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if the provided <paramref name="id" /> is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the grade cannot be retrieved due to an internal error.</exception>
    public async Task<Grade> GetByIdAsync(Guid id)
    {
        return await this.gradeRepositoryMock.GetByIdAsync(id);
    }

    /// <summary>
    ///     Asynchronously updates the specified grade in the data store.
    /// </summary>
    /// <param name="value">The grade entity to update. Cannot be null. The entity's Id property must be a valid GUID.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    public async Task UpdateAsync(Grade value)
    {
        await this.gradeRepositoryMock.UpdateAsync(value);
    }

    /// <summary>
    ///     Converts a list of Grade entities to a list of GradeBindingModel instances for use in data binding scenarios.
    /// </summary>
    /// <remarks>
    ///     Each GradeBindingModel is populated with the corresponding Grade's identifier, course
    ///     information retrieved by course ID, and grade value. The method synchronously waits for course retrieval, which
    ///     may impact performance if used with large lists or in UI threads.
    /// </remarks>
    /// <param name="grades">The list of Grade entities to convert. Cannot be null.</param>
    /// <returns>
    ///     A list of GradeBindingModel objects representing the converted grades. The list will be empty if the input list
    ///     is empty.
    /// </returns>
    private async Task<List<GradeBindingModel>> ConvertToGradeBindingModel(List<Grade> grades)
    {
        List<GradeBindingModel> gradeBindingModels = [];
        for (int i = 0; i < grades.Count; i++)
        {
            GradeBindingModel gradeBindingModel = new()
                                                  {
                                                      Id = grades[i].Id,
                                                      Course = await this.courseServiceMock.GetByIdAsync(grades[i].CourseId),
                                                      Note = grades[i].Value
                                                  };
            gradeBindingModels.Add(gradeBindingModel);
        }

        return gradeBindingModels;
    }
}