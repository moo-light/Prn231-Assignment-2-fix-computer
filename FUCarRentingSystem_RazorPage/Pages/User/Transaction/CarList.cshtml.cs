using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OData.QueryBuilder.Builders;
using System.Net.Http.Headers;

namespace FUCarRentingSystem_RazorPage.Pages.User.Transaction
{
    public class CarListModel : PageModel
    {
        private readonly HttpClient _client;

        public CarListModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            PageUri = Constants.ApiRoute.CarsApi;
        }


        public IList<Car>? Cars { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public CarRentalDTO? CarRental { get; set; }
        public string PageUri { get; }

        public async Task<IActionResult> OnGetAsync()
        {
            CarRental ??= HttpContext.Session.GetString($"RentCar")?.Deserialize<CarRentalDTO>() ?? new CarRentalDTO
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
                                                    .ToUri();
            CarRental.UpdateRentPrice(Cars?.FirstOrDefault(x => x.Id == CarRental.CarId)?.RentPrice ?? 0);
            List<CarRental>? carRentals = await _client.GetAsync<List<CarRental>>(uri.ToString());
            if (Cars?.Count > 0 && carRentals?.Count > 0)
            {
                var removeList = new List<Car>();
                foreach (var car in Cars)
                {
                    if (carRentals.Any(x => x.CarId == car.Id)) removeList.Add(car);
                }
                removeList.ForEach(x => Cars.Remove(x));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRent(int id)
        {
            var response = await _client.GetAsync($"{PageUri}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return RedirectToAction("", new { CarRental });
            }
            string strdata = await response.Content.ReadAsStringAsync();
            var car = strdata.Deserialize<Car>();
            if (car != null)
            {
                CarRental.CarId = id;
                CarRental.UpdateRentPrice(car.RentPrice);
            }
            HttpContext.Session.SetString($"RentCar", CarRental.Serialize());
            return RedirectToPage("./Create");
        }

    }
}
