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
}