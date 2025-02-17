#!/bin/bash

echo "xcode build"

xcodebuild archive -destination "generic/platform=iOS" -project LottieProxy.xcodeproj -scheme LottieProxy -configuration Release -archivePath Output/Output-iphoneos SKIP_INSTALL=NO ENABLE_BITCODE=NO BUILD_LIBRARY_FOR_DISTRIBUTION=YES

xcodebuild archive -destination "generic/platform=iOS Simulator" -project LottieProxy.xcodeproj -scheme LottieProxy -configuration Release -archivePath Output/Output-iphonesimulator SKIP_INSTALL=NO ENABLE_BITCODE=NO BUILD_LIBRARY_FOR_DISTRIBUTION=YES

xcodebuild archive -destination "generic/platform=macOS,variant=Mac Catalyst" -project LottieProxy.xcodeproj -scheme LottieProxy -configuration Release -archivePath Output/Output-maccatalyst SKIP_INSTALL=NO ENABLE_BITCODE=NO BUILD_LIBRARY_FOR_DISTRIBUTION=YES

echo "create xcframework"
xcodebuild -create-xcframework -archive Output/Output-iphoneos.xcarchive -framework LottieProxy.framework -archive Output/Output-iphonesimulator.xcarchive -framework LottieProxy.framework -output Output/LottieProxy-ios.xcframework

xcodebuild -create-xcframework -archive Output/Output-maccatalyst.xcarchive -framework LottieProxy.framework -output Output/LottieProxy-maccatalyst.xcframework

echo "sharpie bind"
sharpie bind --sdk=iphoneos18.2 --output="Output" --namespace="Com.Airbnb.Lottie" --scope="Output/LottieProxy-ios.xcframework/ios-arm64/LottieProxy.framework/Headers" "Output/LottieProxy-ios.xcframework/ios-arm64/LottieProxy.framework/Headers/LottieProxy-Swift.h"
