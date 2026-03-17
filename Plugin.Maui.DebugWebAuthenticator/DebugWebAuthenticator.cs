namespace Plugin.Maui.DebugWebAuthenticator;

public static class DebugWebAuthenticator
{
    public static IWebAuthenticator Default { get; } = Microsoft.Maui.Authentication.WebAuthenticator.Default;
    public static IWebAuthenticator Debug { get; } = new WebAuthenticator();

    public static IWebAuthenticator Current { get; }

    static DebugWebAuthenticator()
    {
#if DEBUG
        Current = Debug;
#else
        Current = Default;
#endif
    }

    public static Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions)
        => Current.AuthenticateAsync(webAuthenticatorOptions);

    public static Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions, CancellationToken cancellationToken)
        => Current.AuthenticateAsync(webAuthenticatorOptions, cancellationToken);
}
