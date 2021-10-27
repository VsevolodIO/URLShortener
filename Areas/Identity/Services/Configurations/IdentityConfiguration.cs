using System.Collections.Generic;

namespace URLShortener.Areas.Identity.Services.Configurations
{
    public class IdentityConfiguration : IIdentityConfiguration
    {
        public string AdminEmail { get; set; }
        public string AdminPassword { get; set; }
        public List<string> Roles { get; set; }
    }
}
