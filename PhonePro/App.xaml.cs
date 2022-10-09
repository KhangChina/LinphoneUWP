using System;

using PhonePro.Services;
using Linphone;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using System.Diagnostics;
using System.Text;

namespace PhonePro
{
    public sealed partial class App : Application
    {
        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }
        private CoreService CoreService { get; } = CoreService.Instance;
        public App()
        {
            InitializeComponent();
            UnhandledException += OnAppUnhandledException;

            // Deferred execution until used. Check https://docs.microsoft.com/dotnet/api/system.lazy-1 for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
            // Start Linphone
            LoggingService.Instance.LogLevel = LogLevel.Debug;
            LoggingService.Instance.Listener.OnLogMessageWritten = OnLog;

            CoreService.CoreStart(Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Dispatcher);
        }
        private void OnLog(LoggingService logService, string domain, LogLevel lev, string message)
        {
            StringBuilder builder = new StringBuilder();
            _ = builder.Append("Linphone-[").Append(lev.ToString()).Append("](").Append(domain).Append(")").Append(message);
            Debug.WriteLine(builder.ToString());
        }
        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/uwp/api/windows.ui.xaml.application.unhandledexception
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
