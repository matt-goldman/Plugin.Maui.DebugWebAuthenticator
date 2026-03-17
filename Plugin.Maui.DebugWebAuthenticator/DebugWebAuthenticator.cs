namespace Plugin.Maui.DebugWebAuthenticator;

internal class DebugWebAuthenticator : IWebAuthenticator
{
    public Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions) => AuthenticateAsync(webAuthenticatorOptions, CancellationToken.None);

    public async Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions, CancellationToken cancellationToken)
    {
        // check if URL and callback URL are not null
        if (webAuthenticatorOptions.Url == null || webAuthenticatorOptions.CallbackUrl == null)
        {
            return new WebAuthenticatorResult();
        }

        var tcs = new TaskCompletionSource<WebAuthenticatorResult>();
        var dismissed = false;

        var webView = new WebView
        {
            Source = new UrlWebViewSource { Url = webAuthenticatorOptions.Url.ToString() }
        };

        var page = new ContentPage { Content = webView };

        webView.Navigating += (_, e) =>
        {
            if (e.Url.StartsWith(webAuthenticatorOptions.CallbackUrl.OriginalString, StringComparison.OrdinalIgnoreCase))
            {
                e.Cancel = true;
                tcs.TrySetResult(new WebAuthenticatorResult(new Uri(e.Url)));
            }
        };

        page.Disappearing += (_, _) =>
        {
            dismissed = true;
            tcs.TrySetResult(new WebAuthenticatorResult());
        };

        using var reg = cancellationToken.Register(() =>
            tcs.TrySetResult(new WebAuthenticatorResult()));
        var navigation = Application.Current!.Windows[0].Page!.Navigation;
        await navigation.PushModalAsync(page);

        var result = await tcs.Task;

        if (!dismissed)
            await navigation.PopModalAsync();

        return result;
    }
}
