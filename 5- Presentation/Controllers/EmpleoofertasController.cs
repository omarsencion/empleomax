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
using System.Data;

namespace empleoswebMax.Controllers
{
    public class EmpleoofertasController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EmpleoofertasController(ApplicationDbContext context, IConfiguration configuration, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            Configuration = configuration;
            _context = context;
        }

        // GET: Empleoofertas
        public async Task<IActionResult> Index()
        {
            return View(await _context.empleo.ToListAsync());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleo = await _context.empleo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleo == null)
            {
                return NotFound();
            }

            return View(empleo);
        }


        //editar
        public async Task<IActionResult> Editar(long? Id, Guid gdtdstds, int pag, string buscar)
        {
            var nPagereturnUrl = pag;
            if(nPagereturnUrl < 1) { nPagereturnUrl = 1; }
            ViewBag.sPagereturnUrl = nPagereturnUrl;
            ViewBag.buscar = buscar;


            if (Id == null)
            {
                return NotFound();
            }

            var empleo = await _context.empleo
                .FirstOrDefaultAsync(m => m.Id == Id && m.Job == gdtdstds);
            if (empleo == null)
            {
                return NotFound();
            }

            return View(empleo);
        }


        public async Task<IActionResult> EditarA(long? Id, Guid gdtdstds, int pag, string buscarpor, string buscar, int all)
        {
            var nPagereturnUrl = pag;
            if (nPagereturnUrl < 1) { nPagereturnUrl = 1; }
            ViewBag.sPagereturnUrl = nPagereturnUrl;
            ViewBag.buscar = buscar;
            ViewBag.buscarpor = buscarpor;
            ViewBag.all = all;


            if (Id == null)
            {
                return NotFound();
            }

            var empleo = await _context.empleo
                .FirstOrDefaultAsync(m => m.Id == Id && m.Job == gdtdstds);
            if (empleo == null)
            {
                return NotFound();
            }

            return View(empleo);
        }
        public IActionResult Create()
        {
            //---------OBTENER PERFIL DE LA EMPRESA---------------------------------------------------------
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            ViewBag.userId = id;
            ViewBag.mail = mail;
            int ncontar;

            List<Empresaperfil> companyList = new List<Empresaperfil>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"SELECT empresaperfils.Id, empresaperfils.IdUser, empresaperfils.Idempresa ,empresaperfils.EmpresaCentro, empresaperfils.PhoneNumber, empresaperfils.Email2, empresaperfils.RNC, empresaperfils.Pais, empresaperfils.Ciudad, empresaperfils.Direccion, empresaperfils.dateadd FROM empresaperfils INNER JOIN AspNetUsers ON AspNetUsers.TypeUser = 69784 and AspNetUsers.Id = '{id}' and empresaperfils.IdUser = '{id}' and empresaperfils.email = '{mail}'"; /*SELECT * FROM empresaperfils WHERE TypeUser = 69784 and IdUser = '{id}' and email = '{mail}'*/
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    ncontar = 0;

                    while (dataReader.Read())
                    {
                        Empresaperfil company       = new Empresaperfil();
                        company.Id                  = Convert.ToInt64(dataReader["Id"]);
                        company.IdUser              = dataReader.GetGuid(dataReader.GetOrdinal("IdUser"));
                        company.Idempresa           = dataReader.GetGuid(dataReader.GetOrdinal("Idempresa"));
                        company.EmpresaCentro       = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        company.PhoneNumber         = Convert.ToString(dataReader["PhoneNumber"]);
                        company.Email2              = Convert.ToString(dataReader["Email2"]);
                        company.RNC                 = Convert.ToString(dataReader["RNC"]);
                        company.Pais                = Convert.ToString(dataReader["Pais"]);
                        company.Ciudad              = Convert.ToString(dataReader["Ciudad"]);
                        company.Direccion           = Convert.ToString(dataReader["Direccion"]);
                        company.dateadd             = Convert.ToDateTime(dataReader["dateadd"]);
                        companyList.Add(company);
                        ncontar = ncontar + 1;
                    }
                }
            }
            ViewBag.companyList = companyList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Job,Idnumempresa,Idempresa,IdUser,Titulotrabajo,Descripciontrabajo,Requisitostrabajo,Ciudadtrabajo,Salariotratar,Salario,Salariohasta,publicosino,desde,hasta,TipoContrato,jornadahrs,diaslaborables,edadminima,edadmaxima,sexo,status,dateadd,idiomas,Areaprofesional,salarioultimoMON,salarioaspiraMON")] Empleo empleo)
        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            ViewBag.userId = id;
            ViewBag.mail = mail;
            int ncontar;

            List<Empresaperfil> companyList = new List<Empresaperfil>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"SELECT empresaperfils.Id, empresaperfils.IdUser, empresaperfils.Idempresa ,empresaperfils.EmpresaCentro, empresaperfils.PhoneNumber, empresaperfils.Email2, empresaperfils.RNC, empresaperfils.Pais, empresaperfils.Ciudad, empresaperfils.Direccion, empresaperfils.dateadd FROM empresaperfils INNER JOIN AspNetUsers ON AspNetUsers.TypeUser = 69784 and AspNetUsers.Id = '{id}' and empresaperfils.IdUser = '{id}' and empresaperfils.email = '{mail}'"; /*SELECT * FROM empresaperfils WHERE TypeUser = 69784 and IdUser = '{id}' and email = '{mail}'*/
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    ncontar = 0;
                    while (dataReader.Read())
                    {
                        Empresaperfil company = new Empresaperfil();
                        company.Id                  = Convert.ToInt64(dataReader["Id"]);
                        company.IdUser              = dataReader.GetGuid(dataReader.GetOrdinal("IdUser"));
                        company.Idempresa           = dataReader.GetGuid(dataReader.GetOrdinal("Idempresa"));
                        company.EmpresaCentro       = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        company.PhoneNumber         = Convert.ToString(dataReader["PhoneNumber"]);
                        company.Email2              = Convert.ToString(dataReader["Email2"]);
                        company.RNC                 = Convert.ToString(dataReader["RNC"]);
                        company.Pais                = Convert.ToString(dataReader["Pais"]);
                        company.Ciudad              = Convert.ToString(dataReader["Ciudad"]);
                        company.Direccion           = Convert.ToString(dataReader["Direccion"]);
                        company.dateadd             = Convert.ToDateTime(dataReader["dateadd"]);
                        ViewBag.newIdempresa        = dataReader.GetGuid(dataReader.GetOrdinal("Idempresa"));
                        ViewBag.newIdnumempresa     = Convert.ToInt64(dataReader["Id"]);
                        companyList.Add(company);
                        ncontar = ncontar + 1;
                    }
                }
            }
            //---------OBTENER PERFIL DE LA EMPRESA---------------------------------------------------------
            ViewBag.newIdempresa = ViewBag.newIdempresa;
            ViewBag.newIdnumempresa = ViewBag.newIdnumempresa;
            ViewBag.companyList = companyList;
            if (ModelState.IsValid)
            {
                Guid Job                = Guid.NewGuid();
                Guid Idempresa          = ViewBag.newIdempresa;
                Int64 Idnumempresa      = ViewBag.newIdnumempresa;
                empleo.Job              = Job;
                empleo.Idnumempresa     = Idnumempresa;
                empleo.Idempresa        = Idempresa;
                _context.Add(empleo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Empresas", "Home");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                                       .Where(y => y.Count > 0)
                                       .ToList();
            }
            return View(empleo);
        }


        //================================================================================================================================
        // GET: Empleoofertas/Edit/5
        //================================================================================================================================
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleo = await _context.empleo.FindAsync(id);
            if (empleo == null)
            {
                return NotFound();
            }
            return View(empleo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Job,Idnumempresa,Idempresa,IdUser,Titulotrabajo,Descripciontrabajo,Requisitostrabajo,Ciudadtrabajo,Salariotratar,Salario,Salariohasta,publicosino,desde,hasta,TipoContrato,jornadahrs,diaslaborables,edadminima,edadmaxima,sexo,status,dateadd")] Empleo empleo)
        {
            if (id != empleo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleoExists(empleo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Empresas", "Home");
            }
            return View(empleo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarbyE(int pag, string buscar, long Id, Guid vc, string Ide, string Titulotrabajo, string Descripciontrabajo, string Requisitostrabajo, string Ciudadtrabajo, Boolean Salariotratar, double Salario, double Salariohasta, Boolean publicosino, DateTime desde, DateTime hasta, int TipoContrato, string jornadahrs, string diaslaborables, int edadminima, int edadmaxima, int sexo, string idiomas, string Areaprofesional, string salarioultimoMON, string salarioaspiraMON, Guid IdU, Empleo empleob)
        {
            var nPagereturnUrl = pag;
            var labusqueda = buscar;
            if (nPagereturnUrl < 1) { nPagereturnUrl = 1; }
            ViewBag.sPagereturnUrl = nPagereturnUrl;

            if (Id != empleob.Id && vc != empleob.Job)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Empleo empleo = new Empleo
                    {
                        status              = true,
                        dateadd             = DateTime.Now,
                        Titulotrabajo       = Titulotrabajo,
                        Descripciontrabajo  = Descripciontrabajo,
                        Requisitostrabajo   = Requisitostrabajo,
                        Ciudadtrabajo       = Ciudadtrabajo,
                        Salariotratar       = true,
                        Salario             = Salario,
                        Salariohasta        = Salariohasta,
                        publicosino         = publicosino,
                        desde               = desde,
                        hasta               = hasta,
                        TipoContrato        = TipoContrato,
                        diaslaborables      = diaslaborables,
                        edadminima          = edadminima,
                        edadmaxima          = edadmaxima,
                        sexo                = sexo,
                        idiomas             = idiomas,
                        Areaprofesional     = Areaprofesional,
                        salarioultimoMON    = salarioultimoMON,
                        salarioaspiraMON    = salarioaspiraMON,
                        jornadahrs          = jornadahrs,
                    };

                    //***************************************************************************************************
                    Empleo d                = _context.empleo.Where(s => s.Id == Id).First();
                    d.Titulotrabajo         = Titulotrabajo;
                    d.Descripciontrabajo    = Descripciontrabajo;
                    d.Requisitostrabajo     = Requisitostrabajo;
                    d.Ciudadtrabajo         = Ciudadtrabajo;
                    d.Salariotratar         = Salariotratar;
                    d.Salario               = Salario;
                    d.Salariohasta          = Salariohasta;
                    d.publicosino           = publicosino;
                    d.desde                 = desde;
                    d.hasta                 = hasta;
                    d.TipoContrato          = TipoContrato;
                    d.diaslaborables        = diaslaborables;
                    d.edadminima            = edadminima;
                    d.edadmaxima            = edadmaxima;
                    d.sexo                  = sexo;
                    d.idiomas               = idiomas;
                    d.Areaprofesional       = Areaprofesional;
                    d.salarioultimoMON      = salarioultimoMON;
                    d.salarioaspiraMON      = salarioaspiraMON;
                    d.jornadahrs            = jornadahrs;
                    _context.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleoExists(empleob.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Redirect("~/Home/Empresas/?page="+pag+"&buscar="+labusqueda);
            }
            return View(empleob);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarbyA(int pag, string buscar, string buscarpor, int all, long Id, Guid vc, string Ide, string Titulotrabajo, string Descripciontrabajo, string Requisitostrabajo, string Ciudadtrabajo, Boolean Salariotratar, double Salario, double Salariohasta, Boolean publicosino, DateTime desde, DateTime hasta, int TipoContrato, string jornadahrs, string diaslaborables, int edadminima, int edadmaxima, int sexo, string idiomas, string Areaprofesional, string salarioultimoMON, string salarioaspiraMON, Guid IdU, EmpleoAdmin empleob)
        {
            var nPagereturnUrl  = pag;
            var labusqueda      = buscar;
            var _buscarpor      = buscarpor;
            var _all            = all;
            int nextUpdate      = 0;
            if (nPagereturnUrl < 1) { nPagereturnUrl = 1; }
            ViewBag.sPagereturnUrl = nPagereturnUrl;

            if (Id != empleob.Id && vc != empleob.Job)
            {
                ViewBag.info = empleob.Id + " " + empleob.Job + "si no existe #1";
                return NotFound();
            }
            else { 
                ViewBag.info2 = empleob.Id + " " + empleob.Job + "si existe #1 aaaaa";
                nextUpdate = 1;
            }

            if (nextUpdate == 1)
            {
                try
                {
                    ViewBag.info = empleob.Id + " " + empleob.Job + "TRY MODEL IS VALID";

                    EmpleoAdmin empleo = new EmpleoAdmin
                    {
                        status                  = true,
                        dateadd                 = DateTime.Now,
                        Titulotrabajo           = Titulotrabajo,
                        Descripciontrabajo      = Descripciontrabajo,
                        Requisitostrabajo       = Requisitostrabajo,
                        Ciudadtrabajo           = Ciudadtrabajo,
                        Salariotratar           = true,
                        Salario                 = Salario,
                        Salariohasta            = Salariohasta,
                        publicosino             = publicosino,
                        desde                   = desde,
                        hasta                   = hasta,
                        TipoContrato            = TipoContrato,
                        diaslaborables          = diaslaborables,
                        edadminima              = edadminima,
                        edadmaxima              = edadmaxima,
                        sexo                    = sexo,
                        idiomas                 = idiomas,
                        Areaprofesional         = Areaprofesional,
                        salarioultimoMON        = salarioultimoMON,
                        salarioaspiraMON        = salarioaspiraMON,
                        jornadahrs = jornadahrs,
                    };

                    //***************************************************************************************************
                    Empleo d                    = _context.empleo.Where(s => s.Id == Id).First();
                    d.Titulotrabajo             = Titulotrabajo;
                    d.Descripciontrabajo        = Descripciontrabajo;
                    d.Requisitostrabajo         = Requisitostrabajo;
                    d.Ciudadtrabajo             = Ciudadtrabajo;
                    d.Salariotratar             = Salariotratar;
                    d.Salario                   = Salario;
                    d.Salariohasta              = Salariohasta;
                    d.publicosino               = publicosino;
                    d.desde                     = desde;
                    d.hasta                     = hasta;
                    d.TipoContrato              = TipoContrato;
                    d.diaslaborables            = diaslaborables;
                    d.edadminima                = edadminima;
                    d.edadmaxima                = edadmaxima;
                    d.sexo                      = sexo;
                    d.idiomas                   = idiomas;
                    d.Areaprofesional           = Areaprofesional;
                    d.salarioultimoMON          = salarioultimoMON;
                    d.salarioaspiraMON          = salarioaspiraMON;
                    d.jornadahrs                = jornadahrs;
                    _context.SaveChanges();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleoExists(empleob.Id))
                    {
                        ViewBag.info = empleob.Id + " " + empleob.Job + "si no existe #2";
                        return NotFound();
                    }
                    else
                    {
                        ViewBag.info = empleob.Id + " " + empleob.Job + "si SI existe";
                        throw;
                    }
                }

                return Redirect("~/Admin/VerEmpleos/?page=" + pag + "&buscar=" + labusqueda +"&buscarpor="+_buscarpor+"&all="+_all);
            }
            else
            {
                var errorList = ModelState.ToDictionary(kvp => kvp.Key,kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
);
                ViewBag.info = empleob.Id + " " + empleob.Job + "modelo no valido" + " " + errorList;
                return View(empleob);
            }
        }


        public async Task<IActionResult> Deletesaw(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleo = await _context.empleo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleo == null)
            {
                return NotFound();
            }

            return View(empleo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var empleo = await _context.empleo.FindAsync(id);
            _context.empleo.Remove(empleo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Empresas", "Home");
        }

        private bool EmpleoExists(long id)
        {
            return _context.empleo.Any(e => e.Id == id);
        }
    }
}
