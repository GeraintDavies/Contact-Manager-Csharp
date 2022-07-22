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
using System.Windows.Shapes;

namespace Contact_Management
{
    /// <summary>
    /// Interaction logic for AddContact.xaml
    /// </summary>
    public partial class AddContact : Window
    {
        SimpleDataSource SDS = new SimpleDataSource("server_name", "db_name", port, "username", "password");
        
        /// <summary>
        /// sets the content for the combo box and max length of all the textboxes
        /// </summary>
        public AddContact()
        {
            InitializeComponent();
            

            comboBox.Items.Add("Mr");
            comboBox.Items.Add("Mrs");
            comboBox.Items.Add("Ms");
            comboBox.Items.Add("Dr");
            

            forenameBox.MaxLength = 20;
            surnameBox.MaxLength = 20;
            emailBox.MaxLength = 320;
            numberBox.MaxLength = 15;
            twitterBox.MaxLength = 16;
            facebookBox.MaxLength = 50;
            companyBox.MaxLength = 70;
            
        }

        /// <summary>
        /// the save button for adding a contact, goes through all the validation before submitting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {

            string title = comboBox.Text;
            string forename = forenameBox.Text;
            string surname = surnameBox.Text;
            string email = emailBox.Text;
            string number = numberBox.Text;
            string twitter = twitterBox.Text;
            string facebook = facebookBox.Text;
            string company = companyBox.Text;

            if(this.ValidTitle(title))
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

                                                    string UpdateString = "INSERT INTO contacts (title, forename, surname, email_address, phone_numbers, twitter_username, facebook_username, company_works_for)" +
                                                "VALUES('" + title + "','" + forename + "','" + surname + "','" + email + "','" + number + "','" + twitter + "','" + facebook + "','" + company + "')";

                                            SDS.Update(UpdateString);
                                            new MainWindow().Show();
                                            this.Close();
                                            
                                        }
                                        else
                                        {
                                            // message to say they have NOT filled it in properly
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
        /// button that goes back to the Mainwindow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        /// <summary>
        /// validating the title combobox
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
            if(forename == "")
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
        /// matching string to regex regular expression pattern.
        /// validating the right input into the textbox
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
                if(email.IndexOf(".", email.IndexOf("@") ) > email.IndexOf("@"))
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
            string sPattern = "^[0-9a-zA-Z- ]+$";
            
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
            else if(facebook == "")
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

