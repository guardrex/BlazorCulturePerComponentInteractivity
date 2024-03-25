using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.JSInterop;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddLocalization();

var host = builder.Build();

CultureInfo culture;
var cultureCookieName = CookieRequestCultureProvider.DefaultCookieName;
var js = host.Services.GetRequiredService<IJSRuntime>();
var localizationCookie = await js.InvokeAsync<string>("blazorCulture.get", cultureCookieName);
var result = CookieRequestCultureProvider.ParseCookieValue(localizationCookie)?.UICultures?[0].Value;

if (result != null)
{
    culture = new CultureInfo(result);
}
else
{
    culture = new CultureInfo("en-US");
    await js.InvokeVoidAsync("blazorCulture.set", cultureCookieName, "en-US");
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
