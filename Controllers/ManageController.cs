using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URLShortener.Areas.Identity.Data;
using URLShortener.Data;
using URLShortener.Models;
using URLShortener.Models.ViewModels;
using URLShortener.Models.ViewModels.Manage;

namespace URLShortener.Controllers
{
    [Authorize(Roles="Admin")]
    public class ManageController : Controller
    {
        private readonly IAnonUrlData _anonUrlData;
        private readonly IUserUrlData _urlData;
        private readonly IAdminData _adminData;
        private readonly UserManager<User> _userManager;

        public ManageController(IAnonUrlData anonUrlData, IUserUrlData urlData, IAdminData adminData, UserManager<User> userManager)
        {
            _anonUrlData = anonUrlData;
            _urlData = urlData;
            _adminData = adminData;
            _userManager = userManager;
        }

        // GET: ManageController
        [Route("[controller]")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: ManageController/Users
        public ActionResult Users()
        {
            var users = _adminData.GetAllUsers();
            if (users.Any())
            {
                var userViews = new List<UserViewModel>();

                foreach (var user in users)
                {
                    userViews.Add(new UserViewModel(user));
                }
                return View(userViews);
            }

            return RedirectToPage("Error");
        }

        public IActionResult UserUrls(string id)
        {
            var urls = _urlData.GetUserUrlsById(id);
            if (urls.Any())
            {
                return View(urls);
            }
            return new NotFoundResult();
        }

        // GET: ManageController/UserEdit/5
        public async Task<ActionResult> UserEdit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToPage("Error");
        }

        // POST: ManageController/UserEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserEdit(User user)
        {
            if (ModelState.IsValid)
            {
                if (await _adminData.UpdateUserAsync(user.Id))
                {
                    return RedirectToAction("Users");
                }
                return RedirectToPage("Error");
            }
            return View(user);
        }

        // GET: ManageController/UserDelete/5
        public async Task<ActionResult> UserDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToPage("Error");
        }

        // POST: ManageController/UserDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserDelete(User user)
        {
            if (user != null)
            {
                if (await _adminData.DeleteUserAsync(user.Id))
                {
                    return RedirectToAction("Users");
                }
                return RedirectToPage("Error");
            }
            return RedirectToPage("Error");
        }

        // GET: ManageController/Urls
        public ActionResult Urls(string s, int p = 1, SortState sort = SortState.DateCreatedAsc)
        {
            int pageSize = 10;

            var userUrls = _urlData.GetAllUrls().AsEnumerable();
            var anonUrls = _anonUrlData.GetAllUrls();
            IQueryable<UrlViewModel> allUrls = Enumerable.Empty<UrlViewModel>().AsQueryable();
            foreach (var url in userUrls)
            {
                allUrls = allUrls.Concat(new[] { new UrlViewModel(url) });;
            }

            foreach (var url in anonUrls)
            {
                allUrls = allUrls.Concat(new[] { new UrlViewModel(url) });
            }


            if (!string.IsNullOrEmpty(s))
            {
                allUrls = allUrls.Where(u => u.FullUrl.Contains(s));
            }


            allUrls = sort switch
            {
                SortState.FullUrlDesc => allUrls.OrderByDescending(s => s.FullUrl),
                SortState.DateCreatedAsc => allUrls.OrderBy(s => s.DateCreated),
                SortState.DateCreatedDesc => allUrls.OrderByDescending(s => s.DateCreated),
                SortState.RedirectCountAsc => allUrls.OrderBy(s => s.RedirectCount),
                SortState.RedirectCountDesc => allUrls.OrderByDescending(s => s.RedirectCount),
                _ => allUrls.OrderByDescending(s => s.DateCreated),
            };

            var count = allUrls.Count();
            allUrls = allUrls.Skip((p - 1) * pageSize).Take(pageSize);

            UrlsIndexViewModel<UrlViewModel> indexViewModel = new UrlsIndexViewModel<UrlViewModel>
            {
                UrlPageViewModel = new UrlPageViewModel(count, p, pageSize),
                UrlSortViewModel = new UrlSortViewModel(sort),
                UrlsFilterViewModel = new UrlsFilterViewModel(s),
                UrlModels = allUrls

            };

            return View(indexViewModel);
        }

        // GET: ManageController/UrlEdit/5
        public async Task<IActionResult> UrlEdit(string shortUrl)
        {
            var userUrl = await _urlData.GetUrlByShortUrlAsync(shortUrl);
            var anonUrl = await _anonUrlData.GetUrlByShortUrlAsync(shortUrl);

            if (userUrl != null)
            {
                return RedirectToAction("Edit", "Urls", new{ id = userUrl.Id});
            }

            if (anonUrl != null)
            {
                return View(anonUrl);
            }

            return RedirectToPage("Error");
        }

        // POST: ManageController/UrlEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrlEdit(AnonUrl model)
        {
            if (ModelState.IsValid)
            {
                await _anonUrlData.UpdateUrlAsync(model);
                return RedirectToAction("Urls");
            }

            return View(model);
        }

        // GET: ManageController/UrlDelete/5
        public async Task<IActionResult> UrlDelete(string shortUrl)
        {
            var userUrl = await _urlData.GetUrlByShortUrlAsync(shortUrl);
            var anonUrl = await _anonUrlData.GetUrlByShortUrlAsync(shortUrl);

            if (userUrl != null)
            {
                return RedirectToAction("Delete", "Urls", new { id = userUrl.Id });
            }

            if (anonUrl != null)
            {
                return View(anonUrl);
            }

            return RedirectToPage("Error");
        }

        // POST: ManageController/UrlDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UrlDelete(AnonUrl anonUrl)
        {
            if (anonUrl != null)
            {
                if (await _anonUrlData.DeleteUrlAsync(anonUrl))
                {
                    return RedirectToAction("Urls");
                }
                return RedirectToPage("Error");
            }
            return RedirectToPage("Error");
        }
    }
}
