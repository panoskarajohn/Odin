using Idenity.Api;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllersWithViews();
services.AddIdentityServer(opt =>
    {
        opt.Authentication.CookieSameSiteMode = SameSiteMode.Lax;
    })
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddTestUsers(Config.TestUsers)
    .AddDeveloperSigningCredential();


var app = builder.Build();

app.UseIdentityServer();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(endpoint =>
{
    endpoint.MapDefaultControllerRoute();
});
app.Run();
