using System;

using PhonePro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PhonePro.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
