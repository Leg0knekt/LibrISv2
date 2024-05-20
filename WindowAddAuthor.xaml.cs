using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
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
    public partial class WindowAddAuthor : Window
    {
        public WindowAddAuthor()
        {
            InitializeComponent();
        }

        private void tbSurname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSurname.Text == "Фамилия (если есть)")
            {
                tbSurname.Text = string.Empty;
                tbSurname.Foreground = Brushes.Black;
                tbSurname.BorderBrush = Brushes.Black;
            }
        }

        private void tbSurname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSurname.Text == string.Empty)
            {
                tbSurname.Text = "Фамилия (если есть)";
                tbSurname.Foreground = Brushes.LightSlateGray;
                tbSurname.BorderBrush = Brushes.LightSlateGray;
            }
        }

        private void tbName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "Имя или псевдоним")
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
                tbName.Text = "Имя или псевдоним";
                tbName.Foreground = Brushes.LightSlateGray;
                tbName.BorderBrush = Brushes.LightSlateGray;
            }
        }

        private void tbPatronymic_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPatronymic.Text == "Отчество (если есть)")
            {
                tbPatronymic.Text = string.Empty;
                tbPatronymic.Foreground = Brushes.Black;
                tbPatronymic.BorderBrush = Brushes.Black;
            }
        }

        private void tbPatronymic_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPatronymic.Text == string.Empty)
            {
                tbPatronymic.Text = "Отчество (если есть)";
                tbPatronymic.Foreground = Brushes.LightSlateGray;
                tbPatronymic.BorderBrush = Brushes.LightSlateGray;
            }
        }

        private void tbPhoto_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPhoto.Text == "Ссылка на фото (не обязательно)")
            {
                tbPhoto.Text = string.Empty;
                tbPhoto.Foreground = Brushes.Black;
                tbPhoto.BorderBrush = Brushes.Black;
            }
        }

        private void tbPhoto_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPhoto.Text == string.Empty)
            {
                tbPhoto.Text = "Ссылка на фото (не обязательно)";
                tbPhoto.Foreground = Brushes.LightSlateGray;
                tbPhoto.BorderBrush = Brushes.LightSlateGray;
            }
        }

        private void bAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            string surname = tbSurname.Text.Trim();
            string name = tbName.Text.Trim();
            string patronymic = tbPatronymic.Text.Trim();
            string photo = tbPhoto.Text.Trim();
            if (name.Length == 0 || name == "Имя или псевдоним")
            {
                tbName.BorderBrush = Brushes.Crimson;
                return;
            }
            if (surname.Length == 0 || surname == "Фамилия (если есть)") surname = " ";
            if (patronymic.Length == 0 || patronymic == "Отчество (если есть)") patronymic = " ";
            if (photo.Length == 0 || photo == "Ссылка на фото (не обязательно)") photo = "/pic/no_image.png";

            NpgsqlCommand command = DBControl.GetCommand("INSERT INTO \"Author\" (surname, firstname, patronymic, photo) VALUES (@surname, @firstname, @patronymic, @photo)");
            try
            {
                command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, surname);
                command.Parameters.AddWithValue("@firstname", NpgsqlDbType.Varchar, name);
                command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, patronymic);
                command.Parameters.AddWithValue("@photo", NpgsqlDbType.Varchar, photo);
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Успешно");
                    DataLoad.LoadAuthors();
                    return;
                }
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка\n" + ex); }
        }
    }
}
