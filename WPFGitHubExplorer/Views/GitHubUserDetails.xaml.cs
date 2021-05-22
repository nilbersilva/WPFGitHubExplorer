using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
using WPFGitHubExplorer.ViewModels;

namespace WPFGitHubExplorer.Views
{
    /// <summary>
    /// Interaction logic for GitHubUserDetails.xaml
    /// </summary>
    public partial class GitHubUserDetails : CustomDialog
    {
        public GitHubUserVM MyVM = new GitHubUserVM();
        public GitHubUserDetails()
        {
            InitializeComponent();
            this.DataContext = MyVM;
        }

        private async void CustomDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                e.Handled = true;
                await ((MetroWindow)Application.Current.MainWindow).HideMetroDialogAsync(this);
            }
        }

        private async void CustomDialog_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MainContainer.IsMouseOver)
            {
                return;
            }
            else
            {
                e.Handled = true;
                await((MetroWindow)Application.Current.MainWindow).HideMetroDialogAsync(this);
            }
        }

        private async void CloseDialog(object sender, RoutedEventArgs e)
        {
            await((MetroWindow)Application.Current.MainWindow).HideMetroDialogAsync(this);
        }

        private void CustomDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(async () =>
            {
                await MyVM.LoadData();
            }), System.Windows.Threading.DispatcherPriority.ApplicationIdle);          
        }
    }
}
