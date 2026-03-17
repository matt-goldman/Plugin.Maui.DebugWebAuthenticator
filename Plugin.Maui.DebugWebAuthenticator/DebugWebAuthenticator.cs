namespace Plugin.Maui.DebugWebAuthenticator;

public static class DebugWebAuthenticator
{
    public static IWebAuthenticator Default { get; } = Microsoft.Maui.Authentication.WebAuthenticator.Default;
    public static IWebAuthenticator Debug { get; } = new WebAuthenticator();

    private static IWebAuthenticator _current;

    static DebugWebAuthenticator()
    {
#if DEBUG
        _current = Debug;
#else
        _current = Default;
#endif
    }

    public static Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions)
        => _current.AuthenticateAsync(webAuthenticatorOptions);

    public static Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions, CancellationToken cancellationToken)
        => _current.AuthenticateAsync(webAuthenticatorOptions, cancellationToken);
}
