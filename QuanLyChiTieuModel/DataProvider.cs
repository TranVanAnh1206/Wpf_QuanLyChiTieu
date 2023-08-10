using QuanLyChiTieuModel.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieuModel
{
    public class DataProvider
    {
        private static DataProvider _instance;

        public static DataProvider Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataProvider();
                }

                return _instance;
            }
            set => _instance = value; 
        }

        public QuanLiChiTieu_V2Entities DB { get; set; }

        private DataProvider()
        {
            DB = new QuanLiChiTieu_V2Entities();
        }
    }
}
