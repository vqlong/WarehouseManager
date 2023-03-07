using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helper
{
    public static class Extension
    {
        public static T? FindAncestor<T>(this FrameworkElement element)
        {
            var parent = element.Parent;
            while (parent is not null)
            {
                if (parent is T t) return t;
                if (parent is FrameworkElement e) parent = e.Parent;
                else return default;
            }
            return default;
        }

        public static string ToSHA256(this string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var sha256bytes = SHA256.Create().ComputeHash(bytes);

            var output = "";
            foreach (var item in sha256bytes)
            {
                output += Convert.ToString(item);
            }

            return output;
        }
    }
}
