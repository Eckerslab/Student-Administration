using StudentAdministrationDatabase.Models;

namespace StudentAdministrationDatabase.Repositories.Interfaces;

/// <summary>
///     Represents a repository interface for managing <see cref="StudentAdministrationDatabase.Models.Course" /> entities
///     in the student administration database.
/// </summary>
/// <remarks>
///     This interface extends <see cref="StudentAdministrationDatabase.Repositories.Interfaces.IBaseRepository{T}" />
///     to provide additional functionality specific to <see cref="StudentAdministrationDatabase.Models.Course" />
///     entities.
/// </remarks>
public interface ICourseRepository : IBaseRepository<Course>
{
    /// <summary>
    ///     Asynchronously retrieves all courses that belong to the specified degree program.
    ///     The filtering is performed in the database.
    /// </summary>
    /// <param name="degreeProgramId">The unique identifier of the degree program.</param>
    /// <returns>A task whose result is the list of matching courses (empty if none match).</returns>
    Task<List<Course>> GetByDegreeProgramIdAsync(Guid degreeProgramId);
}