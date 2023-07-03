using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf_QuanLyChiTieu.ViewModel
{
    public class CtrlBarViewModel : BaseViewModel
    {
        public ICommand CloseCommand { get; set; }
        public ICommand MinimizedCommand { get; set; }
        public ICommand MaximizedCommand { get; set; }
        public ICommand MoveWindowCommand { get; set; }

        public CtrlBarViewModel()
        {
            CloseCommand = new RelayCommand<UserControl>(
                (param) => { return param == null ? false : true; },
                (param) =>
                {
                    FrameworkElement window = GetParentElement(param);

                    var isWindow = window as Window;

                    if (isWindow != null)
                    {
                        isWindow.Close();
                    }
                }
            );

            MinimizedCommand = new RelayCommand<UserControl>(
                (param) => { return param == null ? false : true; },
                (param) =>
                {
                    FrameworkElement window = GetParentElement(param);
                    var isWindow = window as Window;

                    if (isWindow != null)
                    {
                        if (isWindow.WindowState != WindowState.Minimized)
                            isWindow.WindowState = WindowState.Minimized;
                    }    
                }
            );

            MaximizedCommand = new RelayCommand<UserControl>(
                (param) => { return param == null ? false : true; },
                (param) =>
                {
                    FrameworkElement window = GetParentElement(param);

                    var isWindow = window as Window;

                    if (isWindow != null)
                    {
                        if (isWindow.WindowState != WindowState.Maximized)
                            isWindow.WindowState = WindowState.Maximized;
                        else
                            isWindow.WindowState = WindowState.Normal;
                    }
                }
            );

            MoveWindowCommand = new RelayCommand<UserControl>(
                (param) => { return param == null ? false : true; },
                (param) =>
                {
                    FrameworkElement window = GetParentElement(param);

                    var isWindow = window as Window;

                    if (isWindow != null)
                    {
                        isWindow.DragMove();
                    }    
                }
            );
        }

        public FrameworkElement GetParentElement (FrameworkElement param)
        {
            FrameworkElement parent = param;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;

            }

            return parent;
        }
    }
}
