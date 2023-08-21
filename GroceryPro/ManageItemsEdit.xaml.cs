using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace GroceryPro
{
    public partial class ManageItemsEdit : Window
    {
        public ManageItemsEdit()
        {
            InitializeComponent();

            readDataFromDB();
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


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ExitApp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SwitchToManageItem(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ManageItems manageItems = new ManageItems();
            manageItems.Show();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void refreshGridData()
        {
            ItemsDbGridXAML.Items.Clear();
            readDataFromDB();
        }

        private void RefreshDBBtn(object sender, MouseButtonEventArgs e)
        {
            // Refresh grid View
            refreshGridData();
        }

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

        private void UpdateDB(object sender, RoutedEventArgs e)
        {
            if (TextBoxID.Text != "")
            {
                // data process variable
                string itemname = "";
                string price = "";
                string quantity = "";
                string catagory = "";

                string coma1 = "";
                string coma2 = "";
                string coma3 = "";

                // data / parametar
                if (ItemName.Text != "")
                {
                    itemname = $"ItName='{ItemName.Text}'";
                }
                if (Price.Text != "")
                {
                    price = $"ItPrice={Price.Text}";
                }
                if (Quantity.Text != "")
                {
                    quantity = $"ItQty={Quantity.Text}";
                }
                if (Catagory.Text != "")
                {
                    catagory = $"ItCat='{Catagory.Text}'";
                }


                // coma 4x4 = 16 (conditions)
                if (itemname != "" && price != "" && quantity == "" && catagory == "")
                {
                    coma1 = ",";
                }
                else if (itemname != "" && price == "" && quantity != "" && catagory == "")
                {
                    coma1 = ",";
                }
                else if (itemname != "" && price == "" && quantity == "" && catagory != "")
                {
                    coma1 = ",";
                }

                else if (itemname == "" && price != "" && quantity != "" && catagory == "")
                {
                    coma2 = ",";
                }
                else if (itemname == "" && price != "" && quantity == "" && catagory != "")
                {
                    coma2 = ",";
                }

                else if (itemname == "" && price == "" && quantity != "" && catagory != "")
                {
                    coma3 = ",";
                }
                else if (itemname != "" && price != "" && quantity != "" && catagory == "")
                {
                    coma1 = ",";
                    coma2 = ",";
                }
                else if (itemname != "" && price != "" && quantity == "" && catagory != "")
                {
                    coma1 = ",";
                    coma2 = ",";
                }
                else if (itemname != "" && price == "" && quantity != "" && catagory != "")
                {
                    coma1 = ",";
                    coma3 = ",";
                }
                else if (itemname == "" && price != "" && quantity != "" && catagory != "")
                {
                    coma1 = ",";
                    coma3 = ",";
                }
                else if (itemname != "" && quantity != "" && quantity != "" && catagory != "")
                {
                    coma1 = ",";
                    coma2 = ",";
                    coma3 = ",";
                }
                else if (itemname != "" && quantity == "" && quantity == "" && catagory == "")
                {
                    coma1 = "";
                    coma2 = "";
                    coma3 = "";
                }
                else if (itemname == "" && quantity != "" && quantity == "" && catagory == "")
                {
                    coma1 = "";
                    coma2 = "";
                    coma3 = "";
                }
                else if (itemname == "" && quantity == "" && quantity != "" && catagory == "")
                {
                    coma1 = "";
                    coma2 = "";
                    coma3 = "";
                }
                else if (itemname == "" && quantity == "" && quantity == "" && catagory != "")
                {
                    coma1 = "";
                    coma2 = "";
                    coma3 = "";
                }

                // main process
                if (ItemName.Text !="" || Price.Text !="" || Quantity.Text !="" || Catagory.Text != "")
                {
                    try
                    {
                        String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                        SqlDataAdapter adapter = new SqlDataAdapter(); // for adding value
                        String sql = $"UPDATE ItemTbl SET {itemname}{coma1}{price}{coma2}{quantity}{coma3}{catagory} WHERE ItId={TextBoxID.Text}";

                        SqlConnection cnn = new SqlConnection(connectionString);

                        cnn.Open();
                        SqlCommand command = new SqlCommand(sql, cnn);

                        adapter.InsertCommand = new SqlCommand(sql, cnn);
                        adapter.InsertCommand.ExecuteNonQuery();

                        command.Dispose();
                        cnn.Close();

                        refreshGridData();

                        TextBoxID.Text = "";
                        ItemName.Text = "";
                        Price.Text = "";
                        Quantity.Text = "";
                        Catagory.Text = "";
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Empty Field", "Gfresh");
                }
            }
            else
            {
                MessageBox.Show("Enter The ID", "Gfresh");
            }
        }

        private void ClearTextBox(object sender, RoutedEventArgs e)
        {
            TextBoxID.Text = "";
            ItemName.Text = "";
            Price.Text = "";
            Quantity.Text = "";
            Catagory.Text = "";
        }
    }
}

