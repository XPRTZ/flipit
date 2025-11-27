using FastEndpoints;
using Xprtz.FlipIt.Application;
using Xprtz.FlipIt.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();

builder.Services.AddFastEndpoints();

var app = builder.Build();

app.UseInfrastructure();

app.UseCors();

app.UseExceptionHandler();

app.UseFastEndpoints();

app.Run();

namespace Xprtz.FlipIt.ApiService
{
    public partial class Program { }
}
