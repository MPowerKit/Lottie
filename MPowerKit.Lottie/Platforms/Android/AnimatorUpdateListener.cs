using Android.Animation;

namespace MPowerKit.Lottie;

public class AnimatorUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
{
    public Action<float>? Updated { get; set; }

    public void OnAnimationUpdate(ValueAnimator animation)
    {
        ArgumentNullException.ThrowIfNull(animation?.AnimatedValue);

        Updated?.Invoke((float)animation.AnimatedValue!);
    }
}