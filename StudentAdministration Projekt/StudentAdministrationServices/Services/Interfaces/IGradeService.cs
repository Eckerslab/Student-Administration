using StudentAdministrationDatabase.Models;
using StudentAdministrationServices.Models;

namespace StudentAdministrationServices.Services.Interfaces;

/// <summary>
///     Interface for grade service operations.
/// </summary>
public interface IGradeService : IReceiveService<Grade>, IDeleteService, IUpdateService<Grade>, IAddService<Grade>
{
    Task<List<GradeBindingModel>> GetByStudentIdAsync(Guid id);
}