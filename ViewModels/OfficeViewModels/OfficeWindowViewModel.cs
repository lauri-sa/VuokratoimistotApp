using System;
using System.Linq;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using System.Threading.Tasks;
using Ohtu1Project.Repositories;
using Ohtu1Project.Views.OfficeViews;
using System.Collections.ObjectModel;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.OfficeViewModels
{
    /// <summary>
    /// Datacontext class for OfficeWindow. Inherits MainViewModel class.
    /// </summary>
    internal class OfficeWindowViewModel : MainViewModel
    {
        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                EnableButtons = _selectedIndex < 0 ? false : true;
                OnPropertyChanged();
            }
        }

        private int _tabIndex;
        public int TabIndex
        {
            get
            {
                return _tabIndex;
            }
            set
            {
                _tabIndex = value;
                EnableAddButton = (OfficesCollection.Count < 1 && TabIndex == 1) ? false : true;
                LostFocus();
                OnPropertyChanged();
            }
        }

        private int _id;
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                _=GetOfficeSpaces();
                OnPropertyChanged();
            }
        }

        private int _comboBoxIndex;
        public int ComboBoxIndex { get { return _comboBoxIndex; } set { _comboBoxIndex  = value; OnPropertyChanged(); } }

        private bool _enableButtons;
        public bool EnableButtons { get { return _enableButtons; } set { _enableButtons = value; OnPropertyChanged(); } }

        private bool _enableAddButton = true;
        public bool EnableAddButton { get { return _enableAddButton; } set { _enableAddButton = value; OnPropertyChanged(); } }

        private bool _enableOfficesComboBox;
        public bool EnableOfficesComboBox { get { return _enableOfficesComboBox; } set { _enableOfficesComboBox = value; OnPropertyChanged(); } }

        private OfficeModel _officeModel;
        public OfficeModel OfficeModel { get { return _officeModel; } set { _officeModel  = value; OnPropertyChanged(); } }

        private OfficeSpaceModel _officeSpaceModel;
        public OfficeSpaceModel OfficeSpaceModel { get { return _officeSpaceModel; } set { _officeSpaceModel = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeModel> _officesCollection;
        public ObservableCollection<OfficeModel> OfficesCollection { get { return _officesCollection; } set { _officesCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeSpaceModel> _officeSpacesCollection;
        public ObservableCollection<OfficeSpaceModel> OfficeSpacesCollection { get { return _officeSpacesCollection; } set { _officeSpacesCollection = value; OnPropertyChanged(); } }

        /// <summary>
        /// Command that is responsible for ContentRendered event.
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand(Loaded);

        /// <summary>
        /// Command that is responsible for lose focus from datagrid.
        /// </summary>
        public ICommand LostFocusCommand => new DelegateCommand(LostFocus);

        /// <summary>
        /// A command for the add button.
        /// </summary>
        public ICommand AddButtonCommand => new DelegateCommand(AddButton);

        /// <summary>
        /// A command for the modify button.
        /// </summary>
        public ICommand ModifyButtonCommand => new DelegateCommand(ModifyButton);

        /// <summary>
        /// A command for the delete button.
        /// </summary>
        public ICommand DeleteButtonCommand => new DelegateCommand(DeleteButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        /// <summary>
        /// Calls GetCustomers() method asynchronously.
        /// </summary>
        private async void Loaded()
        {
            await GetOffices();
        }

        /// <summary>
        /// Asynchronously fetches all offices from the database via OfficeRepository
        /// and assigns the resulting collection to the OfficesCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetOffices()
        {
            try
            {
                OfficesCollection = await OfficeRepository.FetchAllOfficesFullDetails();
                EnableOfficesComboBox = OfficesCollection.Count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetOffices();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Asynchronously fetches all office spaces for specific office from the database via OfficeSpaceRepository
        /// and assigns the resulting collection to the OfficeSpacesCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetOfficeSpaces()
        {
            try
            {
                OfficeSpacesCollection = await OfficeSpaceRepository.FetchAllOfficeSpacesFullDetail(ID);
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetOfficeSpaces();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Sets SelectedIndex property value to -1.
        /// </summary>
        private void LostFocus()
        {
            SelectedIndex = -1;
        }

        /// <summary>
        /// Event handler for the add button.
        /// If the tab index is 0, opens a new AddOfficeWindow and calls the GetOffices method.
        /// If the tab index is 1, opens a new AddOfficeSpaceWindow and calls the GetOfficeSpaces method.
        /// </summary>
        private async void AddButton()
        {
            if (TabIndex == 0)
            {
                WindowManager.OpenWindow(new AddOfficeWindow());
                await GetOffices();
                ComboBoxIndex = 0;
            }
            else if (TabIndex == 1)
            {
                AddOfficeSpaceWindowViewModel.OfficeSpaceModel = new OfficeSpaceModel
                {
                    OfficeID = ID,
                    OfficeName = OfficesCollection.FirstOrDefault(office => office.ID == ID).OfficeName
                };
                WindowManager.OpenWindow(new AddOfficeSpaceWindow());
                await GetOfficeSpaces();
            }
        }

        /// <summary>
        /// Handles the modify button click event for either offices or office spaces, and opens a corresponding window
        /// for updating the selected entity. Also updates the list of offices/offices spaces and resets the combo box
        /// index accordingly.
        /// </summary>
        private async void ModifyButton()
        {
            if (TabIndex == 0)
            {
                UpdateOfficeWindowViewModel.OfficeModel = OfficeModel;
                WindowManager.OpenWindow(new UpdateOfficeWindow());
                await GetOffices();
                ComboBoxIndex = 0;
            }
            else if (TabIndex == 1)
            {
                this.OfficeSpaceModel.OfficeName = OfficesCollection.FirstOrDefault(office => office.ID == ID).OfficeName;
                UpdateOfficeSpaceWindowViewModel.OfficeSpaceModel = OfficeSpaceModel;
                WindowManager.OpenWindow(new UpdateOfficeSpaceWindow());
                await GetOfficeSpaces();
            }
        }

        /// <summary>
        /// Handles the delete button click event for either offices or office spaces, and opens a confirmation window before
        /// deleting. Also updates the list of offices/offices spaces and resets the combo box index accordingly.
        /// </summary>
        private async void DeleteButton()
        {
            if (TabIndex == 0)
            {
                DeleteConfirmationWindowViewModel.DeleteAction = () => OfficeRepository.DeleteOffice(OfficeModel.ID);
                WindowManager.OpenWindow(new DeleteConfirmationWindow());
                await GetOffices();
                await GetOfficeSpaces();
                ComboBoxIndex = 0;
            }
            else if (TabIndex == 1)
            {
                DeleteConfirmationWindowViewModel.DeleteAction = () => OfficeSpaceRepository.DeleteOfficeSpace(OfficeSpaceModel.ID);
                WindowManager.OpenWindow(new DeleteConfirmationWindow());
                await GetOfficeSpaces();
            }
        }

        /// <summary>
        /// Event handler for the return button. Closes the window.
        /// </summary>
        private void ReturnButton()
        {
            WindowManager.CloseWindow();
        }
    }
}