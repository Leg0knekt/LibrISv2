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
            }
        }
    }
}
