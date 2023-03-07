using EntityDataAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public class UnitViewModel : ViewModelBase
    {
        public ObservableCollection<Unit> Units { get; } = new ObservableCollection<Unit>();
        public Unit Current { get => GetProperty<Unit>(); set => SetProperty(value == null ? new Unit() : value.GetClone<Unit>()); }
        public ICommand Insert { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }
        public UnitViewModel()
        { 

            using var context = new WarehouseContext();
            Units = new ObservableCollection<Unit>(context.Units.Where(u => u.IsDeleted == false));

            Insert = new RelayCommand<Unit>(
            u =>
            {
                if (Current is null || string.IsNullOrWhiteSpace(Current.Name)) return false;

                if (Units.Any(u => u.Name.Equals(Current.Name))) return false;
                return true;
            },
            u =>
            {
                var unit = new Unit { Name = Current.Name };
                using var context = new WarehouseContext();
                context.Add(unit);
                context.SaveChanges();
                Units.Add(unit);
            });

            Update = new RelayCommand<Unit>(
                u =>
                {
                    if (Current is null || string.IsNullOrWhiteSpace(Current.Name) || Current.Id <= 0) return false;

                    if (Units.Any(u => u.Name.Equals(Current.Name))) return false;
                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    context.Update(Current);
                    context.SaveChanges();

                    ReloadModel(Units, Current); 
                });

            Delete = new RelayCommand<Unit>(
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

                    RemoveModel(Units, Current);
                });
        }
    }
}
