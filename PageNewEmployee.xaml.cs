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
    public partial class PageNewEmployee : Page
    {
        public PageNewEmployee()
        {
            InitializeComponent();
            DataLoad.LoadDepartments();
            DataLoad.LoadRoles();
            cbDepartment.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Departments });
            cbRole.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Roles });
        }

        private void bAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (tbSurname.Text.Trim().Length == 0 ||
                tbFirstname.Text.Trim().Length == 0 ||
                tbPhone.Text.Trim().Length == 0 ||
                tbTIN.Text.Trim().Length == 0 ||
                tbPosition.Text.Trim().Length == 0 ||
                tbLogin.Text.Trim().Length == 0 ||
                cbDepartment.SelectedIndex != -1 ||
                cbRole.SelectedIndex != -1)
            {
                string salt = PasswordGeneration.GenerateSalt();
                string pass = PasswordGeneration.GeneratePass();
                int code = ((Department)cbDepartment.SelectedItem).Code;
                string role = ((Role)cbRole.SelectedItem).Name;

                NpgsqlCommand command = DBControl.GetCommand("INSERT INTO \"Employee\" VALUES(@tin, @surname, @firstname, @patronymic, @phone, " +
                                                                                             "@workplace, @position, @role, @login, @hashedpass, @salt)");
                try
                {
                    command.Parameters.AddWithValue("@tin", NpgsqlDbType.Varchar, tbTIN.Text.Trim());
                    command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, tbSurname.Text.Trim());
                    command.Parameters.AddWithValue("@firstname", NpgsqlDbType.Varchar, tbFirstname.Text.Trim());
                    command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, tbPatronymic.Text.Trim() ?? "");
                    command.Parameters.AddWithValue("@phone", NpgsqlDbType.Varchar, tbPhone.Text.Trim());
                    command.Parameters.AddWithValue("@workplace", NpgsqlDbType.Integer, code);
                    command.Parameters.AddWithValue("@position", NpgsqlDbType.Varchar, tbPosition.Text.Trim());
                    command.Parameters.AddWithValue("@role", NpgsqlDbType.Varchar, role);
                    command.Parameters.AddWithValue("@login", NpgsqlDbType.Varchar, tbLogin.Text.Trim());
                    command.Parameters.AddWithValue("@hashedpass", NpgsqlDbType.Varchar, PasswordGeneration.GenerateHash(pass, salt));
                    command.Parameters.AddWithValue("@salt", NpgsqlDbType.Varchar, salt);
                    int result = command.ExecuteNonQuery();
                    if (result == 1)
                    {
                        MessageBox.Show("Сотрудник успешно добавлен. Для входа используйте логин " + tbLogin.Text.Trim() + " и пароль " + pass);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex);
                }
                tbTIN.Text = string.Empty;
                tbSurname.Text = string.Empty;
                tbFirstname.Text = string.Empty;
                tbPatronymic.Text = string.Empty;
                tbPhone.Text = string.Empty;
                tbPosition.Text = string.Empty;
                tbLogin.Text = string.Empty;
                cbDepartment.SelectedIndex = -1;
                cbRole.SelectedIndex = -1;
            }
        }

        // Изменение выбора
        private void cbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDepartment.SelectedIndex != -1)
            {
                tbDepartmentTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbDepartmentTip.Foreground = Brushes.LightSlateGray;
                tbDepartmentTip.Visibility = Visibility.Visible;
            }
        }
        private void cbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRole.SelectedIndex != -1)
            {
                tbRoleTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbRoleTip.Foreground = Brushes.LightSlateGray;
                tbRoleTip.Visibility = Visibility.Visible;
            }
        }

        // Обработка фокусов
        private void tbSurname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSurname.Text == "Фамилия")
            {
                tbSurname.Text = string.Empty;
                tbSurname.Foreground = Design._dark;
            }
        }
        private void tbSurname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSurname.Text == string.Empty)
            {
                tbSurname.Text = "Фамилия";
                tbSurname.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbFirstname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbFirstname.Text == "Имя")
            {
                tbFirstname.Text = string.Empty;
                tbFirstname.Foreground = Design._dark;
            }
        }
        private void tbFirstname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbFirstname.Text == string.Empty)
            {
                tbFirstname.Text = "Имя";
                tbFirstname.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbPatronymic_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPatronymic.Text == "Отчество (если есть)")
            {
                tbPatronymic.Text = string.Empty;
                tbPatronymic.Foreground = Design._dark;
            }
        }
        private void tbPatronymic_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPatronymic.Text == string.Empty)
            {
                tbPatronymic.Text = "Отчество (если есть)";
                tbPatronymic.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPhone.Text == "Телефон")
            {
                tbPhone.Text = string.Empty;
                tbPhone.Foreground = Design._dark;
            }
        }
        private void tbPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPhone.Text == string.Empty)
            {
                tbPhone.Text = "Телефон";
                tbPhone.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbTIN_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbTIN.Text == "ИНН")
            {
                tbTIN.Text = string.Empty;
                tbTIN.Foreground = Design._dark;
            }
        }
        private void tbTIN_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbTIN.Text == string.Empty)
            {
                tbTIN.Text = "ИНН";
                tbTIN.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbPosition_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPosition.Text == "Должность")
            {
                tbPosition.Text = string.Empty;
                tbPosition.Foreground = Design._dark;
            }
        }
        private void tbPosition_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPosition.Text == string.Empty)
            {
                tbPosition.Text = "Должность";
                tbPosition.Foreground = Brushes.LightSlateGray;
            }
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

    }
}
