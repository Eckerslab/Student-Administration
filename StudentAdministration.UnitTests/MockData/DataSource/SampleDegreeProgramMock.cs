using StudentAdministrationDatabase.Models;

namespace StudentAdministrationTests.MockData.DataSource;

internal static class SampleDegreeProgramMock
{
    public static List<DegreeProgram> GetDegreePrograms()
    {
        return new List<DegreeProgram>
        {
            CreateSample(Guid.Parse("8a20eb66-e15c-4a5f-9c55-8d90c76974d4"), "Wirtschaftsinformatik Bachelor"),
            CreateSample(Guid.Parse("ebeb1657-37bd-4781-a69a-6602805bc34f"), "Wirtschaftsinformatik Master"),
            CreateSample(Guid.Parse("fc72afcf-ed7f-46de-b3cb-a173fd379920"), "Allgemeine Informatik Bachelor")
        };

        static DegreeProgram CreateSample(Guid id, string name)
        {
            return new DegreeProgram
            {
                Id = id,
                Name = name,
                Courses = SampleCoursesMock.GetCourses().Where(c => c.DegreeProgramId == id).ToList()
            };
        }
    }
}