using StudentAdministrationDatabase.Models;

namespace StudentAdministrationDatabase.SampleData;

/// <summary>
///     Sample data for courses.
/// </summary>
public class SampleCourses
{
    /// <summary>
    ///     Gets sample courses.
    /// </summary>
    /// <returns></returns>
    public static List<Course> GetCourses()
    {
        return new List<Course>
               {
                   GetSample(Guid.Parse("725e835b-733d-43c0-86f8-69392bcb70f0"), "Moderne Datenbankkonzepte", 9, Guid.Parse("ebeb1657-37bd-4781-a69a-6602805bc34f")),
                   GetSample(Guid.Parse("e9ed736d-f029-4518-8641-65a58ce0d7db"), "Business Consulting", 9, Guid.Parse("ebeb1657-37bd-4781-a69a-6602805bc34f")),
                   GetSample(Guid.Parse("ba25d4db-27b9-4118-a384-46535fe3d954"), "Programmieren 1", 6, Guid.Parse("8a20eb66-e15c-4a5f-9c55-8d90c76974d4")),
                   GetSample(Guid.Parse("59b0f640-47ce-47da-b54d-0922f3048d26"), "Programmieren 2", 9, Guid.Parse("8a20eb66-e15c-4a5f-9c55-8d90c76974d4")),
                   GetSample(Guid.Parse("8ab3add5-1510-4f2e-8bff-151f3bc33883"), "Programmieren 1", 6, Guid.Parse("fc72afcf-ed7f-46de-b3cb-a173fd379920")),
                   GetSample(Guid.Parse("9a5c5352-f6db-41c6-ab7c-fd19a3fd37a3"), "Programmieren 2", 9, Guid.Parse("fc72afcf-ed7f-46de-b3cb-a173fd379920")),
                   GetSample(Guid.Parse("d880f582-fa90-409f-a3d7-b9209218760a"), "BWL 1", 6, Guid.Parse("8a20eb66-e15c-4a5f-9c55-8d90c76974d4")),
                   GetSample(Guid.Parse("96867406-8e3b-4441-b475-22d4a484cf6c"), "BWL 2", 9, Guid.Parse("8a20eb66-e15c-4a5f-9c55-8d90c76974d4"))
               };

        static Course GetSample(Guid id, string name, int credit, Guid degreeProgramId)
        {
            return new Course
                   {
                       Id = id,
                       Name = name,
                       Credit = credit,
                       DegreeProgramId = degreeProgramId
                   };
        }
    }
}