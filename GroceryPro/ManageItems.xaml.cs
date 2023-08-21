using System;
using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Text.RegularExpressions; // for textbox number

namespace GroceryPro
{
    /// <summary>
    /// Interaction logic for ManageItems.xaml
    /// </summary>
    public partial class ManageItems : Window
    {
        public ManageItems()
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

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
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


        private void SaveDataToDB(object sender, RoutedEventArgs e)
        {

            if (ItemName.Text != "" && Quantity.Text != "" && Price.Text != "" && Catagory.Text != "")
            {
                String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
                String sql = $"INSERT INTO ItemTbl (ItName,ItQty,ItPrice,ItCat) VALUES('{ItemName.Text}',{Quantity.Text},{Price.Text},'{Catagory.Text}')";
                SqlDataAdapter adapter = new SqlDataAdapter(); // for adding value

                SqlConnection cnn = new SqlConnection(connectionString);

                cnn.Open();
                SqlCommand command = new SqlCommand(sql, cnn);

                adapter.InsertCommand = new SqlCommand(sql, cnn);
                adapter.InsertCommand.ExecuteNonQuery();

                command.Dispose();
                cnn.Close();

                refreshGridData();

                ItemName.Text = "";
                Quantity.Text = "";
                Price.Text = "";
                Catagory.Text = "";

            }
            else
            {
                MessageBox.Show("Empty Fields !!");
            }
        }

        private void clearData()
        {
            ItemName.Text = "";
            Quantity.Text = "";
            Price.Text = "";
            Catagory.Text = "";
        }

        private void ClearInputFields(object sender, RoutedEventArgs e)
        {
            clearData();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void deleteDataFromDb(string id)
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand();

            String sql = $"DELETE ItemTbl WHERE ItId={id}";

            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();

            adapter.DeleteCommand = new SqlCommand(sql, cnn);
            adapter.DeleteCommand.ExecuteNonQuery();

            command.Dispose();
            cnn.Close();

            refreshGridData();
            clearData();
        }


        private void DeletRow(object sender, RoutedEventArgs e)
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            String sql = "SELECT ItId,ItName,ItQty,ItPrice,ItCat FROM ItemTbl";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = command.ExecuteReader();

            string data = "";
            while (dataReader.Read())
            {
                AddItems item = new AddItems();

                item.ID = (int)dataReader.GetValue(0);
                item.Items = (string)dataReader.GetValue(1);

                if (ItemName.Text == (string)dataReader.GetValue(1))
                {
                    data = $"{(int)dataReader.GetValue(0)} {(string)dataReader.GetValue(1)}";
                    string id = $"{(int)dataReader.GetValue(0)}";
                    deleteDataFromDb(id);
                }
            }
            if (data == "")
            {
                MessageBox.Show("Item Name Dosent Match!!", "Gfresh");
            }
            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }

        private void updateDb(string id, string parameter, string data)
        {
            String connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\maruf\\source\\repos\\Gfresh\\GroceryPro\\GforceDB.mdf;Integrated Security=True;Connect Timeout=30";
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand();


            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();

            String sql = $"UPDATE ItemTbl SET {parameter}= {data} WHERE id={id}";
            adapter.UpdateCommand = new SqlCommand(sql, cnn);
            adapter.UpdateCommand.ExecuteNonQuery();


            command.Dispose();
            cnn.Close();
        }

        private void UpdateValues(object sender, RoutedEventArgs e)
        {
            // edit window
            this.Hide();
            ManageItemsEdit manageItemsEdit = new ManageItemsEdit();
            manageItemsEdit.Show();
        }
    }
}
