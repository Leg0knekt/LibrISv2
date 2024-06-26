﻿using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Windows.Media.Animation;
using System.Windows.Annotations;

namespace LibrISv2
{
    public partial class PageAddIssue : Page
    {
        public static string thisBook;
        public static int thisBookAmount;
        public ObservableCollection<Author> SelectedAuthors = new ObservableCollection<Author>();
        public static List<string> Numbers = new List<string>();
        private bool addCheck = false;
        public PageAddIssue()
        {
            InitializeComponent();
            DataLoad.LoadAuthors();
            DataLoad.LoadBBK();
            DataLoad.LoadIssueTypes();
            DataLoad.LoadPublishers();
            DataLoad.LoadUDK();
            lvAuthors.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Authors });
            cbBBK.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.LibClassifications });
            cbType.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.IssueTypes });
            cbPublisher.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Publishers });
            cbUDK.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.DecClassifications });
            thisBook = string.Empty;
            thisBookAmount = 0;
            addCheck = false;
        }

        // Фильтрация
        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lvAuthors != null) AuthorFilter(lvAuthors.ItemsSource, tbFilter.Text.Trim());
            foreach (Author sa in SelectedAuthors)
            {
                if (lvAuthors != null) lvAuthors.SelectedItems.Add(sa);
            }
        }
        private void AuthorFilter(object author, string filterString)
        {
            if (author == null) return;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(author);
            if (view != null)
            {
                view.Filter = writer =>
                {
                    if (filterString == "Найти автора" || string.IsNullOrEmpty(filterString)) return true;
                    if (writer is Author _author)
                    {
                        return _author.Surname.ToLower().Contains(filterString) ||
                               _author.Surname.ToUpper().Contains(filterString) ||
                               _author.FirstName.ToLower().Contains(filterString) ||
                               _author.FirstName.ToUpper().Contains(filterString) ||
                               // Как должна выглядеть строка, чтобы всё нормально работало
                               //_author.Patronymic.Contains(filterString, StringComparison.CurrentCultureIgnoreCase)
                               _author.Patronymic.ToLower().Contains(filterString) ||
                               _author.Patronymic.ToUpper().Contains(filterString);
                    }
                    else return false;
                };
            }
            else return;
        }

        // Кнопки
        private void bAdd_Click(object sender, RoutedEventArgs e)
        {
            string warning = "Необходимо заполнить поля:\n";
            string keyword = " ";
            int troubles = 0;
            int amountChecked = AmountFillingCheck(tbAmount.Text.Trim());
            int pageChecked = PageNumFillingCheck(tbPageNum.Text.Trim());
            int yearChecked = YearFillingCheck(tbYear.Text.Trim());
            if (!IDFillingCheck(tbID.Text.Trim()))
            {
                warning += "«ISBN/ISSN»\n";
                troubles++;
            }
            if (!NameFillingCheck(tbName.Text.Trim()))
            {
                warning += "«Наименование»\n";
                troubles++;
            }
            if (!PublisherFillingCheck((Publisher)cbPublisher.SelectedItem))
            {
                warning += "«Издательство»\n";
                troubles++;
            }
            if (yearChecked == 0)
            {
                warning += "«Год издания»\n";
                troubles++;
            }
            if (pageChecked == 0)
            {
                warning += "«Кол-во страниц»\n";
                troubles++;
            }
            if (!BBKFillingCheck((LibraryClassification)cbBBK.SelectedItem))
            {
                warning += "«ББК»\n";
                troubles++;
            }
            if (!UDKFillingCheck((DecimalClassification)cbUDK.SelectedItem))
            {
                warning += "«УДК»\n";
                troubles++;
            }
            if (!ASignFillingCheck(tbSign.Text.Trim()))
            {
                warning += "«Авторский знак»\n";
                troubles++;
            }
            if (amountChecked == 0)
            {
                warning += "«В наличии»\n";
                troubles++;
            }
            if (!TypeFillingCheck((IssueType)cbType.SelectedItem))
            {
                warning += "«Тип издания»\n";
                troubles++;
            }
            if (!AuthorFillingCheck((Author)lvAuthors.SelectedItem))
            {
                warning += "«Автор»\n";
                troubles++;
            }
            if (!TagFillingCheck(tbTag.Text.Trim()))
            {
                warning += "«Ключевые слова»\n";
                troubles++;
            }
            if (troubles > 0) { MessageBox.Show(warning + "Введите все необходимые данные и повторите попытку"); }

            if (troubles == 0)
            {

                CreateBook(tbID.Text.Trim(),
                           tbName.Text.Trim(),
                           ((IssueType)cbType.SelectedItem).Type,
                           ((LibraryClassification)cbBBK.SelectedItem).Index,
                           ((DecimalClassification)cbUDK.SelectedItem).Index,
                           yearChecked,
                           ((Publisher)cbPublisher.SelectedItem).OGRN,
                           pageChecked,
                           MainWindow.currentUserWorkplace,
                           amountChecked,
                           " ",
                           tbImage.Text.Trim(),
                           tbSign.Text.Trim(),
                           tbTag.Text.ToLower().Trim());

                thisBook = tbID.Text.Trim();
                thisBookAmount = amountChecked;
                if (addCheck == false)
                {
                    WindowAddNum windowAddNum = new WindowAddNum();
                    windowAddNum.Show();
                }
                else
                {
                    addCheck = false;
                }
            }
        }
        private void bAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            WindowAddAuthor wAddAuthor = new WindowAddAuthor();
            wAddAuthor.Show();
        }
        private void bAddPublisher_Click(object sender, RoutedEventArgs e)
        {
            WindowAddPublisher wAddPublisher = new WindowAddPublisher();
            wAddPublisher.Show();
        }
        private void bCancelFilter_Click(object sender, RoutedEventArgs e)
        {
            tbFilter.Text = "Найти автора";
            tbFilter.Foreground = Brushes.LightSlateGray;
        }
        private void bClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
        private void ClearFields()
        {
            tbAmount.Text = "В наличии";
            tbAmount.Foreground = Brushes.LightSlateGray;

            tbFilter.Text = "Найти автора";
            tbFilter.Foreground = Brushes.LightSlateGray;
            tbFilter.IsEnabled = false;

            tbID.Text = "ISBN / ISSN";
            tbID.Foreground = Brushes.LightSlateGray;

            tbImage.Text = "Ссылка на изображение";
            tbImage.Foreground = Brushes.LightSlateGray;

            tbName.Text = "Наименование";
            tbName.Foreground = Brushes.LightSlateGray;

            tbPageNum.Text = "Кол-во страниц";
            tbPageNum.Foreground = Brushes.LightSlateGray;

            tbSign.Text = "Авторский знак";
            tbSign.Foreground = Brushes.LightSlateGray;

            tbYear.Text = "Год издания";
            tbYear.Foreground = Brushes.LightSlateGray;

            tbTag.Text = "Ключевые слова";
            tbTag.Foreground = Brushes.LightSlateGray;

            cbBBK.SelectedIndex = -1;
            tbBBKTip.Foreground = Brushes.LightSlateGray;

            cbType.SelectedIndex = -1;
            tbTypeTip.Foreground = Brushes.LightSlateGray;

            cbPublisher.SelectedIndex = -1;
            tbPubTip.Foreground = Brushes.LightSlateGray;

            cbUDK.SelectedIndex = -1;
            tbUDKTip.Foreground = Brushes.LightSlateGray;

            lvAuthors.SelectedIndex = -1;
            lvAuthors.BorderBrush = Brushes.LightSlateGray;
            lvAuthors.IsEnabled = false;
        }
        //
        public static void CreateNumbers()
        {
            bool success = false;
            foreach (string num in Numbers)
            {
                
                NpgsqlCommand cmd = DBControl.GetCommand("INSERT INTO \"Nums\" (book, num) VALUES (@book, @num)");
                try
                {
                    cmd.Parameters.AddWithValue("@book", NpgsqlDbType.Varchar, thisBook);
                    cmd.Parameters.AddWithValue("@num", NpgsqlDbType.Varchar, num);
                    int result = cmd.ExecuteNonQuery();
                    if (result == 1)
                    {
                        success = true;
                    }
                }
                catch
                {
                    success = false;
                }
            }
            
            switch (success)
            {
                case true:
                    MessageBox.Show("Добавлено книг: " + thisBookAmount);
                    Numbers.Clear();
                    break;
                case false:
                    NpgsqlCommand del1 = DBControl.GetCommand("DELETE FROM \"Authorship\" WHERE issue = @id");
                    try
                    {
                        del1.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, thisBook);
                        int result = del1.ExecuteNonQuery();
                        if (result == 1)
                        {
                            NpgsqlCommand del2 = DBControl.GetCommand("DELETE FROM \"Issue\" WHERE identifier = @id");
                            try
                            {
                                del2.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, thisBook);
                                int res = del2.ExecuteNonQuery();
                                if (res == 1)
                                {

                                }
                            }
                            catch (Exception)
                            {
                                return;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                    MessageBox.Show("Один или несколько указанных инвентарных номеров уже зарегистрированы. Введите другое значение");
                    break;
            } 
            success = false;
            thisBookAmount = 0;
            thisBook = string.Empty;
        }
        private void CreateBook(string id, string name, string type, string bbk, string udk, int year, string publisher, int pages, int storage, int amount, string annotation, string image, string authorsign, string keyword)
        {
            NpgsqlCommand precmd = DBControl.GetCommand("SELECT identifier FROM \"Issue\" WHERE identifier = @id");
            precmd.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, id);
            NpgsqlDataReader reader = precmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                NpgsqlCommand command = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, pagecount, storage, amount, annotation, image, authorsign, keyword) " +
                                                             "VALUES (@id, @name, @type, @bbk, @udk, @year, @publisher, @pages, @storage, @amount, @annotation, @image, @authorsign, @keyword)");
                try
                {
                    command.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, id);
                    command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, name);
                    command.Parameters.AddWithValue("@type", NpgsqlDbType.Varchar, type);
                    command.Parameters.AddWithValue("@bbk", NpgsqlDbType.Varchar, bbk);
                    command.Parameters.AddWithValue("@udk", NpgsqlDbType.Varchar, udk);
                    command.Parameters.AddWithValue("@year", NpgsqlDbType.Integer, year);
                    command.Parameters.AddWithValue("@publisher", NpgsqlDbType.Varchar, publisher);
                    command.Parameters.AddWithValue("@pages", NpgsqlDbType.Integer, pages);
                    command.Parameters.AddWithValue("@storage", NpgsqlDbType.Integer, storage);
                    command.Parameters.AddWithValue("@amount", NpgsqlDbType.Integer, amount);
                    command.Parameters.AddWithValue("@annotation", NpgsqlDbType.Varchar, annotation);
                    command.Parameters.AddWithValue("@image", NpgsqlDbType.Varchar, image ?? "/pic/no_image.png");
                    command.Parameters.AddWithValue("@authorsign", NpgsqlDbType.Varchar, authorsign);
                    command.Parameters.AddWithValue("@keyword", NpgsqlDbType.Varchar, keyword);
                    int result = command.ExecuteNonQuery();
                    if (result == 1)
                    {
                        if (lvAuthors.SelectedIndex != -1)
                        {
                            foreach (Author author in lvAuthors.SelectedItems)
                            {
                                command = DBControl.GetCommand("INSERT INTO \"Authorship\" (issue, author) VALUES (@isbn, @author)");
                                try
                                {
                                    command.Parameters.AddWithValue("@isbn", NpgsqlDbType.Varchar, id);
                                    command.Parameters.AddWithValue("@author", NpgsqlDbType.Integer, author.Id);
                                    result = command.ExecuteNonQuery();
                                    if (result < 1)
                                    {

                                    }
                                }
                                catch (Exception ax)
                                {
                                    MessageBox.Show("Ошибка установления авторства\n" + ax);
                                    return;
                                }
                            }
                            //bClear_Click(sender, e);
                        }
                        else
                        {
                            MessageBox.Show("Периодическое издание добавлено");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не удалось добавить запись\n" + ex);
                    return;
                }
            }
            else
            {
                precmd = null;
                ClearFields();
                MessageBox.Show("Данное издание уже есть в списке");
                addCheck = true;
                return;
            }
        }
        // Изменение выбора ComboBox и ListView
        private void lvAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvAuthors.BorderBrush == Brushes.Crimson) lvAuthors.BorderBrush = Brushes.LightSlateGray;

            if (lvAuthors.SelectedIndex != -1)
            {
                foreach (Author a in lvAuthors.SelectedItems)
                {
                    SelectedAuthors.Add(a);
                }
            }
        }
        private void cbBBK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbBBK.SelectedIndex != -1)
            {
                tbBBKTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbBBKTip.Foreground = Brushes.LightSlateGray;
                tbBBKTip.Visibility = Visibility.Visible;
            }
        }
        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbType.SelectedIndex == -1)
            {
                tbTypeTip.Visibility = Visibility.Visible;
            }
            else
            {
                tbTypeTip.Foreground = Brushes.LightSlateGray;
                tbTypeTip.Visibility = Visibility.Collapsed;
                if (((IssueType)cbType.SelectedItem).Type == "Книжное")
                {
                    lvAuthors.IsEnabled = true;
                    tbFilter.IsEnabled = true;
                    SelectedAuthors.Clear();
                }
                if (((IssueType)cbType.SelectedItem).Type != "Книжное")
                {
                    lvAuthors.SelectedIndex = -1;
                    lvAuthors.IsEnabled = false;
                    tbFilter.IsEnabled = false;
                    SelectedAuthors.Clear();
                }
            }
        }
        private void cbPublisher_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPublisher.SelectedIndex != -1)
            {
                tbPubTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbPubTip.Foreground = Brushes.LightSlateGray;
                tbPubTip.Visibility = Visibility.Visible;
            }
        }
        private void cbUDK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbUDK.SelectedIndex != -1)
            {
                tbUDKTip.Visibility = Visibility.Collapsed;
            }
            else
            {
                tbUDKTip.Foreground = Brushes.LightSlateGray;
                tbUDKTip.Visibility = Visibility.Visible;
            }
        }

        // Обработка получения/потери фокуса
        private void tbID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbID.Text == "ISBN / ISSN")
            {
                tbID.Text = string.Empty;
                tbID.Foreground = Design._dark;
            }
        }
        private void tbID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbID.Text == string.Empty)
            {
                tbID.Text = "ISBN / ISSN";
                tbID.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "Наименование")
            {
                tbName.Text = string.Empty;
                tbName.Foreground = Design._dark;
            }
        }
        private void tbName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == string.Empty)
            {
                tbName.Text = "Наименование";
                tbName.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbYear_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbYear.Text == "Год издания")
            {
                tbYear.Text = string.Empty;
                tbYear.Foreground = Design._dark;
            }
        }
        private void tbYear_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbYear.Text == string.Empty)
            {
                tbYear.Text = "Год издания";
                tbYear.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbPageNum_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbPageNum.Text == "Кол-во страниц")
            {
                tbPageNum.Text = string.Empty;
                tbPageNum.Foreground = Design._dark;
            }
        }
        private void tbPageNum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbPageNum.Text == string.Empty)
            {
                tbPageNum.Text = "Кол-во страниц";
                tbPageNum.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbSign_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbSign.Text == "Авторский знак")
            {
                tbSign.Text = string.Empty;
                tbSign.Foreground = Design._dark;
            }
        }
        private void tbSign_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbSign.Text == string.Empty)
            {
                tbSign.Text = "Авторский знак";
                tbSign.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbAmount.Text == "В наличии")
            {
                tbAmount.Text = string.Empty;
                tbAmount.Foreground = Design._dark;
            }
        }
        private void tbAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbAmount.Text == string.Empty)
            {
                tbAmount.Text = "В наличии";
                tbAmount.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbImage_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbImage.Text == "Ссылка на изображение")
            {
                tbImage.Text = string.Empty;
                tbImage.Foreground = Design._dark;
            }
        }
        private void tbImage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbImage.Text == string.Empty)
            {
                tbImage.Text = "Ссылка на изображение";
                tbImage.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbFilter.Text == "Найти автора")
            {
                tbFilter.Text = string.Empty;
                tbFilter.Foreground = Design._dark;
            }
        }
        private void tbFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbFilter.Text == string.Empty)
            {
                tbFilter.Text = "Найти автора";
                tbFilter.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbTag_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbTag.Text == "Ключевые слова")
            {
                tbTag.Text = string.Empty;
                tbTag.Foreground = Design._dark;
            }
        }
        private void tbTag_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbTag.Text == string.Empty)
            {
                tbTag.Text = "Ключевые слова";
                tbTag.Foreground = Brushes.LightSlateGray;
            }
        }

        // Проверка заполнения полей
        private int AmountFillingCheck(string amount)
        {
            if (amount == "В наличии")
            {
                tbAmount.Foreground = Brushes.Crimson;
                return 0;
            }
            try { return int.Parse(amount); }
            catch { return 0; }
        }
        private bool AuthorFillingCheck(Author author)
        {
            if (author == null)
            {
                if (cbType.SelectedIndex != -1)
                {
                    if (((IssueType)cbType.SelectedItem).Type != "Книжное") return true;
                    if (((IssueType)cbType.SelectedItem).Type == "Книжное")
                    {
                        lvAuthors.BorderBrush = Brushes.Crimson;
                        return false;
                    }
                }
            }
            return true;
        }
        private bool ASignFillingCheck(string sign)
        {
            if (cbType.SelectedIndex != -1)
            {
                if (((IssueType)cbType.SelectedItem).Type != "Книжное") return true;
                else
                {
                    if (sign == "Авторский знак")
                    {
                        tbSign.Foreground = Brushes.Crimson;
                        return false;
                    }
                    else return true;
                }
            }
            else
            {
                if (sign == "Авторский знак")
                {
                    tbSign.Foreground = Brushes.Crimson;
                    return false;
                }
                else return true;
            }
        }
        private bool BBKFillingCheck(LibraryClassification bbk)
        {
            if (bbk == null)
            {
                tbBBKTip.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private bool IDFillingCheck(string id)
        {
            if (id == "ISBN / ISSN")
            {
                tbID.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private bool NameFillingCheck(string name)
        {
            if (name == "Наименование")
            {
                tbName.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private int PageNumFillingCheck(string num)
        {
            if (num == "Кол-во страниц")
            {
                tbPageNum.Foreground = Brushes.Crimson;
                return 0;
            }
            try { return int.Parse(num); }
            catch { return 0; }
        }
        private bool PublisherFillingCheck(Publisher publisher)
        {
            if (publisher == null)
            {
                tbPubTip.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private bool TypeFillingCheck(IssueType type)
        {
            if (type == null)
            {
                tbTypeTip.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private bool UDKFillingCheck(DecimalClassification udk)
        {
            if (udk == null)
            {
                tbUDKTip.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
        private int YearFillingCheck(string year)
        {
            if (year == "Год издания")
            {
                tbYear.Foreground = Brushes.Crimson;
                return 0;
            }
            try { return int.Parse(year); }
            catch { return 0; }
        }
        private bool TagFillingCheck(string tag)
        {
            if (tag == "Ключевые слова")
            {
                tbTag.Foreground = Brushes.Crimson;
                return false;
            }
            else return true;
        }
    }
}
