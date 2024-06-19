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
    public partial class PageCatalog : Page
    {
        public PageCatalog()
        {
            InitializeComponent();
            DataLoad.LoadCatalog();
            lvCatalog.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Cat });
        }

        private void bDel_Click(object sender, RoutedEventArgs e)
        {
            NpgsqlCommand del = DBControl.GetCommand("DELETE FROM \"Nums\" WHERE num = @num");
            try
            {
                del.Parameters.AddWithValue("@num", NpgsqlDbType.Varchar, ((Catalog)lvCatalog.SelectedItem as Catalog).Num);
                int result = del.ExecuteNonQuery();
                if (result > 0)
                {
                    NpgsqlCommand cmd = DBControl.GetCommand("SELECT * FROM \"Nums\" WHERE book = @book");
                    try
                    {
                        cmd.Parameters.AddWithValue("book", NpgsqlDbType.Varchar, ((Catalog)lvCatalog.SelectedItem as Catalog).Id);
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            NpgsqlCommand del2 = DBControl.GetCommand("DELETE FROM \"Authorship\" WHERE issue = @id AND author = @author");
                            try
                            {
                                del2.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, ((Catalog)lvCatalog.SelectedItem as Catalog).Id);
                                del2.Parameters.AddWithValue("@author", NpgsqlDbType.Integer, ((Catalog)lvCatalog.SelectedItem as Catalog).AuthorCode);
                                int res2 = del2.ExecuteNonQuery();
                                if (res2 == 1)
                                {
                                    NpgsqlCommand del3 = DBControl.GetCommand("DELETE FROM \"Issue\" WHERE identifier = @id");
                                    try
                                    {
                                        del3.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, ((Catalog)lvCatalog.SelectedItem as Catalog).Id);
                                        int res3 = del3.ExecuteNonQuery();
                                        if (res3 == 1)
                                        {
                                            //MessageBox.Show("1");
                                            DataLoad.LoadCatalog();
                                        }
                                    }
                                    catch
                                    {
                                        //MessageBox.Show("2");
                                        DataLoad.LoadCatalog();
                                        return;
                                    }
                                    del3 = null;
                                }
                            }
                            catch
                            {
                                //MessageBox.Show("3");
                                DataLoad.LoadCatalog();
                                return;
                            }
                            del2 = null;
                        }
                        else
                        {
                            //MessageBox.Show("4");
                            DataLoad.LoadCatalog();
                            return;
                        }
                    }
                    catch
                    {
                        //MessageBox.Show("5");
                        DataLoad.LoadCatalog();
                        return;
                    }
                    cmd = null;
                }
                del = null;
            }
            catch
            {
                //MessageBox.Show("6");
                DataLoad.LoadCatalog();
                return;
            }
            DataLoad.LoadCatalog();
        }

        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = "Поиск";
            tbSearch.Foreground = Brushes.LightSlateGray;
        }

        private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.Text = string.Empty;
            tbSearch.Foreground = Design._dark;
        }
        private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSearch.Text == string.Empty)
            {
                tbSearch.Text = "Поиск";
                tbSearch.Foreground = Brushes.LightSlateGray;
            }
        }
        
        // Фильтр
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lvCatalog != null) CatalogFilter(lvCatalog.ItemsSource, tbSearch.Text.Trim());
        }
        private void CatalogFilter(object book, string filterString)
        {
            if (book == null) return;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(book);
            if (view != null)
            {
                view.Filter = issue =>
                {
                    if (filterString == "Поиск" || string.IsNullOrEmpty(filterString)) return true;
                    if (issue is Catalog _issue)
                    {
                        return _issue.Id.Contains(filterString) ||
                               _issue.Name.ToLower().Contains(filterString) ||
                               _issue.Name.ToUpper().Contains(filterString) ||
                               // Как должна выглядеть строка, чтобы всё нормально работало
                               //_issue.Name.Contains(filterString, StringComparison.CurrentCultureIgnoreCase)
                               _issue.Author.ToLower().Contains(filterString) ||
                               _issue.Author.ToUpper().Contains(filterString) || 
                               _issue.Num.ToLower().Contains(filterString) || 
                               _issue.Num.ToUpper().Contains(filterString);
                    }
                    else return false;
                };
            }
            else return;
        }
    }
}
