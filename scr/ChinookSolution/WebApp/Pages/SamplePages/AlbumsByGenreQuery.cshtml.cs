#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
using WebApp.Helpers; //required to use the Paginator
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

        [BindProperty(SupportsGet = true)]
        public int? GenreId { get; set; }

        [BindProperty]
        public List<AlbumsListBy> AlbumsOfGenre { get; set; }

        #region Paginator variables
        //my desired page size
        private const int PAGE_SIZE = 5;
        //instance for Paginator
        public Paginator Pager { get; set; }
        
        #endregion

        //currentPage will appear on your Url as a Get Parameter value
        //  url address....?currentPage=n
        public void OnGet(int? currentPage)
        {
            //remember that this method executes as the page first comes up BEFORE
            //      anything has happened on the page (including its FIRST display)
            //any code in this method MUST handle missing data on query arguments


            GenreList = _genreServices.GetAllGenres();
            //the .Sort() method for List<T> class
            GenreList.Sort((x, y) => x.DisplayText.CompareTo(y.DisplayText));

            if (GenreId.HasValue && GenreId.Value > 0)
            {
                //installing the paginator
                //determine the page number to use on the paginator
                int pageNumber = currentPage.HasValue ? currentPage.Value : 1;

                //use the paginator to setup data needed for paging
                PageState current = new(pageNumber, PAGE_SIZE);

                //total rows in the complete query collection
                int totalrows = 0;

                //the RedirectToPage() causes a OnGet to execute
                //obtain your raw data for your query

                //for efficiency of data being transferred, we will pass the 
                //  current page number and the desired page size to the backend query
                //the returned collection will only have the rows of the whole query
                //  collection that will actually be shown (PAGE-SIZE or less)
                //the total number of records for the whole query collection will be
                //  returned as an out parameter. This value is need by the Paginator
                //  to set up its display
                AlbumsOfGenre = _albumServices.AlbumsByGenre((int)GenreId,
                                            pageNumber, PAGE_SIZE, out totalrows);

                //once the query is complete, use the returned row count in instantizating
                //  an instance of the Paginator
                Pager = new Paginator(totalrows, current);
            }
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
            return RedirectToPage(new { GenreId = GenreId}); //causes a Get request which will force OnGet() to execute
        }
    }
}
