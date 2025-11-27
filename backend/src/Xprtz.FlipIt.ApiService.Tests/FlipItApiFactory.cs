using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using Respawn;
using Testcontainers.MsSql;
using Xprtz.FlipIt.Contract;
using Xprtz.FlipIt.Contract.Cards;
using Xprtz.FlipIt.Contract.Quizzes;
using Xprtz.FlipIt.Contract.Topics;
using Xprtz.FlipIt.Domain.Common;
using Xprtz.FlipIt.Infrastructure.Common;
using Xprtz.FlipIt.Infrastructure.Persistence;
using Xunit;

namespace Xprtz.FlipIt.ApiService.Tests;

public class FlipItApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private static readonly DateTime DateTimeNowInTests = new(2000, 1, 1);

    private const int RandomNumberGeneratorSeed = 42;

    private readonly MsSqlContainer _container = new MsSqlBuilder().WithPassword("test123!").WithCleanUp(true).Build();

    private IServiceScope _scope = null!;
    private HttpClient _client = null!;
    private Respawner _respawner = null!;

    public IDateTimeProvider DateTimeProvider { get; private set; } = null!;

    internal FlipItDbContext DbContext { get; private set; } = null!;

    public ITopicApi TopicApi { get; private set; } = null!;
    public IQuizApi QuizApi { get; private set; } = null!;
    public ICardApi CardApi { get; private set; } = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<FlipItDbContext>));
            services.AddDbContext<FlipItDbContext>(options =>
                options.UseSqlServer(_container.GetConnectionString())
            );

            services.RemoveAll<IRandomNumberGenerator>();
            services.AddTransient<IRandomNumberGenerator>(_ => new RandomNumberGenerator(RandomNumberGeneratorSeed));

            services.RemoveAll<IDateTimeProvider>();

            var dateTimeProvider = A.Fake<IDateTimeProvider>();
            A.CallTo(() => dateTimeProvider.Now).Returns(DateTimeNowInTests);

            services.AddSingleton<IDateTimeProvider>(_ => dateTimeProvider);
        });
    }

    public async Task SeedData() => await TestData.SeedData(DbContext);

    public async Task ResetDatabase() => await _respawner.ResetAsync(_container.GetConnectionString());

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        _scope = Services.CreateScope();

        DateTimeProvider = _scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

        DbContext = _scope.ServiceProvider.GetRequiredService<FlipItDbContext>();

        _respawner = await Respawner.CreateAsync(
            _container.GetConnectionString(),
            new() { TablesToIgnore = ["__EFMigrationsHistory"], SchemasToInclude = ["dbo"] }
        );

        _client = CreateClient();

        var refitSettings = RefitSettingsFactory.Create();

        TopicApi = RestService.For<ITopicApi>(_client, refitSettings);
        CardApi = RestService.For<ICardApi>(_client, refitSettings);
        QuizApi = RestService.For<IQuizApi>(_client, refitSettings);
    }

    public new async Task DisposeAsync()
    {
        _client.Dispose();
        await DbContext.DisposeAsync();
        _scope.Dispose();
        await base.DisposeAsync();
        await _container.DisposeAsync();
    }
}
