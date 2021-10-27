using System;
using Microsoft.AspNetCore.Identity;
using URLShortener.Areas.Identity.Data;

namespace URLShortener.Models.ViewModels.Manage
{
    public class UrlViewModel
    {
        public UrlViewModel()
        {
            
        }

        public UrlViewModel(UserUrl url)
        {
            Id = url.Id;
            User = new User
            {
                UserName = url.UserId
            };
            FullUrl = url.FullUrl;
            ShortUrl = url.ShortUrl;
            DateCreated = url.DateCreated;
            RedirectCount = url.RedirectCount;
            IpAddress = "-";
        }
        public UrlViewModel(AnonUrl url)
        {
            Id = url.Id;
            User = new User
            {
                UserName = "ANON"
            };
            FullUrl = url.FullUrl;
            ShortUrl = url.ShortUrl;
            DateCreated = url.DateCreated;
            RedirectCount = url.RedirectCount;
            IpAddress = url.IpAddress;
        }

        public int Id { get; set; }
        public User User { get; set; }
        public string FullUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? RedirectCount { get; set; }
        public string IpAddress { get; set; }
    }
}
