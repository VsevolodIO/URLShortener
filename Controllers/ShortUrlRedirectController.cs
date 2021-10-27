using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Data;

namespace URLShortener.Controllers
{
    [AllowAnonymous]
    public class ShortUrlRedirectController : Controller
    {
        private readonly IAnonUrlData _anonGlobalUrlData;
        private readonly IUserUrlData _userGlobalUrlData;

        public ShortUrlRedirectController(IAnonUrlData anonGlobalUrlData, IUserUrlData userGlobalUrlData)
        {
            _anonGlobalUrlData = anonGlobalUrlData;
            _userGlobalUrlData = userGlobalUrlData;
        }
        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> Index(string shortUrl)
        {
            if (await _anonGlobalUrlData.UpdateCountByShortUrlAsync(shortUrl) == null && shortUrl != "Error")
            {
                if (await _userGlobalUrlData.UpdateCountByShortUrlAsync(shortUrl) == null)
                {
                    return RedirectToAction("Index", "AnonUrl");
                }
                var userUrlModel = await _userGlobalUrlData.GetUrlByShortUrlAsync(shortUrl);
                return Redirect(userUrlModel.FullUrl);
            }
            var anonUrlModel = await _anonGlobalUrlData.GetUrlByShortUrlAsync(shortUrl);
            return Redirect(anonUrlModel.FullUrl);
        }
    }
}
