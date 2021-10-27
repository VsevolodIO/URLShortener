using System;

namespace URLShortener.Services.Configurations
{
    public class AnonUrlConfiguration : IAnonUrlConfiguration
    {
        public int MaxAnonUrlsCreatedByIp { get; set; }
        public TimeSpan UnblockIpTimeSpan { get; set; }
    }
}
