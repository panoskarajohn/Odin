using Backoffice;
using Backoffice.ApiServices;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.
services.AddControllersWithViews();

services.AddScoped<IEventService, EventService>();
services.AddTransient<AuthenticationDelegateHandler>(); //Authentication Middleware
services.AddHttpClient<IEventService, EventService>("EventApi",client =>
{
    client.BaseAddress = new Uri("http://localhost:2000");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<AuthenticationDelegateHandler>(); // Here we should delegate to an authentication handler

services.AddAuthentication(option =>
{
    option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, option =>
    {
        option.RequireHttpsMetadata = false;
        option.UsePkce = false;
        option.Authority = "http://localhost:5000";
        option.ClientId = "backoffice";
        option.ClientSecret = "secret";
        option.ResponseType = "code";
        option.Scope.Add("openid");
        option.Scope.Add("profile");
        option.SaveTokens = true;
        option.GetClaimsFromUserInfoEndpoint = true;
    });

services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

services.AddSingleton(new ClientCredentialsTokenRequest()
{
    Address = "http://localhost:5000/connect/token",
    ClientId = "eventClient",
    ClientSecret = "secret",
    Scope = "eventApi"
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();