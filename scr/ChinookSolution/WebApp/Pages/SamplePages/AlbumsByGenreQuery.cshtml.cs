#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
#endregion

namespace WebApp.Pages.SamplePages
{
    public class AlbumsByGenreQueryModel : PageModel
    {
        #region Private variable and DI constructor
        //sets up the access to your services you desire to use for
        //  this page
        private readonly ILogger<IndexModel> _logger;
        private readonly GenreServices _genreServices;
        private readonly AlbumServices _albumServices;

        //accepting the injected services (dependency injection)
        public AlbumsByGenreQueryModel(ILogger<IndexModel> logger,
                                        GenreServices genreservices,
                                        AlbumServices albumservices)
        {
            _logger = logger;
            _genreServices = genreservices;
            _albumServices = albumservices;
        }
        #endregion

        #region FeedBack and ErrorHandling
        [TempData]
        public string FeedBack { get; set; }
        public bool HasFeedBack => !string.IsNullOrWhiteSpace(FeedBack);

        [TempData]
        public string ErrorMsg { get; set; }
        public bool HasErrorMsg => !string.IsNullOrWhiteSpace(ErrorMsg);

        #endregion

        [BindProperty]
        public List<SelectionList> GenreList { get; set; }

        [BindProperty]
        public int GenreId { get; set; }

        public void OnGet()
        {
            GenreList = _genreServices.GetAllGenres();
            //the .Sort() method for List<T> class
            GenreList.Sort((x, y) => x.DisplayText.CompareTo(y.DisplayText));
        }

        public IActionResult OnPost()
        {
            if(GenreId == 0)
            {
                FeedBack = "You did not select a genre";
            }
            else
            {
                FeedBack = $"You selected genre id of {GenreId}";
            }
            return RedirectToPage(); //causes a Get request which will force OnGet() to execute
        }
    }
}
