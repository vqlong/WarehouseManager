using EntityDataAccess;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<InStock> InStocks { get; } = new ObservableCollection<InStock>();

        public MainViewModel()
        {
            LoadInStocks();
        }

        public void LoadInStocks()
        {
            using var context = new WarehouseContext();
            var stocks = context.Items.Where(i => i.IsDeleted == false).Select(i => new InStock { Id = i.Id, Name = i.Name }).ToList();
            InStocks.Clear();
            foreach (var stock in stocks)
            {
                stock.TotalInput = context.InputInfos.Where(i => i.ItemId == stock.Id && i.IsDeleted == false).Sum(i => i.Count);
                stock.TotalOutput = context.OutputInfos.Where(i => i.ItemId == stock.Id && i.IsDeleted == false).Sum(i => i.Count);
                stock.Count = stock.TotalInput - stock.TotalOutput;
                InStocks.Add(stock);
            }
            var view = CollectionViewSource.GetDefaultView(InStocks);
            view.Refresh();
        }
    }
}
