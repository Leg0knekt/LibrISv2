using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibrISv2
{
    public partial class PageNewClient : Page
    {
        public PageNewClient()
        {
            InitializeComponent();
            DataLoad.LoadSocialStatuses();
            DataLoad.LoadClients();
            lvClients.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Clients });
            cbStatus.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.SocialStatuses });
        }

        // Работа с базой
        private void AddClient(string libcard, string surname, string name, string patronymic, string phone, int year, string status)
        {
            NpgsqlCommand precmd = DBControl.GetCommand("SELECT libcard FROM \"Client\" WHERE libcard = @libcard");
            precmd.Parameters.AddWithValue("@libcard", NpgsqlDbType.Varchar, libcard);
            NpgsqlDataReader reader = precmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand command = DBControl.GetCommand("INSERT INTO \"Client\" (libcard, surname, firstname, patronymic, phone, socialstatus, debt, birthyear)" +
                                                             "VALUES (@libcard, @surname, @firstname, @patronymic, @phone, @socialstatus, @debt, @birthyear)");
                string patr = "";
                if (tbPatronymic.Text != "Отчество (если есть)") { patr = tbPatronymic.Text; }
                try
                {
                    command.Parameters.AddWithValue("@libcard", NpgsqlDbType.Varchar, libcard);
                    command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, surname);
                    command.Parameters.AddWithValue("@firstname", NpgsqlDbType.Varchar, name);
                    command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, patr);
                    command.Parameters.AddWithValue("@phone", NpgsqlDbType.Varchar, phone);
                    command.Parameters.AddWithValue("@socialstatus", NpgsqlDbType.Varchar, status);
                    command.Parameters.AddWithValue("@debt", NpgsqlDbType.Boolean, false);
                    command.Parameters.AddWithValue("@birthyear", NpgsqlDbType.Integer, year);
                    int result = command.ExecuteNonQuery();
                    if (result == 1)
                    {
                        MessageBox.Show("Запись добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось добавить запись\n" + ex);
                    return;
                }
            }
            else MessageBox.Show("Читательский билет с таким номером уже зарегистрирован");
        }
        private void UpdateClient(string libcard, string surname, string name, string patronymic, string phone, int year, string status)
        {
            NpgsqlCommand command = DBControl.GetCommand("UPDATE \"Client\" SET libcard = @libcard, surname = @surname, firstname = @firstname, patronymic = @patronymic, " +
                                                         "phone = @phone, birthyear = @birthyear, socialstatus = @socialstatus, debt = @debt WHERE libcard = @libcard");
            try
            {
                command.Parameters.AddWithValue("@libcard", NpgsqlDbType.Varchar, libcard);
                command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, surname);
                command.Parameters.AddWithValue("@firstname", NpgsqlDbType.Varchar, name);
                command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, patronymic);
                command.Parameters.AddWithValue("@phone", NpgsqlDbType.Varchar, phone);
                command.Parameters.AddWithValue("@birthyear", NpgsqlDbType.Integer, year);
                command.Parameters.AddWithValue("@socialstatus", NpgsqlDbType.Varchar, status);
                command.Parameters.AddWithValue("@debt", NpgsqlDbType.Boolean, false);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Запись обновлена");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось обновить данные\n" + ex);
            }
        }

        // Поиск
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lvClients != null) ClientFilter(lvClients.ItemsSource, tbSearch.Text.Trim());
        }
        private void ClientFilter(object client, string searchString)
        {
            if (client == null) return;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(client);
            if (view != null)
            {
                view.Filter = visitor =>
                {
                    if (searchString == "Найти" || string.IsNullOrEmpty(searchString)) return true;
                    if (visitor is Client _client)
                    {
                        return _client.LibCard.ToLower().Contains(searchString) ||
                               _client.LibCard.ToUpper().Contains(searchString) ||
                               _client.Surname.ToLower().Contains(searchString) ||
                               _client.Surname.ToUpper().Contains(searchString) ||
                               _client.FirstName.ToLower().Contains(searchString) ||
                               _client.FirstName.ToUpper().Contains(searchString) ||
                               // Как должна выглядеть строка, чтобы всё нормально работало
                               //_client.Patronymic.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)
                               _client.Patronymic.ToLower().Contains(searchString) ||
                               _client.Patronymic.ToUpper().Contains(searchString);
                    }
                    else return false;
                };
            }
            else return;
        }

        // Кнопки
        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            tbLibCard.Text = "Читательский билет";
            tbLibCard.Foreground = Brushes.LightSlateGray;

            tbSurname.Text = "Фамилия";
            tbSurname.Foreground = Brushes.LightSlateGray;

            tbName.Text = "Имя";
            tbName.Foreground = Brushes.LightSlateGray;

            tbPatronymic.Text = "Отчество (если есть)";
            tbPatronymic.Foreground = Brushes.LightSlateGray;

            tbPhone.Text = "Телефон";
            tbPhone.Foreground = Brushes.LightSlateGray;

            tbSearch.Text = "Найти";
            tbSearch.Foreground = Brushes.LightSlateGray;

            tbYear.Text = "Год рождения";
            tbYear.Foreground = Brushes.LightSlateGray;

            cbStatus.SelectedIndex = -1;
            tbStatusTip.Visibility = Visibility.Visible;
            tbStatusTip.Foreground = Brushes.LightSlateGray;

            lvClients.SelectedIndex = -1;
        }
        private void bSaveClient_Click(object sender, RoutedEventArgs e)
        {
            string warning = "Необходимо заполнить поля:\n";
            int troubles = 0;
            int year = BirthYearFillingCheck(tbYear.Text.Trim());
            string libcard = tbLibCard.Text.Trim();
            string surname = tbSurname.Text.Trim();
            string name = tbName.Text.Trim();
            string patronymic = tbPatronymic.Text.Trim();
            string phone = tbPhone.Text.Trim();
            string status = "";

            if (!LibCardFillingCheck(libcard))
            {
                warning += "«Читательский билет»\n";
                troubles++;
            }
            if (!SurnameFillingCheck(surname))
            {
                warning += "«Фамилия»\n";
                troubles++;
            }
            if (!NameFillingCheck(name))
            {
                warning += "«Имя»\n";
                troubles++;
            }
            if (!PhoneFillingCheck(phone))
            {
                warning += "«Телефон»\n";
                troubles++;
            }
            if (year == 0)
            {
                warning += "«Год рождения»\n";
                troubles++;
            }
            if (!StatusFillingCheck((SocialStatus)cbStatus.SelectedItem))
            {
                warning += "«Статус»\n";
                troubles++;
            }
            else status = ((SocialStatus)cbStatus.SelectedItem as SocialStatus).Status;

            if (troubles > 0)
            {
                MessageBox.Show(warning + "Введите все необходимые данные и повторите попытку");
                return;
            }

            if (troubles == 0)
            {

                if (lvClients.SelectedIndex == -1)
                {
                    AddClient(libcard, surname, name, patronymic, phone, year, status);
                    bClear_Click(sender, e);
                }
                else
                {
                    UpdateClient(libcard, surname, name, patronymic, phone, year, status);
                    bClear_Click(sender, e);
                }
                DataLoad.LoadClients();
            }
        }
        private void bCancelFilter_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = "Найти";
        }

        // Выбор значений
        private void cbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbStatus.SelectedIndex != -1) tbStatusTip.Visibility = Visibility.Hidden;
            else tbStatusTip.Visibility = Visibility.Visible;
        }
        private void lvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Client selectedClient = (Client)lvClients.SelectedItem;
            if (selectedClient != null)
            {
                lvClients.BorderBrush = Design._dark;

                tbLibCard.Foreground = tbSurname.Foreground = tbName.Foreground = tbPatronymic.Foreground = tbPhone.Foreground = tbYear.Foreground = Design._dark;
                tbLibCard.Text = selectedClient.LibCard;
                tbSurname.Text = selectedClient.Surname;
                tbName.Text = selectedClient.FirstName;
                tbPatronymic.Text = selectedClient.Patronymic;
                tbPhone.Text = selectedClient.Phone;
                tbYear.Text = selectedClient.BirthYear.ToString();

                for (int i = 0; i < lvClients.Items.Count; i++)
                {

                    if (((SocialStatus)cbStatus.Items[i]).Status == selectedClient.SocialStatus)
                    {
                        cbStatus.SelectedIndex = i;
                        return;
                    }
                }


            }
            else lvClients.BorderBrush = Brushes.LightSlateGray;
        }

        // Обработка фокусов
        private void tbLibCard_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbLibCard.Text == "Читательский билет")
            {
                tbLibCard.Text = string.Empty;
                tbLibCard.Foreground = Design._dark;
            }
        }
        private void tbLibCard_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbLibCard.Text == string.Empty)
            {
                tbLibCard.Text = "Читательский билет";
                tbLibCard.Foreground = Brushes.LightSlateGray;
            }
        }
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
        private void tbName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "Имя")
            {
                tbName.Text = string.Empty;
                tbName.Foreground = Design._dark;
            }
        }
        private void tbName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == string.Empty)
            {
                tbName.Text = "Имя";
                tbName.Foreground = Brushes.LightSlateGray;
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
        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text == "Найти")
            {
                tbSearch.Text = string.Empty;
                tbSearch.Foreground = Design._dark;
            }
        }
        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text == string.Empty)
            {
                tbSearch.Text = "Найти";
                tbSearch.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbYear_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbYear.Text == "Год рождения")
            {
                tbYear.Text = string.Empty;
                tbYear.Foreground = Design._dark;
            }
        }
        private void tbYear_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbYear.Text == string.Empty)
            {
                tbYear.Text = "Год рождения";
                tbYear.Foreground = Brushes.LightSlateGray;
            }
        }

        // Проверка заполнения полей
        private bool LibCardFillingCheck(string libcard)
        {
            if (libcard == "Читательский билет")
            {
                tbLibCard.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private bool SurnameFillingCheck(string surname)
        {
            if (surname == "Фамилия")
            {
                tbSurname.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private bool NameFillingCheck(string name)
        {
            if (name == "Имя")
            {
                tbName.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private bool PhoneFillingCheck(string phone)
        {
            if (phone == "Телефон")
            {
                tbPhone.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private int BirthYearFillingCheck(string year)
        {
            if (year == "Год рождения")
            {
                tbYear.Foreground = Brushes.Crimson;
                return 0;
            }
            try { return int.Parse(year); }
            catch { return 0; }
        }
        private bool StatusFillingCheck(SocialStatus status)
        {
            if (status == null)
            {
                tbStatusTip.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
    }
}
