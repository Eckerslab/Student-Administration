namespace StudentAdministrationServices.Services.Interfaces;

public interface IAddService<T> where T : class
{
    /// <summary>
    ///     Adds a new value of type <typeparamref name="T" />.
    /// </summary>
    /// <param name="value">The entity to add.</param>
    /// <returns>A task that represents the asynchronous add operation.</returns>
    Task AddAsync(T value);
}