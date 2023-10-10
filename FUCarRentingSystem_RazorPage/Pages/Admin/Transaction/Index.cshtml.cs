using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Transaction
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;
        private int _count;

        public IndexModel()
        {
            _client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("applidation/json");
            _client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public IList<CarRental>? CarRental { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; } = string.Empty!;
        [BindProperty(SupportsGet = true)]
        public int? PageNumber { get; set; } = 1;
        public decimal PageCount { get => Math.Ceiling((decimal)_count / Constants.PageSize); private set => _count = (int)value; }

        public async Task OnGetAsync()
        {
            Dictionary<string, object?> @params = new Dictionary<string, object?>
            {
                {"$filter","status eq true"},
                {"$orderBy",$"PickupDate desc"}
            };
            CarRental = await _client.GetAsync<List<CarRental>>(Constants.ApiRoute.CarRentalsApi, @params);
            var uricount = new UriBuilder(Constants.ApiRoute.CarRentalsApi.Replace("api", "odata") + "/$count").Uri.AddQuery(@params);
            PageCount = await _client.GetAsync<int>(uricount.ToString());

        }
    }
}
