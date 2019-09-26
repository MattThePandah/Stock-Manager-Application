// Core System Usings
using System;
using System.ComponentModel;
using System.Windows;

// Personal Created Using statements
using Stock_Manager_Application.Model;

namespace Stock_Manager_Application
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static string userName;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            // Creates a new user
            User user = new User
            {
                // Information from Textbox
                UserName = usernameTextBox.Text,
                Password = passwordTextBox.Text
            };

            // Run authorisation method and store result here TODO: Create this method.
            bool isAuthorised = false;

            if (isAuthorised)
            {
                // We take the original value from the user and store it locally.
                userName = user.UserName;

                // Hides the current window.
                Hide();

                // We show the home screen instead of the login screen.
                // TODO: Write home screen methods
            }
            else
            {
                MessageBox.Show("Invalid Username or Password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        // This is called when the Login Window has loaded successfully.
        private void loginWindow_Loaded(object sender, EventArgs e)
        {
            // This sets the text box on launch to be the core focus
            usernameTextBox.Focus();
        }

        // This is called when the login window has closed succesfully.
        private void loginWindow_Closed(object sender, EventArgs e)
        {
            // This gracefully and progmatically shuts down the application
            Application.Current.Shutdown();
            return;
        }
    }
}
