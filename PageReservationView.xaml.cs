using Npgsql;
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

namespace LibrISv2
{
    public partial class PageReservationView : Page
    {
        public static ObservableCollection<ReportBooks> Books { get; set; } = new ObservableCollection<ReportBooks>();
        public static ObservableCollection<string> Keys { get; set; } = new ObservableCollection<string>();
        public PageReservationView()
        {
            InitializeComponent();
            KeyLoad();
            DataLoad.LoadIssues();
            cbKey.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = Keys });
        }
        
        public static void BooksLoad()
        {
            //string text = string.Empty;
            //var sp = new StackPanel();
            //NpgsqlCommand command = DBControl.GetCommand("SELECT keyword, storage, name, SUM (amount) AS total FROM \"Issue\" GROUP BY (keyword, storage, name)");
            //NpgsqlDataReader reader = command.ExecuteReader();
            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        if (reader.GetInt32(1) == MainWindow.currentUserWorkplace)
            //        {
            //            string a = reader.GetString(0);
            //            string c = reader.GetString(2);
            //            int d = reader.GetInt32(3);
            //            text += a + " " + c + " " + d.ToString() + "\n";
            //        }
            //    }
            //}
            //reader.Close();
            //MessageBox.Show(text);
        }

        public static void KeyLoad()
        {
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

        private void bPrint_Click(object sender, RoutedEventArgs e)
        {
            BooksLoad();
        }
    }
}
