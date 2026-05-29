using Microsoft.Extensions.Logging;
using StudentAdministration.UI.Resources.Languages;
using StudentAdministration.UI.ViewModel.Interfaces;
using System.Diagnostics;

namespace StudentAdministration.UI.Views;

public partial class AddEditStudentPage : ContentPage
{
    private readonly ILogger<AddEditStudentPage> logger;

    public AddEditStudentPage(IAddEditStudentPageViewModel addEditStudentPageViewModel, ILogger<AddEditStudentPage> logger)
    {
        this.InitializeComponent();
        this.BindingContext = addEditStudentPageViewModel;
        this.logger = logger;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        if (this.BindingContext is not IAddEditStudentPageViewModel viewModel)
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