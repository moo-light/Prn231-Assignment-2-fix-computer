using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using FUCarRentingSystem_RazorPage.Utils;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.Cars
{
    public class ProducerModel : PageModel
    {
        private HttpClient _client;

        public ProducerModel()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public CarProducer CarProducer { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carproducer = await _client.GetAsync<CarProducer>(Constants.ApiRoute.ProducersApi+$"/{id}");
            if (carproducer == null)
            {
                return NotFound();
            }
            else 
            {
                CarProducer = carproducer;
            }
            return Page();
        }
    }
}
