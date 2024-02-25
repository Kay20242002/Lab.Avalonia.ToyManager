using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Converters;

public class EnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null || !value.GetType().IsEnum)
            return AvaloniaProperty.UnsetValue;

        var fieldInfo = value.GetType().GetField(value.ToString());
        var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

        if (descriptionAttribute != null)
            return descriptionAttribute.Description;

        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null || targetType == null || !targetType.IsEnum)
            return AvaloniaProperty.UnsetValue;

        foreach (var field in targetType.GetFields())
        {
            var descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (descriptionAttribute != null && descriptionAttribute.Description == (string)value)
                return field.GetValue(null);
            if (field.Name == (string)value)
                return field.GetValue(null);
        }

        return AvaloniaProperty.UnsetValue;
    }
}
