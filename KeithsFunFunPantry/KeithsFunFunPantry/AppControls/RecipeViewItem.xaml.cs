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

namespace KeithsFunFunPantry.AppControls
{
    /// <summary>
    /// Interaction logic for RecipeViewItem.xaml
    /// </summary>
    public partial class RecipeViewItem : UserControl
    {
        public RecipeViewItem(Recipe r)
        {
            InitializeComponent();
            DataContext = r;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("boi");
        }
    }
}
