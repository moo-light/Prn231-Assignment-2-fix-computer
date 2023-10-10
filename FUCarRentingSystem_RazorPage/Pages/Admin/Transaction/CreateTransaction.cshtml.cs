using Domain.Entities;
using DTOS.DTOS;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Transaction
{
    public class CreateTransactionModel : PageModel
    {
        private readonly HttpClient _client;

        public CreateTransactionModel()
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
            if (CarRental.CarId != null)
            {
                Car = await _client.GetAsync<Car>(PageUriCar + $"/{CarRental.CarId}");
            }
            HttpContext.Session.SetString("RentCar", CarRental.Serialize());
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
