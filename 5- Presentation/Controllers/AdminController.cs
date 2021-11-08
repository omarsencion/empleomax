using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Hosting;

using System.Diagnostics;
using X.PagedList;
using Microsoft.AspNetCore.Http;
using EmpleosWebMax.Infrastructure.Core;

namespace EmpleosWebMax.UI.Web.Controllers
{
    public class AdminController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;


        public AdminController(IConfiguration configuration, ILogger<AdminController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            Configuration = configuration;
            _context = context;
            webHostEnvironment = hostEnvironment;

        }


        public IActionResult Index()
        {

            return View();
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ VER EMPLEOS ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//        
        [Authorize]
        public IActionResult VerEmpleos(string buscarpor, string buscar, int all, int? page, /*string buscar,*/ string refe, int ns)
        {
            ViewBag.buscarpor   = "";
            ViewBag.buscar      = "";
            ViewBag.all         = all;
            all = ViewBag.all;
            if (page < 1) { page = 1; }
            ViewBag.sPagereturnUrl = page;
            string sTypeCompany = "";
            if(all == 0) { sTypeCompany = $" AND AspNetUsers.TypeAdd > 0"; }
            else if(all == 1){ sTypeCompany = $" AND AspNetUsers.TypeAdd = 2 "; }
            else { sTypeCompany = $" AND AspNetUsers.TypeAdd = 2 "; }

            string sBuscarpor    = "";
            if(!String.IsNullOrEmpty(buscarpor) && !String.IsNullOrEmpty(buscar))
            {
                if (buscarpor == "vacante") { sBuscarpor = $" And empleo.Titulotrabajo like '%{buscar}%'"; ViewBag.buscarpor = buscarpor; ViewBag.buscar = buscar; }   
                else if (buscarpor == "empresa") { sBuscarpor = $" And empresaperfils.EmpresaCentro like '%{buscar}%'"; ViewBag.buscarpor = buscarpor; ViewBag.buscar = buscar; }
                else { sBuscarpor = ""; }
            } else { sBuscarpor = ""; }
            List<EmpleoDto> EmpresaList = new List<EmpleoDto>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"select empleo.*,empresaperfils.EmpresaCentro from empleo JOIN empresaperfils ON empleo.Idempresa = empresaperfils.Idempresa JOIN AspNetUsers ON AspNetUsers.Id = empresaperfils.IdUser WHERE empleo.statusGral < 20 And AspNetUsers.TypeUser = 69784 {sTypeCompany} {sBuscarpor} ORDER BY empleo.Id DESC;";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        EmpleoDto Userx = new EmpleoDto();
                        Userx.Id = Convert.ToInt64(dataReader["Id"]);
                        Guid To_guid            = Guid.Parse(dataReader["Job"].ToString());
                        Userx.Job               = To_guid;
                        Userx.Titulotrabajo     = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        Userx.Ciudadtrabajo     = Convert.ToString(dataReader["Ciudadtrabajo"]).ToUpper();
                        Userx.publicosino       = Convert.ToBoolean(dataReader["publicosino"]);
                        Userx.diaslaborables    = Convert.ToString(dataReader["diaslaborables"]).ToUpper();
                        Userx.dateadd           = Convert.ToDateTime(dataReader["dateadd"]);
                        Userx.EmpresaCentro     = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        Userx.statusGral        = Convert.ToInt32(dataReader["statusGral"]);
                        EmpresaList.Add(Userx);
                    }
                }
            }
 
            var pageNumber = page ?? 1; 
            int pageSize = 4; 
            var onePageOfEmpleos2 = EmpresaList.ToPagedList(pageNumber, pageSize);
            return View(onePageOfEmpleos2); 
        }

        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult Main()
        {
            var id = _userManager.GetUserId(User);
            string UserTo_ = "";
            //---------APLICAR ---------------------------------------------------------
            List<TicketDto> UserxList = new List<TicketDto>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string sql2 = $"Select * From Tickets where To_ = '{id}' ORDER BY Id DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        TicketDto Userx     = new TicketDto();
                        Userx.Id            = Convert.ToInt64(dataReader["Id"]);
                        Userx.Categoria     = Convert.ToString(dataReader["Categoria"]).ToUpper();
                        Userx.Titulo        = Convert.ToString(dataReader["Titulo"]).ToUpper();
                        Userx.Mensaje       = Convert.ToString(dataReader["Mensaje"]).ToUpper();
                        Userx.StatusTicket  = Convert.ToInt16(dataReader["StatusTicket"]);
                        Userx.TicketNumber  = Convert.ToString(dataReader["TicketNumber"]);
                        Userx.From_         = dataReader.GetGuid(dataReader.GetOrdinal("From_"));
                        Userx.To_           = dataReader.GetGuid(dataReader.GetOrdinal("To_"));
                        string From_x       = Userx.From_.ToString();
                        if (From_x != null)
                        {
                            var d           = _context.Users.Where(s => s.Id.Equals(From_x)).First();
                            UserTo_         = d.FirstName + " " + d.LastName;
                        }
                        Userx.NameTo_ = UserTo_;
                        Userx.FechaTicket = Convert.ToDateTime(dataReader["FechaTicket"]);
                        ViewBag.IdUser = Userx.Id;

                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;
            return View();

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ LOGOUT +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//  
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Admin");
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ EMPRESAS LISTADO +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//        
        [Authorize]
        [HttpGet]
        [HttpPost]
        public IActionResult Empresas()
        {
            //----------------------------------------------EMPRESAS ---------------------------------------------------------
            List<Empresaperfil> EmpresaList = new List<Empresaperfil>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string sql2 = $"Select * from empresaperfils order by Id DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Empresaperfil Userx = new Empresaperfil();
                        Userx.Id            = Convert.ToInt64(dataReader["Id"]);
                        Userx.email         = Convert.ToString(dataReader["email"]).ToUpper();
                        Userx.EmpresaCentro = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        Userx.RNC           = Convert.ToString(dataReader["RNC"]).ToUpper();
                        Userx.Pais          = Convert.ToString(dataReader["Pais"]).ToUpper();
                        Userx.Ciudad        = Convert.ToString(dataReader["Ciudad"]).ToUpper();
                        Userx.Foto          = Convert.ToString(dataReader["Foto"]).ToUpper();
                        ViewBag.Id          = Userx.Id;
                        EmpresaList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = EmpresaList;
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ EMPRESAS LISTADO BUSQUEDA ++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        public IActionResult EmpresasB()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> EmpresasB(string buscar, int q)
        {
            int newQuebuscar = 0;
            int sQuebuscar = 0;
            string columna = "";
            string sColumna = "";
            newQuebuscar = q;

            //-----------------------------COLUMNA DONDE BUSCAR------------------------------------
            if (newQuebuscar == 1) { columna = "EmpresaCentro"; sColumna = "Nombre de Empresa"; }
            else if (newQuebuscar == 2) { columna = "RNC"; sColumna = "número de RNC"; }
            else if (newQuebuscar == 3) { columna = "Pais"; sColumna = "el País"; }
            else if (newQuebuscar == 4) { columna = "Ciudad"; sColumna = "la Ciudad"; }
            else { newQuebuscar = 0; }
            //-----------------------------QUE BUSCAR ---------------------------------------------
            if (!String.IsNullOrEmpty(buscar)) { sQuebuscar = 1; }
            string buscarx = buscar;
            string buscars = null;

            ViewBag.q = newQuebuscar;
            ViewBag.columna = sColumna;
            ViewBag.sBuscar = buscar;
            if (newQuebuscar > 0 && newQuebuscar < 5 && sQuebuscar == 1) { buscars = $" Where {columna} like '%{buscarx}%'"; }
            else { buscars = "Where EmpresaCentro = 'uy44-d-d44f11f4d-sdhtsr4785d---ddd'"; }

            //----------------------------------------------EMPRESAS ---------------------------------------------------------
            List<Empresaperfil> EmpresaList = new List<Empresaperfil>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select * from empresaperfils {buscars} order by Id DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Empresaperfil Userx = new Empresaperfil();
                        Userx.Id            = Convert.ToInt64(dataReader["Id"]);
                        Userx.email         = Convert.ToString(dataReader["email"]).ToUpper();
                        Userx.EmpresaCentro = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        Userx.RNC           = Convert.ToString(dataReader["RNC"]).ToUpper();
                        Userx.Pais          = Convert.ToString(dataReader["Pais"]).ToUpper();
                        Userx.Ciudad        = Convert.ToString(dataReader["Ciudad"]).ToUpper();
                        Userx.Foto          = Convert.ToString(dataReader["Foto"]).ToUpper();
                        ViewBag.Id          = Userx.Id;
                        EmpresaList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = EmpresaList;
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ USUARIOS LISTADO BUSQUEDA ++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        public IActionResult Usuarios()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Usuarios(string buscar, int q)
        {
            int newQuebuscar = 0;
            int sQuebuscar = 0;
            string columna = "";
            string sColumna = "";
            newQuebuscar = q;

            //-----------------------------COLUMNA DONDE BUSCAR------------------------------------
            if (newQuebuscar == 1) { columna = "AspNetUsers.FirstName"; sColumna = "por Nombre"; }
            else if (newQuebuscar == 2) { columna = "AspNetUsers.LastName"; sColumna = "por Apellido"; }
            else if (newQuebuscar == 3) { columna = "userInfos.Pais"; sColumna = "el País"; }
            else if (newQuebuscar == 4) { columna = "userInfos.Ciudad"; sColumna = "la Ciudad"; }
            else if (newQuebuscar == 5) { columna = "AspNetUsers.UserName"; sColumna = "eMail"; }
            else { newQuebuscar = 0; }
            //-----------------------------QUE BUSCAR ---------------------------------------------
            if (!String.IsNullOrEmpty(buscar)) { sQuebuscar = 1; }
            string buscarx = buscar;
            string buscars = null;

            ViewBag.q = newQuebuscar;
            ViewBag.columna = sColumna;
            ViewBag.sBuscar = buscar;
            if (newQuebuscar > 0 && newQuebuscar < 6 && sQuebuscar == 1) { buscars = $" Where TypeUser = 255485 AND {columna} like '%{buscarx}%'"; }
            else { buscars = "Where TypeUser = 255485 AND FirstName = 'uy44-d-d44f11f4d-sdhtsr4785d--5-ddd'"; }
            //----------------------------------------------EMPRESAS ---------------------------------------------------------
            List<Personas> EmpresaList = new List<Personas>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select AspNetUsers.*,userInfos.* from AspNetUsers JOIN userInfos ON userInfos.IdUser = AspNetUsers.Id {buscars} order by userInfos.dateadd DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Personas Userx = new Personas();

                        Guid To_guid        = Guid.Parse(dataReader["Id"].ToString());
                        Userx.Id            = To_guid;
                        Userx.UserName      = Convert.ToString(dataReader["UserName"]).ToUpper();
                        Userx.FirstName     = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName      = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Pais          = Convert.ToString(dataReader["Pais"]).ToUpper();
                        Userx.Ciudad        = Convert.ToString(dataReader["Ciudad"]).ToUpper();
                        Userx.Estadolaboral = Convert.ToString(dataReader["Estadolaboral"]).ToUpper();
                        Userx.Foto          = Convert.ToString(dataReader["Foto"]).ToUpper();
                        ViewBag.Id          = Userx.Id;
                        EmpresaList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = EmpresaList;
            return View();
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ USUARIOS LISTADO GENERAL  ++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        public IActionResult UsuariosAll()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> UsuariosAll(string buscarNombre, string buscarApellido, int q)
        {
            string sQuebuscarNombre = "";
            string sQuebuscarApellido = "";
            //-----------------------------QUE BUSCAR ---------------------------------------------
            if (!String.IsNullOrEmpty(buscarNombre)) { sQuebuscarNombre = $" AND AspNetUsers.FirstName LIKE '%{buscarNombre}%' "; } else { sQuebuscarNombre = ""; }
            if (!String.IsNullOrEmpty(buscarApellido)) { sQuebuscarApellido = $" AND AspNetUsers.LastName LIKE '%{buscarApellido}%' "; } else { sQuebuscarApellido = ""; }
            //----------------------------------------------EMPRESAS ---------------------------------------------------------
            List<Personas> EmpresaList = new List<Personas>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select AspNetUsers.*,userInfos.* from AspNetUsers JOIN userInfos ON userInfos.IdUser = AspNetUsers.Id WHERE AspNetUsers.TypeUser = 255485 {sQuebuscarNombre} {sQuebuscarApellido} order by userInfos.dateadd DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Personas Userx = new Personas();

                        Guid To_guid = Guid.Parse(dataReader["Id"].ToString());
                        Userx.Id            = To_guid;
                        Userx.StatusGeneral = Convert.ToInt16(dataReader["StatusGeneral"]); //**************************
                        Userx.UserName      = Convert.ToString(dataReader["UserName"]).ToUpper();
                        Userx.FirstName     = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName      = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Pais          = Convert.ToString(dataReader["Pais"]).ToUpper();
                        Userx.Ciudad        = Convert.ToString(dataReader["Ciudad"]).ToUpper();
                        Userx.Estadolaboral = Convert.ToString(dataReader["Estadolaboral"]).ToUpper();
                        Userx.Foto          = Convert.ToString(dataReader["Foto"]).ToUpper();
                        ViewBag.Id          = Userx.Id;
                        EmpresaList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = EmpresaList;
            return View();
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++   L O G I N ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {


            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
                if (result.Succeeded)
                {
                    ViewBag.CheckUserType   = 0;
                    var id                  = _userManager.GetUserId(User);
                    var mail                = user.Email;//_userManager.GetUserName(User);
                    ViewBag.iduser          = "00000000-0000-0000-0000-000000000000";
                    int nRealStatus         = 0;
                    int statusgen           = 0;

                    List<ApplicationUserDto> lasexperienciasList = new List<ApplicationUserDto>();
                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = $"SELECT * from AspNetUsers WHERE typeUser = 987541254 And Status = 5 And UserName = '{mail}'";
                        SqlCommand command = new SqlCommand(sql, connection);
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                ApplicationUserDto lasexperiencias  = new ApplicationUserDto();
                                Guid IdUserCandidatoguid            = Guid.Parse(dataReader["Id"].ToString());
                                lasexperiencias.Id                  = IdUserCandidatoguid;
                                lasexperiencias.Status              = Convert.ToInt16(dataReader["Status"]);
                                nRealStatus                         = lasexperiencias.Status;
                                lasexperiencias.StatusGeneral       = Convert.ToInt32(dataReader["StatusGeneral"]);
                                statusgen                           = lasexperiencias.StatusGeneral;
                                lasexperiencias.TypeUser            = Convert.ToInt32(dataReader["TypeUser"]);
                                ViewBag.iduser                      = lasexperiencias.Id;
                                ViewBag.CheckUserType               = lasexperiencias.TypeUser;
                                lasexperienciasList.Add(lasexperiencias);

                            }
                        }
                    }


                    if (ViewBag.CheckUserType == 987541254 && nRealStatus == 5 && statusgen == 1)
                    {
                        return RedirectToAction("Main", "Admin");
                    }
                    return RedirectToAction("Logout", "Admin");
                }


                ModelState.AddModelError(string.Empty, "ingrese un email y password validos");
                return View(user);
            }
            return View(user);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ VACANTES  ++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        public IActionResult Vacantes()
        {
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ CHECK USER +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        [HttpGet]
        [HttpPost]
        //[AllowAnonymous]
        [Authorize]
        public async Task<IActionResult> CheckUserName(string username)
        {
            string mensajeUserName = "";
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                mensajeUserName = "Error, digite otro email";
            }
            else { mensajeUserName = ""; }

            ViewBag.mensajeUserName = mensajeUserName;
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ CREAR EMPRESA Y VACANTES B +++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//  
        public IActionResult CEV3()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CEV3(AddVacante vacante) //, string username
        {
            string myuser = "";
            myuser = vacante.UserName;
            int nUserName = 0;
            ViewBag.myuser = myuser;
            //=========================== VALIDAR USUARIO OTRA VEZ ==============================
            var user = await _userManager.FindByNameAsync(myuser);
            if (user != null)
            {
                nUserName = 1;
            }
            else { nUserName = 2; }
            return View(vacante);

        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ CREAR EMPRESA Y VACANTES C +++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   

        public IActionResult CEV()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> CEV(EmpresaViewModel model, string UserName, string EmpresaCentro, string RNC, string PhoneNumber, string Pais, string Ciudad, string Direccion, string Titulotrabajo, string Descripciontrabajo, string Requisitostrabajo, string Ciudadtrabajo, double Salario, double Salariohasta, Boolean publicosino, DateTime desde, DateTime hasta, int TipoContrato, string jornadahrs, string diaslaborables, int edadminima, int edadmaxima, int sexo, string idiomas, string Areaprofesional, string salarioultimoMON, string salarioaspiraMON, IFormFile file) //RegisterViewModel model

        {
            int abc             = 1; // Validar e iniciar la creacion de usuarios
            int addEmpresa      = 0; // validar e insertar empresa
            int addEmpleo       = 0; // validar e insertar empresa
            ViewBag.Id          = "00000000-0000-0000-0000-000000000000";
            string Password     = "123456";
            string UserName2    = UserName;
            ViewBag.UserName2   = UserName2;
            if (abc == 1)
            {
                var user = new ApplicationUser()
                {
                    UserName        = UserName2,
                    Email           = UserName2,
                    DateAdd         = DateTime.Now,
                    TypeUser        = 69784,
                    FirstName       = "System",
                    LastName        = "EmpleoMax",
                    Sexo            = true,
                    Status          = 69,
                    StatusGeneral   = 2,
                    TypeAdd         = 2,
                };

                var result = await _userManager.CreateAsync(user, Password);

                if (result.Succeeded)
                {
                    addEmpresa      = 1;
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    ViewBag.Id      = _userManager.GetUserId(User);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                } 

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                ViewBag.asu = $"Add User Ok #{addEmpresa}";
            }
            else { ViewBag.asu = "hola 2"; addEmpresa = 0; }

            //============================== SEARCH USER FOR EMPRESA PERFIL =======================
            Guid To_guid = new Guid();
            List<Personas> EmpresaList = new List<Personas>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select Id from AspNetUsers WHERE UserName = '{UserName2}'";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    addEmpresa = 0;
                    while (dataReader.Read())
                    {
                        Personas Userx  = new Personas();
                        To_guid         = Guid.Parse(dataReader["Id"].ToString());
                        Userx.Id        = To_guid;
                        ViewBag.Id      = Userx.Id;
                        EmpresaList.Add(Userx);
                        addEmpresa      = 1;
                    }
                }
            }
            //=============================== ADD EMPRESA PERFIL ===============================================
            var fileName = "";
            int uploadif = 0;
            if (file != null)
            {
                fileName = System.IO.Path.GetFileName(file.FileName); //=== N O M B R E   DEL  DOCUMENTO A SUBIR
            }
            else { fileName = "adduser.png"; uploadif = 1; }
            //=============================== NOBRE AL DOCUMENTO A SUBIR =======================================

            string Namedocument = fileName;
            if (uploadif == 0)
            { 
                int sizeOfString = Namedocument.Length;
                int sizeX = 0;
                string SubString = "";
                if (sizeOfString > 10)
                {
                    sizeX = sizeOfString - 10;
                    int newVal = Namedocument.Length - (Namedocument.Length - sizeX);
                    SubString = Namedocument.Substring(newVal);
                }
                else { SubString = Namedocument; }
                Random rnd = new Random();
                int newrnd = rnd.Next(5000, 798855);
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var finalString = new String(stringChars);
                Namedocument = finalString + newrnd + "_" + SubString;
                fileName = Namedocument;

                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "logo");
                var fileName2 = Path.Combine(uploadsFolder, fileName);
                using (var localFile = System.IO.File.OpenWrite(fileName2))
                using (var uploadedFile = file.OpenReadStream())
                {
                    uploadedFile.CopyTo(localFile);
                }
            }
            //=====================================================================================================


            string EmpresaCentro2   = EmpresaCentro;
            string RNC2             = RNC;
            string PhoneNumber2     = PhoneNumber;
            string Pais2            = Pais;
            string Ciudad2          = Ciudad;
            string Direccion2       = Direccion;
            //string IdUserNEW        = ViewBag.Id;
            if (addEmpresa == 1)
            {
                //string uniqueFileName = UploadedFile(model);
                Empresaperfil empresaperfils = new Empresaperfil
                {
                    IdUser          = To_guid, //ViewBag.Id, //IdUser = Guid.Parse(ViewBag.Id),
                    email           = ViewBag.UserName2,
                    EmpresaCentro   = EmpresaCentro2,
                    PhoneNumber     = PhoneNumber2,
                    Email2          = ViewBag.UserName2,
                    RNC             = RNC2,
                    Pais            = Pais2,
                    Ciudad          = Ciudad2,
                    Direccion       = Direccion2,
                    status          = true,
                    Idempresa       = Guid.NewGuid(),
                    Foto            = fileName,
                    dateadd         = DateTime.Now,
                };
                _context.Add(empresaperfils);
                await _context.SaveChangesAsync();
                addEmpleo = 1;
            }
            //===================================AGREGAR EMPLEO =================================================================================
            ViewBag.addEmplosGral = 0;
           if (addEmpleo == 1)
            {
                List<Empresaperfil> companyList = new List<Empresaperfil>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"SELECT * FROM empresaperfils WHERE IdUser = '{ViewBag.Id}' and email = '{ViewBag.UserName2}'"; 
                    SqlCommand command = new SqlCommand(sql, connection);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Empresaperfil company   = new Empresaperfil();
                            ViewBag.newIdempresa    = dataReader.GetGuid(dataReader.GetOrdinal("Idempresa"));
                            ViewBag.newIdnumempresa = Convert.ToInt64(dataReader["Id"]);
                            ViewBag.addEmplosGral   = 2;
                            companyList.Add(company);
                        }
                    }
                }
            }
            
            
            //---------OBTENER PERFIL DE LA EMPRESA---------------------------------------------------------
            ViewBag.newIdempresa = ViewBag.newIdempresa;
            ViewBag.newIdnumempresa = ViewBag.newIdnumempresa;
            if (ViewBag.addEmplosGral == 2)
            {
                Empleo empleo = new Empleo
                {
                    IdUser              = ViewBag.Id, 
                    status              = true,
                    Idempresa           = ViewBag.newIdempresa,
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


                Guid Job                = Guid.NewGuid();
                Guid Idempresa          = ViewBag.newIdempresa;
                Int64 Idnumempresa      = ViewBag.newIdnumempresa;
                empleo.Job              = Job;
                empleo.Idnumempresa     = Idnumempresa;
                empleo.Idempresa        = Idempresa;
                _context.Add(empleo);
                await _context.SaveChangesAsync();
                return RedirectToAction("VerEmpleos", "Admin");
            }

            return View();
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ VER EMPESAS 2 ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//        
        [Authorize]
        public IActionResult VerEmresas2()
        {

            //----------------------------------------------EMPRESAS ---------------------------------------------------------
            List<Empresaperfil> EmpresaList = new List<Empresaperfil>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"select empresaperfils.* from empresaperfils JOIN  AspNetUsers ON AspNetUsers.Id = empresaperfils.IdUser WHERE AspNetUsers.TypeUser = 69784 AND AspNetUsers.TypeAdd = 2 ORDER BY empresaperfils.Id DESC;";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Empresaperfil Userx         = new Empresaperfil();
                        Userx.Id                    = Convert.ToInt64(dataReader["Id"]);
                        Userx.EmpresaCentro         = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        Userx.PhoneNumber           = Convert.ToString(dataReader["PhoneNumber"]);
                        Userx.Email2                = Convert.ToString(dataReader["Email2"]);
                        Userx.RNC                   = Convert.ToString(dataReader["RNC"]);
                        Userx.Pais                  = Convert.ToString(dataReader["Pais"]);
                        Userx.Ciudad                = Convert.ToString(dataReader["Ciudad"]);
                        Userx.Direccion             = Convert.ToString(dataReader["Direccion"]);
                        Userx.dateadd               = Convert.ToDateTime(dataReader["dateadd"]);
                        Userx.Idempresa             = dataReader.GetGuid(dataReader.GetOrdinal("Idempresa"));
                        Userx.Foto                  = Convert.ToString(dataReader["Foto"]);
                        Userx.IdUser                = dataReader.GetGuid(dataReader.GetOrdinal("IdUser"));
                        EmpresaList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = EmpresaList;
            return View();
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ CREAR VACANTES Con emresas ya creadas  ++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        public IActionResult CEV2a(int Id, string Ide, string IdU)
        {
            ViewBag.newIdempresa = Ide;//ViewBag.newIdempresa;
            ViewBag.newIdnumempresa = Id;//ViewBag.newIdnumempresa;
            ViewBag.IdU = IdU;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CEV2(int Id, string Ide, string Titulotrabajo, string Descripciontrabajo, string Requisitostrabajo, string Ciudadtrabajo, double Salario, double Salariohasta, Boolean publicosino, DateTime desde, DateTime hasta, int TipoContrato, string jornadahrs, string diaslaborables, int edadminima, int edadmaxima, int sexo, string idiomas, string Areaprofesional, string salarioultimoMON, string salarioaspiraMON, Guid IdU) //RegisterViewModel model

        {

            ViewBag.newIdnumempresa     = 0;
            ViewBag.addEmplosGral       = 0;
            ViewBag.newIdempresa        = Guid.Parse(Ide);
            ViewBag.newIdnumempresa     = Id;
            ViewBag.IdU = IdU;

            if (ViewBag.newIdempresa != null && ViewBag.newIdnumempresa > 0 && ViewBag.IdU != null) { ViewBag.addEmplosGral = 2; }

            if (ViewBag.addEmplosGral == 2)
            {
                ViewBag.test = "Hello";

                Empleo empleo = new Empleo
                {
                    Idempresa           = ViewBag.newIdempresa,
                    Idnumempresa        = ViewBag.newIdnumempresa,
                    status              = true,
                    IdUser              = IdU,
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
                    Job                 = Guid.NewGuid(),
            };


                _context.Add(empleo);
                await _context.SaveChangesAsync();
                return RedirectToAction("VerEmpleos", "Admin");
            }
            //====================================================================================================================

            return View();
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS VACANTES Con emresas ya creadas  ++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           [HttpPost]

        [ActionName("EditVacante")]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Post(Empleo empleo, string St, int gdfsd) 

        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            if(gdfsd == null) { gdfsd = 0; }

            int status1 = 0;
            if(St != null) {
                if (St == "a") { status1 = 20; }
                else if (St == "x") { status1 = 0; }
                else if (St == "r") { status1 = 1; } 
                else { status1 = 0; }
            }

            if (gdfsd > 0) 
            {
                Empleo d = _context.empleo.Where(s => s.Id == gdfsd).First();
            d.statusGral        = status1;
            d.statusGralBy      = Guid.Parse(id);
            d.statusGralDate    = DateTime.Now;
            d.statusGralMail    = mail;            
            _context.SaveChanges();
            }

            return RedirectToAction("VerEmpleos", "Admin");
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS USUARIOS Con emresas ya creadas  ++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("EditUsuario")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Update_Post2(Empleo empleo, string St, string gdfsd) 

        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);

            int status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; } 
                else if (St == "x") { status1 = 1; } 
                else if (St == "r") { status1 = 4; } 
                else { status1 = 0; }
            }

            if (status1 < 30  ) //validar Id
            {
                ApplicationUser d = _context.Users.Where(s => s.Id == gdfsd).First();
                d.StatusGeneral = status1;
                _context.SaveChanges();
            }

            return RedirectToAction("UsuariosAll", "Admin");
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS USUARIOS Con emresas ya creadas  ++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           [HttpPost]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Sexo = model.Sexo,
                    TypeUser = 987541254,
                    Status = 5,
                    StatusGeneral = 1,
                    DateAdd = DateTime.Now,
                    TypeAdd = 2,

                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("UsuariosAdm", "Admin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }

        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UsuariosAdm()
        {


            //----------------------------------------------EMPRESAS ---------------------------------------------------------
            List<Personas> EmpresaList = new List<Personas>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select * from AspNetUsers WHERE TypeUser = 987541254 AND StatusGeneral < 20";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Personas Userx = new Personas();
                        Guid To_guid            = Guid.Parse(dataReader["Id"].ToString());
                        Userx.Id                = To_guid;
                        Userx.StatusGeneral     = Convert.ToInt16(dataReader["StatusGeneral"]); //**************************
                        Userx.Status            = Convert.ToInt16(dataReader["Status"]);
                        Userx.UserName          = Convert.ToString(dataReader["UserName"]).ToUpper();
                        Userx.FirstName         = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName          = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.UserName          = Convert.ToString(dataReader["UserName"]).ToUpper();
                        EmpresaList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = EmpresaList;
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS USUARIOS Con emresas ya creadas  ++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("EditUsuarioAdm")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Update_Post2Adm(string St, string gdfsd) 

        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);

            int status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; }
                else if (St == "x") { status1 = 1; } 
                else if (St == "r") { status1 = 4; } 
                else { status1 = 21; }
            }

            if (status1 < 30) //validar Id
            {
                ApplicationUser d = _context.Users.Where(s => s.Id == gdfsd).First();
                d.StatusGeneral = status1;
                _context.SaveChanges();
            }

            return RedirectToAction("UsuariosAdm", "Admin");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Datos(Guid ty6hhdg87u)
        {
            string id = ty6hhdg87u.ToString();
            if (id == null) { return RedirectToAction("UsuariosAdm", "Admin"); }

            //----------------------------------------------EMPRESAS ---------------------------------------------------------
            List<ApplicationUserDto> EmpresaList = new List<ApplicationUserDto>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select * from AspNetUsers WHERE Id = '{id}' AND TypeUser = 987541254";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ApplicationUserDto Userx = new ApplicationUserDto();
                        Guid To_guid = Guid.Parse(dataReader["Id"].ToString());
                        Userx.Id = To_guid;
                        Userx.FirstName         = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName          = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.PhoneNumber       = Convert.ToString(dataReader["PhoneNumber"]).ToUpper();
                        EmpresaList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = EmpresaList;
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS USUARIOS Con emresas ya creadas  ++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("EditarUser")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Update_Adm(string FirstName, string LastName, string PhoneNumber, Guid fcaffsafs) //, Guid Us, string Mi//Empleo empleo, 

        {
            string id = fcaffsafs.ToString();

            if (id == null) { return RedirectToAction("UsuariosAdm", "Admin"); }

            int status1 = 0;
            if (FirstName == null || LastName == null)
            {
                return RedirectToAction("UsuariosAdm", "Admin");
            }

            if (fcaffsafs != Guid.Empty) 
            {
                ApplicationUser d = _context.Users.Where(s => s.Id == id).First();
                d.FirstName     = FirstName;
                d.LastName      = LastName;
                d.PhoneNumber   = PhoneNumber;
                _context.SaveChanges();
            }

            return RedirectToAction("UsuariosAdm", "Admin");
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ Form chg    ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++// 
        [Authorize]
        public IActionResult UpdForm()
        {
            return View();
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ pwd cha    +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        //[HttpGet]
        //[HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePwd(string OldPassword, string NewPassword)
        {
            var id          = _userManager.GetUserId(User);
            string un_      = _userManager.GetUserName(User);
            string newpass  = NewPassword;
            string old      = OldPassword;
            int next        = 0;
            var user        = await _userManager.FindByNameAsync(un_);
            var password    = await _userManager.CheckPasswordAsync(user, old);

            if (password) { ViewBag.pwd = "si"; next = 1; } else { ViewBag.pwd = "no"; return RedirectToAction("UpdForm", "Admin"); }
            if (user == null) { ViewBag.msg = "nulo"; return RedirectToAction("UpdForm", "Admin"); }
            var newPassword     = _userManager.PasswordHasher.HashPassword(user, newpass);
            user.PasswordHash   = newPassword;
            if (next == 1)
            {
                var res = await _userManager.UpdateAsync(user);
                if (res.Succeeded) { ViewBag.msg = "cambio ok"; return RedirectToAction("Logout", "Admin"); }
                else { ViewBag.msg = "no cambiado"; return RedirectToAction("UpdForm", "Admin"); }
            }

            return View();
        }
    }
    }
