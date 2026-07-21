using System.ComponentModel.DataAnnotations;
using StudentAdministrationDatabase.Models.BaseModels;

namespace StudentAdministrationDatabase.Models;

/// <summary>
///     Student model representing a student entity.
/// </summary>
public class Student : BaseKeyModel
{
    /// <summary>
    ///     gets or sets the number of credits the student has earned.
    /// </summary>
    [Range(0, 200)]
    public int Credits { get; set; } = 0;

    /// <summary>
    ///     Gets or sets the degree program associated with the student.
    /// </summary>
    /// <remarks>
    ///     This property represents the <see cref="DegreeProgram" /> entity that the student is enrolled in.
    /// </remarks>
    public DegreeProgram DegreeProgram { get; set; } = null!;

    /// <summary>
    ///     gets or sets the identifier of the degree program the student is enrolled in.
    /// </summary>
    public Guid DegreeProgramId { get; set; }

    /// <summary>
    ///     gets or sets the email of the student.
    /// </summary>
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///     gets or sets the first name of the student.
    /// </summary>
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the collection of grades assigned to the student.
    /// </summary>
    /// <remarks>
    ///     Each grade represents the student's performance in a specific course.
    /// </remarks>
    public ICollection<Grade>? Grades { get; set; } = new List<Grade>();

    /// <summary>
    ///     gets or sets the last name of the student.
    /// </summary>
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     gets or sets the student number.
    /// </summary>
    public int StudentNumber { get; set; }

    /// <summary>
    ///     Gets or sets the university associated with the student.
    /// </summary>
    /// <remarks>
    ///     This property represents the <see cref="University" /> entity that the student is enrolled in.
    /// </remarks>
    public University University { get; set; } = null!;

    /// <summary>
    ///     gets or sets the identifier of the university the student is associated with.
    /// </summary>
    public Guid UniversityId { get; set; }
}