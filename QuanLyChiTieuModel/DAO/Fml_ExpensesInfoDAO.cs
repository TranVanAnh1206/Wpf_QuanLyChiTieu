using QuanLyChiTieuModel.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieuModel.DAO
{
    public class Fml_ExpensesInfoDAO
    {
        public int AddNewExpenseInfo (Fml_ExpenseInfo fei)
        {
            DataProvider.Instance.DB.Fml_ExpenseInfo.Add(fei);
            DataProvider.Instance.DB.SaveChanges();

            return fei.F_expI_ID;
        }

        public Fml_ExpenseInfo GetExpenseInfoByID (int id)
        {
            return DataProvider.Instance.DB.Fml_ExpenseInfo.Where(x => x.F_expI_ID == id).FirstOrDefault();
        }

        public bool Update(int id, string note, int ecID)
        {
            var fei = DataProvider.Instance.DB.Fml_ExpenseInfo.FirstOrDefault(x => x.F_expI_ID == id);

            if (fei != null)
            {
                fei.F_expI_Note = note;
                fei.Ec_ID = ecID;
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
            var fei = DataProvider.Instance.DB.Fml_ExpenseInfo.FirstOrDefault(x => x.F_expI_ID == id);

            if (fei != null)
            {
                DataProvider.Instance.DB.Fml_ExpenseInfo.Remove(fei);
                DataProvider.Instance.DB.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
