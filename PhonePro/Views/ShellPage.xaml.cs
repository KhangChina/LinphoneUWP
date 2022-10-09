using System;
using Linphone;
using PhonePro.Data;
using PhonePro.Services;
using PhonePro.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace PhonePro.Views
{
    // TODO: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();
        private CoreService CoreService { get; } = CoreService.Instance;
        private Core StoredCore { get; set; }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
            
            showLogin();

            CoreService.AddOnAccountRegistrationStateChangedDelegate(OnAccountRegistrationStateChanged);
        }

        public async void showLogin()
        {
            if (!User.isLogin)
            {
                loginDialogPage dialog = new loginDialogPage();
                dialog.XamlRoot = this.XamlRoot;
                dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
               
                if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                {
                   // CoreService.AddOnAccountRegistrationStateChangedDelegate(OnAccountRegistrationStateChanged);    
                }
                else
                {
                    Application.Current.Exit();
                }
            }
        }
        private void OnAccountRegistrationStateChanged(Core core, Account account, RegistrationState state, string message)
        {
            //RegistrationText.Text = "Your registration state is : " + state.ToString();
            switch (state)
            {
                case RegistrationState.Cleared:
                case RegistrationState.None:
                    CoreService.ClearCoreAfterLogOut();
                    //LogIn.IsEnabled = true;
                    break;

                case RegistrationState.Ok:
                    User.isLogin = true;
                    navigationView.IsPaneVisible = true;
                    shellFrame.Navigate(typeof(MainPage));
                    break;

                case RegistrationState.Progress:
                    shellFrame.Navigate(typeof(loading));
                    navigationView.IsPaneVisible = false;
                    break;

                case RegistrationState.Failed:
                    User.isLogin = false;
                    navigationView.IsPaneVisible = true;
                    shellFrame.Navigate(typeof(MainPage));
                    showLogin();
                    break;

                default:
                    break;
            }

        }
    }
}
