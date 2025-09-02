using System.Text.Json.Serialization;
using Blazorise;
using Blazorise.Icons.Material;
using Blazorise.Material;
using Refit;
using Xprtz.FlipIt.Contract.Cards;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Contract.Topics;
using Xprtz.FlipIt.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

AddBlazorise(builder.Services);

var serializer = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
serializer.Converters.Remove(serializer.Converters.Single(x => x is JsonStringEnumConverter));

var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(serializer)
};

builder
    .Services.AddRefitClient<ITopicApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new("http://localhost:5411"));

builder
    .Services.AddRefitClient<ICardApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new("http://localhost:5411"));

builder
    .Services.AddRefitClient<IQuizApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new("http://localhost:5411"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Xprtz.FlipIt.Web.Client._Imports).Assembly);

app.Run();

void AddBlazorise(IServiceCollection services)
{
    services.AddBlazorise();
    services.AddMaterialProviders().AddMaterialIcons();
}
