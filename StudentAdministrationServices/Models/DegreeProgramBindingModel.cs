namespace StudentAdministrationServices.Models;

/// <summary>
///     Defines a degree program within the student administration system.
/// </summary>
public class DegreeProgramBindingModel
{
    /// <summary>
    ///     Gets or sets the list of courses associated with the degree program.
    /// </summary>
    /// <value>
    ///     A collection of <see cref="CourseBindingModel" /> objects representing the courses included in the degree program.
    /// </value>
    /// <remarks>
    ///     This property is used to define the courses that are part of a specific degree program.
    ///     Each course provides details such as its name, credits, and other relevant information.
    /// </remarks>
    public List<CourseBindingModel> Courses { get; set; } = new();

    /// <summary>
    ///     Gets or sets the unique identifier for the degree program.
    /// </summary>
    /// <remarks>
    ///     This property is used to uniquely identify a specific degree program instance.
    /// </remarks>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the name of the degree program.
    /// </summary>
    /// <value>
    ///     The name of the degree program, which can be <c>null</c>.
    /// </value>
    public string Name { get; set; } = string.Empty;
}