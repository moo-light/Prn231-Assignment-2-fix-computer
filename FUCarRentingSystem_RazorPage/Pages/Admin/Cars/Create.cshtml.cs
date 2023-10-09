using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Cars
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;

        public CreateModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Car Car { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //prepare data
            var json = JsonSerializer.Serialize(Car);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            // send data
            var response = await _client.PostAsync($"{Constants.ApiRoute.CarsApi}", stringContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();

            return Page();
        }

        public override PageResult Page()
        {
            Task task = Task.Run(async () =>
            {
                var carProducers = await _client.GetAsync<List<CarProducer>>( Constants.ApiRoute.ProducersApi);
                ViewData["Producer"] = new SelectList(carProducers, "Id", "ProducerName");
            });
            task.Wait();
            return base.Page();
        }
    }
}
