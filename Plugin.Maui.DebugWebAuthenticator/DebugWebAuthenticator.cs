namespace Plugin.Maui.DebugWebAuthenticator;

public static class DebugWebAuthenticator
{
    public static IWebAuthenticator Default { get; } = Microsoft.Maui.Authentication.WebAuthenticator.Default;
    public static IWebAuthenticator Debug { get; } = new WebAuthenticator();
}
