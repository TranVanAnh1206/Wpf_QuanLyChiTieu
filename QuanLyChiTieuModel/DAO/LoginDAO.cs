using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieuModel.DAO
{
    public class LoginDAO
    {
        public int Login (string username, string password)
        {
            return DataProvider.Instance.DB.Accounts.Where(x => x.AccName == username && x.AccPassword == password).Count();
        }

        public int LoginByStoredProcedure(string username, string password)
        {
            try
            {
                var result = DataProvider.Instance.DB.Accounts.SqlQuery("exec USP_LOGIN '{0}', '{1}'", username, password);

                return 1;
            } 
            catch (Exception)
            {
                return -1;
            }            
        }
    }
}
