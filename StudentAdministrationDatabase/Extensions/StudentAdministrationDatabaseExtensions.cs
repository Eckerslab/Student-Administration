using Microsoft.Extensions.DependencyInjection;
using StudentAdministrationDatabase.Repositories;
using StudentAdministrationDatabase.Repositories.Interfaces;
using StudentAdministrationDatabase.SampleData;

namespace StudentAdministrationDatabase.Extensions;

/// <summary>
///     Database repository extensions for IServiceCollection.
/// </summary>
public static class StudentAdministrationDatabaseExtensions
{
    /// <summary>
    ///     Inject database repositories into the service collection.
    /// </summary>
    /// <param name="service"></param>
    /// <returns></returns>
    public static IServiceCollection AddDataBaseRepositories(this IServiceCollection service)
    {
        service.AddTransient<IStudentRepository, StudentRepository>();
        service.AddTransient<IDegreeProgramRepository, DegreeProgramRepository>();
        service.AddTransient<ICourseRepository, CourseRepository>();
        service.AddTransient<IGradeRepository, GradeRepository>();
        service.AddTransient<IUniversityRepository, UniversityRepository>();
        service.AddTransient<SampleStudents>();
        service.AddTransient<SampleCourses>();
        service.AddTransient<SampleDegreeProgram>();
        service.AddTransient<SampleUniversities>();
        service.AddTransient<SampleGrades>();
        return service;
    }
}