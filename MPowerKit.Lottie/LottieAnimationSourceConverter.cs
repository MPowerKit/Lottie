using System.ComponentModel;
using System.Globalization;

namespace MPowerKit.Lottie;

public abstract class StringTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        sourceType == typeof(string);

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
        destinationType == typeof(string);

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        ConvertFromStringCore(value?.ToString());

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) =>
        ConvertToStringCore(value);

    protected abstract object? ConvertFromStringCore(string? value);

    protected virtual string? ConvertToStringCore(object? value) => throw new NotImplementedException();
}

public class LottieAnimationSourceConverter : StringTypeConverter
{
    protected override object? ConvertFromStringCore(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(LottieAnimationSource)}");

        if (Uri.TryCreate(value, UriKind.Absolute, out var uri) && uri.Scheme != "file")
            return LottieAnimationSource.FromUri(uri);

        return LottieAnimationSource.FromFile(value!);
    }

    protected override string? ConvertToStringCore(object? value) =>
        value switch
        {
            FileLottieAnimationSource fas => fas.File,
            UriLottieAnimationSource uas => uas.Uri?.ToString(),
            _ => throw new NotSupportedException()
        };
}