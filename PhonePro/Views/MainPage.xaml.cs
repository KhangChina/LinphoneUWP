using System;
using PhonePro.ViewModels;
using Linphone;
using Windows.UI.Xaml.Controls;
using PhonePro.Services;
using Microsoft.UI.Xaml.Controls;
using PhonePro.Data;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using System.Threading;

namespace PhonePro.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();
        private CoreService CoreService { get; } = CoreService.Instance;
        private Call IncomingCall;
        public MainPage()
        {
            InitializeComponent();
           
            //txtUser.Text = User.username;
            //txtDomain.Text = User.domain;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            txtUser.Text = User.username;
            txtDomain.Text = User.domain;
            //HelloText.Text += CoreService.Core.DefaultProxyConfig.FindAuthInfo().Username;
            CoreService.AddOnCallStateChangedDelegate(OnCallStateChanged);

            if (CoreService.Core.CurrentCall != null)
            {
                OnCallStateChanged(CoreService.Core, CoreService.Core.CurrentCall, CoreService.Core.CurrentCall.State, null);
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //VideoService.StopVideoStream();
            CoreService.RemoveOnCallStateChangedDelegate(OnCallStateChanged);
            base.OnNavigatedFrom(e);
        }
        private void OnCallStateChanged(Core core, Call call, CallState state, string message)
        {
            loadingName.Visibility = Visibility.Collapsed;
            info.Severity = InfoBarSeverity.Warning;
            info.Message = "Your call state is : " + state.ToString();
            switch (state)
            {
                case CallState.IncomingReceived:
                    IncomingCall = call;
                    info.Severity = InfoBarSeverity.Informational;
                    info.Message= " " + IncomingCall.RemoteAddress.AsString();
                    UIIncomingCallAsync(IncomingCall.RemoteAddress.AsString());
                    //IncomingCallStackPanel.Visibility = Visibility.Visible;
                    //IncommingCallText.Text = 

                    break;

                // The different states a call goes through before your peer answers.
                case CallState.OutgoingInit:
                case CallState.OutgoingProgress:
                case CallState.OutgoingRinging:
                    info.Severity = InfoBarSeverity.Informational;
                    info.Message = "Your call state is : " + state.ToString();
                    btnHangUp.IsEnabled = true;
                    break;

                // The StreamsRunning state is the default one during a call.
                case CallState.StreamsRunning:
                // The UpdatedByRemote state is triggered when the call's parameters are updated
                // for example when video is asked/removed by remote.
                case CallState.UpdatedByRemote:

                    CallInProgressGuiUpdates();
                    if (call.CurrentParams.VideoEnabled)
                    {
                        StartVideoAndUpdateGui();
                    }
                    else
                    {
                        StopVideoAndUpdateGui();
                    }
                    break;

                case CallState.Error:                
                case CallState.End:
                case CallState.Released:
                    IncomingCall = null;
                    EndingCallGuiUpdates();
                   // VideoService.StopVideoStream();
                    break;
            }
        }
        private void CallInProgressGuiUpdates()
        {
            // IncomingCallStackPanel.Visibility = Visibility.Collapsed;
            info.Severity = InfoBarSeverity.Informational;
            info.Message = "Call process: " + txtCall.Text;
            btnCall.IsEnabled = false;
            btnHangUp.IsEnabled = true;
            btnSound.IsEnabled = true;
            btnVideoCall.IsEnabled = true;
            btnMic.IsEnabled = true;
        }
        private void StartVideoAndUpdateGui()
        {
            //VideoGrid.Visibility = Visibility.Visible;
            //Camera.Content = "Switch off Camera";
            //VideoService.StartVideoStream(VideoSwapChainPanel, PreviewSwapChainPanel);
            //Camera.IsEnabled = true;
        }
        private void EndingCallGuiUpdates()
        {

            info.Severity = InfoBarSeverity.Informational;
            info.Message = "Free time";
            btnCall.IsEnabled = true;
            btnHangUp.IsEnabled = false;
            btnSound.IsEnabled = false;
            btnVideoCall.IsEnabled = false;
            btnMic.IsEnabled = false;
            //VideoGrid.Visibility = Visibility.Collapsed;
            //Camera.Content = "Switch on Camera";
            //Mic.Content = "Mute";
            //Sound.Content = "Switch off Sound";
        }
        private void StopVideoAndUpdateGui()
        {
            //Camera.Content = "Switch on Camera";
            btnVideoCall.IsEnabled = true;
            //VideoGrid.Visibility = Visibility.Collapsed;
            //VideoService.StopVideoStream();
        }

        private void btnCall_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            btnCall.IsEnabled = false;
            loadingName.Visibility = Visibility.Visible;
            CoreService.Call(txtCall.Text);
        }

        private void btnHangUp_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CoreService.Core.TerminateAllCalls();
        }

        public async void UIIncomingCallAsync(string number)
        {
            ContentDialog dialog = new ContentDialog();

            // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
            dialog.XamlRoot = this.XamlRoot;
            dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
            dialog.Title = "Answer the phone ?";
            dialog.PrimaryButtonText = "Answer";
            dialog.CloseButtonText = "Cancel";
            dialog.DefaultButton = ContentDialogButton.Primary;
            dialog.Content = "Imcomming from: "+ number;
            var result = await dialog.ShowAsync();
            if(dialog.DefaultButton == ContentDialogButton.Primary)
            {
                if (IncomingCall != null)
                {
                    await CoreService.OpenMicrophonePopup();
                    IncomingCall.Accept();
                    IncomingCall = null;
                }
                else
                {
                    if (IncomingCall != null)
                    {
                        IncomingCall.Decline(Reason.Declined);
                        IncomingCall = null;
                    }
                }
            }
        }
    }
}
