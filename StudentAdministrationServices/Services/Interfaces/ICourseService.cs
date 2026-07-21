using StudentAdministrationServices.Models;

namespace StudentAdministrationServices.Services.Interfaces;

/// <summary>
///     Interface for course service.
/// </summary>
public interface ICourseService : IReceiveService<CourseBindingModel>
{
    Task<List<CourseBindingModel>> GetCoursesByDegreeProgramID(Guid id);
}