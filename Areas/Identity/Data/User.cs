using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using URLShortener.Models;

namespace URLShortener.Areas.Identity.Data
{
    public class User : IdentityUser
    {
        public User()
        {
            UserUrls = new List<UserUrl>();
        }
        public IEnumerable<UserUrl> UserUrls { get; set; }

        public int CreatedUrlsCount { get; set; }
    }
}
