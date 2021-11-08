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
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace empleoswebMax.Controllers
{
    public class UserInfoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        public IConfiguration Configuration { get; }


        public UserInfoesController(IConfiguration configuration, ApplicationDbContext context, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context            = context;
            webHostEnvironment  = hostEnvironment;
            _userManager        = userManager;
            Configuration       = configuration;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.userInfos.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.userInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }
        // GET: UserInfoes/Create
        public IActionResult Crear()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model, string areatrab, string miselect1, string miselect2, string miselect3, string miselect4, IFormFile file)
        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            string checkarea1 = "";
            if(miselect1 != null) { checkarea1 = miselect1; }
            if (miselect2 != null && miselect2 == "secundaria") { checkarea1 = miselect1; } else { checkarea1 = miselect1 + " " + miselect2; }
            if (miselect3 != null && miselect3 == "secundaria") { checkarea1 = checkarea1; } else { checkarea1 = checkarea1 + " " + miselect3; }
            if (miselect4 != null && miselect4 == "secundaria") { checkarea1 = checkarea1; } else { checkarea1 = checkarea1 + " " + miselect4; }
            string Areaprofesional = checkarea1;
            areatrab = areatrab;
            var fileName = "";
            int uploadif = 0;
            if (file != null) { 
                fileName = System.IO.Path.GetFileName(file.FileName); 
            }
            else { fileName = "adduser.png"; uploadif = 1; }

            string Namedocument         = fileName;
                if (uploadif == 0) { 
                    int sizeOfString    = Namedocument.Length;
                    int sizeX           = 0;
                    string SubString    = "";
                    if (sizeOfString > 10)
                    {
                        sizeX = sizeOfString - 10;
                        int newVal  = Namedocument.Length - (Namedocument.Length - sizeX);
                        SubString   = Namedocument.Substring(newVal);
                    }
                    else { SubString    = Namedocument; }
                    Random rnd          = new Random();
                    int newrnd          = rnd.Next(100000, 1325478855);
                    var chars           = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    var stringChars     = new char[8];
                    var random          = new Random();
                    for (int i          = 0; i < stringChars.Length; i++)
                    {
                        stringChars[i]  = chars[random.Next(chars.Length)];
                    }
                    var finalString     = new String(stringChars);
                    Namedocument        = finalString + newrnd + "_" + SubString;
                    fileName            = Namedocument;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    string uploadsFolder    = Path.Combine(webHostEnvironment.WebRootPath, "fotos");
                    var fileName2           = Path.Combine(uploadsFolder, fileName);
                    using (var localFile    = System.IO.File.OpenWrite(fileName2))
                    using (var uploadedFile = file.OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                    }
                }
            if (uploadif >= 0)
            {
                UserInfo userInfos = new UserInfo
                {
                    IdUser              = Guid.Parse(id),
                    Estadocivil         = model.Estadocivil,
                    Estadolaboral       = model.Estadolaboral,
                    nacimiento          = model.nacimiento,
                    Telefono2           = model.Telefono2,
                    Nacionalidad        = model.Nacionalidad,
                    Ultimosalario       = model.Ultimosalario,
                    Salarioaspira       = model.Salarioaspira,
                    Pais                = model.Pais,
                    Ciudad              = model.Ciudad,
                    Direccion           = model.Direccion,
                    Areaprofesional     = Areaprofesional, 
                    Twitter             = model.Twitter,
                    Facebook            = model.Facebook,
                    Instagram           = model.Instagram,
                    CVconfidencial      = model.CVconfidencial,
                    Foto                = fileName,
                    salarioultimoMON    = model.salarioultimoMON,
                    salarioaspiraMON    = model.salarioaspiraMON,
                    DocumentoIDn        = model.DocumentoIDn,
                    DocumentoIDt        = model.DocumentoIDt,
                    Telefono1           = model.Telefono1,
                    Idioma1             = model.Idioma1,
                    Idioma2             = model.Idioma2,
                    Idioma3             = model.Idioma3,
                    Idioma1nivel        = model.Idioma1nivel,
                    Idioma2nivel        = model.Idioma2nivel,
                    Idioma3nivel        = model.Idioma3nivel,
                    dateadd             = DateTime.Now,
                };

                _context.Add(userInfos);
                await _context.SaveChangesAsync();
                return RedirectToAction("Candidatos", "Home");
            }
            else { ViewBag.model = "model"; }
            return View();
        }


        // GET: UserInfoes/Edit/5
        public async Task<IActionResult> Edit5(long id)
        {
            if (id > 0)
            {
                //
            }
            else { id = 3; }

            var userInfo = await _context.userInfos.FindAsync(id);
            if (userInfo == null)
            {
                ViewBag.errornuevo = "no existe";
                return View();
            }
            return View(userInfo);
        }

        // POST: UserInfoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit33(long id, [Bind("Id,IdUser,Foto,Estadocivil,Estadolaboral,nacimiento,Telefono2,Nacionalidad,Ultimosalario,Salarioaspira,Pais,Ciudad,Direccion,Areaprofesional,Twitter,Facebook,Instagram,CVconfidencial")] UserInfo userInfo)
        {
            if (id != userInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(userInfo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Candidatos", "Home");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInfoExists(userInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return RedirectToAction("Candidatos", "Home2");
                        throw;
                    }
                }
            }
            return View(userInfo);
        }

        // GET: UserInfoes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInfo = await _context.userInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            return View(userInfo);
        }

        // POST: UserInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var userInfo = await _context.userInfos.FindAsync(id);
            _context.userInfos.Remove(userInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInfoExists(long id)
        {
            return _context.userInfos.Any(e => e.Id == id);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Edit()
        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            ViewBag.userId = id;
            ViewBag.mail = mail;
            List<UserInfoDto> UserxList = new List<UserInfoDto>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT AspNetUsers.*,userInfos.* FROM userInfos JOIN AspNetUsers ON AspNetUsers.Id = userInfos.IdUser WHERE AspNetUsers.statusgeneral = 1 AND userInfos.IdUser = '{id}'";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        UserInfoDto Userx           = new UserInfoDto();
                        Userx.Estadocivil           = Convert.ToString(dataReader["Estadocivil"]).ToUpper();
                        Userx.Estadolaboral         = Convert.ToString(dataReader["Estadolaboral"]).ToUpper();
                        Userx.nacimiento            = Convert.ToDateTime(dataReader["nacimiento"]);
                        Userx.Telefono2             = Convert.ToString(dataReader["Telefono2"]).ToUpper();
                        Userx.Nacionalidad          = Convert.ToString(dataReader["Nacionalidad"]).ToUpper();
                        Userx.nacimiento            = Convert.ToDateTime(dataReader["nacimiento"]);
                        Userx.Ultimosalario         = Convert.ToDouble(dataReader["Ultimosalario"]);
                        Userx.Salarioaspira         = Convert.ToDouble(dataReader["Salarioaspira"]);
                        Userx.Pais                  = Convert.ToString(dataReader["Pais"]).ToUpper();
                        Userx.Ciudad                = Convert.ToString(dataReader["Ciudad"]);
                        Userx.Direccion             = Convert.ToString(dataReader["Direccion"]).ToUpper();
                        Userx.Areaprofesional       = Convert.ToString(dataReader["Areaprofesional"]).ToUpper();
                        Userx.Twitter               = Convert.ToString(dataReader["Twitter"]);
                        Userx.Facebook              = Convert.ToString(dataReader["Facebook"]);
                        Userx.Instagram             = Convert.ToString(dataReader["Instagram"]);
                        Userx.salarioultimoMON      = Convert.ToString(dataReader["salarioultimoMON"]);
                        Userx.salarioaspiraMON      = Convert.ToString(dataReader["Facebook"]).ToUpper();
                        Userx.DocumentoIDn          = Convert.ToString(dataReader["DocumentoIDn"]).ToUpper();
                        Userx.DocumentoIDt          = Convert.ToString(dataReader["DocumentoIDt"]).ToUpper();
                        Userx.FirstName             = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName              = Convert.ToString(dataReader["LastName"]).ToUpper();
                        ViewBag.IdUser = Userx.Id;
                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;
            return View();
        }




        //========================== EDIT USER =====================================================
        //==========================================================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(long id, UserInfo model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            int gdfsd = 1;
            if (gdfsd > 0) 
            {
                UserInfo d = _context.userInfos.Where(s => s.Id == id).First();
                d.Estadocivil = model.Estadocivil;
                _context.SaveChanges();
                return RedirectToAction("Candidatos", "Home");

            }
            return View();
        }



        //=====================================================================================================================
        //==========================================================================================
        [HttpPost]
        [Authorize]
        public IActionResult Editar(string Estadocivil, string Estadolaboral, DateTime nacimiento, string Telefono2, string Nacionalidad, double Ultimosalario, double Salarioaspira, string Pais, string Ciudad, string Direccion, string Areaprofesional, string Twitter, string Facebook, string Instagram, string salarioultimoMON, string salarioaspiraMON, string DocumentoIDn, string DocumentoIDt, string FirstName, string LastName)
        {
            var id2 = _userManager.GetUserId(User);

            Guid Idg = new Guid(id2);
            if (Idg != Guid.Empty)
            {
                ViewBag.info = Idg;
            }
            int gdfsd = 1;
            if (gdfsd > 0) 
            {
                UserInfo d = _context.userInfos.Where(s => s.IdUser.Equals(Idg)).First();

                _context.SaveChanges();
                d.Estadocivil       = Estadocivil;
                d.Estadolaboral     = Estadolaboral;
                if(nacimiento == null || nacimiento.ToString() == "1/1/0001 12:00:00 AM") { } else { d.nacimiento = nacimiento; }
                d.Telefono2         = Telefono2;
                d.Nacionalidad      = Nacionalidad;
                d.Ultimosalario     = Ultimosalario;
                d.Salarioaspira     = Salarioaspira;
                d.Pais              = Pais;
                d.Ciudad            = Ciudad;
                d.Direccion         = Direccion;
                d.Areaprofesional   = Areaprofesional;
                d.Twitter           = Twitter;
                d.Facebook          = Facebook;
                d.Instagram         = Instagram;
                d.salarioultimoMON  = salarioultimoMON;
                d.salarioaspiraMON  = salarioaspiraMON;
                d.DocumentoIDn      = DocumentoIDn;
                d.DocumentoIDt      = DocumentoIDt;
                d.dateadd           = DateTime.Now;

                ApplicationUser x = _context.Users.Where(s => s.Id == id2).First();
                if (FirstName != null && LastName != null)
                {
                    x.FirstName = FirstName;
                    x.LastName = LastName;
                }
                _context.SaveChanges();

                return RedirectToAction("Candidatos", "Home");

            }
            return View();
        }
    }
}
