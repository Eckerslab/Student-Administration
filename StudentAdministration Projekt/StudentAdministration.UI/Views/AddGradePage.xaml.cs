using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using System.Diagnostics;

namespace StudentAdministration.UI.Views;

public partial class AddGradePage : ContentPage
{
    private readonly ILogger<AddGradePage> logger;

    public AddGradePage(IAddGradePageViewModel addGradePageViewModel, ILogger<AddGradePage> logger)
    {
        this.BindingContext = addGradePageViewModel;
        this.InitializeComponent();
        this.logger = logger;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.BindingContext is not IAddGradePageViewModel viewModel)
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