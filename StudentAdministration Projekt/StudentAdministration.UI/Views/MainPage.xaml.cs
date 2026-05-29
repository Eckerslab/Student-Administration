using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using System.Diagnostics;

namespace StudentAdministration.UI.Views;

public partial class MainPage : ContentPage
{
    private readonly ILogger<MainPage> logger;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MainPage" /> class.
    /// </summary>
    public MainPage(IMainPageViewModel pageViewModel, ILogger<MainPage> logger)
    {
        this.InitializeComponent();
        this.BindingContext = pageViewModel;
        this.logger = logger;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.BindingContext is not IMainPageViewModel viewModel)
        {
            return;
        }

        try
        {
            await viewModel.InitializeAsync();
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage, AppResources.OkayTitle);
        }
    }
}