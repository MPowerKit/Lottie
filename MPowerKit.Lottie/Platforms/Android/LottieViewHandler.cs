using System.ComponentModel;

using Android.Content;
using Android.Graphics;
using Android.Views;

using Com.Airbnb.Lottie;
using Com.Airbnb.Lottie.Model;
using Com.Airbnb.Lottie.Value;

using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace MPowerKit.Lottie;

public class LottieViewHandler : ViewHandler<LottieView, LottieAnimationView>
{
    public static IPropertyMapper<LottieView, LottieViewHandler> LottieViewMapper = new PropertyMapper<LottieView, LottieViewHandler>(ViewMapper)
    {
        [LottieView.SourceProperty.PropertyName] = MapSource,
        [LottieView.CacheCompositionProperty.PropertyName] = MapCacheComposition,
        [LottieView.MinFrameProperty.PropertyName] = MapMinFrame,
        [LottieView.MaxFrameProperty.PropertyName] = MapMaxFrame,
        [LottieView.MinProgressProperty.PropertyName] = MapMinProgress,
        [LottieView.MaxProgressProperty.PropertyName] = MapMaxProgress,
        [LottieView.SpeedProperty.PropertyName] = MapSpeed,
        [LottieView.RepeatModeProperty.PropertyName] = MapRepeatMode,
        [LottieView.RepeatCountProperty.PropertyName] = MapRepeatMode,
        [LottieView.AnimationFrameProperty.PropertyName] = MapAnimationFrame,
        [LottieView.ProgressProperty.PropertyName] = MapProgress,
        [LottieView.EnableMergePathsForKitKatAndAboveProperty.PropertyName] = MapEnableMergePathsForKitKatAndAbove,
        [LottieView.HardwareAccelerationProperty.PropertyName] = MapHardwareAcceleration,
        [LottieView.TintColorProperty.PropertyName] = MapTintColor,
        //[LottieView.AutoPlayProperty.PropertyName] = MapAutoPlay,
    };

    public static CommandMapper<LottieView, LottieViewHandler> LottieViewCommandMapper = new(ViewCommandMapper)
    {
        [nameof(LottieView.Play)] = MapPlay,
        [nameof(LottieView.Pause)] = MapPause,
        [nameof(LottieView.Stop)] = MapStop,
        [nameof(LottieView.Resume)] = MapResume,
    };

    protected AnimatorListener? AnimatorListener { get; set; }
    protected AnimatorUpdateListener? AnimatorUpdateListener { get; set; }
    protected LottieOnCompositionLoadedListener? LottieOnCompositionLoadedListener { get; set; }
    protected LottieFailureListener? LottieFailureListener { get; set; }

    public LottieViewHandler()
        : base(LottieViewMapper, LottieViewCommandMapper)
    {
    }

    public LottieViewHandler(IPropertyMapper<LottieView, LottieViewHandler>? mapper)
        : base(mapper ?? LottieViewMapper, LottieViewCommandMapper)
    {

    }

    public LottieViewHandler(IPropertyMapper<LottieView, LottieViewHandler>? mapper, CommandMapper<LottieView, LottieViewHandler>? commandMapper)
        : base(mapper ?? LottieViewMapper, commandMapper ?? LottieViewCommandMapper)
    {

    }

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
            Updated = VirtualView.SendAnimationUpdated
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
        if (view.Speed <= 0) return;

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
                Console.WriteLine("Playing");
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

public class AnimationViewRenderer : ViewRenderer<AnimationView, LottieAnimationView>
{
    public AnimationViewRenderer(Context context) : base(context)
    {
    }

    private LottieAnimationView _animationView;
    private AnimatorListener _animatorListener;
    private AnimatorUpdateListener _animatorUpdateListener;
    private LottieOnCompositionLoadedListener _lottieOnCompositionLoadedListener;
    private LottieFailureListener _lottieFailureListener;

    protected override void OnElementChanged(ElementChangedEventArgs<AnimationView> e)
    {
        base.OnElementChanged(e);

        if (e == null)
            return;

        if (e.OldElement != null)
        {
            _animationView.RemoveAnimatorListener(_animatorListener);
            _animationView.RemoveAllUpdateListeners();
            _animationView.RemoveLottieOnCompositionLoadedListener(_lottieOnCompositionLoadedListener);
            _animationView.SetFailureListener(null);
            _animationView.SetOnClickListener(null);
        }

        if (e.NewElement != null)
        {
            if (Control == null)
            {
                _animationView = new LottieAnimationView(Context);
                _animatorListener = new AnimatorListener
                {
                    Cancelled = () => e.NewElement.InvokeStopAnimation(),
                    Ended = () => e.NewElement.InvokeFinishedAnimation(),
                    Paused = () => e.NewElement.InvokePauseAnimation(),
                    Repeated = () => e.NewElement.InvokeRepeatAnimation(),
                    Resumed = () => e.NewElement.InvokeResumeAnimation(),
                    Started = () => e.NewElement.InvokePlayAnimation()
                };
                _animatorUpdateListener = new AnimatorUpdateListener
                {
                    Updated = (progress) => e.NewElement.InvokeAnimationUpdate(progress)
                };
                _lottieOnCompositionLoadedListener = new LottieOnCompositionLoadedListener
                {
                    Loaded = (composition) => e.NewElement.InvokeAnimationLoaded(composition)
                };
                _lottieFailureListener = new LottieFailureListener
                {
                    Failed = (exception) => e.NewElement.InvokeFailure(exception)
                };

                _animationView.AddAnimatorListener(_animatorListener);
                _animationView.AddAnimatorUpdateListener(_animatorUpdateListener);
                _animationView.AddLottieOnCompositionLoadedListener(_lottieOnCompositionLoadedListener);
                _animationView.SetFailureListener(_lottieFailureListener);

                _animationView.TrySetAnimation(e.NewElement);

                e.NewElement.PlayCommand = new Command(() => _animationView.PlayAnimation());
                e.NewElement.PauseCommand = new Command(() => _animationView.PauseAnimation());
                e.NewElement.ResumeCommand = new Command(() => _animationView.ResumeAnimation());
                e.NewElement.StopCommand = new Command(() =>
                {
                    _animationView.CancelAnimation();
                    _animationView.Progress = 0.0f;
                });
                e.NewElement.ClickCommand = new Command(() => _animationView.PerformClick());

                e.NewElement.PlayMinAndMaxFrameCommand = new Command((object paramter) =>
                {
                    if (paramter is (int minFrame, int maxFrame))
                    {
                        _animationView.SetMinAndMaxFrame(minFrame, maxFrame);
                        _animationView.PlayAnimation();
                    }
                });
                e.NewElement.PlayMinAndMaxProgressCommand = new Command((object paramter) =>
                {
                    if (paramter is (float minProgress, float maxProgress))
                    {
                        _animationView.SetMinAndMaxProgress(minProgress, maxProgress);
                        _animationView.PlayAnimation();
                    }
                });
                e.NewElement.ReverseAnimationSpeedCommand = new Command(() => _animationView.ReverseAnimationSpeed());

                _animationView.SetCacheComposition(e.NewElement.CacheComposition);
                //_animationView.SetFallbackResource(e.NewElement.FallbackResource.);
                //_animationView.Composition = e.NewElement.Composition;

                if (e.NewElement.MinFrame != int.MinValue)
                    _animationView.SetMinFrame(e.NewElement.MinFrame);
                if (e.NewElement.MinProgress != float.MinValue)
                    _animationView.SetMinProgress(e.NewElement.MinProgress);
                if (e.NewElement.MaxFrame != int.MinValue)
                    _animationView.SetMaxFrame(e.NewElement.MaxFrame);
                if (e.NewElement.MaxProgress != float.MinValue)
                    _animationView.SetMaxProgress(e.NewElement.MaxProgress);

                _animationView.Speed = e.NewElement.Speed;

                _animationView.ConfigureRepeat(e.NewElement.RepeatMode, e.NewElement.RepeatCount);

                if (!string.IsNullOrEmpty(e.NewElement.ImageAssetsFolder))
                    _animationView.ImageAssetsFolder = e.NewElement.ImageAssetsFolder;

                //TODO: see if this needs to be enabled
                //_animationView.Scale = Convert.ToSingle(e.NewElement.Scale);

                _animationView.Frame = e.NewElement.AnitmationFrame;
                _animationView.Progress = e.NewElement.Progress;

                _animationView.EnableMergePathsForKitKatAndAbove(e.NewElement.EnableMergePathsForKitKatAndAbove);

                SetNativeControl(_animationView);

                if (e.NewElement.AutoPlay || e.NewElement.IsAnimating)
                    _animationView.PlayAnimation();

                e.NewElement.Duration = _animationView.Duration;
                e.NewElement.IsAnimating = _animationView.IsAnimating;

                Console.WriteLine($"Element Changed");
            }
        }
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (_animationView == null || Element == null || e == null)
            return;

        if (e.PropertyName == AnimationView.AnimationProperty.PropertyName)
        {
            _animationView.TrySetAnimation(Element);

            if (Element.AutoPlay || Element.IsAnimating)
                _animationView.PlayAnimation();
        }

        //if (e.PropertyName == AnimationView.AutoPlayProperty.PropertyName)
        //    _animationView.AutoPlay = (Element.AutoPlay);

        if (e.PropertyName == AnimationView.CacheCompositionProperty.PropertyName)
            _animationView.SetCacheComposition(Element.CacheComposition);

        //if (e.PropertyName == AnimationView.FallbackResource.PropertyName)
        //    _animationView.SetFallbackResource(e.NewElement.FallbackResource);

        //if (e.PropertyName == AnimationView.Composition.PropertyName)
        //    _animationView.Composition = e.NewElement.Composition;

        if (e.PropertyName == AnimationView.EnableMergePathsForKitKatAndAboveProperty.PropertyName)
            _animationView.EnableMergePathsForKitKatAndAbove(Element.EnableMergePathsForKitKatAndAbove);

        if (e.PropertyName == AnimationView.MinFrameProperty.PropertyName)
            _animationView.SetMinFrame(Element.MinFrame);

        if (e.PropertyName == AnimationView.MinProgressProperty.PropertyName)
            _animationView.SetMinProgress(Element.MinProgress);

        if (e.PropertyName == AnimationView.MaxFrameProperty.PropertyName)
            _animationView.SetMaxFrame(Element.MaxFrame);

        if (e.PropertyName == AnimationView.MaxProgressProperty.PropertyName)
            _animationView.SetMaxProgress(Element.MaxProgress);

        if (e.PropertyName == AnimationView.SpeedProperty.PropertyName)
            _animationView.Speed = Element.Speed;

        if (e.PropertyName == AnimationView.RepeatModeProperty.PropertyName || e.PropertyName == AnimationView.RepeatCountProperty.PropertyName)
            _animationView.ConfigureRepeat(Element.RepeatMode, Element.RepeatCount);

        if (e.PropertyName == AnimationView.ImageAssetsFolderProperty.PropertyName && !string.IsNullOrEmpty(Element.ImageAssetsFolder))
            _animationView.ImageAssetsFolder = Element.ImageAssetsFolder;

        //TODO: see if this needs to be enabled
        //if (e.PropertyName == AnimationView.ScaleProperty.PropertyName)
        //    _animationView.Scale = Element.Scale;

        if (e.PropertyName == AnimationView.AnimationFrameProperty.PropertyName)
            _animationView.Frame = Element.AnitmationFrame;

        if (e.PropertyName == AnimationView.ProgressProperty.PropertyName)
            _animationView.Progress = Element.Progress;

        Console.WriteLine($"Property changed: {e.PropertyName}");

        base.OnElementPropertyChanged(sender, e);
    }
}