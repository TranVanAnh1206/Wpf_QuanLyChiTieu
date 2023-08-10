using QuanLyChiTieuModel.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieuModel.DAO
{
    public class Psn_ExpenseInfoDAO
    {
        public Psn_ExpenseInfo GetExpenseInfoByID (int id)
        {
            return DataProvider.Instance.DB.Psn_ExpenseInfo.Where(x => x.P_expI_ID == id).FirstOrDefault();
        }

        public int AddNewExpenseInfo(Psn_ExpenseInfo pei)
        {
            try
            {
                DataProvider.Instance.DB.Psn_ExpenseInfo.Add(pei);
                DataProvider.Instance.DB.SaveChanges();

                return pei.P_expI_ID;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Update (int id, string note, int ecID)
        {
            try
            {
                var pei = DataProvider.Instance.DB.Psn_ExpenseInfo.Where(x => x.P_expI_ID == id).FirstOrDefault();

                pei.Ec_ID = ecID;
                pei.P_expI_Note = note;
                DataProvider.Instance.DB.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete (int id)
        {
            try
            {
                var pei = DataProvider.Instance.DB.Psn_ExpenseInfo.FirstOrDefault(x => x.P_expI_ID == id);

                DataProvider.Instance.DB.Psn_ExpenseInfo.Remove(pei);
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
