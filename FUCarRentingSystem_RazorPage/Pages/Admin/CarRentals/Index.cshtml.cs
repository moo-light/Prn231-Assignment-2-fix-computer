using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.CarRentals
{
    public class IndexModel : PageModel
    {
        private readonly Domain.AppDBContext _context;

        public IndexModel(Domain.AppDBContext context)
        {
            _context = context;
        }

        public IList<CarRental> CarRental { get;set; } = default!;

        public async Task OnGetAsync()
        {
                CarRental = await _context.CarRentals
                .Include(c => c.Car)
                .Include(c => c.Customer).ToListAsync();
        }
    }
}
