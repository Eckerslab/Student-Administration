using StudentAdministrationDatabase.Models;

namespace StudentAdministrationDatabase.Repositories.Interfaces;

/// <summary>
///     Represents a repository that provides data access operations for student entities.
/// </summary>
/// <remarks>
///     This interface extends the generic repository functionality defined by
///     <see
///         cref="IBaseRepository{Student}" />
///     to support operations specific to students. Implementations are responsible for
///     managing the persistence and retrieval of <see cref="Student" /> objects.
/// </remarks>
public interface IStudentRepository : IBaseRepository<Student>
{
    /// <summary>
    ///     Updates the specified <see cref="Student" /> entity in the repository.
    /// </summary>
    /// <param name="value">
    ///     The <see cref="Student" /> entity containing the updated data. The entity must have a valid identifier.
    /// </param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if the <see cref="Student" /> entity with the specified identifier does not exist in the repository.
    /// </exception>
    Task UpdateAsync(Student value);
}