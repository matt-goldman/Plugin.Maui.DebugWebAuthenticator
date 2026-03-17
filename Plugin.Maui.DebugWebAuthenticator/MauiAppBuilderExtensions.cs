namespace Plugin.Maui.DebugWebAuthenticator;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseDebugWebAuthenticator(this MauiAppBuilder builder)
    {
#if DEBUG
        builder.Services.AddSingleton<IWebAuthenticator, WebAuthenticator>();
#else
        builder.Services.AddSingleton<IWebAuthenticator, Microsoft.Maui.Authentication.WebAuthenticator>();
#endif
        return builder;
    }
}
