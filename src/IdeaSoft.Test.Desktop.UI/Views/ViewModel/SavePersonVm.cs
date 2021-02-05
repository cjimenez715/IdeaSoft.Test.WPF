using IdeaSoft.Test.Desktop.UI.Models;
using IdeaSoft.Test.Desktop.UI.Services;
using IdeaSoft.Test.Desktop.UI.ViewModel;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IdeaSoft.Test.Desktop.UI.Views.ViewModel
{
    public class SavePersonVm: ViewModelBase
    {
        #region Properties
        private SavePersonDto selectedItem;
        private bool wasSaved;

        public SavePersonDto SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public bool WasSaved 
        { 
            get => wasSaved;
            set 
            {
                wasSaved = value;
                if (value)
                {
                    ResetSelectedItem();
                    if (OnSaveCompleted != null)
                    {
                        OnSaveCompleted(this, EventArgs.Empty);
                    }
                }
            }
        }
        #endregion

        #region Services
        private readonly IPersonService _personService;
        #endregion

        #region Constructor
        public SavePersonVm(IPersonService personService)
        {
            _personService = personService;
        }
        #endregion

        #region Events
        public event EventHandler OnSaveCompleted;
        public event EventHandler OnCancel;
        #endregion

        #region Methods

        private async Task SavePersonAsync()
        {
            if(SelectedItem != null)
            {
                var result = await _personService.SavePersonAsync(SelectedItem);
                WasSaved = string.IsNullOrEmpty(result);
            }
        }

        public void ResetSelectedItem()
        {
            SelectedItem = new SavePersonDto();
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get { return new DelegateCommand<object>(ExecuteSave, CanExecuteSave); }
        }

        private void ExecuteSave(object context)
        {
            _ = SavePersonAsync();
        }

        private bool CanExecuteSave(object context)
        {
            return true;
        }

        public ICommand CancelCommand
        {
            get { return new DelegateCommand<object>(ExecuteCancel, CanExecuteCancel); }
        }


        private void ExecuteCancel(object context = null)
        {
            if(OnCancel != null)
            {
                ResetSelectedItem();
                OnCancel(this, EventArgs.Empty);
            }
        }

        private bool CanExecuteCancel(object context)
        {
            return true;
        }
        #endregion
    }
}
