using System;
using System.Collections.Generic;
using System.IO;
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

namespace LibrISv2
{
    public partial class PageConnectionProperties : Page
    {
        private static string server;
        private static string port;
        private static string username;
        private static string database;
        private static string password;

        public PageConnectionProperties()
        {
            InitializeComponent();
            tbServer.Text = server = MainWindow.server;
            tbPort.Text = port = MainWindow.port;
            tbUsername.Text = username = MainWindow.user;
            tbDatabase.Text = database = MainWindow.database;
            tbPassword.Text = password = MainWindow.password;
        }

        private void bChangeProperties_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("../connect.txt"))
            {
                StreamWriter sw = new StreamWriter("../connect.txt", false);
                sw.Write(tbServer.Text.Trim());
                sw.Write('|');
                sw.Write(tbPort.Text.Trim());
                sw.Write('|');
                sw.Write(tbUsername.Text.Trim());
                sw.Write('|');
                sw.Write(tbDatabase.Text.Trim());
                sw.Write('|');
                sw.Write(tbPassword.Text.Trim());
                sw.Close();
            }
            else
            {
                StreamWriter sw = File.CreateText("../connect.txt");
                sw.Write(tbServer.Text.Trim());
                sw.Write('|');
                sw.Write(tbPort.Text.Trim());
                sw.Write('|');
                sw.Write(tbUsername.Text.Trim());
                sw.Write('|');
                sw.Write(tbDatabase.Text.Trim());
                sw.Write('|');
                sw.Write(tbPassword.Text.Trim());
                sw.Close();
            }
            MessageBox.Show("Внесенные данные сохранены. Изменения вступят в силу после перезапуска");
        }

        private void tbServer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbServer.Text.Trim() == server)
            {
                tbServer.Text = string.Empty;
                tbServer.Foreground = Design._dark;
            }
        }
        private void tbServer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbServer.Text.Trim() == string.Empty)
            {
                tbServer.Text = server;
                tbServer.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text.Trim() == password)
            {
                tbPassword.Text = string.Empty;
                tbPassword.Foreground = Design._dark;
            }
        }
        private void tbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text.Trim() == string.Empty)
            {
                tbPassword.Text = password;
                tbPassword.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbDatabase_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbDatabase.Text.Trim() == database)
            {
                tbDatabase.Text = string.Empty;
                tbDatabase.Foreground = Design._dark;
            }
        }
        private void tbDatabase_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbDatabase.Text == string.Empty)
            {
                tbDatabase.Text = database;
                tbDatabase.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text.Trim() == username)
            {
                tbUsername.Text = string.Empty;
                tbUsername.Foreground = Design._dark;
            }
        }
        private void tbUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text == string.Empty)
            {
                tbUsername.Text = username;
                tbUsername.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbPort_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPort.Text.Trim() == port)
            {
                tbPort.Text = string.Empty;
                tbPort.Foreground = Design._dark;
            }
        }
        private void tbPort_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPort.Text == string.Empty)
            {
                tbPort.Text = port;
                tbPort.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
