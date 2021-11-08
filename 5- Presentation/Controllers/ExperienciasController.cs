using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace empleoswebMax.Controllers
{
    public class ExperienciasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IConfiguration Configuration { get; }
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExperienciasController(IConfiguration configuration, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context        = context;
            _userManager    = userManager;
            Configuration   = configuration;
        }


        public async Task<IActionResult> Index()
        {
            Guid id         = Guid.Parse(_userManager.GetUserId(User));
            var mail        = _userManager.GetUserName(User);
            ViewBag.userId  = id;
            ViewBag.mail    = mail;

            return View(await _context.experiencias.Where(x => x.IdUser == id).Where(x => x.email == mail).OrderBy(x => x.desde).ToListAsync());

        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiencias = await _context.experiencias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (experiencias == null)
            {
                return NotFound();
            }

            return View(experiencias);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("IdUser,email,Empresa,Posicion,FuncionesRol,Aportes,desde,hasta,status,dateadd")] Experiencias experiencias)
        {

            if (ModelState.IsValid)
            {
                _context.Add(experiencias);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Candidatos", "Home");
            }
            else
            {
                return View(experiencias);
            }
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiencias = await _context.experiencias.FindAsync(id);
            if (experiencias == null)
            {
                return NotFound();
            }
            return View(experiencias);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,IdUser,email,Empresa,Posicion,FuncionesRol,Aportes,desde,hasta,status,dateadd")] Experiencias experiencias)
        {
            if (id != experiencias.Id)
            {
                //return NotFound();
                return RedirectToAction("Error", "Home");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(experiencias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExperienciasExists(experiencias.Id))
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
            return View(experiencias);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiencias = await _context.experiencias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (experiencias == null)
            {
                return NotFound();
            }

            return View(experiencias);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var experiencias        = await _context.experiencias.FindAsync(id);
            _context.experiencias.Remove(experiencias);
            await _context.SaveChangesAsync();
            return RedirectToAction("Candidatos", "Home");
        }

        private bool ExperienciasExists(long id)
        {
            return _context.experiencias.Any(e => e.Id == id);
        }
    }
}
