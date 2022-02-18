#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
#endregion

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        #region Private varilable and DI constructor
        //sets up the access to your services you desire to use for
        //  this page
        private readonly ILogger<IndexModel> _logger;
        private readonly AboutService _aboutServices;

        //accepting the injected services (dependency injection)
        public IndexModel(ILogger<IndexModel> logger,
                          AboutService aboutservices)
        {
            _logger = logger;
            _aboutServices = aboutservices;
        }
        #endregion

        #region FeedBack and ErrorHandling
        public string FeedBack { get; set; }
        public bool HasFeedBack => !string.IsNullOrWhiteSpace(FeedBack);
        #endregion
       
        public void OnGet()
        {
            DbVersionInfo info = _aboutServices.GetDbVersion();
            if (info == null)
            {
                FeedBack = "DbVersion Unknown";
            }
            else
            {
                FeedBack = $"Version: {info.Major}.{info.Minor}.{info.Build} " +
                    $" Release date of {info.ReleaseDate.ToShortDateString()}";
            }

        }
    }
}