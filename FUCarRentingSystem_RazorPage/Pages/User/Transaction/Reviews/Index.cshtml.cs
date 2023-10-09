using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using System.Net.Http.Headers;
using FUCarRentingSystem_RazorPage.Utils;
using OData.QueryBuilder.Builders;

namespace FUCarRentingSystem_RazorPage.Pages.User.Transaction.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;

        public IndexModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.ReviewsApi;
        }

        public Review Review { get; set; } = default!;
        public string PageUri { get; }
        [BindProperty(SupportsGet = true)]
        public int CarId { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var customerId = HttpContext.Session.GetInt32("id");
            var uri = new ODataQueryBuilder(PageUri).For<Review>($"{CarId},{customerId}").ByList().ToUri();
            var review = await _client.GetAsync<Review>(uri.ToString());
            if (review == null)
            {
                return NotFound();
            }
            else 
            {
                Review = review;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var customerId = HttpContext.Session.GetInt32("id");
            var uri = new ODataQueryBuilder(PageUri).For<Review>($"{CarId},{customerId}").ByList().ToUri();

            var review = await _client.GetAsync<Review>(uri.ToString());

            if (review != null)
            {
                Review = review;
                var response = await _client.DeleteAsync(uri.ToString());
                Console.WriteLine(response.IsSuccessStatusCode);
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["ErrorMessage"] = "Remove Failed!";
                    return Page();
                }
            }

            return RedirectToPage("../Index");
        }
    }
}
