using System;
using PhonePro.Data;
using PhonePro.Services;
using PhonePro.ViewModels;
using Linphone;
using Windows.UI.Xaml.Controls;
using System.IO;
using Windows.Storage;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Windows.UI.Xaml;

namespace PhonePro.Views
{
    public sealed partial class loginDialogPage : ContentDialog
    {
        private CoreService CoreService { get; } = CoreService.Instance;
       
       
        public loginDialogPage()
        {
           
            InitializeComponent();
            //Dev default testing
            txtDomain.Text = "192.168.100.251";
            txtUsername.Text = "369";
            txtPassword.Password = "htgsoft@";

           

        }
  
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
          
            //Login domain
            string identity = "sip:" + txtUsername.Text + "@" + txtDomain.Text;
          
            CoreService.LogIn(identity, txtPassword.Password);
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
