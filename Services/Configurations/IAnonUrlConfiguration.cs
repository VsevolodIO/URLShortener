﻿using System;

namespace URLShortener.Services.Configurations
{
    public interface IAnonUrlConfiguration
    {
        public int MaxAnonUrlsCreatedByIp { get; set; }
        public TimeSpan UnblockIpTimeSpan { get; set; }

    }
}
