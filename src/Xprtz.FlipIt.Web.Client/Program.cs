using Blazorise.Icons.Material;
using Blazorise.Material;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;
using Xprtz.FlipIt.Contract;
using Xprtz.FlipIt.Contract.Cards;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Contract.Topics;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

AddBlazorise(builder.Services);

var refitSettings = RefitSettingsFactory.Create();

builder
    .Services.AddRefitClient<ITopicApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new("http://localhost:5411"));

builder
    .Services.AddRefitClient<ICardApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new("http://localhost:5411"));

builder
    .Services.AddRefitClient<IQuizApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = new("http://localhost:5411"));

await builder.Build().RunAsync();

void AddBlazorise(IServiceCollection services)
{
    services.AddBlazorise();
    services.AddMaterialProviders().AddMaterialIcons();
}
