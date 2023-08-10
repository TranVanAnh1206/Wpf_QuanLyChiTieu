using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_QuanLyChiTieu.Model
{
    public class Categories
    {
        public Categories(string Category)
        {
            this.Category = Category;
        }

        public string Category { get; set; }
    }
}
