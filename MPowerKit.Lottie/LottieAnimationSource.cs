using System.ComponentModel;

namespace MPowerKit.Lottie;

[TypeConverter(typeof(LottieAnimationSourceConverter))]
public abstract class LottieAnimationSource : Element
{
    private readonly WeakEventManager _weakEventManager = new();

    public virtual bool IsEmpty => true;

    public abstract Task<object> LoadAnimationAsync(CancellationToken cancellationToken = default);

    public static LottieAnimationSource FromUri(Uri uri) =>
        new UriLottieAnimationSource { Uri = uri };

    public static LottieAnimationSource FromFile(string file) =>
        new FileLottieAnimationSource { File = file };

    public static LottieAnimationSource FromJson(string json) =>
        new JsonLottieAnimationSource { Json = json };

    public static LottieAnimationSource FromStream(Func<CancellationToken, Task<Stream?>> getter) =>
        new StreamLottieAnimationSource { Stream = getter };

    public static LottieAnimationSource FromStream(Stream stream) =>
        FromStream(token => Task.FromResult<Stream?>(stream));

    public event EventHandler SourceChanged
    {
        add => _weakEventManager.AddEventHandler(value);
        remove => _weakEventManager.RemoveEventHandler(value);
    }

    protected static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is LottieAnimationSource source)
            source._weakEventManager.HandleEvent(source, EventArgs.Empty, nameof(SourceChanged));
    }

    public static implicit operator LottieAnimationSource(string source)
    {
        return Uri.TryCreate(source, UriKind.Absolute, out var uri) && uri.Scheme != "file"
            ? FromUri(uri)
            : FromFile(source);
    }

    public static implicit operator LottieAnimationSource(Uri uri)
    {
        if (uri is null) return null;
        if (!uri.IsAbsoluteUri) throw new ArgumentException("uri is relative");

        return FromUri(uri);
    }
}

public class FileLottieAnimationSource : LottieAnimationSource
{
    public override Task<object> LoadAnimationAsync(CancellationToken cancellationToken = default)
    {
        if (IsEmpty) throw new InvalidOperationException("File is empty.");

        return Task.FromResult<object>(File!);
    }

    public override bool IsEmpty =>
        string.IsNullOrWhiteSpace(File);

    public string? File
    {
        get => (string?)GetValue(FileProperty);
        set => SetValue(FileProperty, value);
    }

    public static readonly BindableProperty FileProperty =
        BindableProperty.Create(
            nameof(File),
            typeof(string),
            typeof(FileLottieAnimationSource),
            propertyChanged: OnSourceChanged);
}

public class JsonLottieAnimationSource : LottieAnimationSource
{
    public override Task<object> LoadAnimationAsync(CancellationToken cancellationToken = default)
    {
        if (IsEmpty) throw new InvalidOperationException("Json is empty.");

        return Task.FromResult<object>(Json!);
    }

    public override bool IsEmpty =>
        string.IsNullOrWhiteSpace(Json);

    public string? Json
    {
        get => (string?)GetValue(JsonProperty);
        set => SetValue(JsonProperty, value);
    }

    public static readonly BindableProperty JsonProperty =
        BindableProperty.Create(
            nameof(Json),
            typeof(string),
            typeof(JsonLottieAnimationSource),
            propertyChanged: OnSourceChanged);
}

public class UriLottieAnimationSource : LottieAnimationSource
{
    public override Task<object> LoadAnimationAsync(CancellationToken cancellationToken = default)
    {
        if (IsEmpty) throw new InvalidOperationException("Uri is empty.");

        return Task.FromResult<object>(Uri!.AbsolutePath);
    }

    public override bool IsEmpty => Uri is null;

    public Uri? Uri
    {
        get => (Uri?)GetValue(UriProperty);
        set => SetValue(UriProperty, value);
    }

    public static readonly BindableProperty UriProperty =
        BindableProperty.Create(
            nameof(Uri),
            typeof(Uri),
            typeof(UriLottieAnimationSource),
            propertyChanged: OnSourceChanged);
}

public class StreamLottieAnimationSource : LottieAnimationSource
{
    public override async Task<object> LoadAnimationAsync(CancellationToken cancellationToken = default)
    {
        if (IsEmpty) throw new InvalidOperationException("Stream is empty.");

        var stream = await Stream!.Invoke(cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Stream is null.");
        return stream;
    }

    public override bool IsEmpty => Stream is null;

    public Func<CancellationToken, Task<Stream?>>? Stream
    {
        get => (Func<CancellationToken, Task<Stream?>>?)GetValue(StreamProperty);
        set => SetValue(StreamProperty, value);
    }

    public static readonly BindableProperty StreamProperty =
        BindableProperty.Create(
            nameof(Stream),
            typeof(Func<CancellationToken, Task<Stream?>>),
            typeof(StreamLottieAnimationSource),
            propertyChanged: OnSourceChanged);
}