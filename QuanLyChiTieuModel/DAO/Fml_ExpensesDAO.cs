using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyChiTieuModel.EF;

namespace QuanLyChiTieuModel.DAO
{
    public class Fml_ExpensesDAO
    {
        public int AddNewFmlExpense(Fml_Expenses fe)
        {
            DataProvider.Instance.DB.Fml_Expenses.Add(fe);
            DataProvider.Instance.DB.SaveChanges();

            return fe.F_exp_ID;
        }

        public Fml_Expenses GetFml_ExpensesByID(int id)
        {
            return DataProvider.Instance.DB.Fml_Expenses.Where(x => x.F_exp_ID == id).FirstOrDefault();
        }

        public bool Update (int id, string name, int price, DateTime? date)
        {
            var fe = DataProvider.Instance.DB.Fml_Expenses.FirstOrDefault(x => x.F_exp_ID == id);

            if (fe != null)
            {
                fe.F_exp_Name = name;
                fe.F_exp_Price = price;
                fe.F_exp_Date = date;

                DataProvider.Instance.DB.SaveChanges();

                return true;
            } 
            else
            {
                return false;
            }
        }

        public bool Dalete(int id)
        {
            var fe = DataProvider.Instance.DB.Fml_Expenses.FirstOrDefault(x => x.F_exp_ID == id);

            if (fe != null)
            {
                DataProvider.Instance.DB.Fml_Expenses.Remove(fe);
                DataProvider.Instance.DB.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public decimal? GetSumExpenseMonth (int month, int year)
        {
            return DataProvider.Instance.DB.Fml_Expenses.Where(x => x.F_exp_Date.Value.Month == month && x.F_exp_Date.Value.Year == year).Sum(x => (decimal?)x.F_exp_Price) ?? 0;
        }

    }
}
