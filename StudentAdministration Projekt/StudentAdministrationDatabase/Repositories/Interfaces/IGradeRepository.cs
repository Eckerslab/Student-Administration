using StudentAdministrationDatabase.Models;

namespace StudentAdministrationDatabase.Repositories.Interfaces;

/// <summary>
///     Represents a repository interface for managing <see cref="Grade" /> entities,
///     providing data access operations specific to grades in the student administration system.
/// </summary>
public interface IGradeRepository : IBaseRepository<Grade>
{
    /// <summary>
    ///     Asynchronously retrieves all grades associated with the specified student identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the student whose grades are to be retrieved.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a list of grades for the specified
    ///     student. The list is empty if the student has no grades.
    /// </returns>
    Task<List<Grade>> GetByStudentIdAsync(Guid id);

    /// <summary>
    ///     Asynchronously updates the specified grade entity in the data store.
    /// </summary>
    /// <param name="value">
    ///     The grade entity to update. The entity must not be null and should contain valid data,
    ///     including an existing identifier.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous update operation.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if the grade entity with the specified identifier does not exist in the data store.
    /// </exception>
    Task UpdateAsync(Grade value);
}