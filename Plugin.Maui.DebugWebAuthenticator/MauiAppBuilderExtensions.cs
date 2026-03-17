using Microsoft.Extensions.Hosting;

namespace Plugin.Maui.DebugWebAuthenticator;

public static class MauiAppBuilderExtensions
{
    public static MauiAppBuilder UseDebugWebAuthenticator(this MauiAppBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {

            builder.Services.AddSingleton<IWebAuthenticator, WebAuthenticator>();
        }
        else
        {   
            builder.Services.AddSingleton<IWebAuthenticator>(Microsoft.Maui.Authentication.WebAuthenticator.Default);
        }

        return builder;
    }
}
