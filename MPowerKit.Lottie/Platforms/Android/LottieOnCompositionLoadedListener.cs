using Com.Airbnb.Lottie;

namespace MPowerKit.Lottie;

public class LottieOnCompositionLoadedListener : Java.Lang.Object, ILottieOnCompositionLoadedListener
{
    public Action<LottieComposition?>? Loaded { get; set; }

    public void OnCompositionLoaded(LottieComposition? composition)
    {
        Loaded?.Invoke(composition);
    }
}
