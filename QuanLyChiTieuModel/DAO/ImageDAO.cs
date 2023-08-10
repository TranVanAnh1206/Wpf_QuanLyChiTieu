using QuanLyChiTieuModel.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChiTieuModel.DAO
{
    public class ImageDAO
    {
        public int AddNewImage(Images img)
        {
            DataProvider.Instance.DB.Images.Add(img);
            DataProvider.Instance.DB.SaveChanges();

            return img.Img_ID;
        }

        public Images GetImageByID (int id)
        {
            return DataProvider.Instance.DB.Images.Where(x => x.Img_ID == id).FirstOrDefault();
        }

        public bool Update(int id, string path)
        {
            var img = DataProvider.Instance.DB.Images.FirstOrDefault(x => x.Img_ID == id);

            if (img != null )
            {
                img.Img_Url = path;
                DataProvider.Instance.DB.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            var img = DataProvider.Instance.DB.Images.FirstOrDefault(x => x.Img_ID == id);

            if (img != null)
            {
                DataProvider.Instance.DB.Images.Remove(img);
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
