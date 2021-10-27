using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using URLShortener.Areas.Identity.Services.Configurations;
using URLShortener.Services.Configurations;

namespace URLShortener.Services.DependencyInjection
{
    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AnonUrlConfiguration>(config.GetSection("AnonUrlConfigurations"));

            services.Configure<SendGridConfiguration>(config.GetSection("SendGridConfigurations"));

            services.Configure<IdentityConfiguration>(config.GetSection("IdentityConfigurations"));

            services.Configure<RecaptchaConfiguration>(config.GetSection("GoogleReCaptchaV2"));

            services.TryAddSingleton<IAnonUrlConfiguration>(sp =>
                sp.GetRequiredService<IOptions<AnonUrlConfiguration>>().Value);

            services.TryAddSingleton<ISendGridConfiguration>(sp =>
                sp.GetRequiredService<IOptions<SendGridConfiguration>>().Value);

            services.TryAddSingleton<IIdentityConfiguration>(sp =>
                sp.GetRequiredService<IOptions<IdentityConfiguration>>().Value);

            services.TryAddSingleton<IRecaptchaConfiguration>(sp =>
                sp.GetRequiredService<IOptions<RecaptchaConfiguration>>().Value);

            return services;
        }
    }
}
