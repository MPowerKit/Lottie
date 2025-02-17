﻿﻿using AddressBook;

using Com.Airbnb.Lottie;

using Foundation;

using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MPowerKit.Lottie;

public partial class LottieViewHandler : ViewHandler<LottieView, CompatibleAnimationView>
{
    protected override CompatibleAnimationView CreatePlatformView()
    {
        return new CompatibleAnimationView();
    }

    protected override void ConnectHandler(CompatibleAnimationView platformView)
    {
        base.ConnectHandler(platformView);

        platformView.AutoresizingMask = UIKit.UIViewAutoresizing.All;
        platformView.ContentMode = UIKit.UIViewContentMode.ScaleAspectFit;
    }

    protected override void DisconnectHandler(CompatibleAnimationView platformView)
    {
        base.DisconnectHandler(platformView);

        if (platformView.IsAnimationPlaying)
        {
            platformView.Stop();
        }
    }

    public static void MapSource(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView.CompatibleAnimation = null;
        view.SendAnimationStopped();

        view.SendSourceChanged();

        handler.PlatformView?.TrySetAnimation(view.Source,view, duration =>
        {
            view.SendAnimationLoaded(duration);

            MapTintColor(handler, view);
        });

        MapAutoPlay(handler, view);
    }

    public static void MapMinMaxFrame(LottieViewHandler handler, LottieView view)
    {
        if (view.MinFrame < 0 || view.MaxFrame < 0) return;

        handler.PlatformView?.PlayFromFrame(view.MinFrame, view.MaxFrame, completed =>
        {
            if (completed) view.SendAnimationFinished();
        });
    }

    public static void MapMinMaxProgress(LottieViewHandler handler, LottieView view)
    {
        if (view.MinProgress < 0f || view.MaxProgress < 0f) return;

        handler.PlatformView?.PlayFromProgress(view.MinProgress, view.MaxProgress, completed =>
        {
            if (completed) view.SendAnimationFinished();
        });
    }

    public static void MapSpeed(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView.AnimationSpeed = view.Speed;
    }

    public static void MapRepeatMode(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView.ConfigureRepeat(view.RepeatMode, view.RepeatCount);
    }

    public static void MapAnimationFrame(LottieViewHandler handler, LottieView view)
    {
        if (view.AnimationFrame < 0) return;

        handler.PlatformView.CurrentFrame = view.AnimationFrame;
    }

    public static void MapProgress(LottieViewHandler handler, LottieView view)
    {
        if (view.Progress < 0f) return;

        handler.PlatformView.CurrentProgress = view.Progress;
    }

    public static void MapHardwareAcceleration(LottieViewHandler handler, LottieView view)
    {
        handler.PlatformView.BackgroundMode = view.HardwareAcceleration
            ? CompatibleBackgroundBehavior.ContinuePlaying
            : CompatibleBackgroundBehavior.PauseAndRestore;
        handler.PlatformView.SetRenderingOption(view.HardwareAcceleration
            ? CompatibleRenderingEngineOption.CoreAnimation
            : CompatibleRenderingEngineOption.Shared);
    }

    public static void MapAutoPlay(LottieViewHandler handler, LottieView view)
    {
        if (!view.AutoPlay
            || handler.PlatformView.IsAnimationPlaying
            || view.State is AnimationState.Playing or AnimationState.Paused) return;

        switch (view.State)
        {
            case AnimationState.Stopped:
                handler.PlatformView?.PlayWithCompletion(completed =>
                {
                    if (completed) view.SendAnimationFinished();
                });
                if (handler.PlatformView.IsAnimationPlaying) view.SendAnimationStarted();
                break;
            default:
                break;
        }
    }

    public static void MapTintColor(LottieViewHandler handler, LottieView view)
    {
        if (view.TintColor is null)
        {
            // at the moment there is no any option to unset tint color
            //handler.PlatformView.SetColorValue(UIKit.UIColor.Clear, new CompatibleAnimationKeypath("**"));
            return;
        }

        handler.PlatformView.SetColorValue(view.TintColor.ToPlatform(), new CompatibleAnimationKeypath("**"));
    }

    public static void MapPlay(LottieViewHandler handler, LottieView view, object? args)
    {
        if (view.State is AnimationState.Playing or AnimationState.Paused || !view.IsLoaded) return;

        handler.PlatformView.PlayWithCompletion(completed =>
        {
            if (completed) view.SendAnimationFinished();
        });
        if (handler.PlatformView.IsAnimationPlaying) view.SendAnimationStarted();
    }

    public static void MapResume(LottieViewHandler handler, LottieView view, object? args)
    {
        if (view.State is not AnimationState.Paused) return;

        handler.PlatformView.PlayWithCompletion(completed =>
        {
            if (completed) view.SendAnimationFinished();
        });
        if (handler.PlatformView.IsAnimationPlaying) view.SendAnimationResumed();
    }

    public static void MapStop(LottieViewHandler handler, LottieView view, object? args)
    {
        if (view.State is AnimationState.Stopped or AnimationState.Finished) return;

        handler.PlatformView.Stop();
        view.SendAnimationStopped();
    }

    public static void MapPause(LottieViewHandler handler, LottieView view, object? args)
    {
        if (view.State is not AnimationState.Playing) return;

        handler.PlatformView.Pause();
        view.SendAnimationUpdated((float)handler.PlatformView.CurrentProgress);
        view.SendAnimationPaused();
    }
}