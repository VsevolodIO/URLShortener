using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Data
{
    public interface IGlobalUrlData<T>
    {
        IQueryable<T> GetAllUrls();
        Task<T> UpdateUrlAsync(T updatedUserUrlModel);
        Task<bool> DeleteUrlAsync(T urlModelToDelete);
        Task<T> GetUrlByIdAsync(int id);
        Task<T> GetUrlByShortUrlAsync(string shortUrl);
        bool ShortUrlExistsGlobal(string shortUrl);
        Task<T> UpdateCountByShortUrlAsync(string shortUrl);
    }
}
