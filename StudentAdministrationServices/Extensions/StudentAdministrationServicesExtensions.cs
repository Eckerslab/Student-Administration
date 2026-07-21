using Microsoft.Extensions.DependencyInjection;
using StudentAdministrationServices.Services;
using StudentAdministrationServices.Services.Interfaces;

namespace StudentAdministrationServices.Extensions;

/// <summary>
///     Provides extension methods for database repository operations within the
///     <see cref="StudentAdministrationServicesExtensions" /> namespace.
/// </summary>
public static class StudentAdministrationServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddTransient<IStudentService, StudentService>();
        service.AddTransient<IDegreeProgramService, DegreeProgramService>();
        service.AddTransient<ICourseService, CourseService>();
        service.AddTransient<IUniversityService, UniversityService>();
        service.AddTransient<IGradeCalculationService, GradeCalculationService>();
        service.AddTransient<IGradeService, GradeService>();

        return service;
    }
}