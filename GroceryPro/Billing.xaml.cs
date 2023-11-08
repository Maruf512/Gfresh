﻿using System.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using MaterialDesignThemes.Wpf;
using System.Windows.Documents;
using System.CodeDom;
using System.Security.Policy;

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

            // ========================= GET STOCK DATA FROM DB
            readDataFromDB();
            // ========================= GET CUSTOMER INFO
            RefreshCustomerDropdown();
            // get data from db for item's list
            ReadFromDB();

        }

        // ===========================================================================
        // ============================= GLOBAL VERIABLES ============================
        // ===========================================================================
        List<string> to_print = new List<string>();

        // ===========================================================================
        // ============================= DATAGRID OBJECTS ============================
        // ===========================================================================
        
        // ============================= STOCK DATAGRID OBJECTS
        public class AddItems
        {
            public int ID { get; set; }
            public string Items { get; set; }
            public int Price { get; set; }
            public int Quantity { get; set; }
            public string Catagory { get; set; }
        }

        // ============================ BILLING DATAGRID OBJECTS
        public class AddBill
        {
            public int ID { get; set; }
            public string Item { get; set; }
            public int Price { get; set; }
            public int Quantity { get; set; }
            public int Total { get; set; }

        }


        // ===========================================================================
        // ============================= Local Functions =============================
        // ===========================================================================

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!! TEST FUCNTIONS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        public static int GetCustomerIdByPhone(string phone)
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = $"SELECT Cid from CustomerInfo where CPhone = '{phone}'";

            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            // get value
            int data = (int)command.ExecuteScalar();

            // close connection
            command.Dispose();
            cnn.Close();

            return data;

        }

        //============================== CHECK IF CUSTOMER EXISTS OR NOT
        public static bool CheckIfCustomerPhoneExistsOrNot(string PhoneNo)
        {

            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = $"SELECT COUNT(*) from CustomerInfo where CPhone like '{PhoneNo}'";

            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);

            // get value
            int userCount = (int)command.ExecuteScalar();

            // close connection
            command.Dispose();
            cnn.Close();
            // check if Customer phone exists or not
            if (userCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }







        // ===================== Billing Section ====================
        // ====== Recive data from db [ItemTbl] Table
        // ====== and add it to billing sections combobox
        // =====================================
        private void ReadFromDB()
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
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

        //====================== READ DATA FROM THE DATABASE
        private void readDataFromDB()
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
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

        // ======================== CLEAR ALL CUSTOMER INPUT FIELDS
        private void ClearCustomerFields()
        {
            Customer_name.Text = "";
            Customer_phone.Text = "";
            Customer_Address.Text = "";
            CustomerDropDown.Text = "";
        }

        // ======================== ADD DATA TO COMBOBOX OR REFRESH FROM DB
        private void RefreshCustomerDropdown()
        {
            //===================== CLEAR CUSTOMER NAME DROPDOWN
            CustomerDropDown.Items.Clear();
            //===================== GET DATA FROM DB AND ASIGN IT ON THE DROPDOWN
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = "SELECT CName,CPhone FROM CustomerInfo";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                AddItems item = new AddItems();

                String CustomerName = (string)dataReader.GetValue(0);
                String CustomerPhone = (string)dataReader.GetValue(1);

                // add data to combobox
                CustomerDropDown.Items.Add(CustomerName + ", " + CustomerPhone);


            }

            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }

        // ==================== PROCESS BILLING DATAGRIDS
        private List<string> process_dataGrid()
        {
            // collect all data from data grid and store it on a list
            List<string> data_list = new List<string>();
            int ctr = 0;
            while (ctr < ItemsDbBillGridXAML.Items.Count)
            {
                AddBill bill_DG_obj = (AddBill)ItemsDbBillGridXAML.Items[ctr];
                string p_data = $"{bill_DG_obj.ID}@{bill_DG_obj.Item}@{bill_DG_obj.Price}@{bill_DG_obj.Quantity}@{bill_DG_obj.Total}";
                data_list.Add(p_data);
                ctr++;
            }

            // sort data and store it in 2 lists
            List<string> main_data = new List<string>();
            List<string> duplicate_data = new List<string>();
            HashSet<string> seen = new HashSet<string>();

            foreach (string Main_item in data_list)
            {
                String[] item = Main_item.Split('@');
                if (seen.Contains(item[1]))
                {
                    duplicate_data.Add(Main_item);  // Duplicate data
                }
                else
                {
                    seen.Add(item[1]);
                    main_data.Add(Main_item);  // Unique data
                }
            }

            // marge duplicate data and main_data
            for (int i = 0; i < main_data.Count; i++)
            {
                for (int j = 0; j < duplicate_data.Count; j++)
                {
                    string[] main_Split = main_data[i].Split('@');
                    string[] duplicate_Split = duplicate_data[j].Split('@');

                    if (main_Split[1] == duplicate_Split[1])
                    {
                        int total_qtty = int.Parse(main_Split[3]) + int.Parse(duplicate_Split[3]);
                        int total = int.Parse(main_Split[4]) + int.Parse(duplicate_Split[4]);
                        main_data[i] = $"{main_Split[0]}@{main_Split[1]}@{main_Split[2]}@{total_qtty}@{total}";
                    }
                }
            }
            return main_data;
        }

        // ========================= REFRESH BILLING DATAGRID FUNCTION
        private void RefreshDataGrid()
        {
            to_print = process_dataGrid();
            ItemsDbBillGridXAML.Items.Clear();

            foreach (string item in to_print)
            {
                AddBill addBill = new AddBill();
                String[] split_item = item.Split('@');

                addBill.ID = int.Parse(split_item[0]);
                addBill.Item = split_item[1];
                addBill.Price = int.Parse(split_item[2]);
                addBill.Quantity = int.Parse(split_item[3]);
                addBill.Total = int.Parse(split_item[4]);

                ItemsDbBillGridXAML.Items.Add(addBill);
            }
        }

        // ===================================== END OF LOCAL FUNCTIONS =====================



        // ===================================== EVENT HANDELLER ===========================
        // =========================== EXIT BUTTON
        private void ExitBtn(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        // =========================== TO DRAG WINDOW
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        // =========================== SHIFT WINDOW TO INVOICE
        private void GotoInvoiceWindow(object sender, RoutedEventArgs e)
        {
            bool is_empty = true;

            // ============= GET Cid BY CUSTOMER PHONE NUMBER

            bool is_exists = CheckIfCustomerPhoneExistsOrNot(Customer_phone.Text);
            if (is_exists)
            {
                int Cid = GetCustomerIdByPhone(Customer_phone.Text);
                string current_date = DateTime.Now.ToString("MM / dd / yyyy");

                string BillingData = "";
                int GrandTotal = 0;
                // to get data from datagrid billing section
                AddBill item = new AddBill();
                foreach (AddBill p in ItemsDbBillGridXAML.Items)
                {
                    BillingData += $"{p.ID}|{p.Item}|{p.Price}|{p.Quantity}|{p.Total}||  ";
                    GrandTotal += p.Total;
                    is_empty = false;
                }

                // if billing datagrid is empty show info messege
                if (is_empty == true)
                {
                    MessageBox.Show("Add Item's to Bill.", "Gfresh", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    //=============== ADD DATA TO BILLING TABLE
                    String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                    String sql = $"INSERT INTO Bills (CInfo,BillingData,GrandTotal,DateTime) VALUES('{Cid}','{BillingData}','{GrandTotal}','{current_date}')";
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    SqlConnection cnn = new SqlConnection(connectionString);
                    cnn.Open();
                    SqlCommand command = new SqlCommand(sql, cnn);
                    adapter.InsertCommand = new SqlCommand(sql, cnn);
                    adapter.InsertCommand.ExecuteNonQuery();
                    command.Dispose();
                    cnn.Close();
                }
                // remove it on final stage just for debuging.
                MessageBox.Show("Added to DB.", "Gfresh", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // ==================== CHANGE THE WINDOW
                this.Hide();
                Invoice invoice = new Invoice(to_print);
                invoice.Show();

            }
            else
            {
                MessageBox.Show("User Dosent Exists!\nAdd User.");
            }

        }
        

        //======================== ADD CUSTOMER TO DB
        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            //================ CHECK IF USER EXISTS OR NOT
            bool dataExists = CheckIfCustomerPhoneExistsOrNot(Customer_phone.Text);

            //================ ASIGN DATA TO DB
            if (dataExists)
            {
                MessageBox.Show("Data Exists");
            }
            else
            {
                if (Customer_name.Text != "" && Customer_phone.Text != "" && Customer_Address.Text != "")
                {

                    string current_date = DateTime.Now.ToString("MM / dd / yyyy");

                    String connectionString2 = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                    String sql2 = $"INSERT INTO CustomerInfo (CName,CPhone,CAddress,RegistrationDate) VALUES('{Customer_name.Text}',{Customer_phone.Text},'{Customer_Address.Text}','{current_date}')";
                    SqlDataAdapter adapter = new SqlDataAdapter(); // for adding value

                    SqlConnection cnn2 = new SqlConnection(connectionString2);

                    cnn2.Open();
                    SqlCommand command2 = new SqlCommand(sql2, cnn2);

                    adapter.InsertCommand = new SqlCommand(sql2, cnn2);
                    adapter.InsertCommand.ExecuteNonQuery();
                    command2.Dispose();
                    cnn2.Close();
                    // refresh combobox
                    RefreshCustomerDropdown();
                    // clear all the fields
                    ClearCustomerFields();
                }
                else
                {
                    MessageBox.Show("Empty Fields !!");
                }
            }
        }

        //=================== CLEAR CUSTOMER INPUT FIELDS
        private void ClearFields(object sender, RoutedEventArgs e)
        {
            Customer_name.Text = "";
            Customer_phone.Text = "";
            Customer_Address.Text = "";
            CustomerDropDown.SelectedIndex = -1;

            AddBillBtn.IsEnabled = true;
        }

        //=================== CLEAR BILLING FIELDS
        private void ClearBillingFields(object sender, RoutedEventArgs e)
        {
            ItemDropDown.SelectedIndex = -1;
            Bill_Quantity.Text = "";
            Bill_Price.Text = "";
            Bill_Discount.Text = "";
        }

        // =================== HANDEL EVENT ON SELECTING AN ITEM
        private void ItemDropDown_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ItemDropDown.SelectedIndex != -1)
            {
                string SelectedItem = ItemDropDown.SelectedItem.ToString();

                String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
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

        // =================== HANDEL EVENT ON SELECTING A CUSTOMER
        private void CustomerDropDown_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(CustomerDropDown.SelectedIndex != -1)
            {
                try
                {
                    //==================== GET DATA FROM DROPDOWN AND PROCESS THAT DATA
                    string SelectedInfo = CustomerDropDown.SelectedItem.ToString();
                    string CustomerName = SelectedInfo.Split(',')[0].Trim();
                    string CustomerPhone = SelectedInfo.Split(',')[1].Trim();

                    //==================== SELECT CUSTOMER FROM DB BY PHONE

                    bool customer_exists = CheckIfCustomerPhoneExistsOrNot(CustomerPhone);

                    if (customer_exists)
                    {

                        String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                        String sql = $"SELECT CName,CAddress from CustomerInfo where CPhone = '{CustomerPhone}'";

                        SqlConnection cnn = new SqlConnection(connectionString);
                        cnn.Open();
                        SqlCommand command = new SqlCommand(sql, cnn);
                        // get value
                        
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            Customer_name.Text = reader.GetString(0);
                            Customer_Address.Text = reader.GetString(1);
                            Customer_phone.Text = CustomerPhone;
                            reader.Close();
                        }
                        //=============== DISSABLE ADD BUTTON
                        AddBillBtn.IsEnabled = false;

                        //=============== CLOSE CONNECTIONS
                        command.Dispose();
                        cnn.Close();


                    }





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

        
        //=====================END=======================


        // ======================= ADD ITEMS TO BILLS
        int counter = 0;
        int TotalBill = 0;
        private void AddToBill(object sender, RoutedEventArgs e)
        {
            if (ItemDropDown.SelectedIndex != -1)
            {
                string SelectedItem = ItemDropDown.SelectedItem.ToString();

                String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
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
                        foreach (AddBill cntr in ItemsDbBillGridXAML.Items)
                        {
                            if (SelectedItem == cntr.Item)
                            {
                                //update this field
                                continue;
                                //ItemsDbBillGridXAML.Items.Contains(cntr.Item);
                            }
                        }
                        // process data
                        counter = counter + 1;
                        item.ID = counter;

                        item.Item = (string)dataReader.GetValue(1);
                        item.Quantity = int.Parse(Bill_Quantity.Text.ToString());
                        item.Price = int.Parse(Bill_Price.Text.ToString());

                        int SubTotal = int.Parse(Bill_Price.Text.ToString()) * int.Parse(Bill_Quantity.Text.ToString());
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
                Bill_Quantity.Text = "";
                Bill_Price.Text = "";
                ItemDropDown.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Select an Item.");
            }
        }



        // =================== Reset all bills ===================
        private void ResetBill(object sender, RoutedEventArgs e)
        {
            ItemDropDown.SelectedIndex = -1;
            Bill_Quantity.Text = "";
            Bill_Price.Text = "";
            Total_Bill.Text = "";
            Bill_Discount.Text = "";

            ItemsDbBillGridXAML.Items.Clear();

        }

        // =================== Refresh billing datagrid =================
        private void RefreshDataGridBtn(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();

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


    }
}
