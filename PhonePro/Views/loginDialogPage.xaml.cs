using System;
using PhonePro.Data;
using PhonePro.ViewModels;

using Windows.UI.Xaml.Controls;

namespace PhonePro.Views
{
    public sealed partial class loginDialogPage : ContentDialog
    {
       

        public loginDialogPage()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            User.username = txtUsername.Text;
            User.password = txtPassword.Password;
            User.domain = txtDomain.Text;

        }

        private void revealModeCheckBox_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (revealModeCheckBox.IsChecked == true)
            {
                txtPassword.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                txtPassword.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }
    }
}
