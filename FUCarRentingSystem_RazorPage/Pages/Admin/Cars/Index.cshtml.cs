using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OData.QueryBuilder.Builders;
using OData.QueryBuilder.Conventions.AddressingEntities.Query;
using System.Net.Http.Headers;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Cars
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;
        private int _count;

        public IndexModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IList<Car>? Cars { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; } = string.Empty!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public decimal PageCount { get => Math.Ceiling((decimal)_count / Constants.PageSize); private set => _count = (int)value; }

        public async Task OnGetAsync()
        {
            Uri uri = new Uri(Constants.ApiRoute.CarsApi)
                  .AddQuery("$top", Constants.PageSize)
                  .AddQuery("$skip", ((PageNumber - 1) * Constants.PageSize))
                  .AddQuery("$filter", $"contains(tolower({nameof(Car.CarName)}),tolower('{Search}')) and status eq true")
                  .AddQuery("$orderBy", $"{nameof(Car.Id)} desc");
            Uri uriCount = new UriBuilder(Constants.ApiRoute.CarsApi.Replace("api", "odata") + $"/$count")
            {
                Query = uri.Query
            }.Uri;
            
            var response = await _client.GetAsync(uri);
            _count = await _client.GetAsync<int>(uriCount.ToString());
            var strData = await response.Content.ReadAsStringAsync();
            Cars = strData.Deserialize<List<Car>>();
        }
    }
}
