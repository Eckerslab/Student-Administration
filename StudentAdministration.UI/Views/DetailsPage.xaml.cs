using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;

namespace StudentAdministration.UI.Views;

public partial class DetailsPage : ContentPage
{
    private readonly ILogger<DetailsPage> logger;

    public DetailsPage(IDetailsPageViewModel detailsPageViewModel, ILogger<DetailsPage> logger)
    {
        BindingContext = detailsPageViewModel;
        InitializeComponent();
        this.logger = logger;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (BindingContext is not IDetailsPageViewModel viewModel) return;

        try
        {
            await viewModel.InitializeAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception,
                AppResources.NavigationErrorMessage);

            await Shell.Current.DisplayAlert(AppResources.ErrorTitle, AppResources.NavigationErrorMessage,
                AppResources.OkayTitle);
        }
    }
}