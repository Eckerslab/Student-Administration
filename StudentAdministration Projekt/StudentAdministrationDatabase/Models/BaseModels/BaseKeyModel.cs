namespace StudentAdministrationDatabase.Models.BaseModels;

/// <summary>
///     Base model that provides a unique identifier property for derived models.
/// </summary>
public class BaseKeyModel
{
    /// <summary>
    ///     gets or sets the unique identifier for the model.
    /// </summary>
    public required Guid Id { get; set; }
}