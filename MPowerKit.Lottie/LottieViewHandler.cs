namespace MPowerKit.Lottie;

#if ANDROID || IOS || MACCATALYST
public partial class LottieViewHandler
{
    public static IPropertyMapper<LottieView, LottieViewHandler> LottieViewMapper = new PropertyMapper<LottieView, LottieViewHandler>(ViewMapper)
    {
        [LottieView.SourceProperty.PropertyName] = MapSource,
        [LottieView.SpeedProperty.PropertyName] = MapSpeed,
        [LottieView.RepeatModeProperty.PropertyName] = MapRepeatMode,
        [LottieView.RepeatCountProperty.PropertyName] = MapRepeatMode,
        [LottieView.AnimationFrameProperty.PropertyName] = MapAnimationFrame,
        [LottieView.ProgressProperty.PropertyName] = MapProgress,
        [LottieView.HardwareAccelerationProperty.PropertyName] = MapHardwareAcceleration,
        [LottieView.TintColorProperty.PropertyName] = MapTintColor,
        [LottieView.AutoPlayProperty.PropertyName] = MapAutoPlay,

#if ANDROID
        [LottieView.EnableMergePathsForKitKatAndAboveProperty.PropertyName] = MapEnableMergePathsForKitKatAndAbove,
        [LottieView.CacheCompositionProperty.PropertyName] = MapCacheComposition,
        [LottieView.MinFrameProperty.PropertyName] = MapMinFrame,
        [LottieView.MaxFrameProperty.PropertyName] = MapMaxFrame,
        [LottieView.MinProgressProperty.PropertyName] = MapMinProgress,
        [LottieView.MaxProgressProperty.PropertyName] = MapMaxProgress,
#elif IOS || MACCATALYST
        [LottieView.MinFrameProperty.PropertyName] = MapMinMaxFrame,
        [LottieView.MaxFrameProperty.PropertyName] = MapMinMaxFrame,
        [LottieView.MinProgressProperty.PropertyName] = MapMinMaxProgress,
        [LottieView.MaxProgressProperty.PropertyName] = MapMinMaxProgress,
#endif
    };

    public static CommandMapper<LottieView, LottieViewHandler> LottieViewCommandMapper = new(ViewCommandMapper)
    {
#if ANDROID || IOS || MACCATALYST
        [nameof(LottieView.Play)] = MapPlay,
        [nameof(LottieView.Pause)] = MapPause,
        [nameof(LottieView.Stop)] = MapStop,
        [nameof(LottieView.Resume)] = MapResume,
#endif
    };

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
}
#endif