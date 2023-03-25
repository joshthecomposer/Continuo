using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using ContinuoClient.Controllers;
using ContinuoClient.Frameworks;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSecret = builder.Configuration["JWTSettings:SecretKey"];

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// builder.Services.AddScoped<AuthenticationStateProvider, ProfileState>();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddScoped<UserController>();
builder.Services.AddScoped<ClientAuthController>();
builder.Services.AddScoped<InstrumentService>();
builder.Services.AddScoped<ClientPracticeTaskController>();
builder.Services.AddScoped<ApiRequestService>();
// builder.Services.AddScoped<ApiRequestServiceFramework>();
builder.Services.AddBlazoredLocalStorage();
// builder.Services.AddAuthentication("BasicAuthentication")
//     .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
