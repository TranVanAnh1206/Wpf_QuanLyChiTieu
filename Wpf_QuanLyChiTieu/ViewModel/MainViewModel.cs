using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Prism.Events;
using QuanLyChiTieuModel.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Wpf_QuanLyChiTieu.Model;

namespace Wpf_QuanLyChiTieu.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IEventAggregator eventAggregator;

        public SnackbarMessageQueue MessageQueue { get; } = new SnackbarMessageQueue();
        private string snackBar_Bg;
        public string SnackBar_Bg { get => snackBar_Bg; set { snackBar_Bg = value; OnPropertyChanged(); } }

        public void ShowSnackbarMessage(string mes)
        {
            MessageQueue.Enqueue(mes);
        }

        // =========== Tab trang chủ ===========

        #region TAB TRANG CHỦ
        public bool IsLoaded = false;

        // Lấy ra ngày tháng hiện tại
        private int currentMonth;
        private int currentYear;
        public int CurrentMonth { get => currentMonth; set { currentMonth = value; OnPropertyChanged(); } }
        public int CurrentYear { get => currentYear; set { currentYear = value; OnPropertyChanged(); } }

        // Khai báo thuộc tính sẽ binding 
        private DateTime? _fromDate;
        private DateTime? _toDate;

        private string _expType;
        private string _expTypeName;

        // Table Expenses
        private string _exp_Name;
        private int _exp_Price;
        private DateTime? _exp_Date;

        // table ExpenseInfo
        private string _exp_Note;

        // other
        private string _category;

        private int _totalExpense;
        public int TotalExpense { get => _totalExpense; set { _totalExpense = value; OnPropertyChanged(); } }

        private int _totalPersonalExpense;
        public int TotalPersonalExpense { get => _totalPersonalExpense; set { _totalPersonalExpense = value; OnPropertyChanged(); } }

        // Selected
        private Categories _selectedCategory;
        private ObservableCollection<Categories> _categories_List;
        private ObservableCollection<MainViewModel> _exp_List;
        public ObservableCollection<MainViewModel> Exp_List { get => _exp_List; set { _exp_List = value; OnPropertyChanged(); } }

        public ICommand LoadMainWindowCommand { get; set; }
        public ICommand CloseMainWindowCommand { get; set; }
        public ICommand FamilyExpenseWindowCommand { get; set; }
        public ICommand PersonalExpenseWindowCommand { get; set; }
        public ICommand RevenuesWindowCommand { get; set; }
        public ICommand FillCommand { get; set; }
        public ICommand DemoCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand AnimationOpenCommand { get; set; }

        public DateTime? FromDate { get => _fromDate; set { _fromDate = value; OnPropertyChanged(); } }
        public DateTime? ToDate { get => _toDate; set { _toDate = value; OnPropertyChanged(); } }
        public string ExpType { get => _expType; set { _expType = value; OnPropertyChanged(); } }

        public string Exp_Name { get => _exp_Name; set { _exp_Name = value; OnPropertyChanged(); } }
        public int Exp_Price { get => _exp_Price; set { _exp_Price = value; OnPropertyChanged(); } }
        public DateTime? Exp_Date { get => _exp_Date; set { _exp_Date = value; OnPropertyChanged(); } }
        public string Exp_Note { get => _exp_Note; set { _exp_Note = value; OnPropertyChanged(); } }
        public string ExpTypeName { get => _expTypeName; set => _expTypeName = value; }

        private int _totalAmount;
        public int TotalAmount { get => _totalAmount; set { _totalAmount = value; OnPropertyChanged(); } }


        private FrameworkElement GetParentElement(FrameworkElement param)
        {
            FrameworkElement parent = param;

            if (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }

        public void UpdateSumFmlExpense()
        {
            decimal? sumFmlExpense = new Fml_ExpensesDAO().GetSumExpenseMonth(CurrentMonth, CurrentYear);
            TotalExpense = sumFmlExpense.HasValue ? (int)sumFmlExpense.Value : 0;
        }

        public void UpdateSumPsnExpense()
        {
            decimal? sumPsnExpense = new Psn_ExpensesDAO().GetSumExpenseMonth(CurrentMonth, CurrentYear);
            TotalPersonalExpense = sumPsnExpense.HasValue ? (int)sumPsnExpense.Value : 0;
        }

        public void UpdateExpense()
        {
            UpdateSumFmlExpense();
            UpdateSumPsnExpense();
        }

        #endregion

        // =========== Tab thống kê ===========
        #region Tab Thống kê
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Lable { get; set; }
        public Func<double, string> Formatter { get; set; }

        public int Thang1_Fml { get => thang1_Fml; set { thang1_Fml = value; OnPropertyChanged(); } }
        public int Thang2_Fml { get => thang2_Fml; set { thang2_Fml = value; OnPropertyChanged(); } }
        public int Thang3_Fml { get => thang3_Fml; set { thang3_Fml = value; OnPropertyChanged(); } }
        public int Thang4_Fml { get => thang4_Fml; set { thang4_Fml = value; OnPropertyChanged(); } }
        public int Thang5_Fml { get => thang5_Fml; set { thang5_Fml = value; OnPropertyChanged(); } }
        public int Thang6_Fml { get => thang6_Fml; set { thang6_Fml = value; OnPropertyChanged(); } }
        public int Thang7_Fml { get => thang7_Fml; set { thang7_Fml = value; OnPropertyChanged(); } }
        public int Thang8_Fml { get => thang8_Fml; set { thang8_Fml = value; OnPropertyChanged(); } }
        public int Thang9_Fml { get => thang9_Fml; set { thang9_Fml = value; OnPropertyChanged(); } }
        public int Thang10_Fml { get => thang10_Fml; set { thang10_Fml = value; OnPropertyChanged(); } }
        public int Thang11_Fml { get => thang11_Fml; set { thang11_Fml = value; OnPropertyChanged(); } }
        public int Thang12_Fml { get => thang12_Fml; set { thang12_Fml = value; OnPropertyChanged(); } }

        private int thang1_Fml;
        private int thang2_Fml;
        private int thang3_Fml;
        private int thang4_Fml;
        private int thang5_Fml;
        private int thang6_Fml;
        private int thang7_Fml;
        private int thang8_Fml;
        private int thang9_Fml;
        private int thang10_Fml;
        private int thang11_Fml;
        private int thang12_Fml;

        public int Thang1_Psn { get => thang1_Psn; set { thang1_Psn = value; OnPropertyChanged(); } }
        public int Thang2_Psn { get => thang2_Psn; set { thang2_Psn = value; OnPropertyChanged(); } }
        public int Thang3_Psn { get => thang3_Psn; set { thang3_Psn = value; OnPropertyChanged(); } }
        public int Thang4_Psn { get => thang4_Psn; set { thang4_Psn = value; OnPropertyChanged(); } }
        public int Thang5_Psn { get => thang5_Psn; set { thang5_Psn = value; OnPropertyChanged(); } }
        public int Thang6_Psn { get => thang6_Psn; set { thang6_Psn = value; OnPropertyChanged(); } }
        public int Thang7_Psn { get => thang7_Psn; set { thang7_Psn = value; OnPropertyChanged(); } }
        public int Thang8_Psn { get => thang8_Psn; set { thang8_Psn = value; OnPropertyChanged(); } }
        public int Thang9_Psn { get => thang9_Psn; set { thang9_Psn = value; OnPropertyChanged(); } }
        public int Thang10_Psn { get => thang10_Psn; set { thang10_Psn = value; OnPropertyChanged(); } }
        public int Thang11_Psn { get => thang11_Psn; set { thang11_Psn = value; OnPropertyChanged(); } }
        public int Thang12_Psn { get => thang12_Psn; set { thang12_Psn = value; OnPropertyChanged(); } }

        private int thang1_Psn;
        private int thang2_Psn;
        private int thang3_Psn;
        private int thang4_Psn;
        private int thang5_Psn;
        private int thang6_Psn;
        private int thang7_Psn;
        private int thang8_Psn;
        private int thang9_Psn;
        private int thang10_Psn;
        private int thang11_Psn;
        private int thang12_Psn;

        public ICommand ExportToExcelCommand { get; set; }


        #endregion

        public ObservableCollection<Categories> Categories_List
        {
            get => _categories_List;
            set
            {
                _categories_List = value;
                OnPropertyChanged();
            }
        }

        public string Category { get => _category; set { _category = value; OnPropertyChanged(); } }

        public Categories SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                Category = SelectedCategory.Category;
            }
        }

        // =========== Tab NGười dùng ===========
        #region Tab người dùng
        private int _uid;
        private string _uAvatar;
        private string _displayName;
        private string _password;
        private string _accName;
        private string _uAddress;
        private DateTime? _uBirth;

        public ICommand ChangedInformationCommand { get; set; }
        public ICommand ChangedAvatarCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand ChangePassCommand { get; set; }
        public ICommand PasswordCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public int Uid { get => _uid; set { _uid = value; OnPropertyChanged(); } }
        public string UAvatar { get => _uAvatar; set { _uAvatar = value; OnPropertyChanged(); } }
        public string DisplayName { get => _displayName; set { _displayName = value; OnPropertyChanged(); } }
        public string AccName { get => _accName; set { _accName = value; OnPropertyChanged(); } }
        public string UAddress { get => _uAddress; set { _uAddress = value; OnPropertyChanged(); } }
        public DateTime? UBirth { get => _uBirth; set { _uBirth = value; OnPropertyChanged(); } }

        private string isVisibility;
        private string isChangeInfoVisibility;
        private string _btnChangePassIsVisiblity;

        public string IsVisibility { get => isVisibility; set { isVisibility = value; OnPropertyChanged(); } }
        public string IsChangeInfoVisibility { get => isChangeInfoVisibility; set { isChangeInfoVisibility = value; OnPropertyChanged(); } }
        public string BtnChangePassIsVisiblity { get => _btnChangePassIsVisiblity; set { _btnChangePassIsVisiblity = value; OnPropertyChanged(); } }

        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        #endregion

        public MainViewModel()
        {
            var dbContext = QuanLyChiTieuModel.DataProvider.Instance.DB;

            CurrentMonth = DateTime.Now.Month;
            CurrentYear = DateTime.Now.Year;

            Categories_List = new ObservableCollection<Categories>
            {
                new Categories( Category = "Chi tiêu cá nhân"),
                new Categories( Category = "Chi tiêu gia đình"),
                new Categories( Category = "Thu nhập cá nhân")
            };

            LogoutCommand = new RelayCommand<Object>(
                (param) => { return true; },
                (param) =>
                {
                    LoginWindow login = new LoginWindow();

                    if (login.DataContext == null) return;

                    var loginVM = login.DataContext as LoginViewModel;

                    loginVM.IsLogin = false;

                    if (loginVM.IsLogin == false)
                    {
                        var confirmed = MessageBox.Show("Bạn thực sự muốn đăng xuất.", "Thông báo.", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (confirmed == MessageBoxResult.Yes)
                        {

                            //FrameworkElement window = GetParentElement(param as FrameworkElement);

                            //var isWindow = window as Window;

                            //isWindow.Close();

                            //login.ShowDialog();

                            MessageBox.Show("Không cho đăng xuất đâu, Lêu lêu :)).", "Thông báo.", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        }
                    }
                }
            );

            AnimationOpenCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(0.5)
                    };
                }
            );

            #region Tab Trang chủ
            UpdateExpense();

            LoadMainWindowCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    IsLoaded = true;

                    if (param == null)
                        return;

                    param.Hide();

                    LoginWindow login = new LoginWindow();
                    login.ShowDialog();

                    if (login.DataContext == null)
                        return;

                    var loginVM = login.DataContext as LoginViewModel;

                    if (loginVM.IsLogin)
                    {
                        param.Show();
                    }
                    else
                    {
                        param.Close();
                    }
                }
            );

            FamilyExpenseWindowCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    FamilyExpensesWindow fe_Window = new FamilyExpensesWindow();
                    fe_Window.ShowDialog();
                }

            );

            PersonalExpenseWindowCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    PersonalExpensesWindow pe_Window = new PersonalExpensesWindow();
                    pe_Window.ShowDialog();
                }
            );

            RevenuesWindowCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    SnackBar_Bg = "Green";
                    ShowSnackbarMessage("Bật window thành công.");
                }
            );

            FillCommand = new RelayCommand<Object>(
                (param) =>
                {
                    //var is_Exist_Result = dbContext.Expenses.Where(exp => exp.ExpType_ID == SelectedExpType.ExpType_ID && exp.Exp_Date >= FromDate && exp.Exp_Date <= ToDate);

                    //if (is_Exist_Result == null || is_Exist_Result.Count() == 0)
                    //{
                    //    MessageBox.Show("Không có thông tin nào được hiển thị.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);
                    //    return false;
                    //}

                    return true;
                },
                (param) =>
                {
                    if (FromDate == null || ToDate == null)
                    {
                        MessageBox.Show("Không được để trống trường ngày tháng.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (SelectedCategory == null)
                    {
                        MessageBox.Show("Vui lòng chọn danh mục.", "Chú ý", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        if (SelectedCategory.Category == "Chi tiêu cá nhân")
                        {
                            var P_Exp_Result = from exp in dbContext.Psn_Expenses
                                               join expI in dbContext.Psn_ExpenseInfo on exp.P_exp_ID equals expI.P_exp_ID into joinedPsn_ExpenseInfo
                                               from expI in joinedPsn_ExpenseInfo.DefaultIfEmpty()
                                               where exp.P_exp_Date >= FromDate && exp.P_exp_Date <= ToDate
                                               select new MainViewModel()
                                               {
                                                   Exp_Date = exp.P_exp_Date,
                                                   Exp_Name = exp.P_exp_Name,
                                                   Exp_Price = (int)exp.P_exp_Price,
                                                   Exp_Note = expI.P_expI_Note,
                                                   ExpTypeName = "Chi tiêu cá nhân"
                                               };

                            TotalAmount = P_Exp_Result.Sum(x => x.Exp_Price);
                            Exp_List = new ObservableCollection<MainViewModel>(P_Exp_Result);
                        }
                        else if (SelectedCategory.Category == "Chi tiêu gia đình")
                        {
                            var F_Exp_Result = from exp in dbContext.Fml_Expenses
                                               join expI in dbContext.Fml_ExpenseInfo on exp.F_exp_ID equals expI.F_exp_ID into joinedFml_ExpenseInfo
                                               from expI in joinedFml_ExpenseInfo.DefaultIfEmpty()
                                               where exp.F_exp_Date >= FromDate && exp.F_exp_Date <= ToDate
                                               select new MainViewModel()
                                               {
                                                   Exp_Date = exp.F_exp_Date,
                                                   Exp_Name = exp.F_exp_Name,
                                                   Exp_Price = (int)exp.F_exp_Price,
                                                   Exp_Note = expI != null ? expI.F_expI_Note : string.Empty,
                                                   ExpTypeName = "Chi tiêu gia đình"
                                               };

                            TotalAmount = F_Exp_Result.Sum(x => x.Exp_Price);
                            Exp_List = new ObservableCollection<MainViewModel>(F_Exp_Result);
                        }
                        else if (SelectedCategory.Category == "Thu nhập cá nhân")
                        {
                            var Rev_Result = from rev in dbContext.Revenues
                                             join RevI in dbContext.RevenueInfo on rev.Rev_ID equals RevI.Rev_ID into joinedRevenueInfo
                                             from expI in joinedRevenueInfo.DefaultIfEmpty()
                                             where rev.Rev_Date >= FromDate && rev.Rev_Date <= ToDate
                                             select new MainViewModel()
                                             {
                                                 Exp_Date = rev.Rev_Date,
                                                 Exp_Name = rev.Rev_Name,
                                                 Exp_Price = (int)rev.Rev_Price,
                                                 Exp_Note = expI != null ? expI.RevI_Note : string.Empty,
                                                 ExpTypeName = "Thu nhập cá nhân"
                                             };

                            TotalAmount = Rev_Result.Sum(x => x.Exp_Price);
                            Exp_List = new ObservableCollection<MainViewModel>(Rev_Result);
                        }
                    }
                }
            );
            #endregion

            // =========== tab Thống kê ===========
            #region Tab Thống Kê

            // Fml_Expenses
            decimal? sumExpenses_Thang1 = new Fml_ExpensesDAO().GetSumExpenseMonth(1, CurrentYear);
            Thang1_Fml = sumExpenses_Thang1.HasValue ? (int)sumExpenses_Thang1.Value : 0;

            ////
            decimal? sumExpenses_Thang2 = new Fml_ExpensesDAO().GetSumExpenseMonth(2, CurrentYear);
            Thang2_Fml = sumExpenses_Thang2.HasValue ? (int)sumExpenses_Thang2.Value : 0;

            ////
            decimal? sumExpenses_Thang3 = new Fml_ExpensesDAO().GetSumExpenseMonth(3, CurrentYear);
            Thang3_Fml = sumExpenses_Thang3.HasValue ? (int)sumExpenses_Thang3.Value : 0;

            ////
            decimal? sumExpenses_Thang4 = new Fml_ExpensesDAO().GetSumExpenseMonth(4, CurrentYear);
            Thang4_Fml = sumExpenses_Thang4.HasValue ? (int)sumExpenses_Thang4.Value : 0;

            //
            decimal? sumExpenses_Thang5 = new Fml_ExpensesDAO().GetSumExpenseMonth(5, CurrentYear);
            Thang5_Fml = sumExpenses_Thang5.HasValue ? (int)sumExpenses_Thang5.Value : 0;

            //
            decimal? sumExpenses_Thang6 = new Fml_ExpensesDAO().GetSumExpenseMonth(6, CurrentYear);
            Thang6_Fml = sumExpenses_Thang6.HasValue ? (int)sumExpenses_Thang6.Value : 0;

            //
            decimal? sumExpenses_Thang7 = new Fml_ExpensesDAO().GetSumExpenseMonth(7, CurrentYear);
            Thang7_Fml = sumExpenses_Thang7.HasValue ? (int)sumExpenses_Thang7.Value : 0;

            //
            decimal? sumExpenses_Thang8 = new Fml_ExpensesDAO().GetSumExpenseMonth(8, CurrentYear);
            Thang8_Fml = sumExpenses_Thang8.HasValue ? (int)sumExpenses_Thang8.Value : 0;

            //
            decimal? sumExpenses_Thang9 = new Fml_ExpensesDAO().GetSumExpenseMonth(9, CurrentYear);
            Thang9_Fml = sumExpenses_Thang9.HasValue ? (int)sumExpenses_Thang9.Value : 0;

            //
            decimal? sumExpenses_Thang10 = new Fml_ExpensesDAO().GetSumExpenseMonth(10, CurrentYear);
            Thang10_Fml = sumExpenses_Thang10.HasValue ? (int)sumExpenses_Thang10.Value : 0;

            //
            decimal? sumExpenses_Thang11 = new Fml_ExpensesDAO().GetSumExpenseMonth(11, CurrentYear);
            Thang11_Fml = sumExpenses_Thang11.HasValue ? (int)sumExpenses_Thang11.Value : 0;

            //
            decimal? sumExpenses_Thang12 = new Fml_ExpensesDAO().GetSumExpenseMonth(12, CurrentYear);
            Thang12_Fml = sumExpenses_Thang12.HasValue ? (int)sumExpenses_Thang12.Value : 0;

            //==============================
            // Psn_Expenses
            decimal? sumPsnExpenses_Thang1 = new Psn_ExpensesDAO().GetSumExpenseMonth(1, CurrentYear);
            Thang1_Psn = sumPsnExpenses_Thang1.HasValue ? (int)sumPsnExpenses_Thang1.Value : 0;

            ////
            decimal? sumPsnExpenses_Thang2 = new Psn_ExpensesDAO().GetSumExpenseMonth(2, CurrentYear);
            Thang2_Psn = sumPsnExpenses_Thang2.HasValue ? (int)sumPsnExpenses_Thang2.Value : 0;

            ////
            decimal? sumPsnExpenses_Thang3 = new Psn_ExpensesDAO().GetSumExpenseMonth(3, CurrentYear);
            Thang3_Psn = sumPsnExpenses_Thang3.HasValue ? (int)sumPsnExpenses_Thang3.Value : 0;

            ////
            decimal? sumPsnExpenses_Thang4 = new Psn_ExpensesDAO().GetSumExpenseMonth(4, CurrentYear);
            Thang4_Psn = sumPsnExpenses_Thang4.HasValue ? (int)sumPsnExpenses_Thang4.Value : 0;

            //
            decimal? sumPsnExpenses_Thang5 = new Psn_ExpensesDAO().GetSumExpenseMonth(5, CurrentYear);
            Thang5_Psn = sumPsnExpenses_Thang5.HasValue ? (int)sumPsnExpenses_Thang5.Value : 0;

            //
            decimal? sumPsnExpenses_Thang6 = new Psn_ExpensesDAO().GetSumExpenseMonth(6, CurrentYear);
            Thang6_Psn = sumPsnExpenses_Thang6.HasValue ? (int)sumPsnExpenses_Thang6.Value : 0;

            //
            decimal? sumPsnExpenses_Thang7 = new Psn_ExpensesDAO().GetSumExpenseMonth(7, CurrentYear);
            Thang7_Psn = sumPsnExpenses_Thang7.HasValue ? (int)sumPsnExpenses_Thang7.Value : 0;

            //
            decimal? sumPsnExpenses_Thang8 = new Psn_ExpensesDAO().GetSumExpenseMonth(8, CurrentYear);
            Thang8_Psn = sumPsnExpenses_Thang8.HasValue ? (int)sumPsnExpenses_Thang8.Value : 0;

            //
            decimal? sumPsnExpenses_Thang9 = new Psn_ExpensesDAO().GetSumExpenseMonth(9, CurrentYear);
            Thang9_Psn = sumPsnExpenses_Thang9.HasValue ? (int)sumPsnExpenses_Thang9.Value : 0;

            //
            decimal? sumPsnExpenses_Thang10 = new Psn_ExpensesDAO().GetSumExpenseMonth(10, CurrentYear);
            Thang10_Psn = sumPsnExpenses_Thang10.HasValue ? (int)sumPsnExpenses_Thang10.Value : 0;

            //
            decimal? sumPsnExpenses_Thang11 = new Psn_ExpensesDAO().GetSumExpenseMonth(11, CurrentYear);
            Thang11_Psn = sumPsnExpenses_Thang11.HasValue ? (int)sumPsnExpenses_Thang11.Value : 0;

            //
            decimal? sumPsnExpenses_Thang12 = new Psn_ExpensesDAO().GetSumExpenseMonth(12, CurrentYear);
            Thang12_Psn = sumPsnExpenses_Thang12.HasValue ? (int)sumPsnExpenses_Thang12.Value : 0;

            SeriesCollection = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "Chi tiêu gia đình",
                    Values = new ChartValues<double> {Thang1_Fml, Thang2_Fml, Thang3_Fml, Thang4_Fml, Thang5_Fml, Thang6_Fml, Thang7_Fml, Thang8_Fml, Thang9_Fml, Thang10_Fml, Thang11_Fml, Thang12_Fml},

                },
                new ColumnSeries
                {
                    Title = "Chi tiêu Cá nhân",
                    Values = new ChartValues<double> {Thang1_Psn, Thang2_Psn, Thang3_Psn, Thang4_Psn, Thang5_Psn, Thang6_Psn, Thang7_Psn, Thang8_Psn, Thang9_Psn, Thang10_Psn, Thang11_Psn, Thang12_Psn},
                },
            };

            Lable = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
            Formatter = value => value.ToString("N");

            ExportToExcelCommand = new RelayCommand<Object>(
                (param) => { return true; },
                (param) =>
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Excel files (*.xlsx)|*.xlsx";
                    sfd.Title = "Lưu file.";

                    if (sfd.ShowDialog() == true)
                    {
                        string filePath = sfd.FileName;

                        if (string.IsNullOrEmpty(filePath))
                        {
                            MessageBox.Show("Đường dẫn không hợp lệ.", "Cảnh báo.", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                            using (ExcelPackage excelPackage = new ExcelPackage())
                            {
                                // đặt tên người tạo file
                                excelPackage.Workbook.Properties.Author = AccName + "_" + Uid;
                                excelPackage.Workbook.Properties.Title = "Bản báo cáo thống kê";

                                excelPackage.Workbook.Worksheets.Add("Báo cáo chung.");
                                excelPackage.Workbook.Worksheets.Add("Báo cáo chi tiêu cá nhân.");
                                excelPackage.Workbook.Worksheets.Add("Báo cáo chi tiêu gia đình .");
                                excelPackage.Workbook.Worksheets.Add("Báo cáo thu nhập cá nhân.");

                                ExcelWorksheet ws = excelPackage.Workbook.Worksheets[0];
                                ws.Name = "Báo cáo tháng.";
                                ws.Cells.Style.Font.Size = 14;
                                ws.Cells.Style.Font.Name = "Arial";
                                string[] arrColHeader = { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };

                                int countColumn = arrColHeader.Count();

                                ws.Cells[1, 1].Value = "Thống kê ngày " + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                                ws.Cells[1, 1, 1, countColumn + 1].Merge = true;
                                ws.Cells[1, 1, 1, countColumn + 1].Style.Font.Bold = true;
                                ws.Cells[1, 1, 1, countColumn + 1].Style.Font.Size = 20;
                                ws.Cells[1, 1, 1, countColumn + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                                int rowIndex = 2;
                                int colIndex = 2;

                                foreach (var item in arrColHeader)
                                {
                                    var cell = ws.Cells[rowIndex, colIndex];

                                    //set màu thành gray
                                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    cell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);


                                    //căn chỉnh các border
                                    var border = cell.Style.Border;
                                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    //gán giá trị
                                    cell.Value = item;

                                    ws.Column(cell.Start.Column).AutoFit();

                                    colIndex++;
                                }

                                colIndex = 2;

                                ws.Cells[rowIndex + 1, 1].Value = "Chi tiêu gia đình";
                                ws.Column(ws.Cells[rowIndex + 1, 1].Start.Column).AutoFit();

                                ws.Cells[rowIndex + 2, 1].Value = "Chi tiêu cá nhân";
                                ws.Column(ws.Cells[rowIndex + 2, 1].Start.Column).AutoFit();

                                ws.Cells[rowIndex + 3, 1].Value = "Thu nhập cá nhân";
                                ws.Column(ws.Cells[rowIndex + 3, 1].Start.Column).AutoFit();


                                // Chi tiêu gia đình
                                ws.Cells[3, colIndex].Value = Thang1_Fml;
                                ws.Column(ws.Cells[3, colIndex].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 1].Value = Thang2_Fml;
                                ws.Column(ws.Cells[3, colIndex + 1].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 2].Value = Thang3_Fml;
                                ws.Column(ws.Cells[3, colIndex + 2].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 3].Value = Thang4_Fml;
                                ws.Column(ws.Cells[3, colIndex + 3].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 4].Value = Thang5_Fml;
                                ws.Column(ws.Cells[3, colIndex + 4].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 5].Value = Thang6_Fml;
                                ws.Column(ws.Cells[3, colIndex + 5].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 6].Value = Thang7_Fml;
                                ws.Column(ws.Cells[3, colIndex + 6].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 7].Value = Thang8_Fml;
                                ws.Column(ws.Cells[3, colIndex + 7].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 8].Value = Thang9_Fml;
                                ws.Column(ws.Cells[3, colIndex + 8].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 9].Value = Thang10_Fml;
                                ws.Column(ws.Cells[3, colIndex + 9].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 10].Value = Thang11_Fml;
                                ws.Column(ws.Cells[3, colIndex + 10].Start.Column).AutoFit();

                                ws.Cells[3, colIndex + 11].Value = Thang12_Fml;
                                ws.Column(ws.Cells[3, colIndex + 11].Start.Column).AutoFit();

                                // Chi tiêu cá nhân
                                ws.Cells[4, colIndex].Value = Thang1_Psn;
                                ws.Column(ws.Cells[4, colIndex].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 1].Value = Thang2_Psn;
                                ws.Column(ws.Cells[4, colIndex + 1].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 2].Value = Thang3_Psn;
                                ws.Column(ws.Cells[4, colIndex + 2].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 3].Value = Thang4_Psn;
                                ws.Column(ws.Cells[4, colIndex + 3].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 4].Value = Thang5_Psn;
                                ws.Column(ws.Cells[4, colIndex + 4].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 5].Value = Thang6_Psn;
                                ws.Column(ws.Cells[4, colIndex + 5].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 6].Value = Thang7_Psn;
                                ws.Column(ws.Cells[4, colIndex + 6].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 7].Value = Thang8_Psn;
                                ws.Column(ws.Cells[4, colIndex + 7].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 8].Value = Thang9_Psn;
                                ws.Column(ws.Cells[4, colIndex + 8].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 9].Value = Thang10_Psn;
                                ws.Column(ws.Cells[4, colIndex + 9].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 10].Value = Thang11_Psn;
                                ws.Column(ws.Cells[4, colIndex + 10].Start.Column).AutoFit();

                                ws.Cells[4, colIndex + 11].Value = Thang12_Psn;
                                ws.Column(ws.Cells[4, colIndex + 11].Start.Column).AutoFit();


                                // Save file
                                byte[] bin = excelPackage.GetAsByteArray();
                                File.WriteAllBytes(filePath, bin);
                            }

                            //MessageBox.Show("Lưu file thành công.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);
                            SnackBar_Bg = "Green";
                            ShowSnackbarMessage("Lưu file thành công.");
                        }
                    }
                }
            );

            #endregion


            // =========== Tab Người dùng ===========
            #region Tab Người dùng
            var acc = dbContext.Accounts.FirstOrDefault();
            var user = dbContext.Users.FirstOrDefault();

            Uid = 1;
            AccName = acc.AccName;
            DisplayName = acc.AccDisplayname;
            Password = acc.AccPassword;
            UAddress = user.u_Address;
            UBirth = user.u_birth;

            IsVisibility = "Collapsed";
            IsChangeInfoVisibility = "Visible";
            BtnChangePassIsVisiblity = "Collapsed";


            ChangedInformationCommand = new RelayCommand<Object>(
                (param) => { return true; },
                (param) =>
                {
                    IsVisibility = "Visible";
                    IsChangeInfoVisibility = "Collapsed";
                }
            );

            SaveCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (isVisibility == "Collapsed") return false;

                    return true;
                },
                (param) =>
                {
                    IsVisibility = "Collapsed";
                    IsChangeInfoVisibility = "Visible";

                    var confirmed = MessageBox.Show("Xác nhận thay đổi thông tin cá nhân ?.", "Thông báo.", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (confirmed == MessageBoxResult.Yes)
                    {
                        MessageBox.Show("Thay đổi thông tin thành công.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            );

            ChangedAvatarCommand = new RelayCommand<Object>(
                (param) => { return true; },
                (param) =>
                {
                    OpenFileDialog ofd = new OpenFileDialog();

                    if (ofd.ShowDialog() == true)
                    {
                        string sourceFile = ofd.FileName;
                        string destinationFolder = "../../Images/FmlExp_Image/";
                        string destinationPath = Path.Combine(Environment.CurrentDirectory, destinationFolder, Path.GetFileName(sourceFile));
                        File.Copy(sourceFile, destinationPath, true);

                        UAvatar = destinationPath;

                    }


                }
            );

            ChangePassCommand = new RelayCommand<Object>(
                (param) => true,
                (param) =>
                {
                    BtnChangePassIsVisiblity = "Visible";
                }
            );

            PasswordCommand = new RelayCommand<PasswordBox>(
                (param) => true,
                (param) =>
                {
                    param.Password = Password;
                }
                );

            CancelCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (IsVisibility != "Visible")
                        return false;

                    return true;
                },
                (param) =>
                {
                    IsVisibility = "Collapsed";
                    IsChangeInfoVisibility = "Visible";
                }

            );

            #endregion
        }
    }
}
