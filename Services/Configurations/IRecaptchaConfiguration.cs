namespace URLShortener.Services.Configurations
{
    public interface IRecaptchaConfiguration
    {
        public string SiteKey { get; set; }
        public string SecretKey { get; set; }
    }
}
