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
    public class EducacionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EducacionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.educacion.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var educacion = await _context.educacion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (educacion == null)
            {
                return NotFound();
            }

            return View(educacion);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUser,email,tipoestudio,Institucion,InstitucionLugar,Titulo,Descripcion,desde,hasta,status,dateadd")] Educacion educacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(educacion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Candidatos", "Home");
            }
            return View(educacion);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var educacion = await _context.educacion.FindAsync(id);
            if (educacion == null)
            {
                return NotFound();
            }
            return View(educacion);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,IdUser,email,tipoestudio,Institucion,InstitucionLugar,Titulo,Descripcion,desde,hasta,status,dateadd")] Educacion educacion)
        {
            if (id != educacion.Id)
            {
                return RedirectToAction("Candidatos", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(educacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducacionExists(educacion.Id))
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
            return View(educacion);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var educacion = await _context.educacion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (educacion == null)
            {
                return NotFound();
            }

            return View(educacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var educacion = await _context.educacion.FindAsync(id);
            _context.educacion.Remove(educacion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Candidatos", "Home");
        }

        private bool EducacionExists(long id)
        {
            return _context.educacion.Any(e => e.Id == id);
        }
    }
}
