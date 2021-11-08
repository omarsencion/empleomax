using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EmpleosWebMax.UI.Web.Controllers
{
    public class ForosallController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment webHostEnvironment;


        public ForosallController(IConfiguration configuration, ApplicationDbContext context, ILogger<AdminController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IWebHostEnvironment hostEnvironment)
        {
            _context            = context;
            Configuration       = configuration;
            webHostEnvironment  = hostEnvironment;
            _logger             = logger;
            _userManager        = userManager;
            _signInManager      = signInManager;

        }

        public async Task<IActionResult> Index()
        {

            //----------------------------------------------CATEGORIAS ---------------------------------------------------------
            List<ForoCategorias> CategoriasList = new List<ForoCategorias>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"select * FROM foroCategorias WHERE StatusForoCategoria = 0 ORDER BY Categoria;";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ForoCategorias Userx    = new ForoCategorias();
                        Userx.Id                = Convert.ToInt64(dataReader["Id"]);
                        Userx.Categoria         = Convert.ToString(dataReader["Categoria"]).ToUpper();
                        CategoriasList.Add(Userx);
                    }
                }
            }
            ViewBag.CategoriasList = CategoriasList;


            List<ForoDto> losforos = new List<ForoDto>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"select Foros.*,foroCategorias.* FROM Foros JOIN foroCategorias ON Foros.IdCategoria = foroCategorias.Id WHERE Foros.StatusForo < 20 ORDER BY Foros.Publicado DESC;";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ForoDto Userx           = new ForoDto();
                        Userx.Id                = Convert.ToInt64(dataReader["Id"]);
                        Userx.Titulo            = Convert.ToString(dataReader["Titulo"]).ToUpper();
                        Userx.Foto              = Convert.ToString(dataReader["Foto"]).ToUpper();
                        Userx.StatusForo        = Convert.ToInt16(dataReader["StatusForo"]);
                        Userx.Publicado         = Convert.ToDateTime(dataReader["Publicado"]);
                        Userx.Categoria         = Convert.ToString(dataReader["Categoria"]).ToUpper();
                        Guid Idforotoguid       = Guid.Parse(dataReader["IdForo"].ToString());
                        Userx.IdForo            = Idforotoguid;
                        ViewBag.userid          = Userx.Id;
                        ViewBag.iddelforo       = Userx.IdForo;
                        losforos.Add(Userx);

                    }
                }
            }
            ViewBag.Foros = losforos;

            return View();
        }


        public async Task<IActionResult> GetCategories()
        {

            List<ForoCategorias> CategoriasList = new List<ForoCategorias>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"select * FROM foroCategorias WHERE StatusForoCategoria = 0";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ForoCategorias Userx    = new ForoCategorias();
                        Userx.Id                = Convert.ToInt64(dataReader["Id"]);
                        Userx.Categoria         = Convert.ToString(dataReader["Categoria"]).ToUpper();
                        CategoriasList.Add(Userx);
                    }
                }
            }
            ViewBag.CategoriasList = CategoriasList;

            return View(); 
        }



        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foro = await _context.Foros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foro == null)
            {
                return NotFound();
            }

            return View(foro);
        }

        public IActionResult GoPost(long Ct)
        {
            string categoria_ = "";
            long nId_ = 0;

            if(Ct >= 0)
            {
                ForoCategorias d = _context.foroCategorias.Where(s => s.Id == Ct).FirstOrDefault();
                categoria_ = d.Categoria;
                nId_ = d.Id;
            }
            ViewBag.categoria_ = categoria_;
            ViewBag.nId_ = nId_;


            return View();
        }



        [HttpGet]
        public IActionResult CreatePost()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GoPostUp(Int16 IdCategoria,string Titulo, string Contenido, string Autor)
        {
            int nx              = 1;
            ViewBag.IdCategoria = IdCategoria;
            ViewBag.Titulo      = Titulo;
            ViewBag.Contenido   = Contenido;
            ViewBag.Autor       = Autor;
            return View();
        }


        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foro = await _context.Foros.FindAsync(id);
            if (foro == null)
            {
                return NotFound();
            }
            return View(foro);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,IdForo,IdUser,IdCategoria,Titulo,Contenido,Foto,StatusForo,Autor,Publicado")] Foro foro)
        {
            if (id != foro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForoExists(foro.Id))
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
            return View(foro);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foro = await _context.Foros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foro == null)
            {
                return NotFound();
            }

            return View(foro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var foro = await _context.Foros.FindAsync(id);
            _context.Foros.Remove(foro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ForoExists(long id)
        {
            return _context.Foros.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Categorias()
        {
            return View(await _context.foroCategorias.Where(s => s.StatusForoCategoria < 3).ToListAsync());
        }



        public IActionResult CrearCatg()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCatg(string Categoria_)
        {
            ForoCategorias foroCategorias = new ForoCategorias
            {
                Categoria = Categoria_,
                StatusForoCategoria = 0,
            };
            if (ModelState.IsValid)
            {
                _context.Add(foroCategorias);
                await _context.SaveChangesAsync();
                return RedirectToAction("Categorias", "Forosall");

            }
            return View(foroCategorias);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS CATEGORIAS  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//  
        [ActionName("EditCategoria")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Update_Post2(ForoCategorias cat, string St, long gdfsd) //, Guid Us, string Mi

        {

            Int16 status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; }
                else if (St == "x") { status1 = 0; } 
                else if (St == "r") { status1 = 1; } 
                else { status1 = 0; }
            }

            if (status1 < 30) 
            {
                ForoCategorias d = _context.foroCategorias.Where(s => s.Id == gdfsd).FirstOrDefault();
                d.StatusForoCategoria = status1;
                _context.SaveChanges();
            }
            return RedirectToAction("Categorias", "Forosall");
        }



        //==============================================================================================================================================================================
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPostgo(ForoDto model, IFormFile file)
        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            var fileName = "";
            Guid idforos = Guid.NewGuid();

            if (file != null)
            {
                fileName = System.IO.Path.GetFileName(file.FileName); //=== N O M B R E   DEL  DOCUMENTO A SUBIR
            }
            else { fileName         = "nofile";}
            string Namedocument     = fileName;
            int sizeOfString        = Namedocument.Length;
            int sizeX               = 0;
            string SubString        = "";
            if (sizeOfString > 10)
            {
                sizeX               = sizeOfString - 10;
                int newVal          = Namedocument.Length - (Namedocument.Length - sizeX);
                SubString           = Namedocument.Substring(newVal);
            }
            else { SubString        = Namedocument; }
            //random number =========================================================================
            Random rnd              = new Random();
            int newrnd              = rnd.Next(100000, 1325478855);
            // random string ========================================================================
            var chars               = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars         = new char[8];
            var random              = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i]      = chars[random.Next(chars.Length)];
            }
            var finalString         = new String(stringChars);
            //=======================================================================================
            Namedocument            = finalString + newrnd + "_" + SubString;
            fileName = Namedocument;
            //======================================== UPLOAD FILE ==================================
            if(file != null && fileName     != "nofile") { 
                    string uploadsFolder    = Path.Combine(webHostEnvironment.WebRootPath, "foro");
                    var fileName2           = Path.Combine(uploadsFolder, fileName);
                    using (var localFile    = System.IO.File.OpenWrite(fileName2))
                    using (var uploadedFile = file.OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                    }
            }


            if (ModelState.IsValid)
            {
                Foro Foros          = new Foro
                {
                    IdForo          = idforos,
                    IdUser          = new Guid(id),
                    IdCategoria     = model.IdCategoria,
                    StatusForo      = 1,
                    Titulo          = model.Titulo,
                    Contenido       = model.Contenido,
                    Foto            = fileName,
                    Autor           = model.Autor,
                    Publicado       = DateTime.Now
                };

                _context.Add(Foros);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Forosall");
            }
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS foro  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//  
        [ActionName("EditForostatus")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Update_forostatus(ForoCategorias cat, string St, long gdfsd) //, Guid Us, string Mi

        {

            Int16 status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; } 
                else if (St == "x") { status1 = 0; } 
                else if (St == "r") { status1 = 1; } 
                else { status1 = 0; }
            }

            if (status1 < 30) 
            {
                Foro d = _context.Foros.Where(s => s.Id == gdfsd).FirstOrDefault();
                d.StatusForo = status1;
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Forosall");
        }
        

    }
}
