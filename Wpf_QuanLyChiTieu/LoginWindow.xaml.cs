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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wpf_QuanLyChiTieu
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Loaded += LoginWindow_Loaded;
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateWindowOpen();
        }

        private void AnimateWindowOpen()
        {
            // Tạo một DoubleAnimation để thay đổi giá trị Opacity của Grid từ 0 thành 1 trong khoảng thời gian 0.5 giây
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.7)
            };

            // Gán animation cho thuộc tính Opacity của Grid
            rootGrid.BeginAnimation(Grid.OpacityProperty, animation);
        }
    }
}
