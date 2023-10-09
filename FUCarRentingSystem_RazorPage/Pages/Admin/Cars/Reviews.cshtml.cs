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
using System.Reflection.Metadata;
using FUCarRentingSystem_RazorPage.Utils;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Cars
{
    public class ReviewsModel : PageModel
    {
        private readonly HttpClient _client;

        public ReviewsModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IList<Review>? Reviews { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Reviews = await _client.GetAsync<List<Review>>(Constants.ApiRoute.ReviewsApi, new Dictionary<string, object?>
            {
                {"$filter",$"{nameof(Review.CarId)} eq {id}" },
                {"$orderBy",$"{nameof(Review.ReviewStar)} desc" }
            });
            if (Reviews == null) return NotFound();
            return Page();
        }
    }
}
