using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroceryPro
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

        }

        // Close Exit Login
        private void Exit_Login_Btn(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Move Window (Drag)
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /// Closing Threads....
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Watermark (Placeholder)
        private void TboxEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            EmailWatermark.Visibility = Visibility.Hidden;
            
        }

        private void TboxEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TboxEmail.Text == "")
            {
                EmailWatermark.Visibility = Visibility.Visible;
            }
        }

        private void TboxPass_GotFocus(object sender, RoutedEventArgs e)
        {
            PassWatermark.Visibility = Visibility.Hidden;

        }

        private void TboxPass_LostFocus(object sender, RoutedEventArgs e)
        {
            if(TboxPass.Password == "")
            {
                PassWatermark.Visibility = Visibility.Visible;
            }
        }
        // Login
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var Login_email = TboxEmail.Text;
            var Login_password = TboxPass.Password;
            if(Login_email != "" && Login_password != "")
            {
                // if admin is logged in then go to manage items section
                // else go to billing section as a seller
                this.Hide();
                ManageItems manageItems = new ManageItems();
                manageItems.Show();
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Empty Fields.", "Gfresh", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GotoBillingWindow(object sender, RoutedEventArgs e)
        {
            // open Seller window
            this.Hide();
            Billing billing = new Billing();
            billing.Show();
        }

        private void GotoSignupWindow(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("It's not Available now!!!", "Gfrash");
        }
    }
}
