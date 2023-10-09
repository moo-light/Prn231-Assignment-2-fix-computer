using Domain;
using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;

namespace FUCarRentingSystem_RazorPage.Pages.User.Transaction.Reviews
{
    public class RateModel : PageModel
    {
        private readonly HttpClient _client;

        public RateModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.ReviewsApi;
        }



        [BindProperty]
        public Review? Review { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int CarId
        {
            get; set;
        }

        public string PageUri { get; }
        public IActionResult OnGet()
        {

            var customerId = HttpContext.Session.GetInt32("id");
            Review = new Review
            {
                CarId = CarId,
                CustomerId = customerId.Value
            };
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            await Console.Out.WriteLineAsync(Review.Serialize());
            if (!ModelState.IsValid) return Page();
            var content = new StringContent(Review.Serialize(), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(PageUri, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("../Index");
            }
            return Page();
        }
    }
}
