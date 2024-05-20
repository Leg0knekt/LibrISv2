using Npgsql;
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

namespace LibrISv2
{
    public partial class PageAddIssue : Page
    {
        public ObservableCollection<Author> SelectedAuthors = new ObservableCollection<Author>();
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
            if (troubles > 0) { MessageBox.Show(warning + "Введите все необходимые данные и повторите попытку"); }

            if (troubles == 0)
            {
                NpgsqlCommand precmd = DBControl.GetCommand("SELECT identifier FROM \"Issue\" WHERE identifier = @id");
                precmd.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, tbID.Text.Trim());
                NpgsqlDataReader reader = precmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    NpgsqlCommand command = DBControl.GetCommand("INSERT INTO \"Issue\" (identifier, name, type, \"BBK\", \"UDK\", year, publisher, " +
                                                                               "pagecount, storage, amount, annotation, image, authorsign, keyword) " +
                                                                "VALUES (@id, @name, @type, @bbk, @udk, @year, @publisher, " +
                                                                        "@pages, @storage, @amount, @annotation, @image, @authorsign, @keyword)");
                    try
                    {
                        command.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, tbID.Text.Trim());
                        command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, tbName.Text.Trim());
                        command.Parameters.AddWithValue("@type", NpgsqlDbType.Varchar, ((IssueType)cbType.SelectedItem).Type);
                        command.Parameters.AddWithValue("@bbk", NpgsqlDbType.Varchar, ((LibraryClassification)cbBBK.SelectedItem).Index);
                        command.Parameters.AddWithValue("@udk", NpgsqlDbType.Varchar, ((DecimalClassification)cbUDK.SelectedItem).Index);
                        command.Parameters.AddWithValue("@year", NpgsqlDbType.Integer, yearChecked);
                        command.Parameters.AddWithValue("@publisher", NpgsqlDbType.Varchar, ((Publisher)cbPublisher.SelectedItem).OGRN);
                        command.Parameters.AddWithValue("@pages", NpgsqlDbType.Integer, pageChecked);
                        command.Parameters.AddWithValue("@storage", NpgsqlDbType.Integer, MainWindow.currentUserWorkplace);
                        command.Parameters.AddWithValue("@amount", NpgsqlDbType.Integer, amountChecked);
                        command.Parameters.AddWithValue("@annotation", NpgsqlDbType.Varchar, " ");
                        command.Parameters.AddWithValue("@image", NpgsqlDbType.Varchar, tbImage.Text.Trim() ?? "/pic/no_image.png");
                        command.Parameters.AddWithValue("@keyword", NpgsqlDbType.Varchar, " ");
                        command.Parameters.AddWithValue("@authorsign", NpgsqlDbType.Varchar, tbSign.Text.Trim());
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
                                        command.Parameters.AddWithValue("@isbn", NpgsqlDbType.Varchar, tbID.Text.Trim());
                                        command.Parameters.AddWithValue("@author", NpgsqlDbType.Integer, author.Id);
                                        result = command.ExecuteNonQuery();
                                        if (result == 1)
                                        {
                                            MessageBox.Show("Книга успешно добавлена");
                                        }
                                    }
                                    catch (Exception ax)
                                    {
                                        MessageBox.Show("Ошибка установления авторства\n" + ax);
                                    }
                                }
                                bClear_Click(sender, e);
                            }
                            else
                            {
                                MessageBox.Show("Периодическое издание добавлено");
                                bClear_Click(sender, e);
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
                else MessageBox.Show("Данное издание уже есть в списке");
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
            tbAmount.Text = "В наличии";
            tbAmount.Foreground = Brushes.LightSlateGray;

            tbFilter.Text = "Найти автора";
            tbFilter.Foreground = Brushes.LightSlateGray;

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
                }
                if (((IssueType)cbType.SelectedItem).Type != "Книжное")
                {
                    lvAuthors.SelectedIndex = -1;
                    lvAuthors.IsEnabled = false;
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
    }
}
