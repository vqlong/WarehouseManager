using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [TypeConverter(typeof(RoleEnumConverter))]
    public enum Role
    {
        [Description("Nhân viên")]
        Staff,
        [Description("Quản lý")]
        Admin
    }

    public class RoleEnumConverter : EnumConverter
    {
        public RoleEnumConverter(Type type) : base(type)
        {
        }

        //public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        //{
        //    if (destinationType == typeof(string)) return true;

        //    return base.CanConvertTo(context, destinationType);
        //}

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                FieldInfo roleFieldInfo = typeof(Role).GetField(value.ToString());
                DescriptionAttribute[] descriptions = (DescriptionAttribute[])roleFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descriptions.Length > 0)
                    return descriptions[0].Description;
                return value.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
