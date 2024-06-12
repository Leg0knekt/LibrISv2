using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Linq;

namespace LibrISv2
{
    public partial class PageReservationView : Page
    {
        public static ObservableCollection<ReportBooks> Books { get; set; } = new ObservableCollection<ReportBooks>();
        public static ObservableCollection<string> Keys { get; set; } = new ObservableCollection<string>();
        public static ComboBox cb;
        private static float books;
        private static float clients;
        public PageReservationView()
        {
            InitializeComponent();
            tbTitle.Text = string.Empty;
            tbLabel.Text = string.Empty;
            books = 0;
            clients = 0;
            Books.Clear();
            KeyLoad();
            cbKey.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = Keys });
            lvReportBooks.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = Books });
            cb = cbKey;
            DataContext = this;
        }
        
        private void BooksLoad()
        {
            if (cb != null)
            {
                NpgsqlCommand command = DBControl.GetCommand("SELECT keyword, identifier, storage, name, amount FROM \"Issue\"");
                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(2) == MainWindow.currentUserWorkplace && reader.GetString(0) == cbKey.SelectedItem.ToString())
                        {
                            Books.Add(new ReportBooks(reader.GetString(0), reader.GetString(1), reader.GetString(3), reader.GetInt32(4)));
                        }
                    }
                }
                reader.Close();
                foreach (ReportBooks book in Books)
                {
                    NpgsqlCommand command1 = DBControl.GetCommand("SELECT COUNT (issue) FROM \"Operation\" WHERE status = 'Выдано' AND issue = @issue");
                    command1.Parameters.AddWithValue("issue", book.Id);
                    NpgsqlDataReader reader1 = command1.ExecuteReader();
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            book.Amount += reader.GetInt32(0);
                        }
                    }
                    reader1.Close();
                    books += book.Amount;
                }
                NpgsqlCommand command2 = DBControl.GetCommand("SELECT COUNT (libcard) FROM \"Client\"");
                NpgsqlDataReader reader2 = command2.ExecuteReader();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clients += reader.GetInt32(0);
                    }
                }
                reader2.Close();

            }
            else return;
        }

        public static void KeyLoad()
        {
            Keys.Clear();
            NpgsqlCommand command = DBControl.GetCommand("SELECT DISTINCT keyword, storage FROM \"Issue\"");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(1) == MainWindow.currentUserWorkplace)
                    {
                        Keys.Add(reader.GetString(0));
                    }
                }
            }
            reader.Close();
        }
        private void cbKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbKey.SelectedIndex == -1)
            {
                bReport.IsEnabled = false;
            }
            else
            {
                bReport.IsEnabled = true;
            }
        }
        private void bReport_Click(object sender, RoutedEventArgs e)
        {
            Books.Clear();
            books = 0;
            clients = 0;
            BooksLoad();
            tbTitle.Text = "Отчет книгообеспеченность за " + DateTime.Now.Year;
            tbLabel.Text = "Коэффициент книгообеспеченности: ";
            float coeff = books / clients;
            var roundCoeff = (Math.Round((decimal)coeff, 1));
            tbCoeff.Text = roundCoeff.ToString();

        }
        private void bPrint_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
