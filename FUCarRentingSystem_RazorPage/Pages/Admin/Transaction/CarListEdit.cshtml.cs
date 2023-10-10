using Domain.Entities;
using DTOS.DTOS;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OData.QueryBuilder.Builders;
using System.Net.Http.Headers;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Transaction
{
    public class CarListModelEdit : PageModel
    {
        private readonly HttpClient _client;

        public CarListModelEdit()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CarsApi;
        }


        public IList<Car>? Cars { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public CarRentalDTO? CarRental { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? CustomerId { get; set; }
        public string PageUri { get; }
        private string Key { get; set; } = default!;

        public async Task<IActionResult> OnPostSelectAsync(int carId, int customerId, string date)
        {
            Key = $"RentCar{carId}{customerId}{date}";
            CarRental ??= HttpContext.Session.GetString(Key)?.Deserialize<CarRentalDTO>() ?? new CarRentalDTO
            {
                PickupDate = DateTime.Today,
                ReturnDate = DateTime.Today,
                CarId = null,
            };
            Cars = await _client.GetAsync<List<Car>>($"{PageUri}?$filter=status eq true&expand=carrentals($filter=status eq true),producer");
            var uri = new ODataQueryBuilder(Constants.ApiRoute.DefaultPath, new OData.QueryBuilder.Options.ODataQueryBuilderOptions
            {
                UseCorrectDateTimeFormat = true
            }).For<CarRental>("CarRentals")
                                                    .ByList()
                                                    .Filter((s, f, o) => s.Status == true
                                                                         && (f.Date(s.PickupDate) <= CarRental.PickupDate
                                                                         && f.Date(s.ReturnDate) >= CarRental.PickupDate
                                                                         || f.Date(s.ReturnDate) >= CarRental.ReturnDate
                                                                         && f.Date(s.ReturnDate) >= CarRental.ReturnDate))
                                                    .Expand(x=>x.Car).Expand(x=>x.Customer)
                                                    .ToUri();
            CarRental.UpdateRentPrice(Cars?.FirstOrDefault(x => x.Id == CarRental.CarId)?.RentPrice ?? 0);
            List<CarRental>? carRentals = await _client.GetAsync<List<CarRental>>(uri.ToString());
            if (Cars?.Count > 0 && carRentals?.Count > 0)
            {
                var removeList = new List<Car>();
                foreach (var car in Cars)
                {
                    if (carRentals.Any(x => x.CarId == car.Id) && car.Id != carId) removeList.Add(car);
                }
                removeList.ForEach(x => Cars.Remove(x));
            }

            HttpContext.Session.SetInt32($"{Key}CusID", CustomerId.Value);
            HttpContext.Session.SetString(Key, CarRental.Serialize());
            return Page();
        }

        public async Task<IActionResult> OnPostRent(int id, int carId, int customerId, string date)
        {
            Key = $"RentCar{carId}{customerId}{date}";

            var response = await _client.GetAsync($"{PageUri}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("", new { CarRental, carId, customerId, date });
            }
            string strdata = await response.Content.ReadAsStringAsync();
            var car = strdata.Deserialize<Car>();
            if (car != null)
            {
                CarRental.CarId = id;
                CarRental.UpdateRentPrice(car.RentPrice);
            }
            HttpContext.Session.SetInt32($"{Key}CusID", CustomerId.Value);
            HttpContext.Session.SetString(Key, CarRental.Serialize());
            return RedirectToPage("./Edit", new { carId, customerId, date });
        }

    }
}
