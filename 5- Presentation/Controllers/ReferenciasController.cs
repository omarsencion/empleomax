using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Domain.Entity;

namespace empleoswebMax.Controllers
{
    public class ReferenciasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReferenciasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Referencias
        public async Task<IActionResult> Index()
        {
            return View(await _context.referencias.ToListAsync());
        }

        // GET: Referencias/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referencias = await _context.referencias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (referencias == null)
            {
                return NotFound();
            }

            return View(referencias);
        }

        // GET: Referencias/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,email,Persona,PhoneNumber,Email2,Empresa,Parentezco,status,dateadd")] Referencias referencias)
        {
            if (ModelState.IsValid)
            {
                _context.Add(referencias);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Candidatos", "Home");
            }
            return View(referencias);
        }

        // GET: Referencias/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referencias = await _context.referencias.FindAsync(id);
            if (referencias == null)
            {
                return NotFound();
            }
            return View(referencias);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,IdUser,email,Persona,PhoneNumber,Email2,Empresa,Parentezco,status,dateadd")] Referencias referencias)
        {
            if (id != referencias.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(referencias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReferenciasExists(referencias.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Candidatos", "Home");
            }
            return View(referencias);
        }

        // GET: Referencias/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var referencias = await _context.referencias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (referencias == null)
            {
                return NotFound();
            }

            return View(referencias);
        }

        // POST: Referencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var referencias = await _context.referencias.FindAsync(id);
            _context.referencias.Remove(referencias);
            await _context.SaveChangesAsync();
            return RedirectToAction("Candidatos", "Home");
        }

        private bool ReferenciasExists(long id)
        {
            return _context.referencias.Any(e => e.Id == id);
        }
    }
}
