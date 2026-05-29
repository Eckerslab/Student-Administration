using StudentAdministrationDatabase.Models.BaseModels;

namespace StudentAdministrationDatabase.Models;

/// <summary>
///     Degree program model representing a degree program entity.
/// </summary>
public class DegreeProgram : BaseModelCommon
{
    /// <summary>
    ///     Gets or sets the collection of courses associated with the degree program.
    /// </summary>
    /// <value>
    ///     A collection of <see cref="Course" /> objects representing the courses offered in the degree program.
    /// </value>
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}