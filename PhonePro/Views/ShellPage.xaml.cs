using System;
using PhonePro.Data;
using PhonePro.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PhonePro.Views
{
    // TODO: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
            showLogin();
        }
        public async void showLogin()
        {
            if (User.username == "")
            {
                loginDialogPage dialog = new loginDialogPage();
                dialog.XamlRoot = this.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    if (User.username == "")
                    {
                        showLogin();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Application.Current.Exit();
                }
            }
        }
    }
}
