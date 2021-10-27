using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using URLShortener.Services.Configurations;

namespace URLShortener.Services
{
    public class GoogleRecaptchaService : IRecaptchaService
    {
        private readonly IOptions<RecaptchaConfiguration> _config;
        private readonly HttpClient _httpClient;

        public GoogleRecaptchaService(IOptions<RecaptchaConfiguration> config)
        {
            _config = config;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.google.com")
            };
        }

        public async Task<RecaptchaResponse> Validate(IFormCollection form)
        {
            var gRecaptchaResponse = form["g-recaptcha-response"];
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("secret", _config.Value.SecretKey),
                new KeyValuePair<string, string>("response", gRecaptchaResponse)
            });

            var response = await _httpClient.PostAsync("/recaptcha/api/siteverify", content);
            var resultContent = await response.Content.ReadAsStringAsync();
            var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(resultContent);

            return captchaResponse;
        }
    }
}
