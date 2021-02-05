using IdeaSoft.Test.Desktop.UI.Models;
using IdeaSoft.Test.Desktop.UI.Services;
using IdeaSoft.Test.Desktop.UI.ViewModel;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace IdeaSoft.Test.Desktop.UI.Views.ViewModel
{
    public class UpdatePersonVm : ViewModelBase
    {
        #region Properties
        public string PersonId { get; private set; }
        private UpdatePersonDto selectedItem;
        private bool wasSaved;

        public UpdatePersonDto SelectedItem
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
                    if (OnUpdateCompleted != null)
                    {
                        OnUpdateCompleted(this, EventArgs.Empty);
                    }
                }
            }
        }
        #endregion

        #region Services
        private readonly IPersonService _personService;
        #endregion

        #region Constructor
        public UpdatePersonVm(IPersonService personService)
        {
            _personService = personService;
            ValidationErrors = new ObservableCollection<ValidationError>();
        }
        #endregion

        #region Events
        public event EventHandler OnUpdateCompleted;
        public event EventHandler OnCancel;
        #endregion

        #region Methods

        private async Task UpdatePersonAsync()
        {
            if (SelectedItem != null && PersonId != null)
            {
                var result = await _personService.UpdatePersonAsync(PersonId, SelectedItem);
                WasSaved = string.IsNullOrEmpty(result);
            }
        }

        public async Task GetPersonByIdAsync()
        {
            if (!string.IsNullOrEmpty(PersonId))
            {
                SelectedItem = await _personService.GetPersonByIdAsync(PersonId);
            }
        }

        public void SetPersonId(string personId)
        {
            PersonId = personId;
            _ = GetPersonByIdAsync();
        }

        public void ResetSelectedItem()
        {
            SelectedItem = new UpdatePersonDto();
            PersonId = string.Empty;
            ValidationErrors.Clear();
        }
        #endregion

        #region Commands
        public ICommand UpdateCommand
        {
            get { return new DelegateCommand<object>(ExecuteUpdate, CanExecuteUpdate); }
        }

        private void ExecuteUpdate(object context)
        {
            _ = UpdatePersonAsync();
        }

        private bool CanExecuteUpdate(object context)
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

        #region Validation
        public ObservableCollection<ValidationError> ValidationErrors { get; private set; }

        public void SetValidationRules(ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                ValidationErrors.Add(e.Error);
            }
            else
            {
                ValidationErrors.Remove(e.Error);
            }
        }

        #endregion
    }
}
