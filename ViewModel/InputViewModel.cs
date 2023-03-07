using EntityDataAccess;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ViewModel
{
    public class InputViewModel : ViewModelBase
    {
        public ObservableCollection<Input> Inputs { get; } = new ObservableCollection<Input>();
        public string InputSerialNumber { get => GetProperty<string>(); set => SetProperty(value); }
        public DateTime? InputDate { get => GetProperty<DateTime?>(); set => SetProperty(value); }
        public string InputMoreInfo { get => GetProperty<string>(); set => SetProperty(value); }
        public Input Current { get => GetProperty<Input>(); set => SetProperty(value); }
        public InputInfo CurrentInputInfo { get => GetProperty<InputInfo>(); set => SetProperty(value == null ? new InputInfo() : value.GetClone<InputInfo>()); }
        public string TextSearch { get => GetProperty<string>(); set => SetProperty(value); }
        public ICommand InsertInput { get; }
        public ICommand SearchInput { get; }
        public ICommand Insert { get; }
        public ICommand Update { get; }
        public ICommand Delete { get; }
        public InputViewModel()
        { 
            using var context = new WarehouseContext();
            Inputs = new ObservableCollection<Input>(context.Inputs.Where(u => u.IsDeleted == false)
                                                                    .Include(i => i.InputInfos)
                                                                    .ThenInclude(ii => ii.Item));

            InsertInput = new RelayCommand<Input>(
            u =>
            { 
                return true;
            },
            u =>
            { 
                using var context = new WarehouseContext();
                var input = new Input();
                var guid = Guid.Empty;
                try
                {
                    if(!string.IsNullOrWhiteSpace(InputSerialNumber)) guid = new Guid(InputSerialNumber);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (guid != Guid.Empty) input.SerialNumber = guid;
                if (InputDate != null) input.Date = (DateTime)InputDate;
                input.MoreInfo = InputMoreInfo;
                context.Add(input);
                context.SaveChanges();
                Inputs.Add(input);
                InputSerialNumber = "";
                InputDate = null;
                InputMoreInfo = "";
            });

            SearchInput = new RelayCommand<ListView>(
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
                        foreach (Input input in lsv.Items)
                        {
                            if(input.SerialNumber == guid)
                            {
                                lsv.SelectedItem = input;
                                break;
                            }
                        }
                    }
                });

            Insert = new RelayCommand<MainViewModel>(
            m =>
            {
                if (Current is null ||
                CurrentInputInfo is null || 
                CurrentInputInfo.ItemId <= 0 || 
                CurrentInputInfo.Count <= 0 ||
                CurrentInputInfo.InputPrice <= 0) return false; 

                return true;
            },
            m =>
            {

                CurrentInputInfo.Id = 0;
                CurrentInputInfo.InputId = Current.Id;
                CurrentInputInfo.Item = null;
                CurrentInputInfo.Input = null;
                if (string.IsNullOrWhiteSpace(CurrentInputInfo.Status)) CurrentInputInfo.Status = "OK";
                using var context = new WarehouseContext();
                context.Add(CurrentInputInfo);
                context.SaveChanges();

                m.LoadInStocks();

                CurrentInputInfo.Item = context.Find<Item>(CurrentInputInfo.ItemId);
                CurrentInputInfo.Input = Current;
                Current.InputInfos.Add(CurrentInputInfo.GetClone<InputInfo>());

                var view = CollectionViewSource.GetDefaultView(Current.InputInfos);
                view.Refresh();
            });

            Update = new RelayCommand<InputInfo>(
                u =>
                {
                    if (Current is null ||
                    CurrentInputInfo is null || 
                    CurrentInputInfo.ItemId <= 0 ||
                    CurrentInputInfo.Count <= 0 ||
                    CurrentInputInfo.InputPrice <= 0) return false;

                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    CurrentInputInfo.Input = null;
                    CurrentInputInfo.Item = null;
                    context.Update(CurrentInputInfo);
                    context.SaveChanges();

                    context.Entry(CurrentInputInfo).State = EntityState.Detached;
                    var inputInfo = Current.InputInfos.FirstOrDefault(ii => ii.Id == CurrentInputInfo.Id);
                    context.Entry(inputInfo).Reload();
                    if (inputInfo.ItemId != inputInfo.Item.Id) inputInfo.Item = context.Find<Item>(inputInfo.ItemId);
                    var view = CollectionViewSource.GetDefaultView(Current.InputInfos); 
                    view.Refresh();
                });

            Delete = new RelayCommand<InputInfo>(
                u =>
                {
                    if (CurrentInputInfo is null || CurrentInputInfo.Id <= 0) return false;
                    return true;
                },
                u =>
                {
                    using var context = new WarehouseContext();
                    context.InputInfos.Where(ii => ii.Id == CurrentInputInfo.Id).ExecuteDelete();

                    var inputInfo = Current.InputInfos.FirstOrDefault(ii => ii.Id == CurrentInputInfo.Id);
                    Current.InputInfos.Remove(inputInfo);
                    var view = CollectionViewSource.GetDefaultView(Current.InputInfos);
                    view.Refresh();
                });
        }
    }
}
