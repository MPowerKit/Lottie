using System.Diagnostics;

using Com.Airbnb.Lottie;

namespace MPowerKit.Lottie;

public static class AnimationViewExtensions
{
    public static async void TrySetAnimation(this LottieAnimationView lottieAnimationView, LottieAnimationSource? source)
    {
        if (source is null)
        {
            lottieAnimationView.ClearAnimation();
        }

        object animation;

        try
        {
            animation = await source!.LoadAnimationAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);

            lottieAnimationView.ClearAnimation();

            return;
        }

        switch (source)
        {
            case FileLottieAnimationSource:
                lottieAnimationView.SetAnimation(animation as string);
                break;
            case JsonLottieAnimationSource:
                lottieAnimationView.SetAnimationFromJson(animation as string, animation.GetHashCode().ToString());
                break;
            case StreamLottieAnimationSource:
                lottieAnimationView.SetAnimation(animation as Stream, animation.GetHashCode().ToString());
                break;
            case UriLottieAnimationSource:
                lottieAnimationView.SetAnimationFromUrl(animation as string, animation as string);
                break;
            default:
                break;
        }
    }

    public static void TrySetAnimation(this LottieAnimationView lottieAnimationView, AnimationView animationView)
    {
        if (lottieAnimationView == null)
            throw new ArgumentNullException(nameof(lottieAnimationView));

        if (animationView == null)
            throw new ArgumentNullException(nameof(animationView));

        switch (animationView.AnimationSource)
        {
            case AnimationSource.AssetOrBundle:
                lottieAnimationView.TrySetAnimation(animationView, animationView.Animation);
                break;
            case AnimationSource.Url:
                if (animationView.Animation is string stringAnimation)
                    lottieAnimationView.SetAnimationFromUrl(stringAnimation);
                break;
            case AnimationSource.Json:
                if (animationView.Animation is string jsonAnimation)
                    lottieAnimationView.SetAnimationFromJson(jsonAnimation);
                break;
            case AnimationSource.Stream:
                lottieAnimationView.TrySetAnimation(animationView, animationView.Animation);
                break;
            //case AnimationSource.EmbeddedResource:
            //    lottieAnimationView.TrySetAnimation(animationView, animationView.GetStreamFromAssembly());
            //    break;
            default:
                break;
        }
    }

    public static void TrySetAnimation(this LottieAnimationView lottieAnimationView, AnimationView animationView, object animation)
    {
        if (lottieAnimationView == null)
            throw new ArgumentNullException(nameof(lottieAnimationView));

        if (animationView == null)
            throw new ArgumentNullException(nameof(animationView));

        switch (animation)
        {
            case int intAnimation:
                lottieAnimationView.SetAnimation(intAnimation);
                break;
            case string stringAnimation:

                //TODO: check if json
                //animationView.SetAnimationFromJson(stringAnimation);
                //TODO: check if url
                //animationView.SetAnimationFromUrl(stringAnimation);

                lottieAnimationView.SetAnimation(stringAnimation);
                break;
            case Stream streamAnimation:
                lottieAnimationView.SetAnimation(streamAnimation, null);
                break;
            case null:
                lottieAnimationView.ClearAnimation();
                break;
            default:
                break;
        }
    }

    public static void ConfigureRepeat(this LottieAnimationView lottieAnimationView, RepeatMode repeatMode, int repeatCount)
    {
        ArgumentNullException.ThrowIfNull(lottieAnimationView);

        lottieAnimationView.RepeatCount = repeatCount;

        switch (repeatMode)
        {
            case RepeatMode.Infinite:
                {
                    lottieAnimationView.RepeatCount = int.MaxValue;
                    lottieAnimationView.RepeatMode = LottieDrawable.Infinite;
                    break;
                }
            case RepeatMode.Restart:
                lottieAnimationView.RepeatMode = LottieDrawable.Restart;
                break;
            case RepeatMode.Reverse:
                lottieAnimationView.RepeatMode = LottieDrawable.Reverse;
                break;
        }
    }
}
