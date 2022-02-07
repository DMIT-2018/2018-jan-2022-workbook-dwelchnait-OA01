using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.SamplePages
{
    public class ControlsModel : PageModel
    {
        [TempData]
        public string Feedback { get; set; }

        [BindProperty]
        public string EmailText { get; set; }
        [BindProperty]
        public string PasswordText { get; set; }
        [BindProperty]
        public string DateTimeText { get; set; }

        [BindProperty]
        public string RadioMeal { get; set; }
        public string[] RadioMeals = new[] { "breakfast", "lunch", "dinner/supper", "snacks" };

        [BindProperty]
        public bool AcceptanceBox { get; set; } //remember value=true on input control

        [BindProperty]
        public string MessageBody { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPostText()
        {
            //this method is tied to the specific button on the form via
            //  the asp-page-handler attribute.
            //the form of the method name is OnPost then concatenate the
            //  value given to the handler attribute

            Feedback = $"Email {EmailText}; Password {PasswordText}; Date {DateTimeText}";
            return Page();
        }

        public IActionResult OnPostRadioCheckArea()
        {
            Feedback = $"Meal {RadioMeal}; Acceptance {AcceptanceBox}; Message {MessageBody}";
            return Page();
        }
    }

    public class SelectionList
    {
        public int ValueId { get; set; }
        public string DisplayText { get; set; }    
    }
}
