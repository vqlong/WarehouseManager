using EntityDataAccess;
using Microsoft.EntityFrameworkCore;
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
    public class ItemViewModel : ViewModelBase
    {
        public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();
        public Item Current { get => GetProperty<Item>(); set => SetProperty(value == null ? new Item() : value.GetClone<Item>()); }
        public ICommand Insert { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }
        public ItemViewModel()
        {
            using var context = new WarehouseContext();
            Items = new ObservableCollection<Item>(context.Items.Where(u => u.IsDeleted == false).Include(i => i.Unit).Include(i => i.Suplier));

            Insert = new RelayCommand<Item>(
            u =>
            {
                if (Current is null ||
                string.IsNullOrWhiteSpace(Current.Name) ||
                Current.UnitId <= 0 ||
                Current.SuplierId <= 0) return false;
 
                if (Items.Any(u => u.Name.Equals(Current.Name))) return false;

                return true;
            },
            u =>
            {

                Current.Id = 0;
                Current.Unit = null;
                Current.Suplier = null;
                using var context = new WarehouseContext();
                context.Add(Current);
                context.SaveChanges();
                var item = context.Items.Include(i => i.Unit).Include(i => i.Suplier).FirstOrDefault(u => u.IsDeleted == false && u.Id == Current.Id);
                Items.Add(item.GetClone<Item>());
            });

            Update = new RelayCommand<Item>(
                u =>
                {
                    if (Current is null ||
                    string.IsNullOrWhiteSpace(Current.Name) ||
                    Current.UnitId <= 0 ||
                    Current.SuplierId <= 0 ||
                    Current.Id <= 0) return false;

                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    Current.Suplier = null;
                    Current.Unit = null;
                    context.Update(Current);
                    context.SaveChanges();

                    Current.Suplier = context.Find<Suplier>(Current.SuplierId);
                    Current.Unit = context.Find<Unit>(Current.UnitId);
                    ReloadModel(Items, Current);
                });

            Delete = new RelayCommand<Item>(
                u =>
                {
                    if (Current is null || Current.Id <= 0) return false;
                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    context.Entry(Current).State = EntityState.Deleted;
                    context.SaveChanges();

                    RemoveModel(Items, Current);
                });
        }
    }
}
