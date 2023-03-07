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
    public class SuplierViewModel : ViewModelBase
    {
        public ObservableCollection<Suplier> Supliers { get; } = new ObservableCollection<Suplier>();
        public Suplier Current { get => GetProperty<Suplier>(); set => SetProperty(value == null ? new Suplier() : value.GetClone<Suplier>()); }
        public ICommand Insert { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }
        public SuplierViewModel()
        {
            using var context = new WarehouseContext();
            Supliers = new ObservableCollection<Suplier>(context.Supliers.Where(u => u.IsDeleted == false));

            Insert = new RelayCommand<Suplier>(
            u =>
            {
                if (Current is null || 
                string.IsNullOrWhiteSpace(Current.Name) || 
                string.IsNullOrWhiteSpace(Current.Address) || 
                string.IsNullOrWhiteSpace(Current.Phone) || 
                string.IsNullOrWhiteSpace(Current.Email) || 
                Current.ContractDate == DateTime.MinValue) return false;
 
                if ( Supliers.Any(u => u.Name.Equals(Current.Name))) return false;

                return true;
            },
            u =>
            {

                Current.Id = 0;
                using var context = new WarehouseContext();
                context.Add(Current);
                context.SaveChanges();
                Supliers.Add(Current.GetClone<Suplier>()); 
            });

            Update = new RelayCommand<Suplier>(
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

                    //context.Entry(Current).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    //context.Entry(Supliers.SingleOrDefault(s => s.Id == Current.Id)).Reload();
                    ReloadModel(Supliers, Current);
                });

            Delete = new RelayCommand<Suplier>(
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

                    RemoveModel(Supliers, Current);
                });
        }
    }
}
