using Npgsql;
using NpgsqlTypes;
using System;
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

namespace LibrISv2
{
    public partial class PageAuthorization : Page
    {
        public PageAuthorization()
        {
            InitializeComponent();
        }

        private void bLogIn_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlCommand command = DBControl.GetCommand("SELECT firstname, patronymic, role, login, hashedpass, salt, workplace, organization " +
                                                         "FROM \"Employee\" JOIN \"Department\" ON \"Employee\".workplace = \"Department\".code " +
                                                         "WHERE (@login = login)");

            command.Parameters.AddWithValue("@login", NpgsqlDbType.Varchar, tbLogin.Text.Trim());
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                string hash = PasswordGeneration.GenerateHash(tbPassword.Password.Trim(), reader.GetString(5));
                if (reader.GetString(4) == hash)
                {
                    MainWindow.DisplayGreetings(reader.GetString(0), reader.GetString(1));
                    MainWindow.currentUserWorkplace = reader.GetInt32(6);
                    MainWindow.currentLibrary = reader.GetString(7);
                    string role = reader.GetString(2);
                    reader.Close();
                    switch (role)
                    {
                        case "Администратор":
                            NavigationService.Navigate(PageControl.PageAdminMenu);
                            break;
                        case "Сотрудник библиотечного фонда":
                            NavigationService.Navigate(PageControl.PageEmployeeMenu);
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Неверный пароль");
                }
                reader.Close();
            }
            else
            {
                MessageBox.Show("Указанный вами логин не существует");
                NavigationService.Navigate(PageControl.PageNewEmployee);            // тестовая функция
            }
            reader.Close();
        }

        private void tbLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text == "Логин")
            {
                tbLogin.Text = string.Empty;
                tbLogin.Foreground = Design._dark;
            }
        }
        private void tbLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text == string.Empty)
            {
                tbLogin.Text = "Логин";
                tbLogin.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            tbPassTip.Visibility = Visibility.Collapsed;
        }
        private void tbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Password == string.Empty)
            {
                tbPassTip.Visibility = Visibility.Visible;
            }
        }
    }
}
