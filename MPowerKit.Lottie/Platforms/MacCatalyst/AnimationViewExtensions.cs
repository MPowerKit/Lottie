using System.Diagnostics;

using Com.Airbnb.Lottie;

using Foundation;

namespace MPowerKit.Lottie;

public static class AnimationViewExtensions
{
    public static async void TrySetAnimation(this CompatibleAnimationView animationView,
        LottieAnimationSource? source,
        LottieView view,
        Action<float> onLoaded)
    {
        if (source is null)
        {
            animationView.CompatibleAnimation = null;
            view.SendAnimationStopped();
            return;
        }

        // imitating async work
        await Task.Delay(1);

        object animation;

        try
        {
            animation = await source!.LoadAnimationAsync().ConfigureAwait(false);

            switch (source)
            {
                case FileLottieAnimationSource:
                    animationView.CompatibleAnimation = CompatibleAnimation.Named(animation as string);
                    onLoaded((float)animationView.Duration);
                    break;
                case JsonLottieAnimationSource:
                    {
                        var data = NSData.FromString(animation as string);
                        animationView.CompatibleAnimation = CompatibleAnimation.From(data);
                        onLoaded((float)animationView.Duration);
                    }
                    break;
                case StreamLottieAnimationSource:
                    using (var sr = new StreamReader(animation as Stream))
                    {
                        var json = await sr.ReadToEndAsync();
                        var data = NSData.FromString(json);
                        animationView.CompatibleAnimation = CompatibleAnimation.From(data);
                        onLoaded((float)animationView.Duration);
                    }
                    break;
                case UriLottieAnimationSource:
                    CompatibleAnimation.LoadedFrom(NSUrl.FromString(animation as string), (animation) =>
                    {
                        animationView.CompatibleAnimation = animation;
                        onLoaded((float)animationView.Duration);
                    });
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            animationView.CompatibleAnimation = null;
            view.SendAnimationFailed(ex);
        }
    }

    public static void ConfigureRepeat(this CompatibleAnimationView animationView, RepeatMode repeatMode, int repeatCount)
    {
        ArgumentNullException.ThrowIfNull(animationView);

        switch (repeatMode)
        {
            case RepeatMode.Infinite:
                animationView.SetLoopMode(CompatibleLottieLoopMode.Loop, repeatCount);
                break;
            case RepeatMode.Restart:
                animationView.SetLoopMode(CompatibleLottieLoopMode.Repeat, repeatCount);
                break;
            case RepeatMode.Reverse:
                animationView.SetLoopMode(CompatibleLottieLoopMode.RepeatBackwards, repeatCount);
                break;
        }
    }
}