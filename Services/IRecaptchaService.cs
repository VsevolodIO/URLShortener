using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace URLShortener.Services
{
    public interface IRecaptchaService
    {
        public Task<RecaptchaResponse> Validate(IFormCollection form);
    }
}
