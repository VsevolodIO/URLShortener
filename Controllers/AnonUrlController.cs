using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Areas.Identity.Attributes;
using URLShortener.Data;
using URLShortener.Models;
using URLShortener.Services;

namespace URLShortener.Controllers
{
    [Route("")]
    [AllowAnonymous]
    [AllowAnonymousOnly]
    public class AnonUrlController : Controller
    {
        private readonly IAnonUrlData _anonUrlData;
        private readonly IUrlGenerator _urlGenerator;

        public AnonUrlController(IAnonUrlData anonUrlData, IUrlGenerator urlGenerator)
        {
            _anonUrlData = anonUrlData;
            _urlGenerator = urlGenerator;
        }

        // GET: AnonUrlController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AnonUrl model)
        {
            if (ModelState.IsValid)
            {
                string newShortUrl = _urlGenerator.GetRandomShortUrl();
                while (_anonUrlData.ShortUrlExistsGlobal(newShortUrl))
                {
                    newShortUrl = _urlGenerator.GetRandomShortUrl();
                }

                model.ShortUrl = newShortUrl;
                var result = await _anonUrlData.AddUrlAsync(model);
                if(result.UrlCreatingSuccessful)
                {
                    TempData["AddUrlSuccess"] = true;
                    HttpContext.Session.SetString("createdShortUrl", model.ShortUrl);
                    return RedirectToAction("success");
                }

                ViewBag.Errors = result.Errors;
                return View(model);

            }

            return View(model);
        }

        //TODO: add logic for requests with empty session data or refactor with JS
        [Route("success")]
        public ActionResult Success(AnonUrl model)
        {
            ViewBag.ShortUrl = HttpContext.Session.GetString("createdShortUrl");
            return View(model);
        }
    }
}
