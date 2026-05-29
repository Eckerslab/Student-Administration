namespace StudentAdministrationServices.Services.Interfaces;

public interface IReceiveService<T> where T : class
{
    /// <summary>
    ///     IEnumerable of T. This method gets all values of type T.
    /// </summary>
    /// <returns></returns>
    Task<List<T>> GetAllAsync();

    /// <summary>
    ///     Guid id, T value. This method updates a value of type T.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> GetByIdAsync(Guid id);
}