namespace StudentAdministrationServices.Services.Interfaces;

public interface IReceiveService<T> where T : class
{
    /// <summary>
    ///     Gets all values of type <typeparamref name="T" />.
    /// </summary>
    /// <returns>A task whose result is the list of all entities.</returns>
    Task<List<T>> GetAllAsync();

    /// <summary>
    ///     Gets a single value of type <typeparamref name="T" /> by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>A task whose result is the matching entity.</returns>
    Task<T> GetByIdAsync(Guid id);
}