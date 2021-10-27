namespace URLShortener.Areas.Identity.Services.Configurations
{
    public interface ISendGridConfiguration
    {
        public string SendGridSenderName { get; set; }
        public string SendGridApiKey { get; set; }
        public string SendGridFrom { get; set; }
    }
}
