using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace WorkTimeSheet.Converters
{
    public class SecondsToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is long longValue)
                return TimeSpan.FromSeconds(longValue).ToString(@"dd\:hh\:mm\:ss");

            if (value is int intValue)
                return TimeSpan.FromSeconds(intValue).ToString(@"dd\:hh\:mm\:ss");

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
