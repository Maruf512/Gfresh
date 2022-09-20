using System;
using System.Threading.Tasks;
using System.Windows;


namespace GroceryPro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// System.Windows.Application.Current.Shutdown(); Exit
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ProgressBarss();

        }

        private void ShiftWindow()
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private async void ProgressBarss()
        {
            double progress = 0.2;
            double countDown = 0;
            while (SplashProgress.Value != 100)
            {
                SplashProgress.Value = countDown;
                SplashParcentance.Content = (int)SplashProgress.Value;
                countDown = progress + countDown;
                await Task.Delay(2);
            }
            /// Login Window
            ShiftWindow();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
