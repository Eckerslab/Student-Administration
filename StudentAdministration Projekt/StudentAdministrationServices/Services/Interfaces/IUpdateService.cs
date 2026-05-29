namespace StudentAdministrationServices.Services.Interfaces;

public interface IUpdateService<T> where T : class
{
    /// <summary>
    ///     Updates the details of an existing student using the provided binding model.
    /// </summary>
    /// <param name="student">The binding model containing the updated student information. Cannot be null.</param>
    /// <param name="value"></param>
    /// <returns>A task that represents the asynchronous edit operation.</returns>
    Task UpdateAsync(T value);
}