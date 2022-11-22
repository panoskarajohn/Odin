var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddIdentityServer();
//
//    .AddInMemoryApiResources(Config.GetApiResources())
//    .AddInMemoryClients(Config.GetClients())
//    .AddInMemoryIdentityResources(Config.GetIdentityResources())
//    .AddTestUsers(Config.GetUsers())
//    .AddDeveloperSigningCredential();


var app = builder.Build();

app.UseIdentityServer();

app.MapGet("/", () => "Hello World!");

app.Run();
