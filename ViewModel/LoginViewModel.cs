using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using Helper;
using EntityDataAccess;

namespace ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            Login = new RelayCommand<PasswordBox>(
                        p => !(p is null || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(p.Password)),
                        p => 
                        {
                            using var context = new WarehouseContext();
                            var user = context.Users.FirstOrDefault(u => u.Username == Username && u.Password == p.Password && u.IsDeleted == false);
                            if (user != null)
                            {
                                IsLogin = true;
                                IsLogout = false;
                                var w = p.FindAncestor<Window>();
                                if (w is not null) w.Hide();
                            }
                            else
                            {
                                IsLogin = false;
                                IsLogout = true;
                                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.");
                            }
                            p.Clear();
                            Username = "";

                        });
        }
        bool _isLogin = false;
        public bool IsLogin { get => _isLogin; private set => OnPropertyChanged(ref _isLogin, value); }
        bool _isLogout = true;
        public bool IsLogout { get => _isLogout; private set => OnPropertyChanged(ref _isLogout, value); }
        string _username = ""; 
        public string Username { get => _username; set => OnPropertyChanged(ref _username, value); }
        public ICommand Login { get; } 
    }
}
