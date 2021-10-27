using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using URLShortener.Areas.Identity.Data;

namespace URLShortener.Data
{
    public class AdminData : IAdminData
    {
        private readonly UserManager<User> _userManager;

        public AdminData(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IQueryable<User> GetAllUsers()
        {
            return _userManager.Users;
        }

        public async Task<bool> UpdateUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var isUpdated = await _userManager.UpdateAsync(user);
            return isUpdated.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var isDeleted = await _userManager.DeleteAsync(user);
            return isDeleted.Succeeded;
        }
    }
}
