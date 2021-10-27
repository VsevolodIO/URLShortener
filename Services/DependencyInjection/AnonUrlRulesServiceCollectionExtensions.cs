using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using URLShortener.Services.Rules.AnonUrlRules;

namespace URLShortener.Services.DependencyInjection
{
    public static class AnonUrlRulesServiceCollectionExtensions
    {
        public static IServiceCollection AddAnonUrlRules(this IServiceCollection services)
        {

            services.TryAddScoped<IAnonUrlRule, MaxAnonUrlsByIp>();

            services.TryAddScoped<IAnonUrlRuleProcessor, AnonUrlRuleProcessor>();

            return services;
        }
    }
}
