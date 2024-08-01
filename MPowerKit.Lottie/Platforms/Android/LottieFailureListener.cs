using Com.Airbnb.Lottie;

namespace MPowerKit.Lottie;

public class LottieFailureListener : Java.Lang.Object, ILottieListener
{
    public Action<Exception>? Failed { get; set; }

    public void OnResult(Java.Lang.Object? error)
    {
        var javaError = error?.ToString();
        Failed?.Invoke(new Exception(javaError));
    }
}
