namespace StudentAdministrationDatabase.Repositories.Interfaces;

/// <summary>
///     Represents a base repository interface that defines common data access operations for entities of type
///     <typeparamref name="T" />.
/// </summary>
/// <typeparam name="T">The type of the entity that the repository manages. Must be a reference type.</typeparam>
public interface IBaseRepository<T> where T : class
{
    /// <summary>
    ///     Asynchronously adds a new entity of type <typeparamref name="T" /> to the repository.
    /// </summary>
    /// <param name="value">The entity to be added. Must not be <c>null</c>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(T value);

    /// <summary>
    ///     Asynchronously deletes an entity of type <typeparamref name="T" /> from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    ///     Asynchronously retrieves all entities of type <typeparamref name="T" /> from the repository.
    /// </summary>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a collection of all entities of
    ///     type <typeparamref name="T" />.
    /// </returns>
    Task<List<T>> GetAllAsync();

    /// <summary>
    ///     Gets an entity of type <typeparamref name="T" /> from the repository by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> GetByIdAsync(Guid id);
}