using System.ComponentModel.DataAnnotations;
using StudentAdministrationDatabase.Models.BaseModels;

namespace StudentAdministrationDatabase.Models;

/// <summary>
///     Grade model representing a grade assigned to a student for a specific course.
/// </summary>
public class Grade : BaseKeyModel
{
    /// <summary>
    ///     Gets or sets the course associated with this grade.
    /// </summary>
    /// <remarks>
    ///     This property represents the course for which the grade is assigned.
    ///     It is a navigation property linking to the <see cref="Course" /> entity.
    /// </remarks>
    public Course Course { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the identifier of the course for which the grade was assigned.
    /// </summary>
    public required Guid CourseId { get; set; }

    /// <summary>
    ///     Gets or sets the student associated with this instance.
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    ///     Gets or sets the identifier of the student who received the grade.
    /// </summary>
    public required Guid StudentId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the grade.
    /// </summary>
    [Range(0, 6)]
    public int Value { get; set; }
}