using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Customers
{
    public class DetailsModel : PageModel
    {
        private HttpClient _client;

        public DetailsModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetStringAsync($"{Constants.ApiRoute.CustomersApi}/{id}");
            var customer = response.Deserialize<Customer>();
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.DeleteAsync($"{Constants.ApiRoute.CustomersApi}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
