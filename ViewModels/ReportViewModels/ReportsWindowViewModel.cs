using System;
using Ohtu1Project.Views;
using Ohtu1Project.Models;
using System.Windows.Input;
using Ohtu1Project.Helpers;
using System.Threading.Tasks;
using Ohtu1Project.Repositories;
using System.Collections.ObjectModel;
using Ohtu1Project.Views.ReportViews;
using Ohtu1Project.ViewModels.PopupViewModels;

namespace Ohtu1Project.ViewModels.ReportViewModels
{
    /// <summary>
    /// Datacontext class for ReportsWindow. Inherits MainViewModel class.
    /// </summary>
    internal class ReportsWindowViewModel : MainViewModel
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
                EnableViewButton = _selectedIndex < 0 ? false : true;
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
                LostFocus();
                OfficeReportsCollection?.Clear();
                ServiceReportsCollection?.Clear();
                EnableSearchButton = (TabIndex == 1 && OfficeSpaceModel == null) ? false : true;
                OnPropertyChanged();
            }
        }

        private int _officeSpaceIndex;
        public int OfficeSpaceIndex { get { return _officeSpaceIndex; } set { _officeSpaceIndex = value; OnPropertyChanged(); } }

        private bool _enableSearchButton;
        public bool EnableSearchButton { get { return _enableSearchButton; } set { _enableSearchButton = value; OnPropertyChanged(); } }

        private bool _enableviewButton;
        public bool EnableViewButton { get { return _enableviewButton; } set { _enableviewButton = value; OnPropertyChanged(); } }

        private bool _enableDatePickerFirstTab;
        public bool EnableDatePickerFirstTab { get { return _enableDatePickerFirstTab; } set { _enableDatePickerFirstTab = value; OnPropertyChanged(); } }

        private bool _enableDatePickerSecondTab;
        public bool EnableDatePickerSecondTab { get { return _enableDatePickerSecondTab; } set { _enableDatePickerSecondTab = value; OnPropertyChanged(); } }

        private bool _enableOfficesComboBox;
        public bool EnableOfficesComboBox
        {
            get
            {
                return _enableOfficesComboBox;
            }
            set
            {
                _enableOfficesComboBox = value;
                //OfficesError = EnableOfficesComboBox == true ? string.Empty : "Toimipisteitä ei löydy";
                OnPropertyChanged();
            }
        }

        private bool _enableOfficeSpacesComboBox;
        public bool EnableOfficeSpacesComboBox
        {
            get
            {
                return _enableOfficeSpacesComboBox;
            }
            set
            {
                _enableOfficeSpacesComboBox = value;
                //OfficeSpacesError = EnableOfficeSpacesComboBox == true ? string.Empty : "Toimitiloja ei löydy";
                OnPropertyChanged();
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                EndDate = StartDate.AddDays(1);
                EndDateDisplayDateStart = EndDate;
                OnPropertyChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate { get { return _endDate; } set { _endDate = value; OnPropertyChanged(); } }

        public DateTime StartDateDisplayDateStart { get; set; }

        private DateTime _endDateDisplayDateStart;
        public DateTime EndDateDisplayDateStart { get { return _endDateDisplayDateStart; } set { _endDateDisplayDateStart = value; OnPropertyChanged(); } }

        private OfficeModel _officeModel;
        public OfficeModel OfficeModel
        {
            get
            {
                return _officeModel;
            }
            set
            {
                _officeModel = value;
                OfficeReportsCollection?.Clear();
                OnPropertyChanged();
            }
        }

        private OfficeModel _officeModelForServices;
        public OfficeModel OfficeModelForServices
        {
            get
            {
                return _officeModelForServices;
            }
            set
            {
                _officeModelForServices = value;

                if (OfficeModelForServices != null)
                {
                    _=GetOfficeSpaces();
                }

                ServiceReportsCollection?.Clear();

                OnPropertyChanged();
            }
        }

        private OfficeSpaceModel _officeSpaceModel;
        public OfficeSpaceModel OfficeSpaceModel
        {
            get
            {
                return _officeSpaceModel;
            }
            set
            {
                _officeSpaceModel = value;
                EnableSearchButton = OfficeSpaceModel == null ? false : true;
                ServiceReportsCollection?.Clear();
                OnPropertyChanged();
            }
        }

        private OfficeReportModel _officeReportModel;
        public OfficeReportModel OfficeReportModel { get { return _officeReportModel; } set { _officeReportModel = value; OnPropertyChanged(); } }

        private ServiceReportModel _serviceReportModel;
        public ServiceReportModel ServiceReportModel { get { return _serviceReportModel; } set { _serviceReportModel = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeModel> _officesCollection;
        public ObservableCollection<OfficeModel> OfficesCollection { get { return _officesCollection; } set { _officesCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeSpaceModel> _officeSpacesCollection;
        public ObservableCollection<OfficeSpaceModel> OfficeSpacesCollection { get { return _officeSpacesCollection; } set { _officeSpacesCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<OfficeReportModel> _officeReportsCollection;
        public ObservableCollection<OfficeReportModel> OfficeReportsCollection { get { return _officeReportsCollection; } set { _officeReportsCollection = value; OnPropertyChanged(); } }

        private ObservableCollection<ServiceReportModel> _serviceReportsCollection;
        public ObservableCollection<ServiceReportModel> ServiceReportsCollection { get { return _serviceReportsCollection; } set { _serviceReportsCollection = value; OnPropertyChanged(); } }

        /// <summary>
        /// Command that is responsible for ContentRendered event.
        /// </summary>
        public ICommand LoadedCommand => new DelegateCommand(Loaded);

        /// <summary>
        /// Command that is responsible for lose focus from datagrid.
        /// </summary>
        public ICommand LostFocusCommand => new DelegateCommand(LostFocus);

        /// <summary>
        /// A command for the search button.
        /// </summary>
        public ICommand SearchButtonCommand => new DelegateCommand(SearchButton);

        /// <summary>
        /// A command for the view button.
        /// </summary>
        public ICommand ViewButtonCommand => new DelegateCommand(ViewButton);

        /// <summary>
        /// A command for the return button.
        /// </summary>
        public ICommand ReturnButtonCommand => new DelegateCommand(ReturnButton);

        public ReportsWindowViewModel()
        {
            StartDate = DateTime.Today;
            StartDateDisplayDateStart = DateTime.Today;
        }

        /// <summary>
        /// Calls GetOffices() method asynchronously.
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
                OfficesCollection = await OfficeRepository.FetchAllOfficesMinorDetails();
                EnableOfficesComboBox = OfficesCollection.Count > 0 ? true : false;
                EnableOfficeSpacesComboBox = OfficesCollection.Count > 0 ? true : false;
                EnableDatePickerFirstTab = OfficesCollection.Count > 0 ? true : false;
                EnableDatePickerSecondTab = OfficesCollection.Count > 0 ? true : false;
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
                OfficeSpacesCollection = await OfficeSpaceRepository.FetchAllOfficeSpacesMinorDetail(OfficeModelForServices.ID);
                EnableOfficeSpacesComboBox = OfficeSpacesCollection.Count > 0 ? true : false;
                EnableDatePickerSecondTab = OfficeSpacesCollection.Count > 0 ? true : false;
                OfficeSpaceIndex = 0;
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
        /// Asynchronously fetches the office reports collection and report services for the specified office and date range,
        /// and assigns it to the OfficeReportsCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task GetOfficeReports()
        {
            try
            {
                OfficeReportsCollection = await ReportsRepository.FetchOfficeReports(OfficeModel.ID, StartDate, EndDate);
                await ReportsRepository.FetchOfficeReportServices(OfficeReportsCollection);
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetOfficeReports();
                WindowManager.OpenWindow(new ErrorWindow());
            }
        }

        /// <summary>
        /// Asynchronously fetches service reports for the specified office space and time period,
        /// and assigns it to the ServiceReportsCollection property.
        /// If an exception is thrown, it logs the error, opens an ErrorWindow and sets the ErrorWindowViewModel's AsyncRetryMethod to itself.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetServiceReports()
        {
            try
            {
                ServiceReportsCollection = await ReportsRepository.FetchServiceReports(OfficeSpaceModel.ID, StartDate, EndDate);
            }
            catch (Exception ex)
            {
                LogWriter.LogError(ex);
                ErrorWindowViewModel.ClearDelegates();
                ErrorWindowViewModel.AsyncRetryMethod = async () => await GetServiceReports();
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
        /// Event handler fo the search button.
        /// Asynchronously calls a report fetching method depending on TabIndex value.
        /// </summary>
        private async void SearchButton()
        {
            if (TabIndex == 0)
            {
                await GetOfficeReports();
            }
            else if (TabIndex == 1)
            {
                await GetServiceReports();
            }
        }

        /// <summary>
        /// Handles the View button click event. Opens a new window to display details of the selected report or service report,
        /// depending on the current tab index.
        /// </summary>
        private void ViewButton()
        {
            if (TabIndex == 0)
            {
                ViewReportWindowViewModel.OfficeReportModel = OfficeReportModel;
                WindowManager.OpenWindow(new ViewReportWindow());
                LostFocus();
            }
            else if (TabIndex == 1)
            {
                ViewServiceReportWindowViewModel.ServiceReportModel = ServiceReportModel;
                WindowManager.OpenWindow(new ViewServiceReportWindow());
                LostFocus();
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