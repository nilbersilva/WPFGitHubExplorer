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
using WPFGitHubExplorer.ViewModels;

namespace WPFGitHubExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GitHubRepositoriesVM MyVM = new GitHubRepositoriesVM();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = MyVM;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await MyVM.LoadData();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.Enter)
            {
                e.Handled = true;
                MyVM.CommandSearch.Execute(null);
            }
        }
    }
}
