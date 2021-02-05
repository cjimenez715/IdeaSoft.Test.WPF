using IdeaSoft.Test.Desktop.UI.Views.ViewModel;
using System;
using System.Windows;

namespace IdeaSoft.Test.Desktop.UI.Views
{
    public partial class SavePersonVw : Window
    {
        public event EventHandler OnSaveCompletedUI;
        public SavePersonVw(SavePersonVm viewModel)
        {
            viewModel.OnCancel += ViewModel_OnCancel;
            viewModel.OnSaveCompleted += ViewModel_OnSaveCompleted;
            InitializeComponent();
            DataContext = viewModel;
            Closing += SavePersonVw_Closing;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            txtName.Focus();
        }

        private void ViewModel_OnSaveCompleted(object sender, System.EventArgs e)
        {
            if(OnSaveCompletedUI != null)
            {
                OnSaveCompletedUI(this, EventArgs.Empty);
            }
            Hide();
        }

        private void ViewModel_OnCancel(object sender, System.EventArgs e)
        {
            OnResetClose();
        }

        private void SavePersonVw_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            OnResetClose();
        }

        private void OnResetClose()
        {
            ((SavePersonVm)DataContext).ResetSelectedItem();
            txtName.Focus();
            Hide();
        }

        public void OpenView()
        {
            ShowDialog();
        }
    }
}
