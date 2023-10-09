using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace FUCarRentingSystem_RazorPage.Pages.User.Profile
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _client;

        public IndexModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CustomersApi;
        }

        public string PageUri { get; }
        public Customer? Customer { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            int? id = HttpContext.Session.GetInt32("id");
            Customer = await _client.GetFromJsonAsync<Customer>($"{PageUri}/{id}");
            if (Customer == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostDelete()
        {
            int? id = HttpContext.Session.GetInt32("id");
            Customer = await _client.GetFromJsonAsync<Customer>($"{PageUri}/{id}");
            if (Customer == null) return NotFound();
            var response = await _client.DeleteAsync($"{PageUri}/{id}");
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Clear();
                HttpContext.Session.SetString("Success","Account Deleted!");
;
                return RedirectToPage("/Index");
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
