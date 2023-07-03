using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Wpf_QuanLyChiTieu.Model;

namespace Wpf_QuanLyChiTieu.ViewModel
{
    public class PersonalExpensesViewModel : BaseViewModel
    {
        private int currentMonth = DateTime.Now.Month;
        private int currentYear = DateTime.Now.Year;

        private int _exp_ID;
        private string _exp_Name;
        private DateTime? _exp_Date; // Chỉ định có được phép null hay không
        private int _exp_Price;

        private int _expI_ID;
        private string _expI_Note;

        private int _ec_ID;
        private string _ec_Name;

        private int _img_ID;
        private string _img_Path;

        private ObservableCollection<ExpenseCategory> _ExpCateg_List;
        private ObservableCollection<PersonalExpensesViewModel> _Exp_List;

        private ExpenseCategory _selectedExpCateg;
        public ExpenseCategory SelectedExpCateg
        {
            get => _selectedExpCateg;
            set
            {
                _selectedExpCateg = value;
                OnPropertyChanged();
            }
        }

        private PersonalExpensesViewModel _selectedItem;
        public PersonalExpensesViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();

                if (SelectedItem != null)
                {
                    Exp_Name = SelectedItem.Exp_Name;
                    Exp_Price = (int)SelectedItem.Exp_Price;
                    Exp_Date = SelectedItem.Exp_Date;
                    SelectedExpCateg = ExpCateg_List.FirstOrDefault(x => x.Ec_ID == SelectedItem.Ec_ID);
                    ExpI_Note = SelectedItem.ExpI_Note;
                    Img_Path = SelectedItem.Img_Path;
                }

            }
        }

        public ICommand AddPerExpenseCommand { get; set; }
        public ICommand UpdatePerExpenseCommand { get; set; }
        public ICommand DeletePerExpenseCommand { get; set; }
        public ICommand CancelPerExpenseCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }
        public ICommand ImportImageCommand { get; set; }

        public int Exp_ID { get => _exp_ID; set { _exp_ID = value; OnPropertyChanged(); } }
        public string Exp_Name { get => _exp_Name; set { _exp_Name = value; OnPropertyChanged(); } }
        public DateTime? Exp_Date { get => _exp_Date; set { _exp_Date = value; OnPropertyChanged(); } }
        public int Exp_Price { get => _exp_Price; set { _exp_Price = value; OnPropertyChanged(); } }

        public int ExpI_ID { get => _expI_ID; set { _expI_ID = value; OnPropertyChanged(); } }
        public string ExpI_Note { get => _expI_Note; set { _expI_Note = value; OnPropertyChanged(); } }

        public int Ec_ID { get => _ec_ID; set { _ec_ID = value; OnPropertyChanged(); } }
        public string Ec_Name { get => _ec_Name; set { _ec_Name = value; OnPropertyChanged(); } }
        public int Img_ID { get => _img_ID; set { _img_ID = value; OnPropertyChanged(); } }
        public string Img_Path { get => _img_Path; set { _img_Path = value; OnPropertyChanged(); } }

        public ObservableCollection<ExpenseCategory> ExpCateg_List { get => _ExpCateg_List; set { _ExpCateg_List = value; OnPropertyChanged(); } }
        public ObservableCollection<PersonalExpensesViewModel> Exp_List { get => _Exp_List; set { _Exp_List = value; OnPropertyChanged(); } }


        public PersonalExpensesViewModel()
        {
            ExpCateg_List = new ObservableCollection<ExpenseCategory>(DataProvider.Instance.DB.ExpenseCategories);

            LoadWindowCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {

                    var queryResult = from exp in DataProvider.Instance.DB.Psn_Expenses
                                      join expInfo in DataProvider.Instance.DB.Psn_ExpenseInfo on exp.P_exp_ID equals expInfo.P_exp_ID into joinedPsnExpenseInfo
                                      from expInfo in joinedPsnExpenseInfo.DefaultIfEmpty()
                                      join expCate in DataProvider.Instance.DB.ExpenseCategories on expInfo.Ec_ID equals expCate.Ec_ID into joinedExpenseCategory
                                      from expCate in joinedExpenseCategory.DefaultIfEmpty()
                                      join img in DataProvider.Instance.DB.Images on exp.P_exp_ID equals img.Img_Relation_ID into joinedImage
                                      from img in joinedImage.DefaultIfEmpty()
                                      where exp.P_exp_Date.Value.Month == currentMonth
                                      select new PersonalExpensesViewModel()
                                      {
                                          Exp_ID = exp.P_exp_ID,
                                          ExpI_ID = expInfo != null ? expInfo.P_expI_ID : 0,
                                          Ec_ID = expCate != null ? expCate.Ec_ID : 0,
                                          Img_ID = img != null ? img.Img_ID : 0,

                                          Exp_Name = exp.P_exp_Name,
                                          Exp_Price = (int)exp.P_exp_Price,
                                          Exp_Date = exp.P_exp_Date,
                                          ExpI_Note = expInfo != null ? expInfo.P_expI_Note : string.Empty,
                                          Img_Path = img != null ? img.Img_Url : string.Empty,
                                          Ec_Name = expCate != null ? expCate.Ec_Name : string.Empty
                                      };


                    Exp_List = new ObservableCollection<PersonalExpensesViewModel>(queryResult);
                }
            );

            AddPerExpenseCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (Exp_Name == string.Empty || Exp_Price == 0 || Exp_Date == null)
                        return false;

                    return true;
                },
                (param) =>
                {
                    try
                    {
                        var newExpense = new Psn_Expenses()
                        {
                            P_exp_Name = Exp_Name,
                            P_exp_Date = Exp_Date,
                            P_exp_Price = Exp_Price
                        };

                        DataProvider.Instance.DB.Psn_Expenses.Add(newExpense);
                        DataProvider.Instance.DB.SaveChanges();

                        var newExpenseDb = DataProvider.Instance.DB.Psn_Expenses.OrderByDescending(pe => pe.P_exp_ID).FirstOrDefault();

                        if (newExpenseDb != null)
                        {
                            if (SelectedExpCateg != null)
                            {
                                var newExpInfo = new Psn_ExpenseInfo()
                                {
                                    P_expI_Note = ExpI_Note,
                                    Ec_ID = SelectedExpCateg.Ec_ID,
                                    P_exp_ID = newExpenseDb.P_exp_ID

                                };

                                DataProvider.Instance.DB.Psn_ExpenseInfo.Add(newExpInfo);
                                DataProvider.Instance.DB.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("Vui lòng chọn danh mục chi tiêu.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);

                            }

                            if (!string.IsNullOrEmpty(Img_Path))
                            {
                                var newExpImg = new Image()
                                {
                                    Img_Name = Exp_Date + "_" + Exp_ID + "_" + Exp_Name,
                                    Img_Url = Img_Path,
                                    Img_Relation_ID = newExpenseDb.P_exp_ID,
                                    Img_Type = "Personal Expense"
                                };

                                DataProvider.Instance.DB.Images.Add(newExpImg);
                                DataProvider.Instance.DB.SaveChanges();
                            }
                        }

                        var newExpenseInfoDb = DataProvider.Instance.DB.Psn_ExpenseInfo.OrderByDescending(x => x.P_expI_ID).FirstOrDefault();
                        var newImgDb = DataProvider.Instance.DB.Images.OrderByDescending(x => x.Img_ID).FirstOrDefault();

                        var lastResult = new PersonalExpensesViewModel()
                        {
                            Exp_ID = newExpenseDb.P_exp_ID,
                            Exp_Name = newExpenseDb.P_exp_Name,
                            Exp_Price = (int)newExpenseDb.P_exp_Price,
                            Exp_Date = newExpenseDb.P_exp_Date,

                            ExpI_ID = newExpenseInfoDb != null ? newExpenseInfoDb.P_expI_ID : 0,
                            ExpI_Note = newExpenseInfoDb != null ? newExpenseInfoDb.P_expI_Note : string.Empty,
                            Ec_ID = newExpenseInfoDb != null ? newExpenseInfoDb.Ec_ID : 0,
                            Ec_Name = newExpenseInfoDb != null ? newExpenseInfoDb.ExpenseCategory.Ec_Name : string.Empty,

                            Img_ID = newImgDb != null ? newImgDb.Img_ID : 0,
                            Img_Path = newImgDb != null ? newImgDb.Img_Url : string.Empty
                        };

                        Exp_List.Add(lastResult);

                        MessageBox.Show("Thêm mới thành công.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);

                        Exp_Name = string.Empty;
                        Exp_Price = 0;
                        Exp_Date = null;
                        SelectedExpCateg = null;
                        ExpI_Note = string.Empty;
                        Img_Path = string.Empty;
                    }
                    catch
                    {
                        MessageBox.Show("Đã xảy ra lỗi.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            );

            UpdatePerExpenseCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (Exp_Name == string.Empty || Exp_Price == 0 || Exp_Date == null)
                        return false;

                    //var is_Exist_Item = DataProvider.Instance.DB.Expenses.Where(exp => exp.Exp_ID == SelectedItem.Exp_ID);

                    //if (is_Exist_Item == null || is_Exist_Item.Count() == 0)
                    //{
                    //    return false;
                    //}

                    return true;
                },
                (param) =>
                {
                    var exp = DataProvider.Instance.DB.Psn_Expenses.Where(e => e.P_exp_ID == SelectedItem.Exp_ID).SingleOrDefault();

                    if (exp != null)
                    {
                        exp.P_exp_Name = Exp_Name;
                        exp.P_exp_Price = Exp_Price;
                        exp.P_exp_Date = Exp_Date;
                        DataProvider.Instance.DB.SaveChanges();

                        var expI = DataProvider.Instance.DB.Psn_ExpenseInfo.Where(e => e.P_expI_ID == SelectedItem.ExpI_ID).SingleOrDefault();

                        if (expI != null)
                        {
                            expI.Ec_ID = SelectedExpCateg.Ec_ID;
                            expI.P_expI_Note = ExpI_Note;
                            DataProvider.Instance.DB.SaveChanges();
                        }
                        else
                        {
                            var newExpI = new Psn_ExpenseInfo()
                            {
                                Ec_ID = SelectedExpCateg.Ec_ID,
                                P_expI_Note = ExpI_Note,
                                P_exp_ID = exp.P_exp_ID
                            };

                            DataProvider.Instance.DB.Psn_ExpenseInfo.Add(newExpI);
                            DataProvider.Instance.DB.SaveChanges();
                        }

                        var expImg = DataProvider.Instance.DB.Images.Where(i => i.Img_ID == SelectedItem.Img_ID).SingleOrDefault();

                        if (expImg != null)
                        {
                            expImg.Img_Url = Img_Path;
                            DataProvider.Instance.DB.SaveChanges();

                        }
                        else
                        {
                            var newExpImg = new Image()
                            {
                                Img_Name = exp.P_exp_Date.Value.Day + exp.P_exp_Date.Value.Month + exp.P_exp_Date.Value.Year + "_" + exp.P_exp_ID + "_" + exp.P_exp_Name,
                                Img_Url = Img_Path,
                                Img_Relation_ID = exp.P_exp_ID,
                                Img_Type = "Personal Expense"
                            };

                            DataProvider.Instance.DB.Images.Add(newExpImg);
                            DataProvider.Instance.DB.SaveChanges();
                        }

                        MessageBox.Show("Cập nhật thành công.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);

                        SelectedItem.Exp_Name = Exp_Name;
                        SelectedItem.Exp_Price = Exp_Price;
                        SelectedItem.Exp_Date = Exp_Date;
                        SelectedItem.ExpI_Note = ExpI_Note;
                        SelectedItem.Ec_Name = SelectedExpCateg.Ec_Name;
                        SelectedItem.Img_Path = Img_Path;

                        // làm trống các trường dữ liệu
                        SelectedItem = null;
                        Exp_Name = string.Empty;
                        Exp_Price = 0;
                        Exp_Date = null;
                        ExpI_Note = string.Empty;
                        SelectedExpCateg = null;
                        Ec_Name = string.Empty;
                        Img_Path = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Đã xảy ra lỗi. Không tồn tại bản ghi này", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            );

            DeletePerExpenseCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (Exp_Name == string.Empty || Exp_Price == 0 || Exp_Date == null)
                        return false;

                    //var is_Exist_Item = DataProvider.Instance.DB.Expenses.Any(exp => exp.Exp_ID == SelectedItem.Exp_ID);
                    //if (!is_Exist_Item) return false;

                    return true;
                },
                (param) =>
                {
                    try
                    {
                        var exp = DataProvider.Instance.DB.Psn_Expenses.FirstOrDefault(e => e.P_exp_ID == SelectedItem.Exp_ID);
                        var expI = DataProvider.Instance.DB.Psn_ExpenseInfo.FirstOrDefault(e => e.P_expI_ID == SelectedItem.ExpI_ID);
                        var expImg = DataProvider.Instance.DB.Images.FirstOrDefault(i => i.Img_ID == SelectedItem.Img_ID);

                        if (exp != null)
                        {
                            if (expImg != null)
                            {
                                DataProvider.Instance.DB.Images.Remove(expImg);
                                DataProvider.Instance.DB.SaveChanges();
                            }

                            if (expI != null)
                            {
                                DataProvider.Instance.DB.Psn_ExpenseInfo.Remove(expI);
                                DataProvider.Instance.DB.SaveChanges();
                            }

                            DataProvider.Instance.DB.Psn_Expenses.Remove(exp);
                            DataProvider.Instance.DB.SaveChanges();

                            MessageBox.Show("Xóa thành công.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);

                            Exp_List.Remove(SelectedItem);

                            SelectedItem = null;
                            Exp_Name = string.Empty;
                            Exp_Price = 0;
                            Exp_Date = null;
                            ExpI_Note = string.Empty;
                            SelectedExpCateg = null;
                            Ec_Name = string.Empty;
                            Img_Path = string.Empty;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Có lỗi phát sinh.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            );

            CancelPerExpenseCommand = new RelayCommand<Object>(
                (param) => { return true; },
                (param) =>
                {
                    SelectedExpCateg = null;
                    SelectedItem = null;

                    Exp_Name = string.Empty;
                    Exp_Price = 0;
                    ExpI_Note = string.Empty;
                    Exp_Date = null;
                    Img_Path = string.Empty;

                }
            );

            ImportImageCommand = new RelayCommand<Object>(
                (param) => { return true; },
                (param) =>
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "All Files (*.*)|*.*";
                    ofd.Multiselect = true;

                    if (ofd.ShowDialog() == true)
                    {
                        //Uri uri = new Uri(ofd.FileName);

                        //string sourceFile = ofd.FileName;
                        //string resourceUri = "..//..//..//Images//FmlExp_Image//" + Path.GetFileName(sourceFile);
                        //File.Copy(sourceFile, resourceUri, true);

                        //ImgPath = uri.ToString();

                        string sourceFile = ofd.FileName;
                        string destinationFolder = "../../Images/FmlExp_Image/";
                        string destinationPath = Path.Combine(Environment.CurrentDirectory, destinationFolder, Path.GetFileName(sourceFile));
                        File.Copy(sourceFile, destinationPath, true);

                        Img_Path = destinationPath;
                    }
                }
            );

        }
    }
}
