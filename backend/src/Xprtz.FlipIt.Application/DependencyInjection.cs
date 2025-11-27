using Microsoft.Extensions.DependencyInjection;
using Xprtz.FlipIt.Application.Quizzes.Services;

namespace Xprtz.FlipIt.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCqrs();
        services.AddRules();

        services.AddScoped<IQuestionsForNextQuizService, QuestionsForNextQuizService>();

        return services;
    }
}
