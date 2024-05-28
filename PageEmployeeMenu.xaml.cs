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
            PageControl.pAddIssue = null;
            MenuFrame.Navigate(PageControl.PageAddIssue);
            bAddIssue.Foreground = Design._azure;
            bSortIssue.Foreground = bIssuance.Foreground = bReserved.Foreground = bNewClient.Foreground = bDebtors.Foreground = Design._dark;
            bAddIssue.Background = Design._myrtle;
            bSortIssue.Background = bIssuance.Background = bReserved.Background = bNewClient.Background = bDebtors.Background = null;
        }

        private void bSortIssue_Click(object sender, RoutedEventArgs e)
        {
            PageControl.pCatalog = null;
            MenuFrame.Navigate(PageControl.PageCatalog);
            bSortIssue.Foreground = Design._azure;
            bAddIssue.Foreground = bIssuance.Foreground = bReserved.Foreground = bNewClient.Foreground = bDebtors.Foreground = Design._dark;
            bSortIssue.Background = Design._myrtle;
            bAddIssue.Background = bIssuance.Background = bReserved.Background = bNewClient.Background = bDebtors.Background = null;
        }

        private void bIssuance_Click(object sender, RoutedEventArgs e)
        {
            PageControl.pIssuance = null;
            MenuFrame.Navigate(PageControl.PageIssuance);
            bIssuance.Foreground = Design._azure;
            bAddIssue.Foreground = bSortIssue.Foreground = bReserved.Foreground = bNewClient.Foreground = bDebtors.Foreground = Design._dark;
            bIssuance.Background = Design._myrtle;
            bAddIssue.Background = bSortIssue.Background = bReserved.Background = bNewClient.Background = bDebtors.Background = null;
        }

        private void bReserved_Click(object sender, RoutedEventArgs e)
        {
            PageControl.pReservationView = null;
            MenuFrame.Navigate(PageControl.PageReservationView);
            bReserved.Foreground = Design._azure;
            bAddIssue.Foreground = bSortIssue.Foreground = bIssuance.Foreground = bNewClient.Foreground = bDebtors.Foreground = Design._dark;
            bReserved.Background = Design._myrtle;
            bAddIssue.Background = bIssuance.Background = bSortIssue.Background = bNewClient.Background = bDebtors.Background = null;
        }

        private void bNewClient_Click(object sender, RoutedEventArgs e)
        {
            PageControl.pNewClient = null;
            MenuFrame.Navigate(PageControl.PageNewClient);
            bNewClient.Foreground = Design._azure;
            bAddIssue.Foreground = bSortIssue.Foreground = bReserved.Foreground = bIssuance.Foreground = bDebtors.Foreground = Design._dark;
            bNewClient.Background = Design._myrtle;
            bAddIssue.Background = bIssuance.Background = bReserved.Background = bSortIssue.Background = bDebtors.Background = null;
        }

        private void bDebtors_Click(object sender, RoutedEventArgs e)
        {
            PageControl.pDebtors = null;
            MenuFrame.Navigate(PageControl.PageDebtors);
            bDebtors.Foreground = Design._azure;
            bAddIssue.Foreground = bSortIssue.Foreground = bReserved.Foreground = bNewClient.Foreground = bIssuance.Foreground = Design._dark;
            bDebtors.Background = Design._myrtle;
            bAddIssue.Background = bIssuance.Background = bReserved.Background = bNewClient.Background = bSortIssue.Background = null;
        }
    }
}
