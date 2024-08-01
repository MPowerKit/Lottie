using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;

namespace MPowerKit.Lottie;

public class LottieView : View
{
    public LottieView()
    {
        PlayAction = Play;
        PauseAction = Pause;
        StopAction = Stop;
        ResumeAction = Resume;
    }

    public void Play()
    {
        Handler?.Invoke(nameof(Play));
    }

    public void Pause()
    {
        Handler?.Invoke(nameof(Pause));
    }

    public void Stop()
    {
        Handler?.Invoke(nameof(Stop));
    }

    public void Resume()
    {
        Handler?.Invoke(nameof(Resume));
    }

    /// <summary>
    /// Called when the Lottie animation starts playing
    /// </summary>
    public event EventHandler? AnimationStarted;

    /// <summary>
    /// Called when the Lottie animation is paused
    /// </summary>
    public event EventHandler? AnimationPaused;

    /// <summary>
    /// Called when the Lottie animation is resumed after pausing
    /// </summary>
    public event EventHandler? AnimationResumed;

    /// <summary>
    /// Called when the Lottie animation is stopped
    /// </summary>
    public event EventHandler? AnimationStopped;

    /// <summary>
    /// Called when the Lottie animation is repeated
    /// </summary>
    public event EventHandler? AnimationRepeated;

    /// <summary>
    /// Called when the Lottie animation is playing with the current progress
    /// </summary>
    public event EventHandler<float>? AnimationUpdated;

    /// <summary>
    /// Called when the Lottie animation is loaded with the Lottie Composition as parameter
    /// </summary>
    public event EventHandler<object?>? AnimationLoaded;

    /// <summary>
    /// Called when the Source was changed
    /// </summary>
    public event EventHandler? SourceChanged;

    /// <summary>
    /// Called when the animation fails to load or when an exception happened when trying to play
    /// </summary>
    public event EventHandler<Exception>? AnimationFailed;

    /// <summary>
    /// Called when the Lottie animation is finished playing
    /// </summary>
    public event EventHandler? AnimationFinished;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationStarted()
    {
        State = AnimationState.Playing;
        AnimationStarted?.Invoke(this, EventArgs.Empty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationResumed()
    {
        State = AnimationState.Playing;
        AnimationResumed?.Invoke(this, EventArgs.Empty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationStopped()
    {
        State = AnimationState.Stopped;
        AnimationStopped?.Invoke(this, EventArgs.Empty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationPaused()
    {
        State = AnimationState.Paused;
        AnimationPaused?.Invoke(this, EventArgs.Empty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationRepeated()
    {
        AnimationRepeated?.Invoke(this, EventArgs.Empty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationUpdated(float progress)
    {
        AnimationUpdated?.Invoke(this, progress);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationLoaded(object? animation)
    {
        IsAnimationLoaded = animation is not null;
#if ANDROID
        Duration = (animation as Com.Airbnb.Lottie.LottieComposition)?.Duration ?? 0f;
#endif
        AnimationLoaded?.Invoke(this, animation);

        if (IsAnimationLoaded && AutoPlay && State is AnimationState.Stopped or AnimationState.Finished)
        {
            Handler!.Invoke(nameof(Play));
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendSourceChanged()
    {
        IsAnimationLoaded = false;
        Duration = 0f;
        SourceChanged?.Invoke(this, EventArgs.Empty);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationFailed(Exception ex)
    {
        AnimationFailed?.Invoke(this, ex);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public void SendAnimationFinished()
    {
        State = AnimationState.Finished;
        AnimationFinished?.Invoke(this, EventArgs.Empty);
    }

    #region PlayAction
    public Action PlayAction
    {
        get => (Action)GetValue(PlayActionProperty);
        protected set => SetValue(PlayActionProperty, value);
    }

    public static readonly BindableProperty PlayActionProperty =
        BindableProperty.Create(
            nameof(PlayAction),
            typeof(Action),
            typeof(LottieView),
            defaultBindingMode: BindingMode.OneWayToSource);
    #endregion

    #region PauseAction
    public Action PauseAction
    {
        get => (Action)GetValue(PauseActionProperty);
        protected set => SetValue(PauseActionProperty, value);
    }

    public static readonly BindableProperty PauseActionProperty =
        BindableProperty.Create(
            nameof(PauseAction),
            typeof(Action),
            typeof(LottieView),
            defaultBindingMode: BindingMode.OneWayToSource);
    #endregion

    #region ResumeAction
    public Action ResumeAction
    {
        get => (Action)GetValue(ResumeActionProperty);
        protected set => SetValue(ResumeActionProperty, value);
    }

    public static readonly BindableProperty ResumeActionProperty =
        BindableProperty.Create(
            nameof(ResumeAction),
            typeof(Action),
            typeof(LottieView),
            defaultBindingMode: BindingMode.OneWayToSource);
    #endregion

    #region StopAction
    public Action StopAction
    {
        get => (Action)GetValue(StopActionProperty);
        protected set => SetValue(StopActionProperty, value);
    }

    public static readonly BindableProperty StopActionProperty =
        BindableProperty.Create(
            nameof(StopAction),
            typeof(Action),
            typeof(LottieView),
            defaultBindingMode: BindingMode.OneWayToSource);
    #endregion

    #region TintColor
    public Color? TintColor
    {
        get { return (Color?)GetValue(TintColorProperty); }
        set { SetValue(TintColorProperty, value); }
    }

    public static readonly BindableProperty TintColorProperty =
        BindableProperty.Create(
            nameof(TintColor),
            typeof(Color),
            typeof(LottieView)
            );
    #endregion

    #region CacheComposition
    public bool CacheComposition
    {
        get { return (bool)GetValue(CacheCompositionProperty); }
        set { SetValue(CacheCompositionProperty, value); }
    }

    public static readonly BindableProperty CacheCompositionProperty =
        BindableProperty.Create(
            nameof(CacheComposition),
            typeof(bool),
            typeof(LottieView),
            true);
    #endregion

    #region MinFrame
    public int MinFrame
    {
        get { return (int)GetValue(MinFrameProperty); }
        set { SetValue(MinFrameProperty, value); }
    }

    public static readonly BindableProperty MinFrameProperty =
        BindableProperty.Create(
            nameof(MinFrame),
            typeof(int),
            typeof(LottieView),
            -1);
    #endregion

    #region MaxFrame
    public int MaxFrame
    {
        get { return (int)GetValue(MaxFrameProperty); }
        set { SetValue(MaxFrameProperty, value); }
    }

    public static readonly BindableProperty MaxFrameProperty =
        BindableProperty.Create(
            nameof(MaxFrame),
            typeof(int),
            typeof(LottieView),
            -1);
    #endregion

    #region MinProgress
    public float MinProgress
    {
        get { return (float)GetValue(MinProgressProperty); }
        set { SetValue(MinProgressProperty, value); }
    }

    public static readonly BindableProperty MinProgressProperty =
        BindableProperty.Create(
            nameof(MinProgress),
            typeof(float),
            typeof(LottieView),
            -1f);
    #endregion

    #region MaxProgress
    public float MaxProgress
    {
        get { return (float)GetValue(MaxProgressProperty); }
        set { SetValue(MaxProgressProperty, value); }
    }

    public static readonly BindableProperty MaxProgressProperty =
        BindableProperty.Create(
            nameof(MaxProgress),
            typeof(float),
            typeof(LottieView),
            -1f);
    #endregion

    #region Speed
    public float Speed
    {
        get { return (float)GetValue(SpeedProperty); }
        set { SetValue(SpeedProperty, value); }
    }

    public static readonly BindableProperty SpeedProperty =
        BindableProperty.Create(
            nameof(Speed),
            typeof(float),
            typeof(LottieView),
            1f);
    #endregion

    #region RepeatMode
    public RepeatMode RepeatMode
    {
        get { return (RepeatMode)GetValue(RepeatModeProperty); }
        set { SetValue(RepeatModeProperty, value); }
    }

    public static readonly BindableProperty RepeatModeProperty =
        BindableProperty.Create(
            nameof(RepeatMode),
            typeof(RepeatMode),
            typeof(LottieView),
            RepeatMode.Restart);
    #endregion

    #region RepeatCount
    public int RepeatCount
    {
        get { return (int)GetValue(RepeatCountProperty); }
        set { SetValue(RepeatCountProperty, value); }
    }

    public static readonly BindableProperty RepeatCountProperty =
        BindableProperty.Create(
            nameof(RepeatCount),
            typeof(int),
            typeof(LottieView)
            );
    #endregion

    #region State
    public AnimationState State
    {
        get { return (AnimationState)GetValue(StateProperty); }
        protected set { SetValue(StateProperty, value); }
    }

    public static readonly BindableProperty StateProperty =
        BindableProperty.Create(
            nameof(State),
            typeof(AnimationState),
            typeof(LottieView),
            defaultBindingMode: BindingMode.OneWayToSource);
    #endregion

    #region AnimationFrame
    public int AnimationFrame
    {
        get { return (int)GetValue(AnimationFrameProperty); }
        set { SetValue(AnimationFrameProperty, value); }
    }

    public static readonly BindableProperty AnimationFrameProperty =
        BindableProperty.Create(
            nameof(AnimationFrame),
            typeof(int),
            typeof(LottieView)
            );
    #endregion

    #region Progress
    public float Progress
    {
        get { return (float)GetValue(ProgressProperty); }
        set { SetValue(ProgressProperty, value); }
    }

    public static readonly BindableProperty ProgressProperty =
        BindableProperty.Create(
            nameof(Progress),
            typeof(float),
            typeof(LottieView)
            );
    #endregion

    #region Duration
    public float Duration
    {
        get { return (float)GetValue(DurationProperty); }
        protected set { SetValue(DurationProperty, value); }
    }

    public static readonly BindableProperty DurationProperty =
        BindableProperty.Create(
            nameof(Duration),
            typeof(float),
            typeof(LottieView),
            defaultBindingMode: BindingMode.OneWayToSource
            );
    #endregion

    #region IsLoaded
    public bool IsAnimationLoaded
    {
        get { return (bool)GetValue(IsAnimationLoadedProperty); }
        protected set { SetValue(IsAnimationLoadedProperty, value); }
    }

    public static readonly BindableProperty IsAnimationLoadedProperty =
        BindableProperty.Create(
            nameof(IsAnimationLoaded),
            typeof(bool),
            typeof(LottieView),
            defaultBindingMode: BindingMode.OneWayToSource
            );
    #endregion

    #region AutoPlay
    public bool AutoPlay
    {
        get { return (bool)GetValue(AutoPlayProperty); }
        set { SetValue(AutoPlayProperty, value); }
    }

    public static readonly BindableProperty AutoPlayProperty =
        BindableProperty.Create(
            nameof(AutoPlay),
            typeof(bool),
            typeof(LottieView),
            true);
    #endregion

    #region HardwareAcceleration
    public bool HardwareAcceleration
    {
        get { return (bool)GetValue(HardwareAccelerationProperty); }
        set { SetValue(HardwareAccelerationProperty, value); }
    }

    public static readonly BindableProperty HardwareAccelerationProperty =
        BindableProperty.Create(
            nameof(HardwareAcceleration),
            typeof(bool),
            typeof(LottieView),
            true);
    #endregion

    #region EnableMergePathsForKitKatAndAbove
    public bool EnableMergePathsForKitKatAndAbove
    {
        get { return (bool)GetValue(EnableMergePathsForKitKatAndAboveProperty); }
        set { SetValue(EnableMergePathsForKitKatAndAboveProperty, value); }
    }

    public static readonly BindableProperty EnableMergePathsForKitKatAndAboveProperty =
        BindableProperty.Create(
            nameof(EnableMergePathsForKitKatAndAbove),
            typeof(bool),
            typeof(LottieView)
            );
    #endregion

    #region Source
    public LottieAnimationSource? Source
    {
        get { return (LottieAnimationSource?)GetValue(SourceProperty); }
        set { SetValue(SourceProperty, value); }
    }

    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(
            nameof(Source),
            typeof(LottieAnimationSource),
            typeof(LottieView)
            );
    #endregion
}

public class AnimationView : View
{
    //public static readonly BindableProperty ImageProperty = BindableProperty.Create(nameof(Image),
    //    typeof(ImageSource), typeof(AnimationView), default(ImageSource));

    public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation),
        typeof(object), typeof(AnimationView));

    public static readonly BindableProperty AnimationSourceProperty = BindableProperty.Create(nameof(AnimationSource),
        typeof(AnimationSource), typeof(AnimationView), AnimationSource.AssetOrBundle);

    public static readonly BindableProperty CacheCompositionProperty = BindableProperty.Create(nameof(CacheComposition),
        typeof(bool), typeof(AnimationView), true);

    //public static readonly BindableProperty FallbackResourceProperty = BindableProperty.Create(nameof(FallbackResource),
    //    typeof(ImageSource), typeof(AnimationView), default(ImageSource));

    //public static readonly BindableProperty CompositionProperty = BindableProperty.Create(nameof(Composition),
    //    typeof(ILottieComposition), typeof(AnimationView), default(ILottieComposition));

    public static readonly BindableProperty MinFrameProperty = BindableProperty.Create(nameof(MinFrame),
        typeof(int), typeof(AnimationView), int.MinValue);

    public static readonly BindableProperty MinProgressProperty = BindableProperty.Create(nameof(MinProgress),
        typeof(float), typeof(AnimationView), float.MinValue);

    public static readonly BindableProperty MaxFrameProperty = BindableProperty.Create(nameof(MaxFrame),
        typeof(int), typeof(AnimationView), int.MinValue);

    public static readonly BindableProperty MaxProgressProperty = BindableProperty.Create(nameof(MaxProgress),
        typeof(float), typeof(AnimationView), float.MinValue);

    public static readonly BindableProperty SpeedProperty = BindableProperty.Create(nameof(Speed),
        typeof(float), typeof(AnimationView), 1.0f);

    public static readonly BindableProperty RepeatModeProperty = BindableProperty.Create(nameof(RepeatMode),
        typeof(RepeatMode), typeof(AnimationView), RepeatMode.Restart);

    public static readonly BindableProperty RepeatCountProperty = BindableProperty.Create(nameof(RepeatCount),
        typeof(int), typeof(AnimationView), 0);

    public static readonly BindableProperty IsAnimatingProperty = BindableProperty.Create(nameof(IsAnimating),
        typeof(bool), typeof(AnimationView), false);

    public static readonly BindableProperty ImageAssetsFolderProperty = BindableProperty.Create(nameof(ImageAssetsFolder),
        typeof(string), typeof(AnimationView), default(string));

    //public static new readonly BindableProperty ScaleProperty = BindableProperty.Create(nameof(Scale),
    //    typeof(float), typeof(AnimationView), 1.0f);

    public static readonly BindableProperty AnimationFrameProperty = BindableProperty.Create(nameof(AnitmationFrame),
        typeof(int), typeof(AnimationView), default(int));

    public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress),
        typeof(float), typeof(AnimationView), 0.0f);

    //TODO: Maybe make TimeSpan
    public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration),
        typeof(long), typeof(AnimationView), default(long));

    public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(nameof(AutoPlay),
        typeof(bool), typeof(AnimationView), true);

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command),
        typeof(ICommand), typeof(AnimationView));

    public static readonly BindableProperty EnableMergePathsForKitKatAndAboveProperty = BindableProperty.Create(nameof(EnableMergePathsForKitKatAndAbove),
        typeof(bool), typeof(AnimationView), false);

    /// <summary>
    /// Returns the duration of an animation (Frames / FrameRate * 1000)
    /// </summary>
    public long Duration
    {
        get { return (long)GetValue(DurationProperty); }
        internal set { SetValue(DurationProperty, value); }
    }

    /// <summary>
    /// Indicates if a Lottie Animation should be cached
    /// </summary>
    public bool CacheComposition
    {
        get { return (bool)GetValue(CacheCompositionProperty); }
        set { SetValue(CacheCompositionProperty, value); }
    }

    /// <summary>
    /// Set the Animation that you want to play. This can be a URL (either local path or remote), Json string, or Stream
    /// </summary>
    public object Animation
    {
        get { return (object)GetValue(AnimationProperty); }
        set { SetValue(AnimationProperty, value); }
    }

    /// <summary>
    /// Indicates where the Animation is located and from which source it should be loaded
    /// Default value is AssetOrBundle
    /// </summary>
    public AnimationSource AnimationSource
    {
        get { return (AnimationSource)GetValue(AnimationSourceProperty); }
        set { SetValue(AnimationSourceProperty, value); }
    }

    //// <summary>
    //// Used in case an animations fails to load
    //// </summary>
    //public ImageSource FallbackResource
    //{
    //    get { return (ImageSource)GetValue(FallbackResourceProperty); }
    //    set { SetValue(FallbackResourceProperty, value); }
    //}

    //public ILottieComposition Composition
    //{
    //    get { return (ILottieComposition)GetValue(CompositionProperty); }
    //    set { SetValue(CompositionProperty, value); }
    //}

    /// <summary>
    /// Sets or gets the minimum frame that the animation will start from when playing or looping.
    /// </summary>
    public int MinFrame
    {
        get { return (int)GetValue(MinFrameProperty); }
        set { SetValue(MinFrameProperty, value); }
    }

    /// <summary>
    /// Sets or gets the minimum progress that the animation will start from when playing or looping.
    /// </summary>
    public float MinProgress
    {
        get { return (float)GetValue(MinProgressProperty); }
        set { SetValue(MinProgressProperty, value); }
    }

    /// <summary>
    /// Sets or gets the maximum frame that the animation will end at when playing or looping.
    /// </summary>
    public int MaxFrame
    {
        get { return (int)GetValue(MaxFrameProperty); }
        set { SetValue(MaxFrameProperty, value); }
    }

    /// <summary>
    /// Sets or gets the maximum progress that the animation will end at when playing or looping.
    /// </summary>
    public float MaxProgress
    {
        get { return (float)GetValue(MaxProgressProperty); }
        set { SetValue(MaxProgressProperty, value); }
    }

    /// <summary>
    /// Returns the current playback speed. This will be &lt; 0 if the animation is playing backwards.
    /// </summary>
    public float Speed
    {
        get { return (float)GetValue(SpeedProperty); }
        set { SetValue(SpeedProperty, value); }
    }

    /// <summary>
    /// Defines what this animation should do when it reaches the end. 
    /// This setting is applied only when the repeat count is either greater than 0 or INFINITE.
    /// Defaults to RESTART.
    /// </summary>
    public RepeatMode RepeatMode
    {
        get { return (RepeatMode)GetValue(RepeatModeProperty); }
        set { SetValue(RepeatModeProperty, value); }
    }

    /// <summary>
    /// Sets how many times the animation should be repeated. If the repeat count is 0, the animation is never repeated.
    /// If the repeat count is greater than 0 or INFINITE, the repeat mode will be taken into account.
    /// The repeat count is 0 by default.
    /// </summary>
    public int RepeatCount
    {
        get { return (int)GetValue(RepeatCountProperty); }
        set { SetValue(RepeatCountProperty, value); }
    }

    /// <summary>
    /// Indicates if the Animation is playing
    /// </summary>
    public bool IsAnimating
    {
        get { return (bool)GetValue(IsAnimatingProperty); }
        internal set { SetValue(IsAnimatingProperty, value); }
    }

    //// <summary>
    //// If you use image assets, you must explicitly specify the folder in assets/ in which they are located because bodymovin uses the name filenames across all compositions
    //// </summary>
    public string ImageAssetsFolder
    {
        get { return (string)GetValue(ImageAssetsFolderProperty); }
        set { SetValue(ImageAssetsFolderProperty, value); }
    }

    //// <summary>
    //// Set the scale on the current composition. 
    //// The only cost of this function is re-rendering the current frame so you may call it frequent to scale something up or down.
    //// </summary>
    //public new float Scale
    //{
    //    get { return (float)GetValue(ScaleProperty); }
    //    set { SetValue(ScaleProperty, value); }
    //}

    /// <summary>
    /// Sets the progress to the specified frame.
    /// If the composition isn't set yet, the progress will be set to the frame when it is.
    /// </summary>
    public int AnitmationFrame
    {
        get { return (int)GetValue(AnimationFrameProperty); }
        set { SetValue(AnimationFrameProperty, value); }
    }

    /// <summary>
    /// Returns the current progress of the animation
    /// </summary>
    public float Progress
    {
        get { return (float)GetValue(ProgressProperty); }
        set { SetValue(ProgressProperty, value); }
    }

    /// <summary>
    /// When true the Lottie animation will automatically start playing when loaded
    /// </summary>
    public bool AutoPlay
    {
        get { return (bool)GetValue(AutoPlayProperty); }
        set { SetValue(AutoPlayProperty, value); }
    }

    /// <summary>
    /// Will be called when the view is clicked
    /// </summary>
    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    /// <summary>
    /// When true the Lottie animation will enable merge paths for devices with KitKat and above
    /// </summary>
    public bool EnableMergePathsForKitKatAndAbove
    {
        get { return (bool)GetValue(EnableMergePathsForKitKatAndAboveProperty); }
        set { SetValue(EnableMergePathsForKitKatAndAboveProperty, value); }
    }

    /// <summary>
    /// Called when the Lottie animation starts playing
    /// </summary>
    public event EventHandler OnPlayAnimation;

    /// <summary>
    /// Called when the Lottie animation is paused
    /// </summary>
    public event EventHandler OnPauseAnimation;

    /// <summary>
    /// Called when the Lottie animation is resumed after pausing
    /// </summary>
    public event EventHandler OnResumeAnimation;

    /// <summary>
    /// Called when the Lottie animation is stopped
    /// </summary>
    public event EventHandler OnStopAnimation;

    /// <summary>
    /// Called when the Lottie animation is repeated
    /// </summary>
    public event EventHandler OnRepeatAnimation;

    /// <summary>
    /// Called when the Lottie animation is playing with the current progress
    /// </summary>
    public event EventHandler<float> OnAnimationUpdate;

    /// <summary>
    /// Called when the Lottie animation is loaded with the Lottie Composition as parameter
    /// </summary>
    public event EventHandler<object> OnAnimationLoaded;

    /// <summary>
    /// Called when the animation fails to load or when an exception happened when trying to play
    /// </summary>
    public event EventHandler<Exception> OnFailure;

    /// <summary>
    /// Called when the Lottie animation is finished playing
    /// </summary>
    public event EventHandler OnFinishedAnimation;

    internal void InvokePlayAnimation()
    {
        OnPlayAnimation?.Invoke(this, EventArgs.Empty);
    }

    internal void InvokeResumeAnimation()
    {
        OnResumeAnimation?.Invoke(this, EventArgs.Empty);
    }

    internal void InvokeStopAnimation()
    {
        OnStopAnimation?.Invoke(this, EventArgs.Empty);
    }

    internal void InvokePauseAnimation()
    {
        OnPauseAnimation?.Invoke(this, EventArgs.Empty);
    }

    internal void InvokeRepeatAnimation()
    {
        OnRepeatAnimation?.Invoke(this, EventArgs.Empty);
    }

    internal void InvokeAnimationUpdate(float progress)
    {
        OnAnimationUpdate?.Invoke(this, progress);
    }

    internal void InvokeAnimationLoaded(object animation)
    {
        OnAnimationLoaded?.Invoke(this, animation);
    }

    internal void InvokeFailure(Exception ex)
    {
        OnFailure?.Invoke(this, ex);
    }

    internal void InvokeFinishedAnimation()
    {
        OnFinishedAnimation?.Invoke(this, EventArgs.Empty);
    }

    internal ICommand PlayCommand { get; set; }
    internal ICommand PauseCommand { get; set; }
    internal ICommand ResumeCommand { get; set; }
    internal ICommand StopCommand { get; set; }
    internal ICommand ClickCommand { get; set; }
    internal ICommand PlayMinAndMaxFrameCommand { get; set; }
    internal ICommand PlayMinAndMaxProgressCommand { get; set; }
    internal ICommand ReverseAnimationSpeedCommand { get; set; }

    /// <summary>
    /// Plays the animation from the beginning. If speed is &lt; 0, it will start at the end and play towards the beginning
    /// </summary>
    public void PlayAnimation()
    {
        PlayCommand.Execute(null);
    }

    /// <summary>
    /// Continues playing the animation from its current position. If speed &lt; 0, it will play backwards from the current position.
    /// </summary>
    public void ResumeAnimation()
    {
        ResumeCommand.Execute(null);
    }

    /// <summary>
    /// Will stop and reset the currently playing animation
    /// </summary>
    public void StopAnimation()
    {
        StopCommand.Execute(null);
    }

    /// <summary>
    /// Will pause the currently playing animation. Call ResumeAnimation to continue
    /// </summary>
    public void PauseAnimation()
    {
        PauseCommand.Execute(null);
    }

    public void PlayMinAndMaxFrame(int minFrame, int maxFrame)
    {
        PlayMinAndMaxFrameCommand.Execute((minFrame, maxFrame));
    }

    public void PlayMinAndMaxProgress(float minProgress, float maxProgress)
    {
        PlayMinAndMaxProgressCommand.Execute((minProgress, maxProgress));
    }

    /// <summary>
    /// Reverses the current animation speed. This does NOT play the animation.
    /// </summary>
    public void ReverseAnimationSpeed()
    {
        ReverseAnimationSpeedCommand.Execute(null);
    }

    public void SetAnimationFromAssetOrBundle(string path)
    {
        AnimationSource = AnimationSource.AssetOrBundle;
        Animation = path;
    }

    public void SetAnimationFromEmbeddedResource(string resourceName, Assembly assembly = null)
    {
        //AnimationSource = AnimationSource.EmbeddedResource;

        //if (assembly == null)
        //    assembly = Xamarin.Forms.Application.Current.GetType().Assembly;

        //Animation = $"resource://{resourceName}?assembly={Uri.EscapeUriString(assembly.FullName)}";
    }

    public void SetAnimationFromJson(string json)
    {
        AnimationSource = AnimationSource.Json;
        Animation = json;
    }

    public void SetAnimationFromUrl(string url)
    {
        AnimationSource = AnimationSource.Url;
        Animation = url;
    }

    public void SetAnimationFromStream(Stream stream)
    {
        AnimationSource = AnimationSource.Stream;
        Animation = stream;
    }
}