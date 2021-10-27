using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace URLShortener.Models
{
    public class AnonUrl
    {
        public int Id { get; set; }

        [Required(ErrorMessage="The URL is required")]
        [MaxLength(500)]
        [Url(ErrorMessage = "Enter correct URL (include http:// etc.)")]
        [DisplayName("Full URL")]
        public string FullUrl { get; set; }

        [DisplayName("Short URL")]
        public string ShortUrl { get; set; }

        [DisplayName("Created Date")]
        public DateTime? DateCreated { get; set; }


        [DefaultValue(0)]
        [DisplayName("Redirect Counts")]
        public int? RedirectCount { get; set; }

        public string IpAddress { get; set; }


    }
}
