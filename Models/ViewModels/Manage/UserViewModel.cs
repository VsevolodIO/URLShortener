using URLShortener.Areas.Identity.Data;

namespace URLShortener.Models.ViewModels.Manage
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            
        }
        public UserViewModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            EmailConfirmed = user.EmailConfirmed;
            CreatedUrlsCount = user.CreatedUrlsCount;
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public int CreatedUrlsCount { get; set; }

    }
}
