using Domain.Entities;
using DTOS.DTOS;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Transaction
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;

        public CreateModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CarRentalsApi;
            PageUriCar = Constants.ApiRoute.CarsApi;
        }

        [BindProperty]
        public CarRentalDTO CarRental { get; set; } = default!;
        [BindProperty]
        public int? CustomerId { get; set; } = default!;
        public Car? Car { get; private set; }
        public string PageUri { get; }
        public string PageUriCar { get; }

        public async Task<IActionResult> OnGetAsync()
        {
            string? strData = HttpContext.Session.GetString("RentCar");
            CarRental = strData?.Deserialize<CarRentalDTO>() ?? new CarRentalDTO
            {
                PickupDate = DateTime.Now,
                ReturnDate = DateTime.Now,
                RentPrice = 0,
            };
            CustomerId = HttpContext.Session.GetInt32("RentCarCusID");
            if (CarRental.CarId != null)
            {
                Car = await _client.GetAsync<Car>(PageUriCar + $"/{CarRental.CarId}");
            }
            HttpContext.Session.SetString("RentCar", CarRental.Serialize());
            return Page();
        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (CarRental.CarId == null)
            {
                ModelState["CarRental.CarId"]?.Errors.Add("Select Your Vehicle!");
                ModelState["CarRental.CarId"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid;
            }
            if (CarRental.CarId != null)
            {
                Car = await _client.GetAsync<Car>(PageUriCar + $"/{CarRental.CarId}");
            }
            if (!ModelState.IsValid)
                return Page();
            // Add Car
            CarRental carrental = new(CarRental, CustomerId.Value);//map Carrental

            var stringContent = new StringContent(carrental.Serialize(), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(PageUri, stringContent);
            // Add Success
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("RentCarCusID");
                HttpContext.Session.Remove("RentCar");
                return RedirectToPage("./Index");
            }
            // Add Failed 
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }

        public override PageResult Page()
        {
            var customers = _client.GetAsync<List<Customer>>(Constants.ApiRoute.CustomersApi);
            customers.Wait();
            ViewData["CustomerId"] = new SelectList(customers.Result, "Id", "CustomerName");
            return base.Page();
        }
    }
}
