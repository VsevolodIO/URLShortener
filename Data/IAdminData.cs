using System.Linq;
using System.Threading.Tasks;
using URLShortener.Areas.Identity.Data;

namespace URLShortener.Data
{
    public interface IAdminData
    {
        IQueryable<User> GetAllUsers();
        Task<bool> UpdateUserAsync(string id);
        Task<bool> DeleteUserAsync(string id);
    }
}
