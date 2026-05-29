namespace StudentAdministrationServices.Services.Interfaces;

public interface IAddService<T> where T : class
{
    /// <summary>
    ///     This method gets all values of type T
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    Task AddAsync(T value);
}