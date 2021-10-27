namespace URLShortener.Models.ViewModels
{
    public class UrlsFilterViewModel
    {
        public string SelectedName { get; private set; }  
        public UrlsFilterViewModel(string url)
        {
            SelectedName = url;
        }
    }
}
