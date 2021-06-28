using System;
using System.IO;
using System.Collections.Generic;
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
using System.Diagnostics;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GetPasswordwords();
        }

        public void GetPasswordwords()
        {
            pwmanListBox.Items.Clear();
            var password = new Password();
            password.CreatePasswordRootStruct();

            string rootLocation = password.GetPasswordRootLocation();
            var passwords = Directory.GetFiles(rootLocation + "\\passwords");
            foreach (var pass in passwords) 
            {
               pwmanListBox.Items.Add(pass.Replace(rootLocation.ToString(), "").Replace(".txt", "").Replace("\\passwords\\", ""));
                
            }
            pwManWelcomeLabel.Content = "Welcome, " + password.GetUserName() + "!";
            if (passwords.Length == 0)
            {
                pwmanListBox.Items.Add("You don't have any passwords yet! Click the 'Set Password' button to create one!");
            }
            
            
        }

        private void pwManListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            var password = new Password();
            //string text = System.IO.File.ReadAllText(password.GetPasswordRootLocation() + @"\passwords\" + pwmanListBox.SelectedItem.ToString() + ".txt");
            string PasswordLocation = password.GetPasswordRootLocation() + @"\passwords\" + pwmanListBox.SelectedItem.ToString() + ".txt";
            bool FileExists = System.IO.File.Exists(PasswordLocation);
            Log log = new Log();
            if (FileExists)
            {

                Process process = new Process();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = @"-windowstyle hidden -noprofile -executionpolicy bypass -file C:\Users\Tyler\AppData\Roaming\PasswordManager\Scripts\get-password.ps1 -Location " + "\"" + PasswordLocation + "\"";
                process.Start();
                log.SetLog("The password for " + pwmanListBox.SelectedItem.ToString() + " has been copied to your clipboard.");
                pwman_ActivityLog_Label.Content = log.GetLog();
            } else
            {
                log.SetLog("Unable to get password.");
            }
        }

        private void pwmanSetPass_Btn_Click(object sender, RoutedEventArgs e)
        {
            var password = new Password();
            Log log = new Log();
            string pass = pwman_SetPass_PasswordBox.Password.ToString();
            if (!String.IsNullOrEmpty(pass) && !String.IsNullOrEmpty(pwman_SetPass_PasswordName.Text))
            {
                Process process = new Process();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = @"-windowstyle hidden -noprofile -executionpolicy bypass -file C:\Users\Tyler\AppData\Roaming\PasswordManager\Scripts\set-password.ps1 -Password " + "\"" + pass + "\"" + @" -PasswordLocation " + "\"" + password.GetPasswordRootLocation() + @"\passwords\" + pwman_SetPass_PasswordName.Text + ".txt" + "\"";
                process.Start();
                log.SetLog("Password created. Double click on password to copy to clipboard.");
                pwman_ActivityLog_Label.Content = log.GetLog();
            } else
            {
                log.SetLog("Unable to set password.");
                pwman_ActivityLog_Label.Content = log.GetLog();


            }
            System.Threading.Thread.Sleep(1000);
            pwman_SetPass_PasswordBox.Password = "";
            pwman_SetPass_PasswordName.Text = "";
            GetPasswordwords();


        }

        public void RemovePassword(string item)
        {
            Password password = new Password();
            Log log = new Log();
            if (!String.IsNullOrEmpty(item))
            {
                string passwordLocation = password.GetPasswordRootLocation() + @"\passwords\" + item + ".txt";
                if (File.Exists(passwordLocation))
                {
                    File.Delete(passwordLocation);
                    log.SetLog("Deleted password \"" + item + "\"");
                    pwman_ActivityLog_Label.Content = log.GetLog();
                }
            }
            else
            {
                log.SetLog("Failed to delet password \"" + item + "\"");
                pwman_ActivityLog_Label.Content = log.GetLog();
            }

            System.Threading.Thread.Sleep(1000);
            GetPasswordwords();
        }

        private void pwmanListBox_KeyDown(object sender, KeyEventArgs e)
        {
            Password password = new Password();
            if (pwmanListBox.SelectedItem != null)
            {
                string passwordLocation = password.GetPasswordRootLocation() + @"\passwords\" + pwmanListBox.SelectedItem.ToString() + ".txt";
                if (Keyboard.IsKeyDown(Key.Delete) && File.Exists(passwordLocation))
                {
                    RemovePassword(pwmanListBox.SelectedItem.ToString());
                }
            }
        }
    }
}
