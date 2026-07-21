using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;

namespace StudentAdministration.UI.Views;

public partial class AddEditStudentPage : ContentPage
{
    private readonly ILogger<AddEditStudentPage> logger;

    public AddEditStudentPage(IAddEditStudentPageViewModel addEditStudentPageViewModel,
        ILogger<AddEditStudentPage> logger)
    {
        InitializeComponent();
        BindingContext = addEditStudentPageViewModel;
        this.logger = logger;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (BindingContext is not IAddEditStudentPageViewModel viewModel) return;

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