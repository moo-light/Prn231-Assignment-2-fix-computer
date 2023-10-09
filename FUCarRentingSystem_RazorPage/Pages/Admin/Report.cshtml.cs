using Domain.Entities;
using Domain.Validations;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OData.QueryBuilder.Builders;
using System.ComponentModel.DataAnnotations;

namespace FUCarRentingSystem_RazorPage.Pages.Admin
{
    public class ReportModel : PageModel
    {
        private readonly HttpClient _client;

        public ReportModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        [DateRange("01/01/1901")]
        public DateTime StartDate { get; set; } = DateTime.Parse($"01/01/{DateTime.Now.Year}");
        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        [DateRange("01/01/1901")]
        public DateTime EndDate { get; set; } = DateTime.Now;
        [BindProperty(SupportsGet = true)]
        public int? CustomerId { get; set; } = null;
        public List<Customer>? Customers { get; private set; }
        public List<CarRental>? CarRentals { get; private set; }
        public Dictionary<string, List<CarRental>> Times { get; private set; } = new();
        public async Task OnGet()
        {
            if(StartDate > EndDate)
            {
                ModelState[nameof(StartDate)]?.Errors.Add("Start Date can't be larger than EndDate");
                ModelState[nameof(StartDate)].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid;
            }
            ODataQueryBuilder builder = new ODataQueryBuilder(Constants.ApiRoute.DefaultPath);
            var uri = builder.For<CarRental>("Carrentals").ByList()
                .OrderByDescending(x => x.PickupDate).Filter(x => StartDate <= x.PickupDate && x.ReturnDate <= EndDate)
                .Filter(x => CustomerId == null || x.CustomerId == CustomerId).ToUri();
            var renting = await _client.GetAsync<List<CarRental>>(uri.ToString());
            CarRentals = renting ?? new();
            // get Customer Name
            uri =  builder.For<Customer>("Customers").ByList().Select(x => new {x.CustomerName,x.Id}).ToUri();
            var customers = await _client.GetAsync<List<Customer>>(uri.ToString());
            Customers = customers ?? new();
        }
    }
}