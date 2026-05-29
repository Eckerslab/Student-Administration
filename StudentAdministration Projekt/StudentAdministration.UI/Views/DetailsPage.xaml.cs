using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using System.Diagnostics;

namespace StudentAdministration.UI.Views;

public partial class DetailsPage : ContentPage
{
    private readonly ILogger<DetailsPage> logger;

    public DetailsPage(IDetailsPageViewModel detailsPageViewModel, ILogger<DetailsPage> logger)
    {
        this.BindingContext = detailsPageViewModel;
        this.InitializeComponent();
        this.logger = logger;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.BindingContext is not IDetailsPageViewModel viewModel)
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