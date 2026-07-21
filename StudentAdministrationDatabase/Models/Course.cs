using System.ComponentModel.DataAnnotations;
using StudentAdministrationDatabase.Models.BaseModels;

namespace StudentAdministrationDatabase.Models;

/// <summary>
///     Course model representing a course in the student administration database.
/// </summary>
public class Course : BaseModelCommon
{
    /// <summary>
    ///     gets or sets the name of the course.
    /// </summary>
    [Range(1, 20)]
    public int Credit { get; set; } = 0;

    /// <summary>
    ///     Gets or sets the degree program associated with the course.
    /// </summary>
    /// <remarks>
    ///     This property represents the relationship between a course and its corresponding degree program.
    ///     The <see cref="DegreeProgram" /> provides additional details about the degree program.
    /// </remarks>
    public DegreeProgram DegreeProgram { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the name of the course.
    /// </summary>
    public required Guid DegreeProgramId { get; set; }
}