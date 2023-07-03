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
    public class FamilyExpenseViewModel : BaseViewModel
    {
        private string imgPath;
        public string ImgPath { get => imgPath; set { imgPath = value; OnPropertyChanged(); } }

        // Command
        public ICommand ExpAddCommand { get; set; }
        public ICommand ExpUpdateCommand { get; set; }
        public ICommand ExpDeleteCommand { get; set; }
        public ICommand ExpCancelCommand { get; set; }
        public ICommand ImportImageCommand { get; set; }
        public ICommand LoadWindowCommand { get; set; }

        // Khai báo biến binding
        private string _ec_Name; // Expenses category
        private int _ec_ID;

        private int _exp_ID; // Expense
        private string _exp_Name;
        private DateTime? _exp_Date;
        private int _exp_Price;

        private int _expI_ID; // Expense Info
        private string _expI_Note;

        private int _img_ID;

        // =======

        private ExpenseCategory _selectedExpenseCategory;
        public ExpenseCategory SelectedExpenseCategory
        {
            get => _selectedExpenseCategory;
            set
            {
                _selectedExpenseCategory = value;
                OnPropertyChanged();
            }
        }

        private FamilyExpenseViewModel _selectedItem;
        public FamilyExpenseViewModel SelectedItem
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
                    SelectedExpenseCategory = ExpCateg_List.FirstOrDefault(ec => ec.Ec_ID == SelectedItem.Ec_ID);
                    ExpI_Note = SelectedItem.ExpI_Note;
                    ImgPath = SelectedItem.ImgPath;
                }
            }
        }

        public int Exp_ID { get => _exp_ID; set { _exp_ID = value; OnPropertyChanged(); } }
        public string Exp_Name { get => _exp_Name; set { _exp_Name = value; OnPropertyChanged(); } }
        public DateTime? Exp_Date { get => _exp_Date; set { _exp_Date = value; OnPropertyChanged(); } }
        public int Exp_Price { get => _exp_Price; set { _exp_Price = value; OnPropertyChanged(); } }
        public int Ec_ID { get => _ec_ID; set { _ec_ID = value; OnPropertyChanged(); } }
        public int ExpI_ID { get => _expI_ID; set { _expI_ID = value; OnPropertyChanged(); } }
        public string ExpI_Note { get => _expI_Note; set { _expI_Note = value; OnPropertyChanged(); } }
        public string Ec_Name { get => _ec_Name; set { _ec_Name = value; OnPropertyChanged(); } }
        public int Img_ID { get => _img_ID; set { _img_ID = value; OnPropertyChanged(); } }


        private ObservableCollection<ExpenseCategory> _expCateg_List;
        private ObservableCollection<FamilyExpenseViewModel> _expenses_List;

        public ObservableCollection<ExpenseCategory> ExpCateg_List { get => _expCateg_List; set { _expCateg_List = value; OnPropertyChanged(); } }
        public ObservableCollection<FamilyExpenseViewModel> Expenses_List { get => _expenses_List; set { _expenses_List = value; OnPropertyChanged(); } }


        int currentMonth = DateTime.Now.Month;
        int currentYear = DateTime.Now.Year;

        public FamilyExpenseViewModel()
        {
            ExpCateg_List = new ObservableCollection<ExpenseCategory>(DataProvider.Instance.DB.ExpenseCategories);

            LoadWindowCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    //var resultQuery = from exp in DataProvider.Instance.DB.Expenses
                    //                  join expInfo in DataProvider.Instance.DB.ExpenseInfo on exp.Exp_ID equals expInfo.Exp_ID
                    //                  join expCate in DataProvider.Instance.DB.ExpenseCategories on expInfo.Ec_ID equals expCate.Ec_ID
                    //                  where exp.ExpType_ID == "et002" && exp.Exp_Date.Value.Month == 7
                    //                  select new FamilyExpenseViewModel()
                    //                  {
                    //                      Exp_Date = exp.Exp_Date,
                    //                      Exp_Name = exp.Exp_Name,
                    //                      Exp_Price = (int)exp.Exp_Price,
                    //                      ExpI_Note = expInfo.ExpI_Node,
                    //                      Ec_Name = expCate.Ec_Name
                    //                  };

                    var resultQuery = from exp in DataProvider.Instance.DB.Fml_Expenses
                                      join expInfo in DataProvider.Instance.DB.Fml_ExpenseInfo on exp.F_exp_ID equals expInfo.F_exp_ID into joinedExpenseInfo
                                      from expInfo in joinedExpenseInfo.DefaultIfEmpty()
                                      join expCate in DataProvider.Instance.DB.ExpenseCategories on expInfo.Ec_ID equals expCate.Ec_ID into joinedExpenseCate
                                      from expCate in joinedExpenseCate.DefaultIfEmpty()
                                      join img in DataProvider.Instance.DB.Images on exp.F_exp_ID equals img.Img_Relation_ID into joinedImages
                                      from img in joinedImages.DefaultIfEmpty()
                                      where exp.F_exp_Date.Value.Month == currentMonth
                                      select new FamilyExpenseViewModel()
                                      {
                                          Exp_ID = exp.F_exp_ID,
                                          Ec_ID = expCate != null ? expCate.Ec_ID : 0,
                                          ExpI_ID = expInfo != null ? expInfo.F_expI_ID : 0,
                                          Img_ID = img != null ? img.Img_ID : 0,

                                          Exp_Date = exp.F_exp_Date,
                                          Exp_Name = exp.F_exp_Name,
                                          Exp_Price = (int)exp.F_exp_Price,
                                          ExpI_Note = expInfo != null ? expInfo.F_expI_Note : string.Empty,
                                          Ec_Name = expCate != null ? expCate.Ec_Name : string.Empty,
                                          ImgPath = img != null ? img.Img_Url : string.Empty
                                      };

                    Expenses_List = new ObservableCollection<FamilyExpenseViewModel>(resultQuery);
                }
            );

            ExpAddCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (SelectedItem != null)
                        return false;

                    if (string.IsNullOrEmpty(Exp_Name) || Exp_Price == 0)
                        return false;

                    return true;
                },
                (param) =>
                {
                    if (string.IsNullOrEmpty(Exp_Name) || Exp_Price == 0 || Exp_Date == null)
                    {
                        MessageBox.Show("Phải điền đầy đủ các trường tên, giá và ngày tháng.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        var newExp = new Fml_Expenses()
                        {
                            F_exp_Name = Exp_Name,
                            F_exp_Date = Exp_Date,
                            F_exp_Price = Exp_Price
                        };

                        DataProvider.Instance.DB.Fml_Expenses.Add(newExp);
                        DataProvider.Instance.DB.SaveChanges();

                        // Lấy ra bản ghi mới nhất vừa ghi vào bảng Expenses
                        var newFmlExpense = DataProvider.Instance.DB.Fml_Expenses.OrderByDescending(e => e.F_exp_ID).FirstOrDefault();

                        if (newFmlExpense != null)
                        {
                            if (SelectedExpenseCategory != null)
                            {
                                var newExpInfo = new Fml_ExpenseInfo()
                                {
                                    F_expI_Note = ExpI_Note,
                                    Ec_ID = SelectedExpenseCategory.Ec_ID,
                                    F_exp_ID = newFmlExpense.F_exp_ID
                                };

                                DataProvider.Instance.DB.Fml_ExpenseInfo.Add(newExpInfo);
                                DataProvider.Instance.DB.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("Hãy chọn danh mục chi tiêu.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }

                            if (!string.IsNullOrEmpty(ImgPath))
                            {
                                var newImage = new Image()
                                {
                                    Img_Name = newFmlExpense.F_exp_Name,
                                    Img_Url = ImgPath,
                                    Img_Relation_ID = newFmlExpense.F_exp_ID,
                                    Img_Type = "Family Expense"
                                };

                                DataProvider.Instance.DB.Images.Add(newImage);
                                DataProvider.Instance.DB.SaveChanges();
                            }
                        }

                        var newFmlExpenseInfo = DataProvider.Instance.DB.Fml_ExpenseInfo.OrderByDescending(e => e.F_expI_ID).FirstOrDefault();
                        var newFmlExpenseImg = DataProvider.Instance.DB.Images.OrderByDescending(e => e.Img_ID).FirstOrDefault();

                        var lastResult = new FamilyExpenseViewModel()
                        {

                            Exp_ID = newFmlExpense.F_exp_ID,
                            Exp_Name = newFmlExpense.F_exp_Name,
                            Exp_Date = newFmlExpense.F_exp_Date,
                            Exp_Price = (int)newFmlExpense.F_exp_Price,
                            
                            Ec_ID = newFmlExpenseInfo != null ? newFmlExpenseInfo.Ec_ID : 0,
                            Ec_Name = newFmlExpenseInfo != null ? newFmlExpenseInfo.ExpenseCategory.Ec_Name : string.Empty,
                            ExpI_ID = newFmlExpenseInfo != null ? newFmlExpenseInfo.F_expI_ID : 0,
                            ExpI_Note = newFmlExpenseInfo != null ? newFmlExpenseInfo.F_expI_Note : string.Empty,

                            Img_ID = newFmlExpenseImg != null ? newFmlExpenseImg.Img_ID : 0,
                            ImgPath = newFmlExpenseImg != null ? newFmlExpenseImg.Img_Url : string.Empty
                        };

                        Expenses_List.Add(lastResult);

                        MessageBox.Show("Thêm thành công.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Làm trống các trường dữ liệu
                        Exp_Name = string.Empty;
                        Exp_Date = null;
                        Exp_Price = 0;
                        Ec_Name = string.Empty;
                        SelectedExpenseCategory = null;
                        ExpI_Note = string.Empty;
                        ImgPath = string.Empty;
                    }
                }

            );

            ExpUpdateCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (string.IsNullOrEmpty(Exp_Name) || Exp_Price == 0)
                        return false;

                    //var is_Exist_Item = DataProvider.Instance.DB.Expenses.Where(x => x.Exp_ID == SelectedItem.Exp_ID);

                    //if (is_Exist_Item == null || is_Exist_Item.Count() == 0)
                    //    return false;

                    return true;
                },
                (param) =>
                {
                    try
                    {
                        var Expense_Item = DataProvider.Instance.DB.Fml_Expenses.Where(x => x.F_exp_ID == SelectedItem.Exp_ID).SingleOrDefault();
                        var ExpenseInfo_Item = DataProvider.Instance.DB.Fml_ExpenseInfo.Where(x => x.F_expI_ID == SelectedItem.ExpI_ID).SingleOrDefault();
                        var Image_Item = DataProvider.Instance.DB.Images.Where(i => i.Img_ID == SelectedItem.Img_ID).SingleOrDefault();

                        if (Expense_Item == null)
                        {
                            MessageBox.Show("Không tồn tại bản ghi này.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            Expense_Item.F_exp_Name = Exp_Name;
                            Expense_Item.F_exp_Price = Exp_Price;
                            Expense_Item.F_exp_Date = Exp_Date;
                            DataProvider.Instance.DB.SaveChanges();


                            if (ExpenseInfo_Item == null)
                            {
                                var newExpInfo = new Fml_ExpenseInfo()
                                {
                                    F_expI_Note = ExpI_Note,
                                    Ec_ID = SelectedExpenseCategory.Ec_ID,
                                    F_exp_ID = SelectedItem.Exp_ID
                                };

                                DataProvider.Instance.DB.Fml_ExpenseInfo.Add(newExpInfo);
                                DataProvider.Instance.DB.SaveChanges();
                            }
                            else
                            {
                                ExpenseInfo_Item.F_expI_Note = ExpI_Note;
                                ExpenseInfo_Item.Ec_ID = SelectedExpenseCategory.Ec_ID;
                                DataProvider.Instance.DB.SaveChanges();
                            }

                            if (Image_Item == null)
                            {
                                var newImg = new Image()
                                {
                                    Img_Name = Expense_Item.F_exp_Date.Value.Day + Expense_Item.F_exp_Date.Value.Month + Expense_Item.F_exp_Date.Value.Year + "_" + Expense_Item.F_exp_ID + "_" + SelectedItem.Exp_Name,
                                    Img_Url = ImgPath,
                                    Img_Relation_ID = SelectedItem.Exp_ID,
                                    Img_Type = "Family Expense"
                                };

                                DataProvider.Instance.DB.Images.Add(newImg);
                                DataProvider.Instance.DB.SaveChanges();
                            } 
                            else
                            {
                                Image_Item.Img_Url = ImgPath;
                                DataProvider.Instance.DB.SaveChanges();
                            }

                            MessageBox.Show("Cập nhật thành công.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);

                            SelectedItem.Exp_Name = Exp_Name;
                            SelectedItem.Exp_Price = Exp_Price;
                            SelectedItem.Exp_Date = Exp_Date;
                            SelectedItem.ExpI_Note = ExpI_Note;
                            SelectedItem.Ec_Name = SelectedExpenseCategory.Ec_Name;

                        }

                        // Làm trống các trường dữ liệu
                        SelectedItem = null;
                        Exp_Name = string.Empty;
                        Exp_Date = null;
                        Exp_Price = 0;
                        Ec_Name = string.Empty;
                        SelectedExpenseCategory = null;
                        ExpI_Note = string.Empty;
                        ImgPath = string.Empty;
                    }
                    catch
                    {
                        MessageBox.Show("Có lỗi phát sinh, Không thể thực hiện hành động này.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            );

            ExpCancelCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (string.IsNullOrEmpty(Exp_Name) || Exp_Price == 0 || Exp_Date == null)
                        return false;

                    return true;
                },
                (param) =>
                {
                    SelectedItem = null;

                    Exp_Name = string.Empty;
                    Exp_Date = null;
                    Exp_Price = 0;
                    ExpI_Note = string.Empty;
                    SelectedExpenseCategory = null;
                    ImgPath = string.Empty;
                }
            );

            ExpDeleteCommand = new RelayCommand<Object>(
                (param) =>
                {
                    if (string.IsNullOrEmpty(Exp_Name) || Exp_Price == 0)
                        return false;

                    //var is_Exist_Item = DataProvider.Instance.DB.Expenses.Where(exp => exp.Exp_ID == SelectedItem.Exp_ID);

                    //if (is_Exist_Item == null || is_Exist_Item.Count() == 0)
                    //    return false;

                    return true;
                },
                (param) =>
                {
                    try
                    {
                        var exp = DataProvider.Instance.DB.Fml_Expenses.FirstOrDefault(e => e.F_exp_ID == SelectedItem.Exp_ID);
                        var expI = DataProvider.Instance.DB.Fml_ExpenseInfo.FirstOrDefault(e => e.F_expI_ID == SelectedItem.ExpI_ID);
                        var expImg = DataProvider.Instance.DB.Images.FirstOrDefault(e => e.Img_ID == SelectedItem.Img_ID);

                        if (exp != null)
                        {
                            if (expImg != null)
                            {
                                DataProvider.Instance.DB.Images.Remove(expImg);
                                DataProvider.Instance.DB.SaveChanges();
                            }

                            if (expI != null)
                            {
                                DataProvider.Instance.DB.Fml_ExpenseInfo.Remove(expI);
                                DataProvider.Instance.DB.SaveChanges();
                            }

                            DataProvider.Instance.DB.Fml_Expenses.Remove(exp);
                            DataProvider.Instance.DB.SaveChanges();

                            MessageBox.Show("Xóa thành công.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);

                            Expenses_List.Remove(SelectedItem);

                            // Làm trống các trường dữ liệu
                            SelectedItem = null;
                            Exp_Name = string.Empty;
                            Exp_Date = null;
                            Exp_Price = 0;
                            Ec_Name = string.Empty;
                            SelectedExpenseCategory = null;
                            ExpI_Note = string.Empty;
                            ImgPath = string.Empty;
                        }
                        else
                        {
                            MessageBox.Show("Xóa không thành công, Không tồn tại bản ghi này", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Có lỗi phát sinh.", "Thông báo.", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
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

                        ImgPath = destinationPath;
                    }
                }
            );
        }


    }
}
