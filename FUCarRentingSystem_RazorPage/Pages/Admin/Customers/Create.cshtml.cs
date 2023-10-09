using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Customers
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;

        public CreateModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //prepare data
            var json = JsonSerializer.Serialize(Customer);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            // send data
            var response = await _client.PostAsync($"{Constants.ApiRoute.CustomersApi}", stringContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
