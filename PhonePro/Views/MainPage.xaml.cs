using System;
using PhonePro.ViewModels;
using Linphone;
using Windows.UI.Xaml.Controls;
using PhonePro.Services;
using Microsoft.UI.Xaml.Controls;
using PhonePro.Data;

namespace PhonePro.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        private CoreService CoreService { get; } = CoreService.Instance;

        public MainPage()
        {
            InitializeComponent();
            CoreService.AddOnAccountRegistrationStateChangedDelegate(OnAccountRegistrationStateChanged);
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
                    txtUser.Text = User.username;
                    txtDomain.Text = User.domain;
                    info.Severity = InfoBarSeverity.Success;
                    info.Message = message;
                    break;

                case RegistrationState.Progress:
                    info.Severity = InfoBarSeverity.Warning;
                    info.Message = message;
                    break;

                case RegistrationState.Failed:
                    User.isLogin = false;
                    info.Severity = InfoBarSeverity.Error;
                    info.Message = message;
                    break;

                default:
                    break;
            }

        }
    }
}
