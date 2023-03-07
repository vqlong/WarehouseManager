using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Helper;

namespace ViewModel
{
    public class TitleBarViewModel : ViewModelBase
    {
        public new ICommand Close { get; } = new RelayCommand<Window>(w => w == null ? false : true, w => { if (AttachManager.GetShouldClose(w)) w.Close(); else w.Hide(); });
        public ICommand Minimize { get; } = new RelayCommand<Window>(w => w == null ? false : true, w => w.WindowState = WindowState.Minimized);
        public ICommand Maximize { get; } = new RelayCommand<Window>(w => w == null ? false : true, w => { if (w.WindowState == WindowState.Normal) w.WindowState = WindowState.Maximized; else w.WindowState = WindowState.Normal; });
        public new ICommand DragMove { get; } = new RelayCommand<Window>(w => w == null ? false : true, w => w.DragMove());
    }
}
