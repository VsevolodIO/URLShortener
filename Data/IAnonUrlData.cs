using System.Threading.Tasks;
using URLShortener.Models;
using URLShortener.Services.Rules;

namespace URLShortener.Data
{
    public interface IAnonUrlData : IGlobalUrlData<AnonUrl>
    {
        Task<AnonUrlResult> AddUrlAsync(AnonUrl newUserUrl);
    }
}
