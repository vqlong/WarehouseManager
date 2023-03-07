using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        Dictionary<string, object> _values = new Dictionary<string, object>();
        protected virtual void SetProperty(object value, [CallerMemberName] string propertyName = "")
        {
            _values[propertyName] = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual T? GetProperty<T>([CallerMemberName] string propertyName = "")
        {
            if (_values.ContainsKey(propertyName))
                return (T)_values[propertyName];
            else
                return default;

        }
        protected virtual void OnPropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void ReloadModel<T>(ObservableCollection<T> collection, T model) where T : Model.Model
        {
            var oldModel = collection.FirstOrDefault(t => t.Id == model.Id);
            var index = collection.IndexOf(oldModel); 
            collection.RemoveAt(index);
            collection.Insert(index, model);
        }
        protected virtual void RemoveModel<T>(ObservableCollection<T> collection, T model) where T : Model.Model
        {
            var oldModel = collection.FirstOrDefault(t => t.Id == model.Id); 
            collection.Remove(oldModel); 
        }
        public ICommand ShowDialog { get; } = new RelayCommand<Window>(w => w != null, w => w.ShowDialog());
    }
}
