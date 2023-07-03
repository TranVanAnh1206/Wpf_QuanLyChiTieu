using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_QuanLyChiTieu.Model;

namespace Wpf_QuanLyChiTieu.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLogin = false;

        private string _username;
        private string _password;

        public string Username { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            Username = "";
            Password = "";

            LoginCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    Login(param);
                }
            );

            CloseCommand = new RelayCommand<Window>(
                (param) => { return true; },
                (param) =>
                {
                    param.Close();
                }
            );

            PasswordChangedCommand = new RelayCommand<PasswordBox>(
                (param) => { return true; },
                (param) =>
                {
                    Password = param.Password;
                }
            );
        }

        private void Login(Window param)
        {
            if (param == null) return;

            var acc_Count = DataProvider.Instance.DB.Accounts.Where(acc => acc.AccName == Username && acc.AccPassword == Password).Count();

            if (acc_Count > 0)
            {
                IsLogin = true;
                param.Hide();
            }    
            else
            {
                IsLogin = false;
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
