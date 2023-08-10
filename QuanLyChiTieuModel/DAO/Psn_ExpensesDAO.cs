using QuanLyChiTieuModel.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieuModel.DAO
{
    public class Psn_ExpensesDAO
    {
        public decimal? GetSumExpenseMonth(int currentMonth, int currentYear)
        {
            return DataProvider.Instance.DB.Psn_Expenses.Where(x => x.P_exp_Date.Value.Month == currentMonth && x.P_exp_Date.Value.Year == currentYear).Sum(x => (decimal?)x.P_exp_Price) ?? 0;
        }

        public Psn_Expenses GetExpenseByID (int id)
        {
            return DataProvider.Instance.DB.Psn_Expenses.Where(x => x.P_exp_ID == id).FirstOrDefault();
        }

        public int AddNewPsnExpense (Psn_Expenses pe)
        {
            try
            {
                DataProvider.Instance.DB.Psn_Expenses.Add(pe);
                DataProvider.Instance.DB.SaveChanges();

                return pe.P_exp_ID;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public Psn_Expenses GetNewPsnExpense ()
        {
            return DataProvider.Instance.DB.Psn_Expenses.Take(1).OrderByDescending(x => x.P_exp_ID).FirstOrDefault();
        }

        public bool Update (int id, string name, int price, DateTime? date)
        {
            try
            {
                var pe = DataProvider.Instance.DB.Psn_Expenses.Where(x => x.P_exp_ID == id).FirstOrDefault();

                pe.P_exp_Name = name;
                pe.P_exp_Price = price;
                pe.P_exp_Date = date;
                DataProvider.Instance.DB.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var pe = DataProvider.Instance.DB.Psn_Expenses.FirstOrDefault(x => x.P_exp_ID == id);

                DataProvider.Instance.DB.Psn_Expenses.Remove(pe);
                DataProvider.Instance.DB.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
