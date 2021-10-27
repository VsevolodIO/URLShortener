using System.Collections.Generic;

namespace URLShortener.Models.ViewModels
{
    public class UrlsIndexViewModel<T>
    {
        public IEnumerable<T> UrlModels { get; set; }
        public UrlPageViewModel UrlPageViewModel { get; set; }
        public UrlSortViewModel UrlSortViewModel { get; set; }
        public UrlsFilterViewModel UrlsFilterViewModel { get; set; }
    }
}
