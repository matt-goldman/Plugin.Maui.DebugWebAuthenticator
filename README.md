![Package icon](/assets/icon.png)

# Plugin.Maui.DebugWebAuthenticator

[![NuGet Version](https://img.shields.io/nuget/v/Plugin.Maui.DebugWebAuthenticator)](https://www.nuget.org/packages/Plugin.Maui.DebugWebAuthenticator)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Plugin.Maui.DebugWebAuthenticator)](https://www.nuget.org/packages/Plugin.Maui.DebugWebAuthenticator)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

An `IWebAuthenticator` implementation that uses an embedded `WebView` instead of the system browser or custom tabs.

## Why?

When using `WebAuthenticator` in a .NET MAUI app on a mobile device, particularly Android, when the system browser or custom tabs are displayed, the operating system backgrounds the app. If you're debugging, after a couple of seconds the debugger considers the app unresponsive and terminates. This then leaves nothing running for the callback to pass the result URL back to. At best the OS starts your app but there is no in-memory authentication state to resume.

A simple workaround is to use an embedded browser instead of switching out of the app. This can lead to App Store and Google Play Store rejection, but is perfectly fine for debugging.

## Getting Started

### Installation

Install the NuGet package:

```xml
<PackageReference Include="Plugin.Maui.DebugWebAuthenticator" Version="1.0.0" />
```

or

```bash
dotnet add package Plugin.Maui.DebugWebAuthenticator
```

### Using DebugWebAuthenticator

There are two options available for using the debug authenticator. One of them mimics the approach used by the built-in `WebAuthenticator`, the other is a more DI-friendly approach.

#### 1. Using via Dependency Injection

In your `MauiProgram.cs`:

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDebugWebAuthenticator(); // Add this line

        return builder.Build();
    }
}
```

This extension method uses compiler directives to register and implementation of `IWebAuthenticator`, either the default .NET MAUI `WebAuthenticator` in `Release` configuration, or the debug version in `Debug` configuration. You can then inject `IWebAuthenticator` into your consuming class and use as normal:

```csharp
public class MyAuthService(IWebAuthenticator authenticator)
{
    public async Task Authenticate(string authUrl, string callbackUrl)
    {
        var options = new WebAuthenticatorOptions
        {
            Url = new Uri(authUrl),
            CallbackUrl = new Uri(callbackUrl)
        }

        return authenticator.AuthenticateAsync(options);
    }
}
```

#### 2. Using via Static Instances

The static `DebugWebAuthenticator` class provides three ways to access an `IWebAuthenticator` instance:

* **Default**: Explicitly returns the default `WebAuthenticator` provided by the .NET MAUI library
* **Debug**: Explicity returns the debug web authenticator that uses the embedded `WebView`
* **Current**: Returns either the default `WebAuthenticator` in `Release` configuration or the debug web authenticator in `Debug` configuration

Examples:

```csharp
public class MyAuthService()
{
    public async Task Authenticate(string authUrl, string callbackUrl)
    {
        var options = new WebAuthenticatorOptions
        {
            Url = new Uri(authUrl),
            CallbackUrl = new Uri(callbackUrl)
        }

        return DebugWebAuthenticator.Current.AuthenticateAsync(options);
    }

    // or if you want more control:

    public async Task Authenticate(string authUrl, string callbackUrl)
    {
        var options = new WebAuthenticatorOptions
        {
            Url = new Uri(authUrl),
            CallbackUrl = new Uri(callbackUrl)
        }
#if DEBUG
        return DebugWebAuthenticator.Debug.AuthenticateAsync(options);
#else
        return DebugWebAuthenticator.Default.AuthenticateAsync(options);
#endif
    }
}
```