using System.Globalization;
using System.Windows.Data;

namespace axion.Converters;

public class EnumConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value?.ToString() == parameter?.ToString();

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value != null && (bool)value
            ? Enum.Parse(targetType, parameter?.ToString()!)
            : Binding.DoNothing;
}