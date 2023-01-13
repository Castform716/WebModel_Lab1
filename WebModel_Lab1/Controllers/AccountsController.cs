using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebModel_Lab1;
using WebModel_Lab1.Models;
using WebModel_Lab1.Models.TagHelperModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebModel_Lab1.Controllers
{
    public class AccountsController : Controller
    {
        BankSystemContext _context;

        public AccountsController(BankSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? Usreou, string? currency, int page = 1, SortState sortOrder = SortState.UsreouAsc)
        {
            int pageSize = 3;

            IQueryable<Account> accs = _context.Accounts.Include(a => a.ItnNavigation).Include(a => a.UsreouNavigation);

            //IQueryable<Account> accs =  _context.Accounts.Include(Account.Usreou);

            if (!string.IsNullOrEmpty(Usreou))
            {
                accs = accs.Where(p => p.Usreou.Contains(Usreou));
            }
            if (!string.IsNullOrEmpty(currency))
            {
                accs = accs.Where(p => p.Currency.Contains(currency));
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.UsreouDesc:
                    accs = accs.OrderByDescending(s => s.Usreou);
                    break;
                case SortState.BalanceAsc:
                    accs = accs.OrderBy(s => s.Balance);
                    break;
                case SortState.BalanceDesc:
                    accs = accs.OrderByDescending(s => s.Balance);
                    break;
                case SortState.CreditAsc:
                    accs = accs.OrderBy(s => s.CreditSum);
                    break;
                case SortState.CreditDesc:
                    accs = accs.OrderByDescending(s => s.CreditSum);
                    break;
                default:
                    accs = accs.OrderBy(s => s.Usreou);
                    break;
            }

            // пагинация
            var count = await accs.CountAsync();
            var items = await accs.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            AccsViewModel viewModel = new AccsViewModel(
            items,
            new PageViewModel(count, page, pageSize),
                new FilterViewModel(await _context.Accounts.ToListAsync(), Usreou, currency),
                new SortViewModel(sortOrder)
            );

            return View(viewModel);
        }

        // GET: Accounts
        //public async Task<IActionResult> Index()
        //{
        //    var bankSystemContext = _context.Accounts.Include(a => a.ItnNavigation).Include(a => a.UsreouNavigation);
        //    return View(await bankSystemContext.ToListAsync());
        //}

        //// GET: Accounts/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null || _context.Accounts == null)
        //    {
        //        return NotFound();
        //    }

        //    var account = await _context.Accounts
        //        .Include(a => a.ItnNavigation)
        //        .Include(a => a.UsreouNavigation)
        //        .FirstOrDefaultAsync(m => m.AccountNumber == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(account);
        //}

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["Itn"] = new SelectList(_context.Customers, "Itn", "Itn");
            ViewData["Usreou"] = new SelectList(_context.Banks, "Usreou", "Usreou");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountNumber,Usreou,Itn,Currency,Balance,CreditSum")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Itn"] = new SelectList(_context.Customers, "Itn", "Itn", account.Itn);
            ViewData["Usreou"] = new SelectList(_context.Banks, "Usreou", "Usreou", account.Usreou);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["Itn"] = new SelectList(_context.Customers, "Itn", "Itn", account.Itn);
            ViewData["Usreou"] = new SelectList(_context.Banks, "Usreou", "Usreou", account.Usreou);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AccountNumber,Usreou,Itn,Currency,Balance,CreditSum")] Account account)
        {
            if (id != account.AccountNumber)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountNumber))
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
            ViewData["Itn"] = new SelectList(_context.Customers, "Itn", "Itn", account.Itn);
            ViewData["Usreou"] = new SelectList(_context.Banks, "Usreou", "Usreou", account.Usreou);
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.ItnNavigation)
                .Include(a => a.UsreouNavigation)
                .FirstOrDefaultAsync(m => m.AccountNumber == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'BankSystemContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string id)
        {
          return (_context.Accounts?.Any(e => e.AccountNumber == id)).GetValueOrDefault();
        }
    }
}
