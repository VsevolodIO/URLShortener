using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Data;
using URLShortener.Models;
using URLShortener.Models.ViewModels;
using URLShortener.Services;
using URLShortener.Areas.Identity.Authorization;


namespace URLShortener.Controllers
{
    //[Authorize(Roles="Member")]
    //[AllowAnonymous]
    public class UrlsController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserUrlData _userUrlData;
        private readonly IUrlGenerator _urlGenerator;


        public UrlsController(IAuthorizationService authorizationService, IUserUrlData userUrlData, IUrlGenerator urlGenerator)
        {
            _authorizationService = authorizationService;
            _userUrlData = userUrlData;
            _urlGenerator = urlGenerator;
        }

        // GET: UrlsController
        [Route("[controller]")]
        public IActionResult Index(string s, int p = 1, SortState sort = SortState.DateCreatedDesc)
        {
            int pageSize = 10;

            var urls = _userUrlData.GetCurrentUserUrls();
                
            if (!string.IsNullOrEmpty(s))
            {
                urls = urls.Where(u => u.FullUrl.Contains(s));
            }

            
            urls = sort switch
            {
                SortState.FullUrlDesc => urls.OrderByDescending(fld => fld.FullUrl),
                SortState.DateCreatedAsc => urls.OrderBy(fld => fld.DateCreated),
                SortState.DateCreatedDesc => urls.OrderByDescending(fld => fld.DateCreated),
                SortState.RedirectCountAsc => urls.OrderBy(fld => fld.RedirectCount),
                SortState.RedirectCountDesc => urls.OrderByDescending(fld => fld.RedirectCount),
                _ => urls.OrderByDescending(fld => fld.DateCreated),
            };

            var count =  urls.Count();
            urls = urls.Skip((p - 1) * pageSize).Take(pageSize);

            var indexViewModel = new UrlsIndexViewModel<UserUrl>
            {
                UrlPageViewModel = new UrlPageViewModel(count, p, pageSize),
                UrlSortViewModel = new UrlSortViewModel(sort),
                UrlsFilterViewModel = new UrlsFilterViewModel(s),
                UrlModels = urls
                
            };
            
            return View(indexViewModel);
        }

        // GET: UrlsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UrlsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<UserUrl>> Create([FromServices] IRecaptchaService recaptchaService, UserUrl model)
        {
            var captchaResponse = await recaptchaService.Validate(Request.Form);
            if (!captchaResponse.Success)
            {
                ModelState.AddModelError("reCaptchaError", "reCAPTCHA error occured. Please try again.");
            }
            if (ModelState.IsValid)
            {
                string newShortUrl = _urlGenerator.GetRandomShortUrl();
                while (_userUrlData.ShortUrlExistsGlobal(newShortUrl))
                {
                    newShortUrl = _urlGenerator.GetRandomShortUrl();
                }

                model.ShortUrl = newShortUrl;
                await _userUrlData.AddUrlAsync(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: UrlsController/Edit/5
        public async Task<ActionResult<UserUrl>> Edit(int id)
        {
            var urlModel = await _userUrlData.GetUrlByIdAsync(id);

            var isAuthorized = await _authorizationService.AuthorizeAsync(User, urlModel, UrlOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            if (urlModel != null)
            {
                return View(urlModel);
            }

            return NotFound();

        }

        // POST: UrlsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult<UserUrl>> Edit(UserUrl model)
        {
            if (ModelState.IsValid)
            {
                var isAuthorized = await _authorizationService.AuthorizeAsync(User, model, UrlOperations.Update);
                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }
                await _userUrlData.UpdateUrlAsync(model);
                return RedirectToAction("Index");
            }

            return View(model);

        }

        // GET: UrlsController/Delete/5
        public async Task<ActionResult<UserUrl>> Delete(int id)
        {
            var urlModel = await _userUrlData.GetUrlByIdAsync(id);

            if (urlModel != null)
            {

                var isAuthorized = await _authorizationService.AuthorizeAsync(User, urlModel, UrlOperations.Delete);
                if (!isAuthorized.Succeeded)
                {
                    return Forbid();
                }

                if (await _userUrlData.DeleteUrlAsync(urlModel))
                {
                    return RedirectToAction("Index");
                }
                else return NotFound();
            }
            else return NotFound();
        }

        
    }
}
