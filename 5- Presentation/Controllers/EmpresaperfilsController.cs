using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.IO;


namespace empleoswebMax.Controllers
{
    public class EmpresaperfilsController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmpresaperfilsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context            = context;
            webHostEnvironment  = hostEnvironment;
            _userManager        = userManager;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _context.empresaperfils.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaperfil = await _context.empresaperfils
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresaperfil == null)
            {
                return NotFound();
            }

            return View(empresaperfil);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpresaViewModel model)
        {
            var id = _userManager.GetUserId(User);
            if (id == null)
            {
                var hgf = "es nulo";
                ViewBag.hgf = hgf;
                return RedirectToAction("Logout", "Aplicaciones");
            }

            var mail = _userManager.GetUserName(User);
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Empresaperfil empresaperfils = new Empresaperfil
                {
                    IdUser          = Guid.Parse(id),
                    email           = model.email,
                    EmpresaCentro   = model.EmpresaCentro,
                    PhoneNumber     = model.PhoneNumber,
                    Email2          = model.Email2,
                    RNC             = model.RNC,
                    Pais            = model.Pais,
                    Ciudad          = model.Ciudad,
                    Direccion       = model.Direccion,
                    status          = true,
                    Idempresa       = Guid.NewGuid(),
                    Foto            = uniqueFileName,
                    dateadd         = DateTime.Now,
                };

                _context.Add(empresaperfils);
                await _context.SaveChangesAsync();
                return RedirectToAction("Empresas", "Home");
            }
            return View();
        }

        private string UploadedFile(EmpresaViewModel model)
        {
            string uniqueFileName = null;

            if (model.Foto != null)
            {
                string uploadsFolder    = Path.Combine(webHostEnvironment.WebRootPath, "logo");
                uniqueFileName          = Guid.NewGuid().ToString() + "_" + model.Foto.FileName;
                string filePath         = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream   = new FileStream(filePath, FileMode.Create))
                {
                    model.Foto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaperfil = await _context.empresaperfils.FindAsync(id);
            if (empresaperfil == null)
            {
                return NotFound();
            }
            return View(empresaperfil);
        }

        //################################# update @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@2
        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Post(Empresaperfil empresaperfil)
        {
            Empresaperfil d = _context.empresaperfils.Where(s => s.Id == empresaperfil.Id).First();
            d.EmpresaCentro = empresaperfil.EmpresaCentro;
            _context.SaveChanges();
            return RedirectToAction("Empresas", "Home");
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresaperfil = await _context.empresaperfils
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresaperfil == null)
            {
                return NotFound();
            }

            return View(empresaperfil);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var empresaperfil = await _context.empresaperfils.FindAsync(id);
            _context.empresaperfils.Remove(empresaperfil);
            await _context.SaveChangesAsync();
            return RedirectToAction("Empresas", "Home");
        }

        private bool EmpresaperfilExists(long id)
        {
            return _context.empresaperfils.Any(e => e.Id == id);
        }
    }
}
