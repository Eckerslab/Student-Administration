using StudentAdministrationDatabase.Models;

namespace StudentAdministrationDatabase.SampleData;

/// <summary>
///     ISampleUniversity implementation providing sample data for universities.
/// </summary>
public class SampleUniversities
{
    /// <summary>
    ///     Gets a collection of sample universities.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<University> GetUniversities()
    {
        return new List<University>
        {
            CreateSample("OTH Regensburg", Guid.Parse("cdc54724-c8c4-44de-a7aa-24104fb482dd"))
        };

        static University CreateSample(string name, Guid id)
        {
            return new University
            {
                Name = name,
                Id = id
            };
        }
    }
}