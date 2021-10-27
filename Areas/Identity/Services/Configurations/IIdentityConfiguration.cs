using System.Collections.Generic;

namespace URLShortener.Areas.Identity.Services.Configurations
{
    public interface IIdentityConfiguration
    {
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public List<string> Roles { get; set; }
    }
}
