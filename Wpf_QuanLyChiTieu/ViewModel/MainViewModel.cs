using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wpf_QuanLyChiTieu.Model;

namespace Wpf_QuanLyChiTieu.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        // =========== Tab trang chủ ===========

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
        private string _exp_Date;

        // table ExpenseInfo
        private string _exp_Note;


        private int _totalExpense;
        public int TotalExpense
        {
            get => _totalExpense;
            set
            {
                _totalExpense = value;
                OnPropertyChanged();
            }
        }

        private int _totalPersonalExpense;
        public int TotalPersonalExpense
        {
            get => _totalPersonalExpense;
            set
            {
                _totalPersonalExpense = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MainViewModel> _exp_List;
        public ObservableCollection<MainViewModel> Exp_List { get => _exp_List; set { _exp_List = value; OnPropertyChanged(); } }

        public ICommand LoadMainWindowCommand { get; set; }
        public ICommand CloseMainWindowCommand { get; set; }
        public ICommand FamilyExpenseWindowCommand { get; set; }
        public ICommand PersonalExpenseWindowCommand { get; set; }
        public ICommand FillCommand { get; set; }
        public ICommand DemoCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public DateTime? FromDate { get => _fromDate; set { _fromDate = value; OnPropertyChanged(); } }
        public DateTime? ToDate { get => _toDate; set { _toDate = value; OnPropertyChanged(); } }
        public string ExpType { get => _expType; set { _expType = value; OnPropertyChanged(); } }

        public string Exp_Name { get => _exp_Name; set { _exp_Name = value; OnPropertyChanged(); } }
        public int Exp_Price { get => _exp_Price; set { _exp_Price = value; OnPropertyChanged(); } }
        public string Exp_Date { get => _exp_Date; set { _exp_Date = value; OnPropertyChanged(); } }
        public string Exp_Note { get => _exp_Note; set { _exp_Note = value; OnPropertyChanged(); } }
        public string ExpTypeName { get => _expTypeName; set => _expTypeName = value; }

        private FrameworkElement GetParentElement(FrameworkElement param)
        {
            FrameworkElement parent = param;

            if (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }

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

        #endregion
        public MainViewModel()
        {
            CurrentMonth = DateTime.Now.Month;
            CurrentYear = DateTime.Now.Year;

            decimal? sumPsnExpense = DataProvider.Instance.DB.Psn_Expenses
                                                 .Where(pe => pe.P_exp_Date.Value.Month == CurrentMonth && pe.P_exp_Date.Value.Year == CurrentYear)
                                                 .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            TotalPersonalExpense = sumPsnExpense.HasValue ? (int) sumPsnExpense.Value : 0;

            decimal? sumFmlExpense = DataProvider.Instance.DB.Fml_Expenses
                                            .Where(fe => fe.F_exp_Date.Value.Month == CurrentMonth && fe.F_exp_Date.Value.Year == CurrentYear)
                                            .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            TotalExpense = sumFmlExpense.HasValue ? (int)sumFmlExpense.Value : 0;

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

            #region Tab Trang chủ
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

            FillCommand = new RelayCommand<Object>(
                (param) =>
                {
                    //var is_Exist_Result = DataProvider.Instance.DB.Expenses.Where(exp => exp.ExpType_ID == SelectedExpType.ExpType_ID && exp.Exp_Date >= FromDate && exp.Exp_Date <= ToDate);

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
                    else
                    {
                        if (FromDate > ToDate)
                        {
                            MessageBox.Show("Không hợp lệ.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                                //var result_List = from Exp in DataProvider.Instance.DB.Expenses
                                //                  join ExpType in DataProvider.Instance.DB.ExpensesType
                                //                  on Exp.ExpType_ID equals ExpType.ExpType_ID
                                //                  where Exp.ExpType_ID == SelectedExpType.ExpType_ID && Exp.Exp_Date >= FromDate && Exp.Exp_Date <= ToDate
                                //                  select new MainViewModel()
                                //                  {
                                //                      Exp_Name = Exp.Exp_Name,
                                //                      Exp_Date = Exp.Exp_Date.ToString(),
                                //                      Exp_Price = (int)Exp.Exp_Price,
                                //                      ExpTypeName = ExpType.ExpType_Name
                                //                  };
                                //Exp_List = new ObservableCollection<MainViewModel>(result_List);
                        }

                    }
                }

            );

            DemoCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    Window1 demo = new Window1();
                    demo.ShowDialog();
                }
            );
            #endregion

            // =========== tab Thống kê ===========
            #region Tab Thống Kê

            // Fml_Expenses
            decimal? sumExpenses_Thang1 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 1 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang1_Fml = sumExpenses_Thang1.HasValue ? (int)sumExpenses_Thang1.Value : 0;

            ////
            decimal? sumExpenses_Thang2 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 2 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang2_Fml = sumExpenses_Thang2.HasValue ? (int)sumExpenses_Thang2.Value : 0;

            ////
            decimal? sumExpenses_Thang3 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 3 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang3_Fml = sumExpenses_Thang3.HasValue ? (int)sumExpenses_Thang3.Value : 0;

            ////
            decimal? sumExpenses_Thang4 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 4 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang4_Fml = sumExpenses_Thang4.HasValue ? (int)sumExpenses_Thang4.Value : 0;

            //
            decimal? sumExpenses_Thang5 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 5 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang5_Fml = sumExpenses_Thang5.HasValue ? (int)sumExpenses_Thang5.Value : 0;

            //
            decimal? sumExpenses_Thang6 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 6 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang6_Fml = sumExpenses_Thang6.HasValue ? (int)sumExpenses_Thang6.Value : 0;

            //
            decimal? sumExpenses_Thang7 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 7 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang7_Fml = sumExpenses_Thang7.HasValue ? (int)sumExpenses_Thang7.Value : 0;

            //
            decimal? sumExpenses_Thang8 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 8 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang8_Fml = sumExpenses_Thang8.HasValue ? (int)sumExpenses_Thang8.Value : 0;

            //
            decimal? sumExpenses_Thang9 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 9 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang9_Fml = sumExpenses_Thang9.HasValue ? (int)sumExpenses_Thang9.Value : 0;

            //
            decimal? sumExpenses_Thang10 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 10 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang10_Fml = sumExpenses_Thang10.HasValue ? (int)sumExpenses_Thang10.Value : 0;

            //
            decimal? sumExpenses_Thang11 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 11 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang11_Fml = sumExpenses_Thang11.HasValue ? (int)sumExpenses_Thang11.Value : 0;

            //
            decimal? sumExpenses_Thang12 = DataProvider.Instance.DB.Fml_Expenses
                                    .Where(exp => exp.F_exp_Date.Value.Month == 12 && exp.F_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.F_exp_Price) ?? 0;

            Thang12_Fml = sumExpenses_Thang12.HasValue ? (int)sumExpenses_Thang12.Value : 0;

            //==============================
            // Psn_Expenses
            decimal? sumPsnExpenses_Thang1 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 1 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang1_Psn = sumPsnExpenses_Thang1.HasValue ? (int)sumPsnExpenses_Thang1.Value : 0;

            ////
            decimal? sumPsnExpenses_Thang2 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 2 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang2_Psn = sumPsnExpenses_Thang2.HasValue ? (int)sumPsnExpenses_Thang2.Value : 0;

            ////
            decimal? sumPsnExpenses_Thang3 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 3 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang3_Psn = sumPsnExpenses_Thang3.HasValue ? (int)sumPsnExpenses_Thang3.Value : 0;

            ////
            decimal? sumPsnExpenses_Thang4 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 4 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang4_Psn = sumPsnExpenses_Thang4.HasValue ? (int)sumPsnExpenses_Thang4.Value : 0;

            //
            decimal? sumPsnExpenses_Thang5 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 5 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang5_Psn = sumPsnExpenses_Thang5.HasValue ? (int)sumPsnExpenses_Thang5.Value : 0;

            //
            decimal? sumPsnExpenses_Thang6 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 6 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang6_Psn = sumPsnExpenses_Thang6.HasValue ? (int)sumPsnExpenses_Thang6.Value : 0;

            //
            decimal? sumPsnExpenses_Thang7 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 7 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang7_Psn = sumPsnExpenses_Thang7.HasValue ? (int)sumPsnExpenses_Thang7.Value : 0;

            //
            decimal? sumPsnExpenses_Thang8 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 8 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang8_Psn = sumPsnExpenses_Thang8.HasValue ? (int)sumPsnExpenses_Thang8.Value : 0;

            //
            decimal? sumPsnExpenses_Thang9 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 9 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang9_Psn = sumPsnExpenses_Thang9.HasValue ? (int)sumPsnExpenses_Thang9.Value : 0;

            //
            decimal? sumPsnExpenses_Thang10 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 10 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang10_Psn = sumPsnExpenses_Thang10.HasValue ? (int)sumPsnExpenses_Thang10.Value : 0;

            //
            decimal? sumPsnExpenses_Thang11 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 11 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

            Thang11_Psn = sumPsnExpenses_Thang11.HasValue ? (int)sumPsnExpenses_Thang11.Value : 0;

            //
            decimal? sumPsnExpenses_Thang12 = DataProvider.Instance.DB.Psn_Expenses
                                    .Where(exp => exp.P_exp_Date.Value.Month == 12 && exp.P_exp_Date.Value.Year == CurrentYear)
                                    .Sum(exp => (decimal?)exp.P_exp_Price) ?? 0;

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

            #endregion
        }
    }
}
