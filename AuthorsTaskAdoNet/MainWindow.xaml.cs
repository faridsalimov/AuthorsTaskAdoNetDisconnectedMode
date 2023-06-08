using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AuthorsTaskAdoNet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn = new SqlConnection();
        string cs = ConfigurationManager.ConnectionStrings["myconn"].ConnectionString;
        DataTable table;
        SqlDataReader reader;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            if (idTextBox.Text != String.Empty)
            {
                if (firstNameTextBox.Text != String.Empty)
                {
                    if (lastNameTextBox.Text != String.Empty)
                    {
                        conn.ConnectionString = cs;
                        conn.Open();

                        using (SqlConnection conn = new SqlConnection())
                        {
                            string insertQuery = "INSERT INTO Authors (Id,Firstname,Lastname) VALUES (@id,@firstName,@lastName)";
                            using (SqlCommand command = new SqlCommand(insertQuery, conn))
                            {
                                command.Parameters.AddWithValue("@id", idTextBox.Text);
                                command.Parameters.AddWithValue("@firstName", firstNameTextBox.Text);
                                command.Parameters.AddWithValue("@lastName", lastNameTextBox.Text);
                                command.ExecuteNonQuery();
                            }
                            conn.Close();
                        }

                        idTextBox.Clear();
                        firstNameTextBox.Clear();
                        lastNameTextBox.Clear();

                        MessageBox.Show("Author has been successfully added to the database.", "Successful!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    else
                    {
                        MessageBox.Show("Last Name section looks blank.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                else
                {
                    MessageBox.Show("First Name section looks blank.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else
            {
                MessageBox.Show("ID section looks blank.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (deleteIdTextBox.Text != String.Empty)
            {
                conn.ConnectionString = cs;
                conn.Open();

                using (SqlConnection conn = new SqlConnection())
                {
                    string deleteQuery = "DELETE FROM Authors WHERE Id = @id";

                    using (SqlCommand command = new SqlCommand(deleteQuery, conn))
                    {
                        command.Parameters.AddWithValue("@id", deleteIdTextBox.Text);
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }

                deleteIdTextBox.Clear();
            }

            else
            {
                MessageBox.Show("ID section looks blank.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void showAllButton_Click(object sender, RoutedEventArgs e)
        {
            conn.ConnectionString = cs;
            conn.Open();

            using (SqlConnection conn = new SqlConnection())
            {
                var da = new SqlDataAdapter();
                var set = new DataSet();
                SqlCommand command = new SqlCommand("SELECT * FROM Authors", conn);
                da.SelectCommand = command;
                da.Fill(set, "AuthorsSet");
                authorsDataGrid.ItemsSource = set.Tables[0].DefaultView;
            }
        }
    }
}
