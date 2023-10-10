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
using NuGet.Protocol;
using OData.QueryBuilder.Builders;

namespace FUCarRentingSystem_RazorPage.Pages.User.Transaction
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;

        public IndexModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CarRentalsApi;
            PageUriReview = Constants.ApiRoute.ReviewsApi;
        }

        public IList<CarRental>? CarRental { get; set; } = default!;

        public IList<bool> HaveRate { get; set; } = default!;
        public string PageUri { get; }
        public string PageUriReview { get; }

        public async Task<IActionResult> OnGetAsync()
        {
            int? customerId = HttpContext.Session.GetInt32("id");

            string path = $"{PageUri}/?$filter=status eq true and customerId eq {customerId}" +
                $"&$expand=customer($select=id,customername),car($select=id,carname)" +
                $"&$orderby=PickupDate desc";
            CarRental = await _client.GetAsync<List<CarRental>>(path);

            path = new ODataQueryBuilder($"{PageUriReview}").For<Review>("").ByList()
                .Select(x => x.CarId).Filter(x=>x.CustomerId == customerId).ToUri().ToString();
            var rating = await _client.GetAsync<List<Review>>(path);
            HaveRate = new List<bool>();
            foreach (var item in CarRental)
            {
                Console.WriteLine(path);
                Console.WriteLine(item.CarId +" "+ rating.Any(x => x.CarId == item.CarId));
                if (rating.Any(x => x.CarId == item.CarId)) HaveRate.Add(true);
                else HaveRate.Add(false);
            }
            return Page();

        }
    }
}
