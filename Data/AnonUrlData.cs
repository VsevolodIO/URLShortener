using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using URLShortener.Models;
using URLShortener.Services.Rules;
using URLShortener.Services.Rules.AnonUrlRules;

namespace URLShortener.Data
{
    public class AnonUrlData : IAnonUrlData
    {
        private readonly UrlShortenerDbContext _db;
        private readonly IHttpContextAccessor _accessor;
        private readonly IAnonUrlRuleProcessor _anonUrlRuleProcessor;

        public AnonUrlData(UrlShortenerDbContext db, IHttpContextAccessor accessor, IAnonUrlRuleProcessor anonUrlRuleProcessor)
        {
            _db = db;
            _accessor = accessor;
            _anonUrlRuleProcessor = anonUrlRuleProcessor;
        }

        public async Task<AnonUrlResult> AddUrlAsync(AnonUrl newAnonUrl)
        {
            newAnonUrl.RedirectCount = 0;
            newAnonUrl.DateCreated = DateTime.Now;
            newAnonUrl.IpAddress = _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var (passedRules, errors) = await _anonUrlRuleProcessor.PassesAllRulesAsync(newAnonUrl);

            if (!passedRules)
                return AnonUrlResult.Failure(errors);

            await _db.AnonUrls.AddAsync(newAnonUrl);
            await _db.SaveChangesAsync();
            return AnonUrlResult.Success(newAnonUrl);
        }

        public async Task<bool> DeleteUrlAsync(AnonUrl anonUrlToDelete)
        {
            if (await _db.AnonUrls.AnyAsync(u => u.Id == anonUrlToDelete.Id))
            {
                _db.AnonUrls.Remove(anonUrlToDelete);
                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public IQueryable<AnonUrl> GetAllUrls()
        {
            return _db.AnonUrls.OrderByDescending(u => u.DateCreated);
        }

        public async Task<AnonUrl> GetUrlByIdAsync(int id)
        {
            return await _db.AnonUrls.FindAsync(id);
        }

        public async Task<AnonUrl> GetUrlByShortUrlAsync(string shortUrl)
        {

            if (await _db.AnonUrls.AnyAsync(u => u.ShortUrl == shortUrl))
            {
                return _db.AnonUrls.FirstOrDefault(u => u.ShortUrl == shortUrl);
            }
            return null;
        }

        public bool ShortUrlExistsGlobal(string shortUrl)
        {
            bool check = _db.UserUrls.Any(u => u.ShortUrl == shortUrl) ||
                         _db.AnonUrls.Any(u => u.ShortUrl == shortUrl);
            return check;
        }
        
        public async Task<AnonUrl> UpdateCountByShortUrlAsync(string shortUrl)
        {
            if (await _db.AnonUrls.AnyAsync(u => u.ShortUrl == shortUrl))
            {
                var urlModel = await GetUrlByShortUrlAsync(shortUrl);
                urlModel.RedirectCount++;
                await UpdateUrlAsync(urlModel);
                return urlModel;
            }

            return null;
        }

        

        public async Task<AnonUrl> UpdateUrlAsync(AnonUrl updatedAnonUrl)
        {
            if (await _db.AnonUrls.AnyAsync(u => u.Id == updatedAnonUrl.Id))
            {
                _db.AnonUrls.Update(updatedAnonUrl);
                await _db.SaveChangesAsync();
                return updatedAnonUrl;
            }

            return null;
        }

    }
}
