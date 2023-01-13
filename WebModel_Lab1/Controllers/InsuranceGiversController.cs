using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebModel_Lab1;
using WebModel_Lab1.Models;

namespace WebModel_Lab1.Controllers
{
    public class InsuranceGiversController : Controller
    {
        private readonly BankSystemContext _context;

        public InsuranceGiversController(BankSystemContext context)
        {
            _context = context;
        }

        // GET: InsuranceGivers
        public async Task<IActionResult> Index()
        {
            var bankSystemContext = _context.InsuranceGivers.Include(i => i.InsuranceObjectNavigation);
            return View(await bankSystemContext.ToListAsync());
        }

        // GET: InsuranceGivers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.InsuranceGivers == null)
            {
                return NotFound();
            }

            var insuranceGiver = await _context.InsuranceGivers
                .Include(i => i.InsuranceObjectNavigation)
                .FirstOrDefaultAsync(m => m.InsuranceUsreou == id);
            if (insuranceGiver == null)
            {
                return NotFound();
            }

            return View(insuranceGiver);
        }

        // GET: InsuranceGivers/Create
        public IActionResult Create()
        {
            ViewData["InsuranceObject"] = new SelectList(_context.Banks, "Usreou", "Usreou");
            return View();
        }

        // POST: InsuranceGivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InsuranceUsreou,BankCountry,InsuranceAmount,InsuranceObject,IsBank")] InsuranceGiver insuranceGiver)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(insuranceGiver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["InsuranceObject"] = new SelectList(_context.Banks, "Usreou", "Usreou", insuranceGiver.InsuranceObject);
            return View(insuranceGiver);
        }

        // GET: InsuranceGivers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.InsuranceGivers == null)
            {
                return NotFound();
            }

            var insuranceGiver = await _context.InsuranceGivers.FindAsync(id);
            if (insuranceGiver == null)
            {
                return NotFound();
            }
            ViewData["InsuranceObject"] = new SelectList(_context.Banks, "Usreou", "Usreou", insuranceGiver.InsuranceObject);
            return View(insuranceGiver);
        }

        // POST: InsuranceGivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("InsuranceUsreou,BankCountry,InsuranceAmount,InsuranceObject,IsBank")] InsuranceGiver insuranceGiver)
        {
            if (id != insuranceGiver.InsuranceUsreou)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(insuranceGiver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InsuranceGiverExists(insuranceGiver.InsuranceUsreou))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["InsuranceObject"] = new SelectList(_context.Banks, "Usreou", "Usreou", insuranceGiver.InsuranceObject);
            return View(insuranceGiver);
        }

        // GET: InsuranceGivers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.InsuranceGivers == null)
            {
                return NotFound();
            }

            var insuranceGiver = await _context.InsuranceGivers
                .Include(i => i.InsuranceObjectNavigation)
                .FirstOrDefaultAsync(m => m.InsuranceUsreou == id);
            if (insuranceGiver == null)
            {
                return NotFound();
            }

            return View(insuranceGiver);
        }

        // POST: InsuranceGivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.InsuranceGivers == null)
            {
                return Problem("Entity set 'BankSystemContext.InsuranceGivers'  is null.");
            }
            var insuranceGiver = await _context.InsuranceGivers.FindAsync(id);
            if (insuranceGiver != null)
            {
                _context.InsuranceGivers.Remove(insuranceGiver);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InsuranceGiverExists(string id)
        {
          return (_context.InsuranceGivers?.Any(e => e.InsuranceUsreou == id)).GetValueOrDefault();
        }
    }
}
