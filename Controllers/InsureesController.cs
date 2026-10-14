using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarInsurance.Models;
using CarInsurance.Data;

namespace CarInsurance.Controllers
{
    public class InsureesController : Controller
    {
        private readonly InsuranceContext _context;

        public InsureesController(InsuranceContext context)
        {
            _context = context;
        }

        // GET: INSUREES
        public async Task<IActionResult> Index() => View(await _context.Insurees.ToListAsync());

        // GET: INSUREES/Details/5
        public async Task<IActionResult> Details(System.Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // GET: INSUREES/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: INSUREES/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Calculate the quote on the server; never trust a quote from the form.
                insuree.Quote = CalculateQuote(insuree);

                _context.Add(insuree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuree);
        }
        private decimal CalculateQuote(Insuree insuree)
        {
            // Start with the base monthly premium.
            decimal quote = 50m;

            // Calculate the customer's age.
            int age = DateTime.Today.Year - insuree.DateOfBirth.Year;

            // Correct the age if their birthday has not occurred yet this year.
            if (insuree.DateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            // Apply the age surcharge.
            if (age <= 18)
            {
                quote += 100m;
            }
            else if (age <= 25)
            {
                quote += 50m;
            }
            else
            {
                quote += 25m;
            }

            // Apply the vehicle year surcharges.
            if (insuree.CarYear < 2000)
            {
                quote += 25m;
            }

            if (insuree.CarYear > 2015)
            {
                quote += 25m;
            }

            // Apply the Porsche surcharge.
            if (string.Equals(insuree.CarMake?.Trim(), "Porsche",
                StringComparison.OrdinalIgnoreCase))
            {
                quote += 25m;

                // Apply the additional Porsche 911 Carrera surcharge.
                if (string.Equals(insuree.CarModel?.Trim(), "911 Carrera",
                    StringComparison.OrdinalIgnoreCase))
                {
                    quote += 25m;
                }
            }

            // Add $10 for each speeding ticket.
            quote += insuree.SpeedingTickets * 10m;

            // Apply the DUI surcharge.
            if (insuree.DUI)
            {
                quote *= 1.25m;
            }

            // Apply the full coverage surcharge.
            if (string.Equals(insuree.CoverageType?.Trim(), "Full Coverage",
                StringComparison.OrdinalIgnoreCase))
            {
                quote *= 1.50m;
            }

            // Round the monthly quote to two decimal places.
            return Math.Round(quote, 2);
        }

        // GET: INSUREES/Edit/5
        public async Task<IActionResult> Edit(System.Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees.FindAsync(id);
            if (insuree == null)
            {
                return NotFound();
            }
            return View(insuree);
        }

        // POST: INSUREES/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(System.Guid? id, [Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (id != insuree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(insuree);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsureeExists(insuree.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(insuree);
        }

        // GET: INSUREES/Delete/5
        public async Task<IActionResult> Delete(System.Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuree = await _context.Insurees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (insuree == null)
            {
                return NotFound();
            }

            return View(insuree);
        }

        // POST: INSUREES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(System.Guid? id)
        {
            var insuree = await _context.Insurees.FindAsync(id);
            if (insuree != null)
            {
                _context.Insurees.Remove(insuree);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsureeExists(System.Guid? id)
        {
            return _context.Insurees.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Admin()
        {
            // Recalculate every quote when the Admin page loads.
            var insurees = await _context.Insurees
                .OrderBy(i => i.LastName)
                .ThenBy(i => i.FirstName)
                .ToListAsync();

            foreach (var insuree in insurees)
            {
                insuree.Quote = CalculateQuote(insuree);
            }

            await _context.SaveChangesAsync();
            return View(insurees);
        }
    }
}



