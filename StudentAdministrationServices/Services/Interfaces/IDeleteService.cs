namespace StudentAdministrationServices.Services.Interfaces;

public interface IDeleteService
{
    /// <summary>
    ///     Deletes the entity with the given unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteAsync(Guid id);
}