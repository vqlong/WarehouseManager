using EntityDataAccess;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;

namespace ViewModel
{
    public class OutputViewModel : ViewModelBase
    {
        public ObservableCollection<Output> Outputs { get; } = new ObservableCollection<Output>();
        public string OutputSerialNumber { get => GetProperty<string>(); set => SetProperty(value); }
        public DateTime? OutputDate { get => GetProperty<DateTime?>(); set => SetProperty(value); }
        public int CustomerId { get => GetProperty<int>(); set => SetProperty(value); }
        public string OutputMoreInfo { get => GetProperty<string>(); set => SetProperty(value); }
        public Output Current { get => GetProperty<Output>(); set => SetProperty(value); }
        public OutputInfo CurrentOutputInfo { get => GetProperty<OutputInfo>(); set => SetProperty(value == null ? new OutputInfo() : value.GetClone<OutputInfo>()); }
        public string TextSearch { get => GetProperty<string>(); set => SetProperty(value); }
        public ICommand InsertOutput { get; }
        public ICommand SearchOutput { get; }
        public ICommand Insert { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }
        public OutputViewModel()
        {
            using var context = new WarehouseContext();
            Outputs = new ObservableCollection<Output>(context.Outputs.Where(o => o.IsDeleted == false) 
                                                                    .Include(o => o.OutputInfos)
                                                                    .ThenInclude(oi => oi.Item)
                                                                    .Include(o => o.Customer));


            InsertOutput = new RelayCommand<Output>(
            u =>
            {
                if (CustomerId <= 0) return false;
                return true;
            },
            u =>
            {
                using var context = new WarehouseContext();
                var output = new Output();
                var guid = Guid.Empty;
                try
                {
                    if (!string.IsNullOrWhiteSpace(OutputSerialNumber)) guid = new Guid(OutputSerialNumber);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (guid != Guid.Empty) output.SerialNumber = guid;
                if (OutputDate != null) output.Date = (DateTime)OutputDate;
                output.CustomerId = CustomerId;
                output.MoreInfo = OutputMoreInfo;
                context.Add(output);
                context.SaveChanges();
                output.Customer = context.Find<Customer>(output.CustomerId);
                Outputs.Add(output);
                OutputSerialNumber = "";
                OutputDate = null;
                CustomerId = 0;
                OutputMoreInfo = "";
            });

            SearchOutput = new RelayCommand<ListView>(
                lsv =>
                {
                    return true;
                },
                lsv =>
                {
                    var guid = Guid.Empty;
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(TextSearch)) guid = new Guid(TextSearch);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    if (guid != Guid.Empty)
                    {
                        foreach (Output output in lsv.Items)
                        {
                            if (output.SerialNumber == guid)
                            {
                                lsv.SelectedItem = output;
                                break;
                            }
                        }
                    }
                });

            Insert = new RelayCommand<MainViewModel>(
            m =>
            {
                if (Current is null ||
                CurrentOutputInfo is null || 
                CurrentOutputInfo.ItemId <= 0 ||
                CurrentOutputInfo.Count <= 0 || 
                CurrentOutputInfo.OutputPrice <= 0) return false;

                var stock = m.InStocks.FirstOrDefault(i => i.Id == CurrentOutputInfo.ItemId);
                if (stock is not null && stock.Count < CurrentOutputInfo.Count) return false;

                return true;
            },
            m =>
            {

                CurrentOutputInfo.Id = 0;
                CurrentOutputInfo.OutputId = Current.Id;
                CurrentOutputInfo.Item = null;
                CurrentOutputInfo.Output = null;
                if (string.IsNullOrWhiteSpace(CurrentOutputInfo.Status)) CurrentOutputInfo.Status = "OK";
                using var context = new WarehouseContext();
                context.Add(CurrentOutputInfo);
                context.SaveChanges();

                m.LoadInStocks();

                CurrentOutputInfo.Item = context.Find<Item>(CurrentOutputInfo.ItemId); 
                CurrentOutputInfo.Output = Current;
                Current.OutputInfos.Add(CurrentOutputInfo.GetClone<OutputInfo>());

                var view = CollectionViewSource.GetDefaultView(Current.OutputInfos);
                view.Refresh();
            });

            Update = new RelayCommand<InputInfo>(
                u =>
                {
                    if (Current is null ||
                    CurrentOutputInfo is null || 
                    CurrentOutputInfo.ItemId <= 0 ||
                    CurrentOutputInfo.Count <= 0 || 
                    CurrentOutputInfo.OutputPrice <= 0) return false;

                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    CurrentOutputInfo.Output = null;
                    CurrentOutputInfo.Item = null; 
                    context.Update(CurrentOutputInfo);
                    context.SaveChanges();

                    context.Entry(CurrentOutputInfo).State = EntityState.Detached;
                    var outputInfo = Current.OutputInfos.FirstOrDefault(oi => oi.Id == CurrentOutputInfo.Id);
                    context.Entry(outputInfo).Reload();
                    if (outputInfo.ItemId != outputInfo.Item.Id) outputInfo.Item = context.Find<Item>(outputInfo.ItemId); 
                    var view = CollectionViewSource.GetDefaultView(Current.OutputInfos);
                    view.Refresh();
                });

            Delete = new RelayCommand<OutputInfo>(
                u =>
                {
                    if (CurrentOutputInfo is null || CurrentOutputInfo.Id <= 0) return false;
                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    context.OutputInfos.Where(oi => oi.Id == CurrentOutputInfo.Id).ExecuteDelete();

                    var outputInfo = Current.OutputInfos.FirstOrDefault(oi => oi.Id == CurrentOutputInfo.Id);
                    Current.OutputInfos.Remove(outputInfo);
                    var view = CollectionViewSource.GetDefaultView(Current.OutputInfos);
                    view.Refresh();
                });
        }
    }
}
