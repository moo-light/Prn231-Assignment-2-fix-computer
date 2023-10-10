using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FUCarRentingSystem_RazorPage.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _client;

        public RegisterModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CustomersApi;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        [DataType(DataType.Password)]
        [BindProperty]
        [Required]
        public string ConfirmPassword { get; set; } = default!;
        public string PageUri { get; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // Valid Password Checking
            ModelStateEntry? modelStateEntry = ModelState[nameof(ConfirmPassword)];
            if (modelStateEntry != null && ConfirmPassword != Customer.Password) {
                modelStateEntry.ValidationState = ModelValidationState.Invalid;
                modelStateEntry.Errors.Add("Confirm Your Password");
            }

            if (!ModelState.IsValid || Customer == null)
            {
                return Page();
            }

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
            var json = JsonSerializer.Serialize(Customer, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(PageUri, content);
            if (response.IsSuccessStatusCode)
            {
                var strData = await response.Content.ReadAsStringAsync();
                var customer =  JsonSerializer.Deserialize<Customer>(strData,options);
                ISession session = HttpContext.Session;

                session.SetInt32("id", customer.Id);
                session.SetString("role", customer.Role);
                session.SetString("name", customer.CustomerName);
                return RedirectToPage("./Index");
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
