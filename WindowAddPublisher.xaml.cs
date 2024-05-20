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
using System.Windows.Shapes;

namespace LibrISv2
{
    public partial class WindowAddPublisher : Window
    {
        public WindowAddPublisher()
        {
            InitializeComponent();
        }

        private void tbOGRN_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbOGRN.Text == "ОГРН")
            {
                tbOGRN.Text = string.Empty;
                tbOGRN.Foreground = Brushes.Black;
                tbOGRN.BorderBrush = Brushes.Black;
            }
        }

        private void tbOGRN_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbOGRN.Text == string.Empty)
            {
                tbOGRN.Text = "ОГРН";
                tbOGRN.Foreground = Brushes.LightSlateGray;
                tbOGRN.BorderBrush = Brushes.LightSlateGray;
            }
        }

        private void tbName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "Название")
            {
                tbName.Text = string.Empty;
                tbName.Foreground = Brushes.Black;
                tbName.BorderBrush = Brushes.Black;
            }
        }

        private void tbName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == string.Empty)
            {
                tbName.Text = "Название";
                tbName.Foreground = Brushes.LightSlateGray;
                tbName.BorderBrush = Brushes.LightSlateGray;
            }
        }

        private void bAccept_Click(object sender, RoutedEventArgs e)
        {
            string ogrn = tbOGRN.Text.Trim();
            string name = tbName.Text.Trim();
            if (ogrn.Length == 13 && name.Length != 0 && name != "Название")
            {
                NpgsqlCommand command = DBControl.GetCommand("SELECT \"OGRN\", name FROM \"Publisher\" WHERE \"OGRN\" = @ogrn ORDER BY name");
                command.Parameters.AddWithValue("@ogrn", NpgsqlDbType.Varchar, ogrn);
                NpgsqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    NpgsqlCommand cmd = DBControl.GetCommand("INSERT INTO \"Publisher\" (\"OGRN\", name) VALUES (@ogrn, @name)");
                    try
                    {
                        cmd.Parameters.AddWithValue("@ogrn", NpgsqlDbType.Varchar, ogrn);
                        cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, name);
                        int result = cmd.ExecuteNonQuery();
                        if (result == 1)
                        {
                            MessageBox.Show("Успешно");
                            DataLoad.LoadPublishers();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Произошла ошибка\n" + ex);
                    }
                }
                else MessageBox.Show("Вы ввели неверный ОГРН или данное издательство уже есть в списке. Проверьте данные и попробуйте ещё раз");
                reader.Close();
            }
            else
            {
                if (ogrn.Length != 13) tbOGRN.BorderBrush = Brushes.Crimson;
                if (name.Length == 0 || name == "Название") tbName.BorderBrush = Brushes.Crimson;
                return;
            }
        }
    }
}
