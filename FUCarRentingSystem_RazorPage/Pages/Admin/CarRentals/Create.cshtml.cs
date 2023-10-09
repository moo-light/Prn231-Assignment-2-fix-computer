using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain;
using Domain.Entities;

namespace FUCarRentingSystem_RazorPage.Pages.Admin.CarRentals
{
    public class CreateModel : PageModel
    {
        private readonly Domain.AppDBContext _context;

        public CreateModel(Domain.AppDBContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CarId"] = new SelectList(_context.Cars, "Id", "CarName");
        ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "CustomerName");
            return Page();
        }

        [BindProperty]
        public CarRental CarRental { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.CarRentals == null || CarRental == null)
            {
                return Page();
            }

            _context.CarRentals.Add(CarRental);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
