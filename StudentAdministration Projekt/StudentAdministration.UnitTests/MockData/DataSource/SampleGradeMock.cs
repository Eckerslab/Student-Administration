using StudentAdministrationDatabase.Models;

namespace StudentAdministrationTests.MockData.DataSource;

internal static class SampleGradeMock
{
    /// <summary>
    ///     Gets sample grades.
    /// </summary>
    /// <returns></returns>
    public static List<Grade> GetGrades()
    {
        return
        [
            GetSample(Guid.Parse("ab554e19-c7e1-471d-9855-29b3a90598b0"), Guid.Parse("59b0f640-47ce-47da-b54d-0922f3048d26"), Guid.Parse("7b2680c2-9420-4e7e-a2d4-97f4d9931b26"), 3),
            GetSample(Guid.Parse("59bcd467-0261-4a04-abe9-3910cb092694"), Guid.Parse("8ab3add5-1510-4f2e-8bff-151f3bc33883"), Guid.Parse("96d71862-bb38-4e1f-a3fd-12f43582d35e"), 2),
            GetSample(Guid.Parse("b88992aa-3f54-4440-9424-9c6c64516401"), Guid.Parse("9a5c5352-f6db-41c6-ab7c-fd19a3fd37a3"), Guid.Parse("96d71862-bb38-4e1f-a3fd-12f43582d35e"), 1),
            GetSample(Guid.Parse("2e88416b-adf4-41ba-978f-084daa3b801c"), Guid.Parse("d880f582-fa90-409f-a3d7-b9209218760a"), Guid.Parse("7b2680c2-9420-4e7e-a2d4-97f4d9931b26"), 6)
        ];

        static Grade GetSample(Guid id, Guid courseId, Guid studentId, int value)
        {
            return new Grade
                   {
                       Id = id,
                       CourseId = courseId,
                       StudentId = studentId,
                       Value = value,
                       Course = SampleCoursesMock.GetCourses().FirstOrDefault(c => c.Id == courseId) ?? throw new InvalidOperationException()
                   };
        }
    }
}