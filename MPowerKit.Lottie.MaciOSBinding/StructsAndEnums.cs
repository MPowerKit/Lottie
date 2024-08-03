using ObjCRuntime;

namespace Com.Airbnb.Lottie
{
    [Native]
    public enum CompatibleBackgroundBehavior : long
    {
        Stop = 0,
        Pause = 1,
        PauseAndRestore = 2,
        ForceFinish = 3,
        ContinuePlaying = 4
    }

    [Native]
    public enum CompatibleLottieLoopMode : long
    {
        PlayOnce = 0,
        Loop = 1,
        AutoReverse = 2,
        Repeat = 3,
        RepeatBackwards = 4
    }

    [Native]
    public enum CompatibleRenderingEngineOption : long
    {
        Shared = 0,
        DefaultEngine = 1,
        Automatic = 2,
        MainThread = 3,
        CoreAnimation = 4
    }
}
