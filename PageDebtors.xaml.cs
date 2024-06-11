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
    public partial class PageDebtors : Page
    {
        public PageDebtors()
        {
            InitializeComponent();
            DataLoad.LoadOperations();
            lvDebt.SetBinding(ItemsControl.ItemsSourceProperty, new Binding() { Source = DBControl.Operations });
        }

        private void lvDebt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDebt.SelectedIndex != -1)
            {
                tb1.Text = (lvDebt.SelectedItem as Operation).Client;
                tb2.Text = (lvDebt.SelectedItem as Operation).ExtraClient;
                tb3.Text = (lvDebt.SelectedItem as Operation).ExtraPhone;
                tb4.Text = (lvDebt.SelectedItem as Operation).Issue;
                tb5.Text = (lvDebt.SelectedItem as Operation).Issuance.ToShortDateString();
                tb6.Text = (lvDebt.SelectedItem as Operation).ReturningDate.ToShortDateString();
                int penalty = (DateTime.Today - (lvDebt.SelectedItem as Operation).ReturningDate).Days;
                if (penalty < 0) { tb7.Text = "0 руб."; }
                if (penalty > 0 && penalty < 10) { tb7.Text = (penalty * 4).ToString() + " руб."; }
                if (penalty > 10 && penalty < 20) { tb7.Text = (penalty * 7).ToString() + " руб."; }
                if (penalty > 20 && penalty < 30) { tb7.Text = (penalty * 11).ToString() + " руб."; }
                if (penalty > 30 && penalty < 60) { tb7.Text = (penalty * 14).ToString() + " руб."; }
                if (penalty > 60 && penalty < 90) { tb7.Text = (penalty * 18).ToString() + " руб."; }
                if (penalty > 90 && penalty < 120) { tb7.Text = (penalty * 22).ToString() + " руб."; }
                if (penalty > 120 && penalty < 150) { tb7.Text = (penalty * 25).ToString() + " руб."; }
                if (penalty > 150 && penalty < 180) { tb7.Text = (penalty * 29).ToString() + " руб."; }
                if (penalty > 180 && penalty < 210) { tb7.Text = (penalty * 32).ToString() + " руб."; }
                if (penalty > 210 && penalty < 240) { tb7.Text = (penalty * 36).ToString() + " руб."; }
                if (penalty > 240 && penalty < 270) { tb7.Text = (penalty * 40).ToString() + " руб."; }
                if (penalty > 270 && penalty < 300) { tb7.Text = (penalty * 43).ToString() + " руб."; }
                if (penalty > 300 && penalty < 330) { tb7.Text = (penalty * 47).ToString() + " руб."; }
                if (penalty > 330) { tb7.Text = (penalty * 50).ToString() + " руб."; }
            }
        }

        private void DebtFilter(object operation, string status)
        {
            if (operation == null) return;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(operation);
            if (view != null)
            {
                view.Filter = issuance =>
                {
                    if (status == "Выдано" || string.IsNullOrEmpty(status)) return true;
                    if (issuance is Operation _issuance)
                    {
                        return // Как должна выглядеть строка, чтобы всё нормально работало
                               //_issuance.ExtraStatus.Contains(status, StringComparison.CurrentCultureIgnoreCase)
                               _issuance.ExtraStatus.ToLower().Contains(status) ||
                               _issuance.ExtraStatus.ToUpper().Contains(status);
                    }
                    else return false;
                };
            }
            else return;
        }

        private void bAll_Click(object sender, RoutedEventArgs e)
        {
            lvDebt.SelectedIndex = -1;
            DebtFilter(lvDebt.ItemsSource, "Выдано");
        }

        private void bWithPenalty_Click(object sender, RoutedEventArgs e)
        {
            lvDebt.SelectedIndex = -1;
            DebtFilter(lvDebt.ItemsSource, "ПРОСРОЧЕНО!");
        }
    }
}
