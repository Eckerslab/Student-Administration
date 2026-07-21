using System.ComponentModel.DataAnnotations;

namespace StudentAdministrationDatabase.Models.BaseModels;

/// <summary>
///     Base model that provides common properties for derived models.
/// </summary>
public abstract class BaseModelCommon : BaseKeyModel
{
    /// <summary>
    ///     gets or sets the name of the model.
    /// </summary>
    [MaxLength(100)]
    public string? Name { get; set; }
}