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
    public event EventHandler<float>? AnimationLoaded;

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
    public void SendAnimationLoaded(float? duration)
    {
        IsAnimationLoaded = duration is not null;

        Duration = duration ?? 0f;

        AnimationLoaded?.Invoke(this, Duration);

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