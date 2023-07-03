using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf_QuanLyChiTieu
{
    /// <summary>
    /// Interaction logic for PersonalExpensesWindow.xaml
    /// </summary>
    public partial class PersonalExpensesWindow : Window
    {
        public PersonalExpensesWindow()
        {
            InitializeComponent();
        }

        private void ToUpperFirstChar(TextBox txt)
        {
            if (!string.IsNullOrEmpty(txt.Text))
            {
                string firstChar = txt.Text.Substring(0, 1);
                if (firstChar != firstChar.ToUpper())
                {
                    txt.Text = firstChar.ToUpper() + txt.Text.Substring(1);
                    txt.SelectionStart = txt.Text.Length; // Đặt con trỏ văn bản tại cuối TextBox
                }
            }
        }

        private void perExp_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToUpperFirstChar(perExp_Name);
        }

        private void perExp_Note_TextChanged(object sender, TextChangedEventArgs e)
        {
            ToUpperFirstChar(perExp_Note);
        }
    }
}
