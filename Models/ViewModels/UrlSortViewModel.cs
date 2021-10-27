namespace URLShortener.Models.ViewModels
{
    public class UrlSortViewModel
    {
        public SortState FullUrlSort { get; private set; }
        public SortState DateCreatedSort { get; private set; }
        public SortState RedirectCountSort { get; private set; }
        public SortState CurrentSortState { get; private set; }

        public UrlSortViewModel(SortState sortOrder)
        {
            FullUrlSort = sortOrder == SortState.FullUrlAsc ? SortState.FullUrlDesc : SortState.FullUrlAsc;
            DateCreatedSort = sortOrder == SortState.DateCreatedAsc ? SortState.DateCreatedDesc : SortState.DateCreatedAsc;
            RedirectCountSort = sortOrder == SortState.RedirectCountAsc ? SortState.RedirectCountDesc : SortState.RedirectCountAsc;
            CurrentSortState = sortOrder;
        }
    }
}
