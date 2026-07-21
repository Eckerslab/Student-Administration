using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;

namespace StudentAdministration.UI.Views;

public partial class AddGradePage : ContentPage
{
    private readonly ILogger<AddGradePage> logger;

    public AddGradePage(IAddGradePageViewModel addGradePageViewModel, ILogger<AddGradePage> logger)
    {
        BindingContext = addGradePageViewModel;
        InitializeComponent();
        this.logger = logger;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (BindingContext is not IAddGradePageViewModel viewModel) return;

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