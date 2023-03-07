using System;
using System.Windows;

namespace Helper
{
    public class AttachManager
    {
        public static readonly DependencyProperty ShouldCloseProperty =
            DependencyProperty.RegisterAttached("ShouldClose", typeof(bool), typeof(AttachManager), new PropertyMetadata(false));

        public static bool GetShouldClose(DependencyObject d) => (bool)d.GetValue(ShouldCloseProperty);
        public static void SetShouldClose(DependencyObject d, object value) => d.SetValue(ShouldCloseProperty, value);  
    }
}
