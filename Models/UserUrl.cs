using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using URLShortener.Areas.Identity.Data;

namespace URLShortener.Models
{
    public class UserUrl
    {
        public int Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        [Required]
        [MaxLength(500)]
        [Url(ErrorMessage = "Must be a correct URL")]
        [DisplayName("Full URL")]
        public string FullUrl { get; set; }

        [DisplayName("Short URL")]
        public string ShortUrl { get; set; }

        [DisplayName("Created Date")]
        public DateTime? DateCreated { get; set; }


        [DefaultValue(0)]
        [DisplayName("Redirect Counts")]
        public int? RedirectCount { get; set; }

    }
}
