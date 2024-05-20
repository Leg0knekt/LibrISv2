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
    public partial class PageIssuance : Page
    {
        public PageIssuance()
        {
            InitializeComponent();
            DataLoad.LoadClients();
            DataLoad.LoadIssues();
            lvClients.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Clients });
            lvIssues.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Issues });
        }



        // Фильтр
        private void tbClientFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lvClients != null) ClientFilter(lvClients.ItemsSource, tbClientFilter.Text.Trim());
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
        private void tbIssueFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lvIssues != null) IssueFilter(lvIssues.ItemsSource, tbIssueFilter.Text.Trim());
        }
        private void IssueFilter(object issue, string searchString)
        {
            if (issue == null) return;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(issue);
            if (view != null)
            {
                view.Filter = iss =>
                {
                    if (searchString == "Найти" || string.IsNullOrEmpty(searchString)) return true;
                    if (iss is Issue _issue)
                    {
                        return /*_issue.Identifier.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||*/
                               _issue.Identifier.ToLower().Contains(searchString) ||
                               _issue.Identifier.ToUpper().Contains(searchString) ||
                               _issue.Name.ToLower().Contains(searchString) ||
                               _issue.Name.ToUpper().Contains(searchString);
                    }
                    else return false;
                };
            }
            else return;
        }

        // Кнопки
        private void bNextStep_Click(object sender, RoutedEventArgs e)
        {
            if (lvIssues.SelectedIndex != -1 && lvClients.SelectedIndex != -1)
            {
                WindowOperationSelector windowOperationSelector = new WindowOperationSelector();
                windowOperationSelector.Show();
            }
        }
        private void bCancelIssueFilter_Click(object sender, RoutedEventArgs e)
        {
            tbIssueFilter.Text = "Найти";
            tbIssueFilter.Foreground = Brushes.LightSlateGray;
        }
        private void bCancelClientFilter_Click(object sender, RoutedEventArgs e)
        {
            tbClientFilter.Text = "Найти";
            tbClientFilter.Foreground = Brushes.LightSlateGray;
        }

        // Изменение выбора
        private void lvIssues_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void lvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Обработка фокусов
        private void tbIssueFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbIssueFilter.Text == "Найти")
            {
                tbIssueFilter.Text = string.Empty;
                tbIssueFilter.Foreground = Design._dark;
            }
        }
        private void tbIssueFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbIssueFilter.Text == string.Empty)
            {
                tbIssueFilter.Text = "Найти";
                tbIssueFilter.Foreground = Brushes.LightSlateGray;
            }
        }
        private void tbClientFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbClientFilter.Text == "Найти")
            {
                tbClientFilter.Text = string.Empty;
                tbClientFilter.Foreground = Design._dark;
            }
        }
        private void tbClientFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbClientFilter.Text == string.Empty)
            {
                tbClientFilter.Text = "Найти";
                tbClientFilter.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
