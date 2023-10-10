using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Transaction
{
    public class DetailsModel : PageModel
    {
        private readonly HttpClient _client;

        public DetailsModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CarRentalsApi;
        }

        public CarRental CarRental { get; set; } = default!;
        public string PageUri { get; }

        public async Task<IActionResult> OnGetAsync(int carId, int customerId, string date)
        {
            string path = PageUri + $"/{carId},{customerId},{date}";
            CarRental = await _client.GetAsync<CarRental>(path);
            if (CarRental == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int carId, int customerId, string date)
        {
            var response = await _client.DeleteAsync($"{Constants.ApiRoute.CarRentalsApi}/{carId},{customerId},{date}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
