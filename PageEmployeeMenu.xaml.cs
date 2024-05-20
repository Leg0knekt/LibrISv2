using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    public partial class PageEmployeeMenu : Page
    {
        public PageEmployeeMenu()
        {
            InitializeComponent();
        }

        private void bAddIssue_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(PageControl.PageAddIssue);
        }

        private void bSortIssue_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(PageControl.PageCatalog);
        }

        private void bIssuance_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(PageControl.PageIssuance);
        }

        private void bReserved_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(PageControl.PageReservationView);
        }

        private void bNewClient_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(PageControl.PageNewClient);
        }

        private void bDebtors_Click(object sender, RoutedEventArgs e)
        {
            MenuFrame.Navigate(PageControl.PageDebtors);
        }

        private void bBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
