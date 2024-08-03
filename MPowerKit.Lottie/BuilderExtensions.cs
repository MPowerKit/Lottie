namespace MPowerKit.Lottie;

public static class BuilderExtensions
{
    public static MauiAppBuilder UseMPowerKitLottie(this MauiAppBuilder builder)
    {
#if ANDROID || IOS || MACCATALYST
        builder.ConfigureMauiHandlers(hc => hc.AddHandler<LottieView, LottieViewHandler>());
#endif
        return builder;
    }
}