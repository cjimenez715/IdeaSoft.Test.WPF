using IdeaSoft.Test.Desktop.UI.ViewModel;
using System.Windows;

namespace IdeaSoft.Test.Desktop.UI.Views
{
    public partial class SearchPersonVw : Window
    {
        public SearchPersonVw(SearchPersonVm viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Closing += SearchPersonVw_Closing;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtFilter.Focus();
        }

        private void SearchPersonVw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
