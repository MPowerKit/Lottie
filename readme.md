# <img src="icon.png" width="70" height="70" /> MPowerKit.Lottie
#### This is a .NET MAUI and .NET Native library that parses [Adobe After Effects](http://www.adobe.com/products/aftereffects.html) animations exported as json with [Bodymovin](https://github.com/bodymovin/bodymovin) and renders them natively!

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/alexdobrynin)

Inspired by [BaseFlow's](https://github.com/Baseflow/LottieXamarin) Xamarin library, but ported for .NET MAUI and using latest native libraries. Added some extra features like ```HardwareAcceleration```, ```State``` of the animation and ```TintColor```.

## Support
Before opening an issue, be sure you have tested your case with a Sample project from this repo. If you are able to reproduce the issue, be sure this issue is not coming from the native libraries [lottie-android](https://github.com/airbnb/lottie-android) and [lottie-ios](https://github.com/airbnb/lottie-ios), which are used under the hood for this library.

If you feel the lack of functionality, feel free to open a PRs.

### Supported platfroms

* .NET8
* .NET8 for Android (min 6.0)
* .NET8 for iOS (min 13)
* .NET8 for MacCatalyst (min 13.1)

## Download

- Android: [![NuGet](https://img.shields.io/nuget/v/MPowerKit.Lottie.Android)](https://www.nuget.org/packages/MPowerKit.Lottie.Android)
- iOS / Mac Catalyst: [![NuGet](https://img.shields.io/nuget/v/MPowerKit.Lottie.MaciOS)](https://www.nuget.org/packages/MPowerKit.Lottie.MaciOS) (This library has limited set of functionality, but enough to play the animation)
- .NET MAUI: [![NuGet](https://img.shields.io/nuget/v/MPowerKit.Lottie)](https://www.nuget.org/packages/MPowerKit.Lottie)

For the first time, designers can create **and ship** beautiful animations without an engineer painstakingly recreating it by hand. They say a picture is worth 1,000 words so here are 13,000:

![Example1](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Example1.gif)


![Example2](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Example2.gif)


![Example3](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Example3.gif)


![Community](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Community%202_3.gif)


![Example4](https://raw.githubusercontent.com/airbnb/lottie-android/master/gifs/Example4.gif)

All of these animations were created in After Effects, exported with Bodymovin, and rendered natively with no additional engineering effort.

[Bodymovin](https://github.com/bodymovin/bodymovin) is an After Effects plugin created by Hernan Torrisi that exports After effects files as json and includes a javascript web player. We've built on top of his great work to extend its usage to Android, iOS, and React Native.

Read more about it on our [blog post](http://airbnb.design/introducing-lottie/)
Or get in touch on Twitter ([gpeal8](https://twitter.com/gpeal8)) or via lottie@airbnb.com

## Using Lottie for .NET MAUI

Add ```UseMPowerKitLottie()``` to your ```MauiProgram.cs``` file as next

```csharp
builder
    .UseMauiApp<App>()
    .UseMPowerKitLottie();
```

Default usage is:

```xaml
<mpk:LottieView Source="cat.json"
                HeightRequest="150"
                WidthRequest="150" />
```

**Note: if you want to play the animation just using the name of the json, be sure you've put it into Resources/Raw folder as ```MauiAsset```**

**Note: there are several readonly properties which should be bound only with ```Mode=OneWayToSource```**

The full list of properties you can find [here](https://github.com/MPowerKit/Lottie/blob/main/MPowerKit.Lottie/LottieView.cs).

**Also, you can bind actions to play/stop the animation from your ViewModel and invoke them from ViewModel as next:**

ViewModel:
```csharp
public class MainViewModel : BaseViewModel
{
    public Action PlayAction { get; set; }
    public Action StopAction { get; set; }

    private void Play()
    {
        PlayAction?.Invoke();
    }

    private void Stop()
    {
        StopAction?.Invoke();
    }
}
```

Your Xaml:
```xaml
<mpk:LottieView Source="cat.json"
                HeightRequest="150"
                WidthRequest="150"
                Play="{Binding PlayAction, Mode=OneWayToSource}"
                Stop="{Binding StopAction, Mode=OneWayToSource}" />
```

All list of bindable properties you may find here: https://github.com/MPowerKit/Lottie/blob/995df55475620e5708da9dcfe9bbef073a75559c/MPowerKit.Lottie/LottieView.cs#L163

### Using Lottie for Android
Read the official docs [here](https://airbnb.io/lottie/#/android?id=sample-app)

### Using Lottie for iOS / Mac Catalyst
Read the official docs [here](https://airbnb.io/lottie/#/ios?id=installing-lottie)
