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
    public class DeleteModel : PageModel
    {
        private readonly Domain.AppDBContext _context;

        public DeleteModel(Domain.AppDBContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CarRental CarRental { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CarRentals == null)
            {
                return NotFound();
            }

            var carrental = await _context.CarRentals.FirstOrDefaultAsync(m => m.CarId == id);

            if (carrental == null)
            {
                return NotFound();
            }
            else 
            {
                CarRental = carrental;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CarRentals == null)
            {
                return NotFound();
            }
            var carrental = await _context.CarRentals.FindAsync(id);

            if (carrental != null)
            {
                CarRental = carrental;
                _context.CarRentals.Remove(CarRental);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
