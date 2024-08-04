using System.ComponentModel;
using System.Globalization;

namespace MPowerKit.Lottie;

public class LottieAnimationSourceConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var text = value?.ToString();

        if (string.IsNullOrWhiteSpace(text))
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(LottieAnimationSource)}");

        if (Uri.TryCreate(text, UriKind.Absolute, out var uri) && uri.Scheme != "file")
            return LottieAnimationSource.FromUri(uri);

        return LottieAnimationSource.FromFile(text!);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return value switch
        {
            FileLottieAnimationSource fas => fas.File,
            UriLottieAnimationSource uas => uas.Uri?.ToString(),
            _ => throw new NotSupportedException()
        };
    }
}