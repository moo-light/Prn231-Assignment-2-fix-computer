using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Customers
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

        public IList<Customer>? Customers { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; } = string.Empty!;
        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public decimal PageCount { get => Math.Ceiling((decimal)_count / Constants.PageSize); private set => _count = (int)value; }
        public async Task OnGetAsync(string? search = "")
        {
            Uri uri = new Uri(Constants.ApiRoute.CustomersApi)
                  .AddQuery("$top", Constants.PageSize)
                  .AddQuery("$skip", (PageNumber - 1) * Constants.PageSize)
                  .AddQuery("$filter", $"contains(tolower({nameof(Customer.CustomerName)}),tolower('{Search}'))")
                  .AddQuery("$orderBy", $"{nameof(Customer.Id)} desc");

            Uri uriCount = new UriBuilder(Constants.ApiRoute.CustomersApi.Replace("api", "odata") + $"/$count")
            {
                Query = uri.Query
            }.Uri;

            var response = await _client.GetAsync(uri);
            _count = await _client.GetAsync<int>(uriCount.ToString());
            var strData = await response.Content.ReadAsStringAsync();
            Customers = strData.Deserialize<List<Customer>>();
        }

    }
}
