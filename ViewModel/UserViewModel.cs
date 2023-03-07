using EntityDataAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();
        public User Current { get => GetProperty<User>(); set => SetProperty(value == null ? new User() : value.GetClone<User>()); }
        public ICommand Insert { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }
        public ICommand ResetPassword { get; }
        public UserViewModel()
        {
            using var context = new WarehouseContext();
            Users = new ObservableCollection<User>(context.Users.Where(u => u.IsDeleted == false));

            Insert = new RelayCommand<User>(
            u =>
            {
                if (Current is null ||
                string.IsNullOrWhiteSpace(Current.Username) ||
                string.IsNullOrWhiteSpace(Current.DisplayName)) return false;

                if (Users.Any(u => u.Username.Equals(Current.Username))) return false;

                return true;
            },
            u =>
            {

                var user = new User { Username = Current.Username };
                using var context = new WarehouseContext();
                context.Add(user);
                context.SaveChanges();
                Users.Add(user);
            });

            Update = new RelayCommand<User>(
                u =>
                {
                    if (Current is null ||
                    string.IsNullOrWhiteSpace(Current.Username) ||
                    string.IsNullOrWhiteSpace(Current.DisplayName) ||
                    Current.Id <= 0) return false;

                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    var entry = context.Entry(Current);
                    entry.Property(u => u.Role).IsModified = true;
                    entry.Property(u => u.DisplayName).IsModified = true;
                    context.SaveChanges();

                    ReloadModel(Users, Current);
                });

            Delete = new RelayCommand<User>(
                u =>
                {
                    if (Current is null || Current.Id <= 0) return false;
                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    context.Entry(Current).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    context.SaveChanges();

                    RemoveModel(Users, Current);
                });

            ResetPassword = new RelayCommand<User>(
                u =>
                {
                    if (Current is null ||  Current.Id <= 0) return false;

                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    Current.Password = "123";
                    var entry = context.Entry(Current).Property(u => u.Password).IsModified = true;
                    context.SaveChanges();
                    MessageBox.Show($"Đặt lại mật khẩu cho tài khoản {Current.Username} thành công."); 
                });
        }
    }
}
