namespace StudentAdministrationServices.Models;

/// <summary>
///     Class DegreeProgramBindingModel.
/// </summary>
public class DegreeProgramListModel
{
    /// <summary>
    ///     Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}