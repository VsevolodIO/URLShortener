using System.Linq;
using System.Threading.Tasks;
using URLShortener.Models;

namespace URLShortener.Data
{
    public interface IUserUrlData : IGlobalUrlData<UserUrl>
    {
        Task<UserUrl> AddUrlAsync(UserUrl newUserUrl);
        IQueryable<UserUrl> GetCurrentUserUrls();
        IQueryable<UserUrl> GetUserUrlsById(string id);
    }
}
