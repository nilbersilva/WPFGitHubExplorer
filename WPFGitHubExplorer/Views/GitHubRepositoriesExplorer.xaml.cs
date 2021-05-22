using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for GitHubUserExplorer.xaml
    /// </summary>
    public partial class GitHubRepositoriesExplorer : UserControl
    {
        private bool FirstLoad = true;
        public GitHubRepositoriesVM MyVM = new GitHubRepositoriesVM();
        public GitHubRepositoriesExplorer()
        {
            InitializeComponent();
            this.DataContext = MyVM;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                MyVM.CommandSearch.Execute(null);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (FirstLoad)
            {
                FirstLoad = false;
                MyVM.CommandSearch.Execute(null);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessStartInfo psInfo = new ProcessStartInfo(e.Uri.ToString())
            {
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psInfo);
        }
    }
}
