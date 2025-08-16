using Microsoft.UI.Xaml.Data;
using System;

namespace Kubix.Helpers
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double d)
                return d.ToString("N0");

            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
