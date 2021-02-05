using IdeaSoft.Test.Desktop.UI.Models;
using IdeaSoft.Test.Desktop.UI.Services;
using IdeaSoft.Test.Desktop.UI.Views;
using Microsoft.VisualStudio.PlatformUI;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IdeaSoft.Test.Desktop.UI.ViewModel
{
    public class SearchPersonVm: ViewModelBase
    {
        #region Properties
        private ObservableCollection<SearchPersonDto> itemsSource;
        private SearchPersonDto selectedItem;
        private string IdSavedUpdated;
        private string filter;
        private bool wasDeleted;

        public ObservableCollection<SearchPersonDto> ItemsSource
        {
            get => itemsSource;
            set
            {
                itemsSource = value;
                OnPropertyChanged("ItemsSource");
            }
        }

        public SearchPersonDto SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public string Filter
        {
            get => filter;
            set
            {
                filter = value;
                OnPropertyChanged("Filter");
            }
        }

        public bool WasDeleted
        {
            get => wasDeleted;
            set
            {
                wasDeleted = value;
                if (value)
                {
                    _ = LoadPersonListAsync();
                }
            }
        }
        #endregion

        #region Services
        private readonly IPersonService _personService;
        #endregion

        #region Views
        private readonly SavePersonVw _saveUI;
        private readonly UpdatePersonVw _updateUI;
        #endregion

        #region Constructor
        public SearchPersonVm(IPersonService personService, SavePersonVw saveUI, UpdatePersonVw updateUI)
        {
            _personService = personService;
            Filter = string.Empty;
            _ = LoadPersonListAsync();
            _saveUI = saveUI;
            _saveUI.OnSaveCompletedUI += OnLoadPersonListEvent;
            _updateUI = updateUI;
            _updateUI.OnUpdateCompletedUI += OnLoadPersonListEvent;
            IdSavedUpdated = string.Empty;
        }
        #endregion

        #region Methods
        private void OnLoadPersonListEvent(object sender, System.EventArgs e)
        {
            Filter = string.Empty;
            _ = LoadPersonListAsync();
            
        }

        public async Task LoadPersonListAsync()
        {
            ItemsSource = await _personService.GetPersonByFilterAsync(filter);
        }

        private async Task RemovePersonAsync()
        {
            if (SelectedItem != null)
            {
                WasDeleted = await _personService.RemovePersonAsync(SelectedItem.Id);
            }
        }
        #endregion

        #region Commands
        public ICommand FilterCommand
        {
            get { return new DelegateCommand<object>(ExecuteFilter, CanExecuteFilter); }
        }

        private void ExecuteFilter(object context)
        {
            _ = LoadPersonListAsync();
        }

        private bool CanExecuteFilter(object context)
        {
            return true;
        }

        public ICommand RemoveCommand
        {
            get { return new DelegateCommand<object>(ExecuteRemove, CanExecuteRemove); }
        }

        private void ExecuteRemove(object context)
        {
            SelectedItem = (SearchPersonDto)context;
            MessageBoxResult result = MessageBox.Show($"Você tem certeza que quer deletar o cadastro de { SelectedItem }?", "Atenção", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _ = RemovePersonAsync();
            }
        }

        private bool CanExecuteRemove(object context)
        {
            return true;
        }

        public ICommand EditCommand
        {
            get { return new DelegateCommand<object>(ExecuteEdit, CanExecuteEdit); }
        }

        private void ExecuteEdit(object context)
        {
            SelectedItem = (SearchPersonDto)context;
            _updateUI.SetPersonId(SelectedItem.Id);
        }

        private bool CanExecuteEdit(object context)
        {
            return true;
        }

        public ICommand AddCommand
        {
            get { return new DelegateCommand<object>(ExecuteAdd, CanExecuteAdd); }
        }

        private void ExecuteAdd(object context)
        {
            _saveUI.OpenView();
        }

        private bool CanExecuteAdd(object context)
        {
            return true;
        }
        #endregion

    }
}
