using Android.Graphics;
using Android.Views;

using Com.Airbnb.Lottie;
using Com.Airbnb.Lottie.Model;
using Com.Airbnb.Lottie.Value;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MPowerKit.Lottie;

public partial class LottieViewHandler : ViewHandler<LottieView, LottieAnimationView>
{
    protected AnimatorListener? AnimatorListener { get; set; }
    protected AnimatorUpdateListener? AnimatorUpdateListener { get; set; }
    protected LottieOnCompositionLoadedListener? LottieOnCompositionLoadedListener { get; set; }
    protected LottieFailureListener? LottieFailureListener { get; set; }

    protected override LottieAnimationView CreatePlatformView()
    {
        return new LottieAnimationView(Context);
    }

    protected override void ConnectHandler(LottieAnimationView platformView)
    {
        base.ConnectHandler(platformView);

        AnimatorListener = new AnimatorListener
        {
            Cancelled = () =>
            {
                PlatformView.SetLayerType(LayerType.None, null);
                VirtualView.SendAnimationStopped();
            },
            Ended = () =>
            {
                PlatformView.SetLayerType(LayerType.None, null);
                VirtualView.SendAnimationFinished();
            },
            Paused = VirtualView.SendAnimationPaused,
            Repeated = VirtualView.SendAnimationRepeated,
            Resumed = VirtualView.SendAnimationResumed,
            Started = VirtualView.SendAnimationStarted
        };
        platformView.AddAnimatorListener(AnimatorListener);

        AnimatorUpdateListener = new AnimatorUpdateListener
        {
            Updated = (progress) => VirtualView.SendAnimationUpdated(progress)
        };
        platformView.AddAnimatorUpdateListener(AnimatorUpdateListener);

        LottieOnCompositionLoadedListener = new LottieOnCompositionLoadedListener
        {
            Loaded = (composition) => VirtualView.SendAnimationLoaded(composition?.Duration)
        };
        platformView.AddLottieOnCompositionLoadedListener(LottieOnCompositionLoadedListener);

        LottieFailureListener = new LottieFailureListener
        {
            Failed = VirtualView.SendAnimationFailed
        };
        platformView.SetFailureListener(LottieFailureListener);
    }

    protected override void DisconnectHandler(LottieAnimationView platformView)
    {
        base.DisconnectHandler(platformView);

        if (platformView.IsAnimating)
        {
            platformView.CancelAnimation();
        }

        platformView.RemoveAnimatorListener(AnimatorListener);
        AnimatorListener?.Dispose();
        AnimatorListener = null;

        platformView.RemoveUpdateListener(AnimatorUpdateListener);
        AnimatorUpdateListener?.Dispose();
        AnimatorUpdateListener = null;

        platformView.RemoveLottieOnCompositionLoadedListener(LottieOnCompositionLoadedListener!);
        LottieOnCompositionLoadedListener?.Dispose();
        LottieOnCompositionLoadedListener = null;

        platformView.SetFailureListener(null);
        LottieFailureListener?.Dispose();
        LottieFailureListener = null;
    }

    public static void MapSource(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView?.ClearAnimation();

        view.SendSourceChanged();

        handler.PlatformView?.TrySetAnimation(view.Source);

        MapAutoPlay(handler, view);
    }

    public static void MapCacheComposition(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView?.SetCacheComposition(view.CacheComposition);
    }

    public static void MapMinFrame(LottieViewHandler handler, LottieView view)
    {
        if (view.MinFrame < 0) return;

        handler.PlatformView?.SetMinFrame(view.MinFrame);
    }

    public static void MapMaxFrame(LottieViewHandler handler, LottieView view)
    {
        if (view.MaxFrame < 0) return;

        handler.PlatformView?.SetMaxFrame(view.MaxFrame);
    }

    public static void MapMinProgress(LottieViewHandler handler, LottieView view)
    {
        if (view.MinProgress < 0f) return;

        handler.PlatformView?.SetMinProgress(view.MinProgress);
    }

    public static void MapMaxProgress(LottieViewHandler handler, LottieView view)
    {
        if (view.MaxProgress < 0f) return;

        handler.PlatformView?.SetMaxProgress(view.MaxProgress);
    }

    public static void MapSpeed(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView.Speed = view.Speed;
    }

    public static void MapRepeatMode(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView.ConfigureRepeat(view.RepeatMode, view.RepeatCount);
    }

    public static void MapAnimationFrame(LottieViewHandler handler, LottieView view)
    {
        if (view.AnimationFrame < 0) return;

        handler.PlatformView.Frame = view.AnimationFrame;
    }

    public static void MapProgress(LottieViewHandler handler, LottieView view)
    {
        if (view.Progress < 0f) return;

        handler.PlatformView.Progress = view.Progress;
    }

    public static void MapEnableMergePathsForKitKatAndAbove(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView.EnableMergePathsForKitKatAndAbove(view.EnableMergePathsForKitKatAndAbove);
    }

    public static void MapHardwareAcceleration(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView.SetLayerType(view.HardwareAcceleration && view.State is AnimationState.Playing or AnimationState.Paused
            ? LayerType.Hardware
            : LayerType.None, null);
    }

    public static void MapAutoPlay(LottieViewHandler handler, LottieView view)
    {
        if (!view.AutoPlay
            || handler.PlatformView.IsAnimating
            || view.State is AnimationState.Playing or AnimationState.Paused) return;

        switch (view.State)
        {
            case AnimationState.Stopped:
                handler.PlatformView.SetLayerType(view.HardwareAcceleration ? LayerType.Hardware : LayerType.None, null);
                handler.PlatformView?.PlayAnimation();
                break;
            default:
                break;
        }
    }

    public static void MapTintColor(LottieViewHandler handler, LottieView view)
    {
        if (view.TintColor is null)
        {
            handler.PlatformView.ClearValueCallback(new KeyPath("**"), ILottieProperty.ColorFilter);
            return;
        }

        handler.PlatformView.AddValueCallback(new KeyPath("**"),
            ILottieProperty.ColorFilter,
            new LottieValueCallback(new PorterDuffColorFilter(view.TintColor.ToPlatform(), PorterDuff.Mode.SrcAtop!)));
    }

    public static void MapPlay(LottieViewHandler handler, LottieView view, object? args)
    {
        if (view.State is AnimationState.Playing or AnimationState.Paused || !view.IsLoaded) return;

        handler.PlatformView.SetLayerType(view.HardwareAcceleration ? LayerType.Hardware : LayerType.None, null);
        handler.PlatformView?.PlayAnimation();
    }

    public static void MapPause(LottieViewHandler handler, LottieView view, object? args)
    {
        if (view.State is not AnimationState.Playing) return;

        handler.PlatformView?.PauseAnimation();
    }

    public static void MapStop(LottieViewHandler handler, LottieView view, object? args)
    {
        if (view.State is AnimationState.Stopped or AnimationState.Finished) return;

        handler.PlatformView.SetLayerType(LayerType.None, null);
        handler.PlatformView?.CancelAnimation();
    }

    public static void MapResume(LottieViewHandler handler, LottieView view, object? args)
    {
        if (view.State is not AnimationState.Paused) return;

        handler.PlatformView?.ResumeAnimation();
    }
}