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

namespace FUCarRentingSystem_RazorPage.Pages.Admin.CarRentals
{
    public class EditModel : PageModel
    {
        private readonly Domain.AppDBContext _context;

        public EditModel(Domain.AppDBContext context)
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

            var carrental =  await _context.CarRentals.FirstOrDefaultAsync(m => m.CarId == id);
            if (carrental == null)
            {
                return NotFound();
            }
            CarRental = carrental;
           ViewData["CarId"] = new SelectList(_context.Cars, "Id", "CarName");
           ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CustomerName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(CarRental).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarRentalExists(CarRental.CarId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CarRentalExists(int id)
        {
          return (_context.CarRentals?.Any(e => e.CarId == id)).GetValueOrDefault();
        }
    }
}
