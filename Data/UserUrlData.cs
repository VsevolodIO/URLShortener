using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using URLShortener.Areas.Identity.Data;
using URLShortener.Models;

namespace URLShortener.Data
{
    public class UserUrlData : IUserUrlData
    {
        private readonly UrlShortenerDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserUrlData(UrlShortenerDbContext db, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<UserUrl> AddUrlAsync(UserUrl newUserUrl)
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            newUserUrl.RedirectCount = 0;
            newUserUrl.DateCreated = DateTime.Now;
            newUserUrl.UserId = currentUserId;
            newUserUrl.User = currentUser;

            currentUser.CreatedUrlsCount++;

            await _userManager.UpdateAsync(currentUser);
            await _db.UserUrls.AddAsync(newUserUrl);
            await _db.SaveChangesAsync();
            return newUserUrl;
        }

        public IQueryable<UserUrl> GetCurrentUserUrls()
        {
            var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _db.UserUrls.Where(u => u.UserId == currentUserId);
        }

        public IQueryable<UserUrl> GetUserUrlsById(string id)
        {
            return _db.UserUrls.Where(u => u.UserId == id);
        }

        public async Task<bool> DeleteUrlAsync(UserUrl userUrlToDelete)
        {
            if (await _db.UserUrls.AnyAsync(u => u.Id == userUrlToDelete.Id))
            {
                _db.UserUrls.Remove(userUrlToDelete);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IQueryable<UserUrl> GetAllUrls()
        {
            return _db.UserUrls.OrderByDescending(u => u.DateCreated).AsNoTracking();
        }

        public async Task<UserUrl> GetUrlByIdAsync(int id)
        {
            return await _db.UserUrls.FindAsync(id);
        }

        public async Task<UserUrl> GetUrlByShortUrlAsync(string shortUrl)
        {

            if (await _db.UserUrls.AnyAsync(u => u.ShortUrl == shortUrl))
            {
                return _db.UserUrls.FirstOrDefault(u => u.ShortUrl == shortUrl);
            }
            return null;
        }

        public bool ShortUrlExistsGlobal(string shortUrl)
        {
            bool check = _db.UserUrls.Any(u => u.ShortUrl == shortUrl) ||
                         _db.AnonUrls.Any(u => u.ShortUrl == shortUrl);
            return check;
        }


        public async Task<UserUrl> UpdateCountByShortUrlAsync(string shortUrl)
        {
            if (_db.UserUrls.Any(u => u.ShortUrl == shortUrl))
            {
                var urlModel = await GetUrlByShortUrlAsync(shortUrl);
                urlModel.RedirectCount++;
                await UpdateUrlAsync(urlModel);
                return urlModel;
            }
            return null;
        }

        public async Task<UserUrl> UpdateUrlAsync(UserUrl updatedUserUrl)
        {
            if (await _db.UserUrls.AnyAsync(u => u.Id == updatedUserUrl.Id))
            {
                _db.UserUrls.Update(updatedUserUrl);
                await _db.SaveChangesAsync();
                return updatedUserUrl;
            }

            return null;
        }

    }
}
