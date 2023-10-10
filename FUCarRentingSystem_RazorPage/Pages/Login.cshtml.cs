using Azure;
using Domain.Entities;
using DTOS.DTOS;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FUCarRentingSystem_RazorPage.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _client;

        public LoginModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.SignIn;
        }
        [BindProperty]
        public LoginDTO LoginDTO { get; set; }
        public string PageUri { get; }
        public IActionResult OnGetLogout()
        {
            ISession session = HttpContext.Session;
            session.Remove("id");
            session.Remove("role");
            session.Remove("name");
            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();


            var json = LoginDTO.Serialize();
            StringContent content = new(json,System.Text.Encoding.UTF8,"application/json");
            var response = await _client.PostAsync(PageUri, content);
            if (!response.IsSuccessStatusCode)
            {
                ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
                return Page();
            }
            // Login Success
            var strData = await response.Content.ReadAsStringAsync();
            var customer = MyJsonSerializer.Deserialize<Customer>(strData);
            ISession session = HttpContext.Session;

            session.SetInt32("id", customer.Id);
            session.SetString("role", customer.Role);
            session.SetString("name", customer.CustomerName);
            return RedirectToPage("./Index");
        }
    }
}
