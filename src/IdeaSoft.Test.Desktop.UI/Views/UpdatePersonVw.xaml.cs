using IdeaSoft.Test.Desktop.UI.Views.ViewModel;
using System;
using System.Windows;

namespace IdeaSoft.Test.Desktop.UI.Views
{
    public partial class UpdatePersonVw : Window
    {
        public event EventHandler OnUpdateCompletedUI;
        public UpdatePersonVw(UpdatePersonVm viewModel)
        {
            viewModel.OnCancel += ViewModel_OnCancel;
            viewModel.OnUpdateCompleted += ViewModel_OnUpdateCompleted;
            InitializeComponent();
            DataContext = viewModel;
            Closing += UpdatePersonVw_Closing;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void ViewModel_OnUpdateCompleted(object sender, System.EventArgs e)
        {
            if(OnUpdateCompletedUI != null)
            {
                OnUpdateCompletedUI(this, EventArgs.Empty);
            }
            Hide();
        }

        private void ViewModel_OnCancel(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void UpdatePersonVw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            ((UpdatePersonVm)DataContext).ResetSelectedItem();
            Hide();
        }

        public void SetPersonId(string personId)
        {
            ((UpdatePersonVm)DataContext).SetPersonId(personId);
            ShowDialog();
        }
    }
}
