using System.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

namespace GroceryPro
{
    /// <summary>
    /// Interaction logic for Billing.xaml
    /// </summary>
    public partial class Billing : Window
    {
        public Billing()
        {
            InitializeComponent();

            // get data for instock
            readDataFromDB();
            // get data from db for Customer name
            RefreshComboBox();


        }

        // Grid Vies Items List
        public class AddItems
        {
            public int ID { get; set; }
            public string Items { get; set; }
            public int Price { get; set; }
            public int Quantity { get; set; }
            public string Catagory { get; set; }
        }

        // ========================= Local Functions ======================

        private void readDataFromDB()
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\Documents\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = "SELECT ItId,ItName,ItQty,ItPrice,ItCat FROM ItemTbl";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                AddItems item = new AddItems();

                item.ID = (int)dataReader.GetValue(0);
                item.Items = (string)dataReader.GetValue(1);
                item.Quantity = (int)dataReader.GetValue(2);
                item.Price = (int)dataReader.GetValue(3);
                item.Catagory = (string)dataReader.GetValue(4);

                ItemsDbGridXAML.Items.Add(item);
            }

            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }

        private void ClearCustomerFields()
        {
            Customer_name.Text = "";
            Customer_phone.Text = "";
            Customer_Address.Text = "";
            CustomerDropDown.Text = "";
        }

        // add data to combobox or refresh from db
        private void RefreshComboBox()
        {
            // clear Customer Name dropdown before updating
            CustomerDropDown.Items.Clear();

            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\Documents\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = "SELECT CName FROM CustomerInfo";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                AddItems item = new AddItems();

                String CustomerData = (string)dataReader.GetValue(0);

                // add data to combobox
                CustomerDropDown.Items.Add(CustomerData);

            }

            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }




        private void ExitBtn(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void GotoInvoiceWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Invoice invoice = new Invoice();
            invoice.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GotoLogoutWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }


        


        // Customer section
        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            // assign values of input fields to the db table
            // read that data and show it in combobox

            // add to combobox
            


            // assign data to db
            if (Customer_name.Text != "" && Customer_phone.Text != "" && Customer_Address.Text != "")
            {
                String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\Documents\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                String sql = $"INSERT INTO CustomerInfo (CName,CPhone,CAddress) VALUES('{Customer_name.Text}',{Customer_phone.Text},'{Customer_Address.Text}')";
                SqlDataAdapter adapter = new SqlDataAdapter(); // for adding value


                SqlConnection cnn = new SqlConnection(connectionString);

                cnn.Open();
                SqlCommand command = new SqlCommand(sql, cnn);

                adapter.InsertCommand = new SqlCommand(sql, cnn);
                adapter.InsertCommand.ExecuteNonQuery();

                command.Dispose();
                cnn.Close();

                // refresh combobox
                RefreshComboBox();

                // clear all the fields
                ClearCustomerFields();
            }
            else
            {
                MessageBox.Show("Empty Fields !!");
            }



        }

        // clear customer input fields
        private void ClearFields(object sender, RoutedEventArgs e)
        {
            ClearCustomerFields();
        }

        // ===================== Billing Section ====================


        // reset btn
        private void BillResetFields(object sender, RoutedEventArgs e)
        {
            ItemDropDown.SelectedIndex = 0;
            Bill_Quantity.Text = "";
            Bill_Price.Text = "";
        }
    }
}
