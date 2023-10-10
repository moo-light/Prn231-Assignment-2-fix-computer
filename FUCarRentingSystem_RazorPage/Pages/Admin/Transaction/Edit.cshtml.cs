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
using System.Reflection.Metadata;
using FUCarRentingSystem_RazorPage.Utils;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DTOS.DTOS;
using Microsoft.AspNetCore.Http;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Transaction
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _client;

        public EditModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            PageUriCar = Constants.ApiRoute.CarsApi;
            PageUri = Constants.ApiRoute.CarRentalsApi;
        }

        [BindProperty]
        public CarRentalDTO CarRental { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int? CustomerIdentity { get; set; } = default!;
        public Car? Car { get; private set; }
        public string PageUriCar { get; }
        public string PageUri { get; }
        private string Key { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int carId, int customerId, string date)
        {
            Key = $"RentCar{carId}{customerId}{date}";

            string? strData = HttpContext.Session.GetString(Key);
            CarRental = strData?.Deserialize<CarRentalDTO>();
            // create new rental if session unavailiable
            if (CarRental == null)
            {
                var carRental = await _client.GetAsync<CarRental>(PageUri + $"/{carId},{customerId},{date}");
                if (carRental == null) return NotFound("Carrentals Not found");
                if (carRental.PickupDate < DateTime.Today) return BadRequest("Can't Edit Transaction that already procedded");
                CarRental = new CarRentalDTO
                {
                    CarId = carRental.CarId,
                    PickupDate = carRental.PickupDate,
                    RentPrice = carRental.RentPrice,
                    ReturnDate = carRental.ReturnDate
                };
                CustomerIdentity =  customerId;
            }
            else
            {
                // help reminding customerId
                CustomerIdentity = HttpContext.Session.GetInt32($"{Key}CusID"); ;
            }
            // save session
            HttpContext.Session.SetInt32($"{Key}CusID", CustomerIdentity.Value);
            HttpContext.Session.SetString(Key, CarRental.Serialize());
            return Page();

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int carId, int customerId, string date)
        {
            Key = $"RentCar{carId}{customerId}{date}";
            Console.WriteLine(Key);
            if (CarRental.CarId == null)
            {
                ModelState["CarRental.CarId"]?.Errors.Add("Select Your Vehicle!");
                ModelState["CarRental.CarId"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid;
            }

            if (!ModelState.IsValid)
                return Page();
            // Add Car
            CarRental carrental = new(CarRental, CustomerIdentity.Value);//map Carrental

            var stringContent = new StringContent(carrental.Serialize(), System.Text.Encoding.UTF8, "application/json");
            string requestUri = PageUri + $"/{carId},{customerId},{date}";
            var response = await _client.PutAsync(requestUri, stringContent);
            // Add Success
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove($"{Key}CusID");
                HttpContext.Session.Remove(Key);
                return RedirectToPage("./Index");
            }
            // Add Failed 
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
            return Page();
        }

        public override PageResult Page()
        {
            if (CarRental.CarId != null)
            {
                var car = _client.GetAsync<Car>(PageUriCar + $"/{CarRental.CarId}");
                car.Wait();
                Car = car.Result;
            }
            var customers = _client.GetAsync<List<Customer>>(Constants.ApiRoute.CustomersApi);
            customers.Wait();
            ViewData["CustomerId"] = new SelectList(customers.Result, "Id", "CustomerName");
            return base.Page();
        }
    }
}
