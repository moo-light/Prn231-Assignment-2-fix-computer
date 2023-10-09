using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Cars
{
    public class DetailsModel : PageModel
    {
        private HttpClient _client;

        public DetailsModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Car Car { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var response = await _client.GetStringAsync($"{Constants.ApiRoute.CarsApi}/{id}");
            var car = response.Deserialize<Car>();
            if (car == null)
            {
                return NotFound();
            }
            Car = car;
            return Page();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null) return NotFound();
            var response = await _client.DeleteAsync($"{Constants.ApiRoute.CarsApi}/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
