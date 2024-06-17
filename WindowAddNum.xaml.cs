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
using System.Xml.Linq;

namespace LibrISv2
{
    public partial class WindowAddNum : Window
    {
        int check = 0;
        int amount = PageAddIssue.thisBookAmount;
        string id = PageAddIssue.thisBook;
        public WindowAddNum()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void bNext_Click(object sender, RoutedEventArgs e)
        {
            if (check < amount)
            {
                AddNumber();
                check++;
                tbNum.Text = string.Empty;
            }
            if (check == amount)
            {
                PageAddIssue.CreateNumbers();
                this.Close();
            }
        }

        private void AddNumber()
        {
            if (tbNum.Text.Trim() != null && tbNum.Text.Trim() != string.Empty)
            {
                NpgsqlCommand innercmd = DBControl.GetCommand("SELECT num FROM \"Nums\" WHERE book = @id");
                innercmd.Parameters.AddWithValue("id", NpgsqlDbType.Varchar, tbNum.Text);
                NpgsqlDataReader innerreader = innercmd.ExecuteReader();
                if (!innerreader.HasRows)
                {
                    PageAddIssue.Numbers.Add(tbNum.Text.ToLower().Trim());
                }
                innerreader.Close();
            }
        }
    }
}
