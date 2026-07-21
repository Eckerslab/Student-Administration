using StudentAdministrationDatabase.Models;

namespace StudentAdministrationDatabase.SampleData;

/// <summary>
///     Provides sample data for degree programs, implementing the <see cref="ISampleDegreeProgram" /> interface.
/// </summary>
public class SampleDegreeProgram
{
    public static List<DegreeProgram> GetDegreePrograms()
    {
        return new List<DegreeProgram>
        {
            CreateSample(Guid.Parse("8a20eb66-e15c-4a5f-9c55-8d90c76974d4"), "Wirtschaftsinformatik Bachelor",
                Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd")),
            CreateSample(Guid.Parse("ebeb1657-37bd-4781-a69a-6602805bc34f"), "Wirtschaftsinformatik Master",
                Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd")),
            CreateSample(Guid.Parse("fc72afcf-ed7f-46de-b3cb-a173fd379920"), "Allgemeine Informatik Bachelor",
                Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd"))
        };

        static DegreeProgram CreateSample(Guid id, string name, Guid universityId)
        {
            return new DegreeProgram
            {
                Id = id,
                Name = name
            };
        }
    }
}