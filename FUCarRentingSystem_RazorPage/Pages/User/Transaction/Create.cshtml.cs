using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace FUCarRentingSystem_RazorPage.Pages.User.Transaction
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;

        public CreateModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CarRentalsApi;
            PageUriCar = Constants.ApiRoute.CarsApi;
        }
        [BindProperty]
        public CarRentalDTO CarRental { get; set; }
        public string PageUri { get; }
        public string PageUriCar { get; }
        public Car? Car { get; private set; }

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
        
        public async Task<IActionResult> OnGetPriceAsync(CarRentalDTO carRental)
        {
            if (carRental.CarId == null) return new JsonResult(0.ToString("C2"));
            if (carRental.CarId != null)
            {
                var car = await _client.GetFromJsonAsync<Car>($"{PageUri}/{carRental.CarId}?$filter=status eq true&$select=rentprice");
                if (car == null) return new JsonResult(0.ToString("C2"));
                carRental.UpdateRentPrice(car.RentPrice);
                CarRental = carRental;
                return new JsonResult(carRental.RentPrice.ToString("C2"));
            }

            HttpContext.Session.SetString("RentCar", carRental.Serialize());

            return new JsonResult(0.ToString("C2"));
        }
        public async Task<IActionResult> OnPost()
        {
            var customerId = HttpContext.Session.GetInt32("id");
            if (customerId == null) return Page();
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
            CarRental carrental = new(CarRental, customerId.Value);//map Carrental

            var stringContent = new StringContent(carrental.Serialize(), System.Text.Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(PageUri, stringContent);
            // Add Success
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("RentCar");
                return RedirectToPage("./Index");
            }
            // Add Failed 
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }
    }
}
