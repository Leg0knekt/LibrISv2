﻿using System;
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
    public partial class PageAdminMenu : Page
    {
        public PageAdminMenu()
        {
            InitializeComponent();
        }

        private void bAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            AdminMenuFrame.Navigate(PageControl.PageNewEmployee);
        }
        private void bConnectionProperties_Click(object sender, RoutedEventArgs e)
        {
            AdminMenuFrame.Navigate(PageControl.PageConnectionProperties);
        }
    }
}
