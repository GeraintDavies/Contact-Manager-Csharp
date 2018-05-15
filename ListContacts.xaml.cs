using System;
using System.Collections.Generic;
using System.Data;
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

using System.Data.SqlClient;
using System.Drawing;


namespace Contact_Management
{
    /// <summary>
    /// Interaction logic for ListContacts.xaml
    /// </summary>
    public partial class ListContacts : Window
    {





        public static int i;
        SimpleDataSource SDS = new SimpleDataSource("mysql.cs.bangor.ac.uk", "eeu6da", 3306, "eeu6da", "eeu6da");

        /// <summary>
        /// sets the values in the comboboxes and sets max length of the textboxes
        /// </summary>
        public ListContacts()
        {
            InitializeComponent();

            comboBox.Items.Add("Forename");
            comboBox.Items.Add("Surname");
            comboBox.Items.Add("Email Address");
            comboBox.Items.Add("Phone Number");
            comboBox.Items.Add("Twitter");
            comboBox.Items.Add("Facebook");
            comboBox.Items.Add("Company");

            comboBox1.Items.Add("Mr");
            comboBox1.Items.Add("Mrs");
            comboBox1.Items.Add("Ms");
            comboBox1.Items.Add("Dr");


            forenameBox.MaxLength = 20;
            surnameBox.MaxLength = 20;
            emailBox.MaxLength = 320;
            numberBox.MaxLength = 15;
            twitterBox.MaxLength = 16;
            facebookBox.MaxLength = 50;
            companyBox.MaxLength = 70;



            searchBox.MaxLength = 320;


            DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts", new Dictionary<string, string>());
            dataGrid.ItemsSource = Table.DefaultView;

            
        }

        /// <summary>
        /// send back to the Main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        /// <summary>
        /// delets a contact that is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delbutton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dataGrid.SelectedItem;
            if (drv != null)
            {
                DataRow row = drv.Row;
                i = (row.Field<Int32>("contact_id"));

                MessageBoxResult dr = MessageBox.Show("Are you sure to delete contact?", "Confirmation", MessageBoxButton.YesNo);
                if (dr == MessageBoxResult.Yes)
                {
                    //delete row from database or datagridview...
                    SDS.Update("DELETE FROM contacts WHERE contact_id = " + i + "");
                    DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts", new Dictionary<string, string>());
                    dataGrid.ItemsSource = Table.DefaultView;

                }
                else
                {
                    //Nothing to do

                }

            }
        }

        /// <summary>
        /// can search for a field dependant on the value of the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (comboBox.Text == "Forename")
            {
                // DO SEARCH BAR
                DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts WHERE forename LIKE '" + searchBox.Text + "%'", new Dictionary<string, string>());
                dataGrid.ItemsSource = Table.DefaultView;
            }
            if (comboBox.Text == "Surname")
            {
                DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts WHERE surname LIKE '" + searchBox.Text + "%'", new Dictionary<string, string>());
                dataGrid.ItemsSource = Table.DefaultView;
            }

            if (comboBox.Text == "Email Address")
            {
                DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts WHERE email_address LIKE '" + searchBox.Text + "%'", new Dictionary<string, string>());
                dataGrid.ItemsSource = Table.DefaultView;
            }

            if (comboBox.Text == "Twitter")
            {
                DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts WHERE twitter_username LIKE '%" + searchBox.Text + "%'", new Dictionary<string, string>());
                dataGrid.ItemsSource = Table.DefaultView;
            }

            if (comboBox.Text == "Facebook")
            {
                DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts WHERE facebook_username LIKE '%" + searchBox.Text + "%'", new Dictionary<string, string>());
                dataGrid.ItemsSource = Table.DefaultView;
            }

            if (comboBox.Text == "Phone Number")
            {
                DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts WHERE phone_numbers LIKE '" + searchBox.Text + "%'", new Dictionary<string, string>());
                dataGrid.ItemsSource = Table.DefaultView;
            }

            if (comboBox.Text == "Company")
            {
                DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts WHERE company_works_for LIKE '" + searchBox.Text + "%'", new Dictionary<string, string>());
                dataGrid.ItemsSource = Table.DefaultView;
            }
        }

        /// <summary>
        /// sends the data to the textboxes from the datagrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editbutton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)dataGrid.SelectedItem;
            if (drv != null)
            {
                DataRow row = drv.Row;
                i = (row.Field<Int32>("contact_id"));
                comboBox.Text = (row.Field<string>("title"));
                forenameBox.Text = (row.Field<string>("forename"));
                surnameBox.Text = (row.Field<string>("surname"));
                emailBox.Text = (row.Field<string>("email_address"));
                numberBox.Text = (row.Field<string>("phone_numbers"));
                twitterBox.Text = (row.Field<string>("twitter_username"));
                facebookBox.Text = (row.Field<string>("facebook_username"));
                companyBox.Text = (row.Field<string>("company_works_for"));
                
            }

        }

        

        /// <summary>
        /// saves the contact after editting the data if it meets the validation rules.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void savebutton_Click(object sender, RoutedEventArgs e)
        {
            string title = comboBox.Text;
            string forename = forenameBox.Text;
            string surname = surnameBox.Text;
            string email = emailBox.Text;
            string number = numberBox.Text;
            string twitter = twitterBox.Text;
            string facebook = facebookBox.Text;
            string company = companyBox.Text;
            
            if (this.ValidTitle(title))
            {
                if (this.ValidForename(forename))
                {
                    if (this.ValidSurname(surname))
                    {
                        if (this.ValidEmailAddress(email))
                        {
                            if (this.ValidPhoneNumber(number))
                            {
                                if (this.ValidTwitter(twitter))
                                {
                                    if (this.ValidFacebook(facebook))
                                    {
                                        if (this.ValidCompany(company))
                                        {
                                            // Assigning a textbox to a specific column in the table contacts

                                            string UpdateString = "UPDATE contacts SET title = '" + title + "', forename = '" + forename + "', surname = '" + surname + "', email_address = '" + email + "', phone_numbers = '" + number + "', twitter_username = '" + twitter + "', facebook_username = '" + facebook + "', company_works_for = '" + company + "' WHERE contact_id = '" + i + "';";
                                            SDS.Update(UpdateString);
                                            DataTable Table = SDS.QueryDataTable("SELECT * FROM contacts", new Dictionary<string, string>());
                                            dataGrid.ItemsSource = Table.DefaultView;


                                        }
                                        else
                                        {
                                            // message to say they havent filled it in properly
                                            MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK);
                                            companyBox.BorderBrush = Brushes.Red;
                                        }

                                    }
                                    else
                                    {
                                        // message to say they havent filled it in properly
                                        MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK);
                                        facebookBox.BorderBrush = Brushes.Red;
                                    }
                                }
                                else
                                {
                                    // message to say they havent filled it in properly
                                    MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK);
                                    twitterBox.BorderBrush = Brushes.Red;
                                }
                            }
                            else
                            {
                                // message to say they havent filled it in properly
                                MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK);
                                numberBox.BorderBrush = Brushes.Red;
                            }
                        }
                        else
                        {
                            // message to say they havent filled it in properly
                            MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK);
                            emailBox.BorderBrush = Brushes.Red;
                        }
                    }
                    else
                    {
                        // message to say they havent filled it in properly
                        MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK);
                        surnameBox.BorderBrush = Brushes.Red;
                    }
                }
                else
                {
                    // message to say they havent filled it in properly
                    MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK);
                    forenameBox.BorderBrush = Brushes.Red;
                }
            }
            else
            {
                // message to say they havent filled it in properly
                MessageBox.Show("Invalid Data", "Error", MessageBoxButton.OK);
                comboBox.BorderBrush = Brushes.Red;
            }
        }


        /// <summary>
        /// validating the right input into the textbox.
        /// matching string to regex regular expression pattern.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool ValidTitle(string title)
        {

            if (title == "")
            {

                return false;

            }
            else
            {
                return true;
            }

        }


        /// <summary>
        /// validating the right input into the textbox.
        /// matching string to regex regular expression pattern.
        /// </summary>
        /// <param name="forename"></param>
        /// <returns></returns>
        public bool ValidForename(string forename)
        {
            string sPattern = "^[a-zA-Z-]+$";
            if (forename == "")
            {
                return false;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(forename, sPattern))
            {

                return true;
            }
            else
            {

                return false;
            }

        }
        /// <summary>
        /// validating the right input into the textbox.
        /// matching string to regex regular expression pattern.
        /// </summary>
        /// <param name="surname"></param>
        /// <returns></returns>
        public bool ValidSurname(string surname)
        {
            string sPattern = "^[a-zA-Z-]+$";

            if (surname == "")
            {
                return false;
            }


            if (System.Text.RegularExpressions.Regex.IsMatch(surname, sPattern))
            {
                return true;
            }
            else
            {
                return false;
            }
            //return false;
        }

        /// <summary>
        /// validating the right input into the textbox.
        /// matching string to regex regular expression pattern.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ValidEmailAddress(string email)
        {
            if (email == "")
            {
                return false;
            }

            if (email.IndexOf("@") > -1)
            {
                if (email.IndexOf(".", email.IndexOf("@")) > email.IndexOf("@"))
                {

                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// validating the right input into the textbox.
        /// matching string to regex regular expression pattern.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool ValidPhoneNumber(string number)
        {
            string sPattern = "^[0-9]+$";

            if (number == "")
            {
                return false;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(number, sPattern))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// validating the right input into the textbox.
        /// matching string to regex regular expression pattern.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public bool ValidCompany(string company)
        {
            string sPattern = @"^[a-zA-Z0-9- ]+$";

            if (System.Text.RegularExpressions.Regex.IsMatch(company, sPattern))
            {
                return true;
            }
            else if (company == "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// validating the right input into the textbox.
        /// matching string to regex regular expression pattern.
        /// </summary>
        /// <param name="twitter"></param>
        /// <returns></returns>
        public bool ValidTwitter(string twitter)
        {
            string sPattern = "^@[0-9a-zA-Z_]+$";

            if (System.Text.RegularExpressions.Regex.IsMatch(twitter, sPattern))
            {
                return true;
            }
            else if (twitter == "")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// validating the right input into the textbox.
        /// matching string to regex regular expression pattern.
        /// </summary>
        /// <param name="facebook"></param>
        /// <returns></returns>
        public bool ValidFacebook(string facebook)
        {
            string sPattern = "^@[0-9a-zA-Z_-]+$";

            if (System.Text.RegularExpressions.Regex.IsMatch(facebook, sPattern))
            {
                return true;
            }
            else if (facebook == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
    

    
    

        



    

