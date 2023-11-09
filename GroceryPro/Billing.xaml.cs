using System.Data.SqlClient;
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
    public partial class Billing : Window
    {
        //============================== RUNS ON PROGRAMS STARTUP
        public Billing()
        {
            InitializeComponent();
            // ========================= GET STOCK DATA FROM DB
            ReadDataFromDB();
            // ========================= GET CUSTOMER INFO
            RefreshCustomerDropdown();
            // ========================= READ ITEM NAMES FROM DB FOR ITEM'S DROPDOWN
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

        // ============================ GET CUSTOMER ID FROM DB USING PHONE NUMBER
        public static int GetCustomerIdByPhone(string phone)
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = $"SELECT Cid from CustomerInfo where CPhone = '{phone}'";

            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            //================= READ VALUES
            int data = (int)command.ExecuteScalar();

            //================= CLOSE CONNECTIONS
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
            //================ GET VALUE
            int userCount = (int)command.ExecuteScalar();
            //================ CLOSE CONNECTIONS
            command.Dispose();
            cnn.Close();
            //================ CHECK IF CUSTOMER PHONE EXISTS OR NOT
            if (userCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // ======================= READ ITEM NAMES FROM DB FOR ITEM'S DROPDOWN
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
                //============ POPULATE ITEM'S COMBOBOX
                ItemDropDown.Items.Add(CustomerData);
            }
            //=============== CLOSE CONNECTIONS
            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }

        //====================== READ DATA FROM THE DATABASE
        private void ReadDataFromDB()
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = "SELECT ItId,ItName,ItQty,ItPrice,ItCat FROM ItemTbl";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                AddItems item = new AddItems
                {
                    ID = (int)dataReader.GetValue(0),
                    Items = (string)dataReader.GetValue(1),
                    Quantity = (int)dataReader.GetValue(2),
                    Price = (int)dataReader.GetValue(3),
                    Catagory = (string)dataReader.GetValue(4)
                };

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
                _ = new AddItems();

                String CustomerName = (string)dataReader.GetValue(0);
                String CustomerPhone = (string)dataReader.GetValue(1);

                //=========== ADD DATA TO COMBOBOX
                CustomerDropDown.Items.Add(CustomerName + ", " + CustomerPhone);
            }
            //================ CLOSE CONNECTIONS
            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }

        // ==================== PROCESS BILLING DATAGRIDS
        private List<string> Process_dataGrid()
        {
            //================= COLLECT ALL DATA FROM DATA GRID AND STORE IT ON A LIST
            List<string> data_list = new List<string>();
            int ctr = 0;
            while (ctr < ItemsDbBillGridXAML.Items.Count)
            {
                AddBill bill_DG_obj = (AddBill)ItemsDbBillGridXAML.Items[ctr];
                string p_data = $"{bill_DG_obj.ID}@{bill_DG_obj.Item}@{bill_DG_obj.Price}@{bill_DG_obj.Quantity}@{bill_DG_obj.Total}";
                data_list.Add(p_data);
                ctr++;
            }

            //=================== SORT DATA AND STORE IT IN 2 LIST'S
            List<string> main_data = new List<string>();
            List<string> duplicate_data = new List<string>();
            HashSet<string> seen = new HashSet<string>();

            foreach (string Main_item in data_list)
            {
                String[] item = Main_item.Split('@');
                if (seen.Contains(item[1]))
                {
                    duplicate_data.Add(Main_item);  // DUPLICATE DATA
                }
                else
                {
                    seen.Add(item[1]);
                    main_data.Add(Main_item);  // UNIQUE DATA
                }
            }
            //==================== MARGE DUPLICATE DATA AND MAIN_DATA
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
            to_print = Process_dataGrid();
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

        // =============================== END OF LOCAL FUNCTIONS ==========================


        // =================================================================================
        // =============================== EVENT HANDELLER =================================
        // =================================================================================

        // =========================== SHIFT WINDOW TO INVOICE
        private void GotoInvoiceWindow(object sender, RoutedEventArgs e)
        {
            //============== REFRESH BILLING DATAGRID
            RefreshDataGrid();
            bool is_empty = true;
            // ============= GET Cid BY CUSTOMER PHONE NUMBER
            bool is_exists = CheckIfCustomerPhoneExistsOrNot(Customer_phone.Text);
            if (is_exists)
            {
                //========== GET CUSTOMER ID
                int Cid = GetCustomerIdByPhone(Customer_phone.Text);
                string current_date = DateTime.Now.ToString("MM / dd / yyyy");

                string BillingData = "";
                int GrandTotal = 0;
                //================= READ DATA FROM BILLING DATAGRID AND CALCULATE GRAND TOTAL
                _ = new AddBill();
                foreach (AddBill p in ItemsDbBillGridXAML.Items)
                {
                    BillingData += $"{p.ID}|{p.Item}|{p.Price}|{p.Quantity}|{p.Total}||  ";
                    GrandTotal += p.Total;
                    is_empty = false;
                }

                //================= CHECK IF BILLING DATAGRID IS EMPTY OR NOT
                if (is_empty)
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
                MessageBox.Show("User Already Exists!\nSelect a Customer from the Dropdown.");
            }
            else
            {
                if (Customer_name.Text != "" && Customer_phone.Text != "" && Customer_Address.Text != "")
                {
                    //================= GET CURRENT DATE
                    string current_date = DateTime.Now.ToString("MM / dd / yyyy");

                    String connectionString2 = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                    String sql2 = $"INSERT INTO CustomerInfo (CName,CPhone,CAddress,RegistrationDate) VALUES('{Customer_name.Text}',{Customer_phone.Text},'{Customer_Address.Text}','{current_date}')";
                    SqlDataAdapter adapter = new SqlDataAdapter(); // for adding value

                    SqlConnection cnn2 = new SqlConnection(connectionString2);

                    cnn2.Open();
                    SqlCommand command2 = new SqlCommand(sql2, cnn2);

                    adapter.InsertCommand = new SqlCommand(sql2, cnn2);
                    adapter.InsertCommand.ExecuteNonQuery();
                    //================= CLOSE CONNECTIONS
                    command2.Dispose();
                    cnn2.Close();
                    //================== REFRESH CUSTOMER DROPDOWN (COMBOBOX)
                    RefreshCustomerDropdown();
                    //================== CLEAR CUSTOMER INPUT FIELDS
                    ClearCustomerFields();
                }
                else
                {
                    MessageBox.Show("Empty Fields !!!");
                }
            }
        }

        //=================== CLEAR CUSTOMER INPUT FIELDS
        private void ClearFields(object sender, RoutedEventArgs e)
        {
            //=============== CLEAR INPUT FIELDS
            Customer_name.Text = "";
            Customer_phone.Text = "";
            Customer_Address.Text = "";
            CustomerDropDown.SelectedIndex = -1;
            //=============== ENABLE CUSTOMER INPUT FIELDS
            AddBillBtn.IsEnabled = true;
            Customer_name.IsEnabled = true;
            Customer_phone.IsEnabled = true;
            Customer_Address.IsEnabled = true;
        }

        //=================== CLEAR BILLING INPUT FIELDS
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
            //==================== CHECK IF ANY ITEM IS SELECTED OR NOT
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
                        //============ ASIGN VALUES TO INPUT FIELDS FROM DB
                        int ItemQty = (int)dataReader.GetValue(2);
                        int ItemPrice = (int)dataReader.GetValue(3);

                        Bill_Quantity.Text = ItemQty.ToString();
                        Bill_Price.Text = ItemPrice.ToString();
                        //!!!!!!!!!!!!! ADD DISCOUNT INPUT FIELD
                    }
                }
                //============== CLOSE CONNECTIONS
                dataReader.Close();
                command.Dispose();
                cnn.Close();
            }
            else if(ItemDropDown.SelectedIndex == -1)
            {
                //=============== RESET BILLING INPUT FIELDS
                Bill_Quantity.Text = "";
                Bill_Price.Text = "";
                Bill_Discount.Text = "";
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
                        //============= DB CONNECTION LINK AND COMMAND
                        String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Maruf\\source\\repos\\Maruf512\\Gfresh\\GroceryPro\\DataBase\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                        String sql = $"SELECT CName,CAddress from CustomerInfo where CPhone = '{CustomerPhone}'";

                        SqlConnection cnn = new SqlConnection(connectionString);
                        cnn.Open();
                        SqlCommand command = new SqlCommand(sql, cnn);
                        
                        //============== READ VALUES FROM DB
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
                        Customer_name.IsEnabled = false;
                        Customer_Address.IsEnabled = false;
                        Customer_phone.IsEnabled = false;
                        //=============== CLOSE CONNECTIONS
                        command.Dispose();
                        cnn.Close();
                    }
                    else
                    {
                        MessageBox.Show("User Dosen't Exists!\nAdd User.");
                    }
                }
                catch (NullReferenceException)
                {
                    //=============== ENABLE INPUT FIELDS AND BUTTONS
                    AddBillBtn.IsEnabled = true;
                    Customer_name.IsEnabled = true;
                    Customer_Address.IsEnabled = true;
                    Customer_phone.IsEnabled = true;
                }
            }
            else if(CustomerDropDown.SelectedIndex == -1)
            {
                //================== CLEAR INPUT FIELDS
                Customer_name.Text = "";
                Customer_phone.Text = "";
                Customer_Address.Text = "";
                CustomerDropDown.SelectedIndex = -1;
                //================== ENABLE INPUT FIELDS AND BUTTON
                AddBillBtn.IsEnabled = true;
                Customer_name.IsEnabled = true;
                Customer_Address.IsEnabled = true;
                Customer_phone.IsEnabled = true;
            }
        }


        // ======================= ADD ITEMS TO BILLS DataGrid
        int counter = 0;
        int TotalBill = 0;
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ADD DISCOUNT FIELD
        private void AddBillToDataGrid(object sender, RoutedEventArgs e)
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
                        AddBill item = new AddBill();

                        //============== PROCESS DATA
                        counter++;
                        item.ID = counter;

                        item.Item = (string)dataReader.GetValue(1);
                        item.Quantity = int.Parse(Bill_Quantity.Text.ToString());
                        item.Price = int.Parse(Bill_Price.Text.ToString());

                        int SubTotal = int.Parse(Bill_Price.Text.ToString()) * int.Parse(Bill_Quantity.Text.ToString());
                        item.Total = SubTotal;

                        //============== ADD TO DATAGRID
                        ItemsDbBillGridXAML.Items.Add(item);
                        //============== CALCULATE TOTAL BILLS
                        TotalBill += SubTotal;
                        //============== UPDATE TOTAL BILLS TO GUI
                        Total_Bill.Text = TotalBill.ToString() + "Tk";
                    }
                }
                //================ CLOSE OPEND CONNECTIONS
                dataReader.Close();
                command.Dispose();
                cnn.Close();

                //================ CLEAR ALL THE FIELDS
                Bill_Quantity.Text = "";
                Bill_Price.Text = "";
                Bill_Discount.Text = "";
                ItemDropDown.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Select an Item.");
            }
        }

        //=========================== RESET ALL BILLS AND DATAGRID
        private void ResetBill(object sender, RoutedEventArgs e)
        {
            ItemDropDown.SelectedIndex = -1;
            Bill_Quantity.Text = "";
            Bill_Price.Text = "";
            Total_Bill.Text = "";
            Bill_Discount.Text = "";
            ItemsDbBillGridXAML.Items.Clear();
        }
        //=========================== REFRESH BILLING DATAGRID
        private void RefreshDataGridBtn(object sender, RoutedEventArgs e)
        {
            RefreshDataGrid();
        }
        //=========================== TO DRAG WINDOW
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        //=========================== CHANGE TO LOGOUT WINDOW
        private void GotoLogoutWindow(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }
        //=========================== EXIT BUTTON
        private void ExitBtn(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
