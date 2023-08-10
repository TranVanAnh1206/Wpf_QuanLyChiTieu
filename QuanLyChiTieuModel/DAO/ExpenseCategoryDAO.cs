using QuanLyChiTieuModel.EF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieuModel.DAO
{
    public class ExpenseCategoryDAO
    {
        public List<ExpenseCategories> GetAllExpenseCateg ()
        {
            return DataProvider.Instance.DB.ExpenseCategories.ToList();
        }
    }
}
