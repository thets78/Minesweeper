using MinesWeeper.Components;
using MinesWeeper.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<MinesWeeperHighscoreApiClient>(client =>
{
    client.BaseAddress = new(builder.Configuration["MINESWEEPER_API_URL"] ?? "http://localhost");
});

builder.Services.AddHttpClient<UserServiceApiClient>(client =>
{
    client.BaseAddress = new(builder.Configuration["USERSERVICE_API_URL"] ?? "http://localhost");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
