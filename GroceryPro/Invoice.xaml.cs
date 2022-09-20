using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Packaging;
using System.IO;
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
using System.Windows.Xps.Packaging;
using System.Windows.Xps;

namespace GroceryPro
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : Window
    {
        public Invoice()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(print, "invoice");
                }
            }
            finally
            {
                this.IsEnabled = true;
            }


        }

        private void BorderDrag(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }


        private void ExitBtn(object sender, MouseButtonEventArgs e)
        {
            // exit print page and go back to billing page
            this.Hide();
            Billing billing = new Billing();
            billing.Show();
        }

        private void SaveAsPdbBtn(object sender, RoutedEventArgs e)
        {
            //Convert WPF -> XPS -> PDF

            MemoryStream lMemoryStream = new MemoryStream();
            Package package = Package.Open(lMemoryStream, FileMode.Create);
            XpsDocument doc = new XpsDocument(package);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);

            // Pdf Section window
            writer.Write(print);

            doc.Close();
            package.Close();

            // Convert 
            MemoryStream outStream = new MemoryStream();
            PdfSharp.Xps.XpsConverter.Convert(lMemoryStream, outStream, false);

            // Write pdf file
            FileStream fileStream = new FileStream("D:\\Images\\test.pdf", FileMode.Create);
            outStream.CopyTo(fileStream);

            // Clean up
            outStream.Flush();
            outStream.Close();
            fileStream.Flush();
            fileStream.Close();

            MessageBoxResult messageBoxResult = MessageBox.Show("File Saved.", "Gfresh", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
