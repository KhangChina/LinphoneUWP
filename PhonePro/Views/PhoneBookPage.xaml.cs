using System;

using PhonePro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PhonePro.Views
{
    public sealed partial class PhoneBookPage : Page
    {
        public PhoneBookViewModel ViewModel { get; } = new PhoneBookViewModel();

        public PhoneBookPage()
        {
            InitializeComponent();
        }
    }
}
