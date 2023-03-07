using EntityDataAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();
        public Customer Current { get => GetProperty<Customer>(); set => SetProperty(value == null ? new Customer() : value.GetClone<Customer>()); }
        public ICommand Insert { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }
        public CustomerViewModel()
        {
            using var context = new WarehouseContext();
            Customers = new ObservableCollection<Customer>(context.Customers.Where(u => u.IsDeleted == false));

            Insert = new RelayCommand<Customer>(
            u =>
            {
                if (Current is null ||
                string.IsNullOrWhiteSpace(Current.Name) ||
                string.IsNullOrWhiteSpace(Current.Address) ||
                string.IsNullOrWhiteSpace(Current.Phone) ||
                string.IsNullOrWhiteSpace(Current.Email) ||
                Current.ContractDate == DateTime.MinValue) return false;

                if (Customers.Any(u => u.Name.Equals(Current.Name))) return false;

                return true;
            },
            u =>
            {

                Current.Id = 0;
                using var context = new WarehouseContext();
                context.Add(Current);
                context.SaveChanges();
                Customers.Add(Current.GetClone<Customer>());
            });

            Update = new RelayCommand<Customer>(
                u =>
                {
                    if (Current is null ||
                    string.IsNullOrWhiteSpace(Current.Name) ||
                    string.IsNullOrWhiteSpace(Current.Address) ||
                    string.IsNullOrWhiteSpace(Current.Phone) ||
                    string.IsNullOrWhiteSpace(Current.Email) ||
                    Current.ContractDate == DateTime.MinValue ||
                    Current.Id <= 0) return false;

                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    context.Update(Current);
                    context.SaveChanges();

                    ReloadModel(Customers, Current);
                });

            Delete = new RelayCommand<Customer>(
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

                    RemoveModel(Customers, Current);
                });
        }
    }
}
