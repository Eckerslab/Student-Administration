namespace StudentAdministrationServices.Services.Interfaces;

public interface IDeleteService
{
    /// <summary>
    ///     Guid id. This method gets all values of type T.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);
}