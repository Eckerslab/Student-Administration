namespace StudentAdministration.UI.ViewModel.Interfaces;

public interface IInitialize
{
    /// <summary>
    ///     Asynchronously initializes the component, preparing it for use.
    /// </summary>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    Task InitializeAsync();
}