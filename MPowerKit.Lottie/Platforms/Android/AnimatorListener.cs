using Android.Animation;

namespace MPowerKit.Lottie;

public class AnimatorListener : AnimatorListenerAdapter
{
    public Action? Cancelled { get; set; }
    public Action? Ended { get; set; }
    public Action? Paused { get; set; }
    public Action? Repeated { get; set; }
    public Action? Resumed { get; set; }
    public Action? Started { get; set; }

    public override void OnAnimationCancel(Animator? animation)
    {
        base.OnAnimationCancel(animation);
        Cancelled?.Invoke();
    }

    public override void OnAnimationEnd(Animator? animation)
    {
        base.OnAnimationEnd(animation);
        Ended?.Invoke();
    }

    public override void OnAnimationPause(Animator? animation)
    {
        base.OnAnimationPause(animation);
        Paused?.Invoke();
    }

    public override void OnAnimationRepeat(Animator? animation)
    {
        base.OnAnimationRepeat(animation);
        Repeated?.Invoke();
    }

    public override void OnAnimationResume(Animator? animation)
    {
        base.OnAnimationResume(animation);
        Resumed?.Invoke();
    }

    public override void OnAnimationStart(Animator? animation)
    {
        base.OnAnimationStart(animation);
        Started?.Invoke();
    }
}
