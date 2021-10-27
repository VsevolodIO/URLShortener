using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using URLShortener.Data;
using URLShortener.Models;
using URLShortener.Services.Configurations;

namespace URLShortener.Services.Rules.AnonUrlRules
{
    public class MaxAnonUrlsByIp : IAnonUrlRule
    {
        private readonly UrlShortenerDbContext _db;
        private readonly IOptions<AnonUrlConfiguration> _options;

        public MaxAnonUrlsByIp(UrlShortenerDbContext db, IOptions<AnonUrlConfiguration> options)
        {
            _db = db;
            _options = options;
        }
        public async Task<bool> CompliesWithRuleAsync(AnonUrl anonUrl)
        {
            var timeBorder = DateTime.Now.Subtract(_options.Value.UnblockIpTimeSpan);

            var urlsByIpCount = await _db.AnonUrls
                                            .Where(u => (timeBorder <= u.DateCreated) && 
                                                        (u.IpAddress == anonUrl.IpAddress))
                                            .CountAsync();

            return urlsByIpCount < _options.Value.MaxAnonUrlsCreatedByIp;
        }

        public string ErrorMessage =>
            $"Sign in to create more then {_options.Value.MaxAnonUrlsCreatedByIp} links";
    }
}
