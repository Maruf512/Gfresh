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
        public List<string> invoices;
        
        public Invoice(List<string> to_print)
        {
            invoices = to_print;
            InitializeComponent();

            // call methods
            addBillToInvoice();

        }

        // Grid Views Invoice List objects
        public class AddBill
        {
            public int ID { get; set; }
            public string Item { get; set; }
            public int Price { get; set; }
            public int Quantity { get; set; }
            public int Total { get; set; }

        }

        // external modules
        private void addBillToInvoice()
        {
            foreach (string item in invoices)
            {
                AddBill addBill = new AddBill();
                String[] split_item = item.Split('@');

                addBill.ID = int.Parse(split_item[0]);
                addBill.Item = split_item[1];
                addBill.Price = int.Parse(split_item[2]);
                addBill.Quantity = int.Parse(split_item[3]);
                addBill.Total = int.Parse(split_item[4]);

                InvoicedataGrid.Items.Add(addBill);
            }
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
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ==== Make it dinamic === !!!!!!!!!!!!!!!!!!!!!!!!!!
            /*
                string root = @"C:\Temp";
                string subdir = @"C:\Temp\Mahesh";
                // If directory does not exist, create it.
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
             */
            FileStream fileStream = new FileStream($"D:\\Documents\\Gfresh_data\\invoice.pdf", FileMode.Create);
            outStream.CopyTo(fileStream);

            // Clean up
            outStream.Flush();
            outStream.Close();
            fileStream.Flush();
            fileStream.Close();

            MessageBoxResult messageBoxResult = MessageBox.Show("File Saved.", "Gfresh", MessageBoxButton.OK, MessageBoxImage.None);

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

       
    }
}
