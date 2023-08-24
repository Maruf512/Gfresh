using System.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Controls;


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
            // get data from db for item's list
            ReadFromDB();

        }

        // Grid Views Items List
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
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
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

            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
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
            bool dataExists = false;

            // Check db if data exists or not
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = "SELECT CName FROM CustomerInfo";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {

                String CustomerData = (string)dataReader.GetValue(0);

                if (Customer_name.Text == CustomerData)
                {
                    dataExists = true;
                }


            }

            dataReader.Close();
            command.Dispose();
            cnn.Close();




            // assign data to db
            if (dataExists)
            {
                MessageBox.Show("Data Exists");
            }
            else
            {
                if (Customer_name.Text != "" && Customer_phone.Text != "" && Customer_Address.Text != "")
                {
                    String connectionString2 = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                    String sql2 = $"INSERT INTO CustomerInfo (CName,CPhone,CAddress) VALUES('{Customer_name.Text}',{Customer_phone.Text},'{Customer_Address.Text}')";
                    SqlDataAdapter adapter = new SqlDataAdapter(); // for adding value


                    SqlConnection cnn2 = new SqlConnection(connectionString2);

                    cnn2.Open();
                    SqlCommand command2 = new SqlCommand(sql2, cnn2);

                    adapter.InsertCommand = new SqlCommand(sql2, cnn2);
                    adapter.InsertCommand.ExecuteNonQuery();

                    command2.Dispose();
                    cnn2.Close();

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



        }

        // clear customer input fields
        private void ClearFields(object sender, RoutedEventArgs e)
        {
            Customer_name.Text = "";
            Customer_phone.Text = "";
            Customer_Address.Text = "";
            CustomerDropDown.SelectedIndex = -1;

            AddBillBtn.IsEnabled = true;
        }

        // ===================== Billing Section ====================
        // ====== Recive data from db [ItemTbl] Table
        // ====== and add it to billing sections combobox
        // =====================================
        private void ReadFromDB()
        {


            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = "SELECT ItName FROM ItemTbl";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {

                String CustomerData = (string)dataReader.GetValue(0);

                // add data to combobox
                ItemDropDown.Items.Add(CustomerData);


            }

            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }



        // reset btn
        private void BillResetFields(object sender, RoutedEventArgs e)
        {
            ItemDropDown.SelectedIndex = -1;
            Bill_Quantity.Text = "";
            Bill_Price.Text = "";
        }

        // ====================== Item Select Event handeller =====================
        private void ItemDropDown_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (ItemDropDown.SelectedIndex != -1)
            {
                string SelectedItem = ItemDropDown.SelectedItem.ToString();

                String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                String processSqlCmd = "SELECT * FROM ItemTbl";

                String sql = processSqlCmd;
                SqlConnection cnn = new SqlConnection(connectionString);
                cnn.Open();
                SqlCommand command = new SqlCommand(sql, cnn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {

                    String ItemName = (string)dataReader.GetValue(1);

                    if (ItemName == SelectedItem)
                    {
                        int ItemQty = (int)dataReader.GetValue(2);
                        int ItemPrice = (int)dataReader.GetValue(3);

                        Bill_Quantity.Text = ItemQty.ToString();
                        Bill_Price.Text = ItemPrice.ToString();
                    }

                }
                dataReader.Close();
                command.Dispose();
                cnn.Close();
            }
            else if(ItemDropDown.SelectedIndex == -1)
            {
                ItemDropDown.SelectedIndex = -1;
                Bill_Quantity.Text = "";
                Bill_Price.Text = "";
            }
            
        }

        private void CustomerDropDown_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(CustomerDropDown.SelectedIndex != -1)
            {
                try
                {
                    string SelectedInfo = "";
                    SelectedInfo = CustomerDropDown.SelectedItem.ToString();

                    String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                    String processSqlCmd = "SELECT * FROM CustomerInfo";

                    String sql = processSqlCmd;
                    SqlConnection cnn = new SqlConnection(connectionString);
                    cnn.Open();
                    SqlCommand command = new SqlCommand(sql, cnn);
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {

                        String CustomerName = (string)dataReader.GetValue(1);

                        if (CustomerName == SelectedInfo)
                        {
                            int CustomerPhone = (int)dataReader.GetValue(2);
                            String CustomerAddress = (String)dataReader.GetValue(3);

                            Customer_name.Text = CustomerName;
                            Customer_phone.Text = CustomerPhone.ToString();
                            Customer_Address.Text = CustomerAddress.ToString();

                            AddBillBtn.IsEnabled = false;

                        }

                    }
                    dataReader.Close();
                    command.Dispose();
                    cnn.Close();
                }
                catch (NullReferenceException)
                {
                    AddBillBtn.IsEnabled = true;
                }
            }
            else if(CustomerDropDown.SelectedIndex == -1)
            {
                Customer_name.Text = "";
                Customer_phone.Text = "";
                Customer_Address.Text = "";
                CustomerDropDown.SelectedIndex = -1;

                AddBillBtn.IsEnabled = true;
            }

            
            
            
        }


        // Grid Views Billing List
        public class AddBill
        {
            public int ID { get; set; }
            public string Item { get; set; }
            public int Price { get; set; }
            public int Quantity { get; set; }
            public int Total { get; set; }

        }

        int counter = 0;
        int TotalBill = 0;

        private void AddToBill(object sender, RoutedEventArgs e)
        {

            if(ItemDropDown.SelectedIndex != -1)
            {
                string SelectedItem = ItemDropDown.SelectedItem.ToString();

                String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                String processSqlCmd = "SELECT * FROM ItemTbl";

                String sql = processSqlCmd;
                SqlConnection cnn = new SqlConnection(connectionString);
                cnn.Open();
                SqlCommand command = new SqlCommand(sql, cnn);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {

                    String ItemName = (string)dataReader.GetValue(1);

                    if (ItemName == SelectedItem)
                    {
                        // datagrid object
                        AddBill item = new AddBill();
                        // process data
                        counter = counter + 1;
                        item.ID = counter;

                        item.Item = (string)dataReader.GetValue(1);
                        item.Quantity = Int32.Parse(Bill_Quantity.Text.ToString());
                        item.Price = Int32.Parse(Bill_Price.Text.ToString());

                        int SubTotal = Int32.Parse(Bill_Price.Text.ToString()) * Int32.Parse(Bill_Quantity.Text.ToString());
                        item.Total = SubTotal;

                        // add to data grid
                        ItemsDbBillGridXAML.Items.Add(item);
                        // calculate total bill
                        TotalBill = TotalBill + SubTotal;
                        // update total bill to gui
                        Total_Bill.Text = TotalBill.ToString() + "Tk";
                        

                    }

                }
                dataReader.Close();
                command.Dispose();
                cnn.Close();

                // clear fields
                Customer_name.Text = "";
                Customer_phone.Text = "";
                Customer_Address.Text = "";
                CustomerDropDown.SelectedIndex = -1;

            }
            else
            {
                MessageBox.Show("Select an Item.");
            }



            

        }
    }
}
