using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf_QuanLyChiTieu.ViewModel;

namespace Wpf_QuanLyChiTieu.UserControls
{
    /// <summary>
    /// Interaction logic for CtrlBar_UC.xaml
    /// </summary>
    public partial class CtrlBar_UC : UserControl
    {
        public CtrlBarViewModel ctrlbarVM { get; set; }

        public CtrlBar_UC()
        {
            InitializeComponent();

            this.DataContext = ctrlbarVM = new CtrlBarViewModel();
        }
    }
}
