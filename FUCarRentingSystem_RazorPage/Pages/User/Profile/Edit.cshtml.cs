using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using System.Net.Http.Headers;
using FUCarRentingSystem_RazorPage.Utils;
using System.ComponentModel.DataAnnotations;

namespace FUCarRentingSystem_RazorPage.Pages.User.Profile
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;

        public EditModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CustomersApi;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        public string PageUri { get; }
        [BindProperty]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? Id
        {
            get
            {
                return Customer?.Id ?? null;
            }
            set
            {
                if (Customer == null) Customer = new();
                Customer.Id = value.Value;
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Id = HttpContext.Session.GetInt32("id");
            if (Id == null)
            {
                return NotFound();
            }

            var customer = await _client.GetAsync<Customer>(PageUri + $"/{Id}");
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
            if (Customer.Password == ConfirmPassword) ModelState[nameof(ConfirmPassword)].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            if (Customer.Password != ConfirmPassword)
            {
                ModelState[nameof(ConfirmPassword)].Errors.Add("Passwords are not the Same");
                ModelState[nameof(ConfirmPassword)].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid;
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var stringContent = new StringContent(Customer.Serialize(), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(PageUri+$"/{Id}", stringContent);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("./Index");

            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();

        }
    }
}
