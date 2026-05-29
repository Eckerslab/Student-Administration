using StudentAdministrationDatabase.Models;

namespace StudentAdministrationDatabase.Repositories.Interfaces;

/// <summary>
///     Represents a repository interface for managing <see cref="University" /> entities.
///     Extends the <see cref="IBaseRepository{T}" /> interface to provide additional functionality specific to
///     universities.
/// </summary>
public interface IUniversityRepository : IBaseRepository<University>
{
}