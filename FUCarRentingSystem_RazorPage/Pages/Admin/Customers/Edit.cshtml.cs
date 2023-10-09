using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Customers
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;

        public EditModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var response = await _client.GetAsync($"{Constants.ApiRoute.CustomersApi}/{id}");
            var strData = await response.Content.ReadAsStringAsync();
            //  Get customer
            var customer = MyJsonSerializer.Deserialize<Customer>(strData);
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //prepare data
            var json = JsonSerializer.Serialize<Customer>(Customer);
            var stringContent = new StringContent(json, Encoding.UTF8,"application/json");
            // send data
            var response = await _client.PutAsync($"{Constants.ApiRoute.CustomersApi}/{Customer.Id}", stringContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./index");
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }

    }
}
