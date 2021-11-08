using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using X.PagedList;
using EmpleosWebMax.Infrastructure.Core;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;

namespace empleoswebMax.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ISubscriptionService _subscriptionService;


        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ISubscriptionService subscriptionService)
        {
            _logger             = logger;
            _userManager        = userManager;
            Configuration       = configuration;
            _context            = context;
            webHostEnvironment  = hostEnvironment;
            _subscriptionService = subscriptionService;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Legal()
        {
            return View();
        }
        public IActionResult PropiedadIntelectual()
        {
            return View();
        }


        public IActionResult Terminos()
        {
            return View();
        }

        public IActionResult Politicas()
        {
            return View();
        }

        public IActionResult Responsabilidades()
        {
            return View();
        }

        [HttpPost]
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult Aplicaciones()
        {
            var id                              = _userManager.GetUserId(User);
            var mail                            = _userManager.GetUserName(User);
            ViewBag.userId                      = id;
            ViewBag.mail                        = mail;
            List<ApplicationUser2> UserxList    = new List<ApplicationUser2>();
            string connectionString             = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection     = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM AspNetUsers WHERE TypeUser = 255485 And Id = '{id}' and UserName = '{mail}'";
                SqlCommand command              = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ApplicationUser2 Userx  = new ApplicationUser2();
                        Userx.Id                = Convert.ToString(dataReader["Id"]);
                        Userx.FirstName         = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName          = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Email             = Convert.ToString(dataReader["Email"]);
                        Userx.TypeUser          = Convert.ToInt32(dataReader["TypeUser"]);
                        ViewBag.IdUser          = Userx.Id;
                        UserxList.Add(Userx);
                    }
                }
            }

            //---------OBTENER DATOS DEL CANDIDATO ---------------------------------------------------------
            List<AplicacionEmpleo> Empleox = new List<AplicacionEmpleo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT a.Id as IdApp, a.IdJob as IdJob, a.Job, a.IdUser as IdUser, a.Tracking as Tracking, b.Id as Idoferta, b.Titulotrabajo as Titulotrabajo, b.Ciudadtrabajo as Ciudadtrabajo,b.TipoContrato as TipoContrato,b.Areaprofesional as Areaprofesional,b.dateadd as fechaagregado, c.EmpresaCentro as EmpresaCentro, c.RNC as RNC,  c.Idempresa as Idempresa FROM empleoAdds AS a INNER JOIN empleo AS b ON b.Id = a.IdJob and b.Job = a.Job INNER JOIN empresaperfils AS c ON c.Idempresa = b.Idempresa AND c.IdUser = b.IdUser WHERE a.IdUser = '{ViewBag.IdUser}'";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        AplicacionEmpleo todoempleo = new AplicacionEmpleo();
                        todoempleo.IdApp            = Convert.ToInt64(dataReader["IdApp"]);
                        todoempleo.IdJob            = Convert.ToInt64(dataReader["IdJob"]);
                        todoempleo.Job              = dataReader.GetGuid(dataReader.GetOrdinal("Job"));
                        todoempleo.IdUser           = dataReader.GetGuid(dataReader.GetOrdinal("IdUser"));
                        todoempleo.Tracking         = Convert.ToString(dataReader["Tracking"]);
                        todoempleo.Idoferta         = Convert.ToInt64(dataReader["Idoferta"]);
                        todoempleo.EmpresaCentro    = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        todoempleo.RNC              = Convert.ToString(dataReader["RNC"]);
                        todoempleo.Ciudadtrabajo    = Convert.ToString(dataReader["Ciudadtrabajo"]).ToUpper();
                        todoempleo.Titulotrabajo    = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        todoempleo.TipoContrato     = Convert.ToInt16(dataReader["TipoContrato"]);
                        todoempleo.Areaprofesional  = Convert.ToString(dataReader["Areaprofesional"]);
                        todoempleo.Idempresa        = dataReader.GetGuid(dataReader.GetOrdinal("Idempresa"));
                        todoempleo.fechaagregado    = Convert.ToDateTime(dataReader["fechaagregado"]);
                        Empleox.Add(todoempleo); 
                    }
                }
            }
            ViewBag.UserxList   = UserxList;
            ViewBag.Empleox     = Empleox;
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ VER APLICACIONES +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//        
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult VerAplicaciones(int doc, string laoport)
        {
            var id          = _userManager.GetUserId(User);
            string empresa  = "";
            List<CandidatosList_> UserxList = new List<CandidatosList_>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();
                string sql2 = $"Select empleoAdds.*,empleo.*,AspNetUsers.* from empleoAdds join empleo ON empleo.Job = empleoAdds.Job And empleo.Id = empleoAdds.IdJob join AspNetUsers On AspNetUsers.Id = empleoAdds.IdUser where empleoAdds.IdUser = '{id}' ORDER BY empleoAdds.Id DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        CandidatosList_ Userx           = new CandidatosList_();
                        Userx.IdUser                    = dataReader.GetGuid(dataReader.GetOrdinal("IdUser"));
                        Userx.FirstName                 = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName                  = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Titulotrabajo             = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        Userx.Tracking                  = Convert.ToString(dataReader["Tracking"]).ToUpper();
                        Userx.IdJob                     = Convert.ToInt64(dataReader["IdJob"]);
                        Userx.Job                       = dataReader.GetGuid(dataReader.GetOrdinal("Job"));
                        Userx.IdEmpresa                 = dataReader.GetGuid(dataReader.GetOrdinal("IdEmpresa"));
                        Userx.publicosino = Convert.ToBoolean(dataReader["publicosino"]);

                        if (Userx.IdJob > 0) 
                        {
                            Empresaperfil d     = _context.empresaperfils.Where(s => s.Idempresa.Equals(Userx.IdEmpresa)).First();
                            empresa             = d.EmpresaCentro;
                        }
                        if(Userx.publicosino == true)
                        {
                            empresa             = "Información reservada";
                        }
                        else { empresa = empresa; }
                        Userx.empresa           = empresa.ToUpper();
                        ViewBag.IdUser          = Userx.Id;
                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;
            return View();
        }


        //---------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult Aplicar(int Id, string Oportunidad)
        {
            ViewBag.nnId            = Id;
            ViewBag.Oportunidad     = Oportunidad;
            var id                  = _userManager.GetUserId(User);
            var mail                = _userManager.GetUserName(User);
            ViewBag.userId          = id;
            ViewBag.mail            = mail;
            DateTime dateadd = DateTime.Now;

            List<ApplicationUser2> UserxList = new List<ApplicationUser2>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM AspNetUsers WHERE Id = '{id}' and UserName = '{mail}' and TypeUser = 255485";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ApplicationUser2 Userx  = new ApplicationUser2();
                        Userx.Id                = Convert.ToString(dataReader["Id"]);
                        Userx.FirstName         = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName          = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Email             = Convert.ToString(dataReader["Email"]);
                        Userx.TypeUser          = Convert.ToInt32(dataReader["TypeUser"]);
                        Userx.Status            = Convert.ToInt16(dataReader["Status"]);
                        Userx.Sexo              = Convert.ToBoolean(dataReader["Sexo"]);

                        ViewBag.IdUser = Userx.Id;
                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;

            //----------------------------------------------------------------------------------
            if (ViewBag.nnId == 0 || ViewBag.nnId == null)
            {
                return RedirectToAction("Empleos", "Home");
            }
            Boolean status_         = true;
            string tracking_        = "nuevo";
            int userexiste          = 0;
            Guid IdUserEmpresa_     = new Guid("00000000-0000-0000-0000-000000000000");
            string EmailEmpresa_    = "";
            long idemple            = ViewBag.nnId;
            string IdUserEmpresa_ss = "";
            if (idemple > 0)
            {
                if (_context.empleo.Any(u => u.Id.Equals(idemple)))
                { userexiste = 1; }
                else { userexiste = 0; }
                 
                if (userexiste == 1)
                {
                    Empleo u            = _context.empleo.Where(s => s.Id.Equals(idemple)).First(); 
                    IdUserEmpresa_ss    = u.Idempresa.ToString();
                    Empresaperfil b     = _context.empresaperfils.Where(s => s.Idempresa.Equals(IdUserEmpresa_)).First(); 
                    EmailEmpresa_       = b.email;
                }
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Insert Into empleoAdds (IdJob,Job,IdUser,status,dateadd,Tracking,TrackingAdd,EmailCandidato, EmailEmpresa,IdUserEmpresa) Values " +
                    $"({ViewBag.nnId},'{ViewBag.Oportunidad}','{ViewBag.IdUser}','{status_}','{dateadd}','{tracking_}','{dateadd}','{ViewBag.mail}', '{EmailEmpresa_}','{IdUserEmpresa_ss}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return RedirectToAction("Aplicaciones", "Home");
            }

        }


        //############################################ EMPLEO SHARED ##############################################################
        //############################################ EMPLEO SHARED ##############################################################
        [HttpPost]
        [HttpGet]
        public IActionResult Empleos(int? page, string buscar, string refe, int ns)
        {
            int tipoUser = 0;
            int isaut = 0;
            Guid _IdUserPrincipal   = new Guid("00000000-0000-0000-0000-000000000000");
            if (User.Identity.IsAuthenticated)
            {
                var id              = _userManager.GetUserId(User);
                _IdUserPrincipal    = new Guid(id);
                isaut               = 1;
                ApplicationUser x   = _context.Users.Where(s => s.Id == id).First();
                tipoUser            = x.TypeUser;
            }
            ViewBag.tipoUser = tipoUser;

            ViewBag.sPagereturnUrl  = 1;
            var nPagereturnUrl      = page;
            ViewBag.sPagereturnUrl  = nPagereturnUrl;
            ViewBag.refe            = refe;
            string buscarx          = buscar;
            string buscars          = null;

            if (!String.IsNullOrEmpty(buscarx)){ buscars = $"and empleo.Titulotrabajo like '%{buscarx}%'"; }
            else { buscars = ""; }
            List<Ofertas> todoempleoList    = new List<Ofertas>();
            string connectionString         = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select empleo.*,empresaperfils.* from empleo inner join empresaperfils on empresaperfils.Id = empleo.Idnumempresa and empresaperfils.Idempresa = empleo.Idempresa {buscars} WHERE empleo.statusGral = 0 order by empleo.Id DESC"; 
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    while (dataReader.Read())
                    {
                        Ofertas todoempleo = new Ofertas();
                        todoempleo.Id               = Convert.ToInt64(dataReader["Id"]);
                        todoempleo.Job              = dataReader.GetGuid(dataReader.GetOrdinal("Job"));
                        todoempleo.EmpresaCentro    = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        todoempleo.PhoneNumber      = Convert.ToString(dataReader["PhoneNumber"]);
                        todoempleo.Email2           = Convert.ToString(dataReader["Email2"]);
                        todoempleo.RNC              = Convert.ToString(dataReader["RNC"]);
                        todoempleo.Ciudadtrabajo    = Convert.ToString(dataReader["Ciudadtrabajo"]).ToUpper();
                        todoempleo.Titulotrabajo    = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        todoempleo.dateadd          = Convert.ToDateTime(dataReader["dateadd"]);
                        todoempleo.TipoContrato     = Convert.ToInt16(dataReader["TipoContrato"]);
                        todoempleo.jornadahrs       = Convert.ToString(dataReader["jornadahrs"]);
                        todoempleo.Foto             = Convert.ToString(dataReader["Foto"]);
                        todoempleo.publicosino      = Convert.ToBoolean(dataReader["publicosino"]);
                        todoempleo.diaslaborables   = Convert.ToString(dataReader["diaslaborables"]);
                        todoempleo.Descripciontrabajo = Convert.ToString(dataReader["Descripciontrabajo"]);
                        todoempleo.salarioaspiraMON = Convert.ToString(dataReader["salarioaspiraMON"]);
                        todoempleo.salarioultimoMON = Convert.ToString(dataReader["salarioultimoMON"]);
                        string urltext              = todoempleo.Descripciontrabajo;
                        string cadenaLimpia         = "";
                        //=====================================================================================================
                        string result = Regex.Replace(urltext,
                           @"</?(?i:script|embed|object|frameset|frame|iframe|meta|link|style)(.|\n|\s)*?>",
                           string.Empty,RegexOptions.Singleline | RegexOptions.IgnoreCase);
                        urltext = result;

                        urltext = urltext.Replace("<br>", "-_-");
                        System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("<[^>]*>");
                        urltext = rx.Replace(urltext, "");
                        cadenaLimpia = urltext;
                        cadenaLimpia = cadenaLimpia.Replace("-_-", "<br>");
                        cadenaLimpia = cadenaLimpia.Replace(".", ".<br /><br />");
                        cadenaLimpia = cadenaLimpia.Trim();
                        urltext      = cadenaLimpia;
                        todoempleo.Descripciontrabajo = urltext;
                        //-----------------------------------------------------------------------------------------------------

                        todoempleo.Requisitostrabajo = Convert.ToString(dataReader["Requisitostrabajo"]);
                            result = Regex.Replace(todoempleo.Requisitostrabajo,
                               @"</?(?i:script|embed|object|frameset|frame|iframe|meta|link|style)(.|\n|\s)*?>",
                               string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);
                            urltext = result;

                            urltext = urltext.Replace("<br>", "-_-");
                            System.Text.RegularExpressions.Regex rx2 = new System.Text.RegularExpressions.Regex("<[^>]*>");
                            urltext = rx2.Replace(urltext, "");
                            cadenaLimpia = urltext;
                            cadenaLimpia = cadenaLimpia.Replace("-_-", "<br>");
                            cadenaLimpia = cadenaLimpia.Replace(".", ".<br /><br />");
                            cadenaLimpia = cadenaLimpia.Trim();
                            urltext      = cadenaLimpia;
                        //=====================================================================================================                      
                        todoempleo.Requisitostrabajo    = urltext;
                        todoempleo.edadminima           = Convert.ToInt16(dataReader["edadminima"]);
                        todoempleo.edadmaxima           = Convert.ToInt16(dataReader["edadmaxima"]);
                        todoempleo.sexo                 = Convert.ToInt16(dataReader["sexo"]);
                        todoempleo.idiomas              = Convert.ToString(dataReader["idiomas"]);
                        todoempleo.Salario              = Convert.ToDouble(dataReader["Salario"]);
                        todoempleo.Salariohasta         = Convert.ToDouble(dataReader["Salariohasta"]);
                        todoempleo.Salariotratar        = Convert.ToBoolean(dataReader["Salariotratar"]);
                        Guid Idempresatoguid            = Guid.Parse(dataReader["Idempresa"].ToString());
                        todoempleo.Idempresa            = Idempresatoguid;

                                    int Next_Empleos    = 4;
                                    Guid _IdUserEmpresa = Idempresatoguid;
                                    string mensajeUserName = "0";
                                    if (isaut == 1) { if (_context.follows.Any(u => u.IdUserPrincipal.Equals(_IdUserPrincipal) && u.IdUserEmpresa.Equals(_IdUserEmpresa))) { Next_Empleos = 0; mensajeUserName = "fe";  /*Follow existe*/} else { Next_Empleos = 5; mensajeUserName = "fn";  /*Follow NO existe*/} }
                        todoempleo.isFollow = Next_Empleos;



                        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                        var id = _userManager.GetUserId(User);
                        ViewBag.userId          = id;
                        int aplicastealempleo   = 0; 
                        bool isAuthenticated    = User.Identity.IsAuthenticated;
                        int userexiste          = 0;
                        if (isAuthenticated == true)
                        {
                            if (_context.empleoAdds.Any(u => u.IdJob == todoempleo.Id && u.Job.ToString() == todoempleo.Job.ToString() && u.IdUser.ToString() == id.ToString())) { aplicastealempleo = 1; }
                        }
                        else {
                            aplicastealempleo = 2;
                        }
                        todoempleo.aplicoaempleosiono = aplicastealempleo;
                        ViewBag.Job = todoempleo.Job;
                        todoempleoList.Add(todoempleo);
                    }
                }
            }

            ns = ns;
            string mensaje_ns = "";
            if (ns == 1) { mensaje_ns = "La vacante no fue compartida, verifique los datos de envío"; }

            ViewBag.ns          = ns;
            ViewBag.mensaje_ns  = mensaje_ns;

            var pageNumber      = page ?? 1; 
            int pageSize        = 5; 
            var onePageOfEmpleos = todoempleoList.ToPagedList(pageNumber, pageSize);

            return View(onePageOfEmpleos); // Send 25 students to the page.            return View();
        }


        //-----------------------------------------------S E L E C T----------------------------------------------------------------
        public IActionResult Select()
        {
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ C A N D I D A T O S ++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [Authorize]
        public async Task<IActionResult> Candidatos()
        {
            var id          = _userManager.GetUserId(User);
            var mail        = _userManager.GetUserName(User);
            ViewBag.userId  = id;
            ViewBag.mail    = mail;
            
            var result = await _subscriptionService.GetSubscriptioByUser(id);
            if (result.ResultKind == EmpleosWebMax.Common.Enum.ResultKind.Error)
            {
                ViewBag.PlanName = result.Error;
            }
            else 
            {
                ViewBag.PlanName = result.Data.PlanName;
            }
            List<ApplicationUser2> UserxList    = new List<ApplicationUser2>();
            string connectionString             = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection     = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM AspNetUsers WHERE TypeUser = 255485 And Id = '{id}' and UserName = '{mail}'";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    while (dataReader.Read())
                    {
                        ApplicationUser2 Userx  = new ApplicationUser2();
                        Userx.Id                = Convert.ToString(dataReader["Id"]);
                        Userx.FirstName         = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName          = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Email             = Convert.ToString(dataReader["Email"]);
                        Userx.TypeUser          = Convert.ToInt32(dataReader["TypeUser"]);
                        ViewBag.nTypeUser       = Userx.TypeUser;
                        Userx.Status            = Convert.ToInt16(dataReader["Status"]);
                        Userx.Sexo              = Convert.ToBoolean(dataReader["Sexo"]);
                        UserxList.Add(Userx);
                    }
                }
            }

            if (ViewBag.nTypeUser == 255485)
            {
                //
            }
            else
            {
                return RedirectToAction("Logout", "Aplicaciones");
            }

            //---------OBTENER DATOS ADICIONALES DEL CANDIDATO---------------------------------------------------------
            List<UserInfo> CandidatoList = new List<UserInfo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM userInfos WHERE IdUser = '{id}'"; 
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    while (dataReader.Read())
                    {
                        UserInfo candidato          = new UserInfo();
                        candidato.Id                = Convert.ToInt64(dataReader["Id"]);
                        candidato.Foto              = Convert.ToString(dataReader["Foto"]);
                        candidato.Estadocivil       = Convert.ToString(dataReader["Estadocivil"]).ToUpper();
                        candidato.nacimiento        = Convert.ToDateTime(dataReader["nacimiento"]);
                        candidato.Nacionalidad      = Convert.ToString(dataReader["Nacionalidad"]).ToUpper();
                        candidato.Telefono2         = Convert.ToString(dataReader["Telefono2"]).ToUpper();
                        candidato.DocumentoIDn      = Convert.ToString(dataReader["DocumentoIDn"]).ToUpper();
                        candidato.DocumentoIDt      = Convert.ToString(dataReader["DocumentoIDt"]).ToUpper();
                        candidato.Ultimosalario     = Convert.ToDouble(dataReader["Ultimosalario"]);
                        candidato.Salarioaspira     = Convert.ToDouble(dataReader["Salarioaspira"]);
                        candidato.salarioaspiraMON  = Convert.ToString(dataReader["salarioaspiraMON"]).ToUpper();
                        candidato.salarioultimoMON  = Convert.ToString(dataReader["salarioultimoMON"]).ToUpper();
                        candidato.Pais              = Convert.ToString(dataReader["Pais"]).ToUpper();
                        candidato.Ciudad            = Convert.ToString(dataReader["Ciudad"]).ToUpper();
                        candidato.Direccion         = Convert.ToString(dataReader["Direccion"]).ToUpper();
                        candidato.Areaprofesional   = Convert.ToString(dataReader["Areaprofesional"]).ToUpper();
                        candidato.Facebook          = Convert.ToString(dataReader["Facebook"]).ToUpper();
                        candidato.Twitter           = Convert.ToString(dataReader["Twitter"]).ToUpper();
                        CandidatoList.Add(candidato);
                    }
                }
            }


            //---------OBTENER PERFIL DEL CANDIDATO---------------------------------------------------------
            List<Experiencias> lasexperienciasList = new List<Experiencias>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"SELECT experiencias.Id,experiencias.Empresa,experiencias.Posicion,experiencias.FuncionesRol,experiencias.Aportes,experiencias.desde,experiencias.hasta FROM experiencias INNER JOIN AspNetUsers ON AspNetUsers.TypeUser = 255485 and AspNetUsers.Id = '{id}' and experiencias.IdUser = '{id}' and experiencias.email = '{mail}'"; /*SELECT * FROM experiencias WHERE TypeUser = 255485 and IdUser = '{id}' and email = '{mail}'*/
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    while (dataReader.Read())
                    {
                        Experiencias lasexperiencias    = new Experiencias();
                        lasexperiencias.Id              = Convert.ToInt64(dataReader["Id"]);
                        lasexperiencias.Empresa         = Convert.ToString(dataReader["Empresa"]).ToUpper();
                        lasexperiencias.Posicion        = Convert.ToString(dataReader["Posicion"]).ToUpper();
                        lasexperiencias.FuncionesRol    = Convert.ToString(dataReader["FuncionesRol"]);
                        lasexperiencias.Aportes         = Convert.ToString(dataReader["Aportes"]);
                        lasexperiencias.desde           = Convert.ToDateTime(dataReader["desde"]);
                        lasexperiencias.hasta           = Convert.ToDateTime(dataReader["hasta"]);
                        lasexperienciasList.Add(lasexperiencias);                       
                    }
                }
            }

            //---------OBTENER EDUCACION DEL CANDIDATO---------------------------------------------------------
            List<Educacion> EducacionList = new List<Educacion>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM educacion WHERE IdUser = '{id}' and email = '{mail}'"; 
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    while (dataReader.Read())
                    {
                        Educacion estudios          = new Educacion();
                        estudios.Id                 = Convert.ToInt64(dataReader["Id"]);
                        estudios.tipoestudio        = Convert.ToInt32(dataReader["tipoestudio"]);
                        estudios.Institucion        = Convert.ToString(dataReader["Institucion"]).ToUpper();
                        estudios.InstitucionLugar   = Convert.ToString(dataReader["InstitucionLugar"]);
                        estudios.Titulo             = Convert.ToString(dataReader["Titulo"]).ToUpper();
                        estudios.Descripcion        = Convert.ToString(dataReader["Descripcion"]);
                        estudios.desde              = Convert.ToDateTime(dataReader["desde"]);
                        estudios.hasta              = Convert.ToDateTime(dataReader["hasta"]);
                        EducacionList.Add(estudios);

                    }
                }
            }

            //---------OBTENER REFERENCIAS---------------------------------------------------------
            List<Referencias> ReferenciaList = new List<Referencias>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM referencias WHERE IdUser = '{id}' and email = '{mail}'"; /*{Dato_}*/
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    while (dataReader.Read())
                    {
                        Referencias reference       = new Referencias();
                        reference.Id                = Convert.ToInt64(dataReader["Id"]);
                        reference.Persona           = Convert.ToString(dataReader["Persona"]).ToUpper();
                        reference.PhoneNumber       = Convert.ToString(dataReader["PhoneNumber"]);
                        reference.Email2            = Convert.ToString(dataReader["Email2"]);
                        reference.Empresa           = Convert.ToString(dataReader["Empresa"]).ToUpper();
                        reference.Parentezco        = Convert.ToString(dataReader["Parentezco"]);
                        ReferenciaList.Add(reference);
                    }
                }
            }
            //------------------------------------------------------------------
            ViewBag.UserxList           = UserxList;
            ViewBag.CandidatoList       = CandidatoList;
            ViewBag.lasexperienciasList = lasexperienciasList;
            ViewBag.EducacionList       = EducacionList;
            ViewBag.ReferenciaList      = ReferenciaList;
            return View();
        }

        //######################################## E M P R E S A S ############################################
        [Authorize]
        [HttpPost]
        [HttpGet]
        public async Task<IActionResult> Empresas(int? page, string buscar, string refe, int ns)
        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);

            if (!String.IsNullOrEmpty(id)) { }else
            {
                return RedirectToAction("Logout", "Aplicaciones");
            }


            ViewBag.userId          = id;
            ViewBag.mail            = mail;
            int ncontar;
            Int64 nId               = 0;
            ViewBag.sPagereturnUrl  = 1;
            var nPagereturnUrl      = page;
            ViewBag.sPagereturnUrl  = nPagereturnUrl;
            ViewBag.refe            = refe;
            string buscarx          = buscar;
            string buscars          = null;
            ViewBag.buscar          = "";

            var result = await _subscriptionService.GetSubscriptioByUser(id);
            if (result.ResultKind == EmpleosWebMax.Common.Enum.ResultKind.Error)
            {
                ViewBag.PlanName = result.Error;
            }
            else
            {
                ViewBag.PlanName = result.Data.PlanName;
            }

            if (!String.IsNullOrEmpty(buscarx)) { buscars = $"and empleo.Titulotrabajo like '%{buscarx}%'"; ViewBag.buscar = buscarx; }
            else { buscars = ""; }

            //---------OBTENER PERFIL DEL CANDIDATO---------------------------------------------------------
            List<Empresaperfil> companyList = new List<Empresaperfil>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"SELECT empresaperfils.Id, empresaperfils.EmpresaCentro, empresaperfils.PhoneNumber, empresaperfils.Email2, empresaperfils.RNC, empresaperfils.Pais, empresaperfils.Ciudad, empresaperfils.Direccion, empresaperfils.dateadd, empresaperfils.Idempresa, empresaperfils.Foto FROM empresaperfils INNER JOIN AspNetUsers ON AspNetUsers.TypeUser = 69784 and AspNetUsers.Id = '{id}' and empresaperfils.IdUser = '{id}' and empresaperfils.email = '{mail}'"; 
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    ncontar = 0;
                    while (dataReader.Read())
                    {
                        Empresaperfil company   = new Empresaperfil();
                        company.Id              = Convert.ToInt64(dataReader["Id"]);
                        company.EmpresaCentro   = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        company.PhoneNumber     = Convert.ToString(dataReader["PhoneNumber"]);
                        company.Email2          = Convert.ToString(dataReader["Email2"]);
                        company.RNC             = Convert.ToString(dataReader["RNC"]);
                        company.Pais            = Convert.ToString(dataReader["Pais"]);
                        company.Ciudad          = Convert.ToString(dataReader["Ciudad"]);
                        company.Direccion       = Convert.ToString(dataReader["Direccion"]);
                        company.dateadd         = Convert.ToDateTime(dataReader["dateadd"]);
                        company.Idempresa       = dataReader.GetGuid(dataReader.GetOrdinal("Idempresa"));
                        company.Foto            = Convert.ToString(dataReader["Foto"]);
                        ViewBag.Idempresa       = company.Idempresa;
                        nId = Convert.ToInt64(dataReader["Id"]);
                        companyList.Add(company);
                        ncontar = ncontar + 1;
                    }
                }
            }

            //---------OBTENER REFERENCIAS---------------------------------------------------------
            List<Ofertas> todoempleoList2 = new List<Ofertas>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select empleo.*,empresaperfils.* from empleo join empresaperfils on empleo.Idnumempresa = {nId} and empresaperfils.Idempresa = empleo.Idempresa Where empleo.statusGral < 20 {buscars}"; /*{Dato_}*/
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader()) 
                {
                    while (dataReader.Read())
                    {
                        Ofertas todoempleo              = new Ofertas();
                        todoempleo.Id                   = Convert.ToInt64(dataReader["Id"]);
                        todoempleo.EmpresaCentro        = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        todoempleo.PhoneNumber          = Convert.ToString(dataReader["PhoneNumber"]);
                        todoempleo.Email2               = Convert.ToString(dataReader["Email2"]);
                        todoempleo.RNC                  = Convert.ToString(dataReader["RNC"]);
                        todoempleo.Idempresa            = dataReader.GetGuid(dataReader.GetOrdinal("Idempresa"));
                        todoempleo.Foto                 = Convert.ToString(dataReader["Foto"]);
                        todoempleo.Ciudadtrabajo        = Convert.ToString(dataReader["Ciudadtrabajo"]).ToUpper();
                        todoempleo.Titulotrabajo        = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        todoempleo.dateadd              = Convert.ToDateTime(dataReader["dateadd"]);
                        todoempleo.TipoContrato         = Convert.ToInt16(dataReader["TipoContrato"]);
                        todoempleo.jornadahrs           = Convert.ToString(dataReader["jornadahrs"]);
                        todoempleo.diaslaborables       = Convert.ToString(dataReader["diaslaborables"]);
                        todoempleo.Descripciontrabajo   = Convert.ToString(dataReader["Descripciontrabajo"]);
                        todoempleo.Requisitostrabajo    = Convert.ToString(dataReader["Requisitostrabajo"]);
                        todoempleo.edadminima           = Convert.ToInt16(dataReader["edadminima"]);
                        todoempleo.edadmaxima           = Convert.ToInt16(dataReader["edadmaxima"]);
                        todoempleo.sexo                 = Convert.ToInt16(dataReader["sexo"]);
                        todoempleo.idiomas              = Convert.ToString(dataReader["idiomas"]);
                        todoempleo.Salario              = Convert.ToDouble(dataReader["Salario"]);
                        todoempleo.Salariohasta         = Convert.ToDouble(dataReader["Salariohasta"]);
                        todoempleo.Salariotratar        = Convert.ToBoolean(dataReader["Salariotratar"]);
                        todoempleo.Job                  = dataReader.GetGuid(dataReader.GetOrdinal("Job"));
                        todoempleo.statusGral           = Convert.ToInt32(dataReader["statusGral"]);
                        ViewBag.Job                     = todoempleo.Job;
                        ViewBag.IdJob                   = todoempleo.Id;

                        if (ViewBag.Job != null)
                        {
                            List<EmpleoAddCant> CantIDList      = new List<EmpleoAddCant>();
                            string connectionStringx            = Configuration["ConnectionStrings:DefaultConnection"];
                            using (SqlConnection connectionx    = new SqlConnection(connectionStringx))
                            {
                                connectionx.Open();
                                string sqlx = $"SELECT COUNT(Id) as cantID2 FROM empleoAdds where IdJob = {ViewBag.IdJob} And  Job = '{ViewBag.Job}'";
                                SqlCommand commandx = new SqlCommand(sqlx, connectionx);
                                using (SqlDataReader dataReaderx = commandx.ExecuteReader())
                                {
                                    while (dataReaderx.Read())
                                    {
                                        EmpleoAddCant Userx = new EmpleoAddCant();
                                        Userx.cantID2       = Convert.ToInt64(dataReaderx["cantID2"]);
                                        CantIDList.Add(Userx);
                                        ViewBag.cantID2     = Userx.cantID2;
                                    }
                                }
                            }
                            ViewBag.cantID = ViewBag.cantID2;
                        }
                        else { ViewBag.cantID = 0; }
                        todoempleo.cantID3 = ViewBag.cantID;
                        todoempleoList2.Add(todoempleo);

                    }
                }
            }


            //--------------------- DATOS DE LA EMPRESA ----------------------
            ViewBag.ncontar         = ncontar;
            ViewBag.companyList     = companyList;
            var pageNumber          = page ?? 1; 
            int pageSize            = 2; 
            var onePageOfEmpleos2   = todoempleoList2.ToPagedList(pageNumber, pageSize);
            return View(onePageOfEmpleos2); 
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS VACANTES Con emresas ya creadas  ++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("EditVacante")]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Post(Empleo empleo, string St, int gdfsd) 

        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            if (gdfsd == null) { gdfsd = 0; }

            int status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; } 
                else if (St == "x") { status1 = 0; } 
                else if (St == "r") { status1 = 1; } 
                else { status1 = 0; }
            }

            if (gdfsd > 0) 
            {
                Empleo d            = _context.empleo.Where(s => s.Id == gdfsd).First();
                d.statusGral        = status1;
                d.statusGralBy      = Guid.Parse(id);
                d.statusGralDate    = DateTime.Now;
                d.statusGralMail    = mail;
                _context.SaveChanges();
            }

            return RedirectToAction("Empresas", "Home");
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ FOLLOWS    +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FollowMe(string followsMe)
        {
                bool b1         = string.IsNullOrEmpty(followsMe);
                int Next_       = 0; //
                string mensajeUserName = "0";
                if (b1 == false)
                { Next_         = 1; mensajeUserName = "nn"; /*no null*/} else { Next_ = 0; mensajeUserName = "nv"; }


                bool isGuid = false;
                if (Next_ == 1)
                {
                    Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
                    isGuid = guidRegEx.IsMatch(followsMe);
                }
                if (isGuid == true) { Next_ = 2; mensajeUserName = "gv"; /*Guid verdadero*/} else {Next_ = 0; mensajeUserName = "gf"; /*Guid falso*/ }


                Guid    _IdUserPrincipal;
                int     _TypeUserPrincipal;
                var id                      = _userManager.GetUserId(User);
                Guid id2                    = new Guid(id);
                string  _MailPrincipal      = "";
                ApplicationUser u           = _context.Users.Where(s => s.Id == id).First(); 
                _IdUserPrincipal            = new Guid(u.Id);
                _MailPrincipal              = u.UserName;
                _TypeUserPrincipal          = u.TypeUser;
                if (_IdUserPrincipal.Equals(id2)) { Next_ = 3; mensajeUserName = "ue"; /* usuario existe*/ } else { Next_ = 0; mensajeUserName = "un"; /*usuario falso*/ }
                Guid _IdUserEmpresa         = new Guid("00000000-0000-0000-0000-000000000000");
                string empresaEmail         = "";
                int _TypeUserEmpresa        = 69784;
                Guid follows2               = new Guid(followsMe);
                if (Next_ == 3){
                Empresaperfil d             = _context.empresaperfils.Where(s => s.Idempresa == follows2).FirstOrDefault();
                empresaEmail                = d.email;
                _IdUserEmpresa              = d.Idempresa;}
                //validando que la empresa existe
                if (_IdUserEmpresa.Equals(follows2)) { Next_ = 4; mensajeUserName = "ee"; /* empresa existe*/ } else { Next_ = 0; mensajeUserName = "en"; /*empresa no existe*/ }

                if (Next_ == 4){if (_context.follows.Any(u => u.IdUserPrincipal.Equals(_IdUserPrincipal) && u.IdUserEmpresa.Equals(_IdUserEmpresa))) { Next_ = 0; mensajeUserName = "fe";  /*Follow existe*/} else { Next_ = 5; mensajeUserName = "fn";  /*Follow NO existe*/}}

                if (Next_ == 5)
                {
                    Follow follows = new Follow
                    {
                        IdUserPrincipal     = _IdUserPrincipal, 
                        MailPrincipal       = _MailPrincipal, 
                        TypeUserPrincipal   = _TypeUserPrincipal, 
                        IdUserEmpresa       = _IdUserEmpresa,
                        MailEmpresa         = empresaEmail,
                        TypeUserEmpresa     = _TypeUserEmpresa,
                        status              = 1,
                        fechaSolicitud      = DateTime.Now, 
                        seguidor            = 2,
                        seguidorStatus      = 1, 
                        seguidorStatusFecha = DateTime.Now,
                        solicitudEnviada    = 1,
                        solicitudRecibida   = 0,
                    };
                    _context.Add(follows);
                    await _context.SaveChangesAsync();
                    mensajeUserName = "ab"; 
                }
                else { Next_ = 0; mensajeUserName = "na";  }


            ViewBag.mensajeUserName = mensajeUserName;

            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ FRIENDS    +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FriendsMe(string userName, string returnUrl, string _Nameinvitado, string mensaje)
        {
            string amigoo           = userName;
            bool b1                 = string.IsNullOrEmpty(amigoo);
            int Next_               = 0; //
            string mensajeUserName  = "0";
            string _MailGuest       = "";
            string Nombreuserprincipal = "";
            if (b1 == false)
            { Next_ = 1; mensajeUserName = "nn"; }
            else { Next_ = 0; mensajeUserName = "nv";  }


            _MailGuest = amigoo;
            if (Next_ == 1)
            {
                string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(_MailGuest, expression))
                {
                    if (Regex.Replace(_MailGuest, expression, string.Empty).Length == 0)
                    { Next_ = 2; mensajeUserName = "ec"; /*Email correcto*/ }
                }
                else
                {
                    Next_ = 0; mensajeUserName = "ef"; /*email falso*/
                }
                //------------------------------------------------------------------------
            }

            Guid _IdUserPrincipal   = new Guid("00000000-0000-0000-0000-000000000000");
            int _TypeUserPrincipal  = 11;
            var id                  = _userManager.GetUserId(User);
            Guid id2                = new Guid(id);
            string _MailPrincipal   = "";

            if (Next_ == 2)
            {
                ApplicationUser u   = _context.Users.Where(s => s.Id == id).First(); //LINQ
                _IdUserPrincipal    = new Guid(u.Id);
                _MailPrincipal      = u.UserName;
                _TypeUserPrincipal  = u.TypeUser;
                Nombreuserprincipal = u.FirstName + " " + u.LastName;

                if (_IdUserPrincipal.Equals(id2)) { Next_ = 3; mensajeUserName = "ue"; /* usuario existe*/ } else { Next_ = 0; mensajeUserName = "un"; /*usuario falso*/ }

            }

            int stamistad = 0;
            Guid _IdUserGuest = new Guid("00000000-0000-0000-0000-000000000000"); 
            int _TypeUserGuest = 11; 
            
            string _mensaje = "";

            if (Next_ == 3)
            {
                if (_context.Users.Any(u => u.UserName.Equals(_MailGuest)))
                {
                    ApplicationUser d = _context.Users.Where(s => s.UserName == _MailGuest).First(); 
                    Next_ = 4; mensajeUserName = "ue"; _MailGuest = d.UserName; _TypeUserGuest = d.TypeUser; _IdUserGuest = new Guid(d.Id);
                }
                else
                {
                    Next_ = 5; mensajeUserName = "un"; _MailGuest = _MailGuest; _TypeUserGuest = _TypeUserGuest; _IdUserGuest = _IdUserGuest; 
                }
                                }

            int ami = 0;
            if (Next_ == 4 || Next_ ==5) { if (_context.friendsall.Any(u => u.IdUserPrincipal.Equals(_IdUserPrincipal) && u.MailGuest.Equals(_MailGuest))) 
                { Next_ = 0; mensajeUserName = "ae";
                    ami = 1;
                }                
                else { Next_ = 6; mensajeUserName = "an"; } }

            int st_amis = 0;
            if(ami == 1 && Next_ == 0)
            {
                Friends d = _context.friendsall.Where(s => s.MailGuest == _MailGuest).FirstOrDefault();
                st_amis = d.amigoStatus;
                if(st_amis == 4) { Next_ = 0; mensajeUserName = "aenr";  }
                else if (st_amis == 1) { Next_ = 0; mensajeUserName = "aera";  }
            }

            if (Next_ == 6)
            {
                Friends friendsall = new Friends
                {
                    IdUserPrincipal         = _IdUserPrincipal,
                    MailPrincipal           = _MailPrincipal,
                    TypeUserPrincipal       = _TypeUserPrincipal,
                    IdUserGuest             = _IdUserGuest,
                    MailGuest               = _MailGuest,
                    TypeUserGuest           = _TypeUserGuest,
                    status                  = 3,
                    fechaSolicitud          = DateTime.Now, 
                    amigo                   = 2, 
                    amigoStatus             = 4,
                    amigoStatusFecha        = DateTime.Now,
                    seguidor                = 0, 
                    seguidorStatus          = 0, 
                    seguidorStatusFecha     = DateTime.Now,
                    solicitudEnviada        = 1,  
                    solicitudRecibida       = 0, 
                    Nameinvitado            = _Nameinvitado,
                    mensaje                 =  mensaje + $"Test {DateTime.Now}"
                };
            _context.Add(friendsall);
            await _context.SaveChangesAsync();
            mensajeUserName = "ab"; 
                ViewBag.TypeAddEmpresa      = ViewBag.TypeAddEmpresa;
                ViewBag.NameFirst           = ViewBag.NameFirst;
                string to_                  = $"informacion@empleomax.com, {_MailGuest}";
                string asunto_              = $"Hola {_Nameinvitado}, {Nombreuserprincipal} te invita al portal #1 de empleos en RD";
                string mensaje_             = mensaje;
                var lafechaes               = DateTime.Now;
                int newmailsend             = 1;

                if(newmailsend == 1 && !String.IsNullOrEmpty(to_))
                {

                    MailMessage mail2   = new MailMessage();

                    mail2.From          = new MailAddress("informacion@empleomax.com"); 
                    mail2.To.Add(to_);
                    mail2.Subject       = $"{asunto_}";
                    mail2.IsBodyHtml    = true;
                    string htmlBody     = "";

                    htmlBody = "" +
                        $"<p style = 'line - height: 150 %; margin - top: 0; margin - bottom: 0' > Hola! <b>{Nombreuserprincipal}</b> te invita a ver y compartir su Curriculum Vitae a través de nuestro portal www.EmpleoMAX.com </p>" +
                        $"<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>{Nombreuserprincipal} ha visto oportuidades de trabajo interesantes:</p>" +
                        $"<b>{mensaje_}</b><br><br>" +
                        $"<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>aprovechamos para invitarlos a crear su perfil en nuestro portal y, para ello le ofrecemos crear tu perfil totalmente gratis.</p><br>" +
                        $"<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'><a href='https://www.empleomax.com/Home/Empleos'>Clic aqui en este link para ver el curriculum de {_Nameinvitado}.</a></p><br><br>" +
                        "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'><br>Para cualquier información, no dude en contactarnos.&nbsp;</p>" +
                        "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>Cordialmente.</p>" +
                        "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>&nbsp;</p>" +
                        "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>El equipo de Empleos Max.</p>" +
                        "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>informacion@empleomax.com</p>" +
                        $"{lafechaes}";

                    mail2.Body = htmlBody;
                    SmtpClient smtp = new SmtpClient("mail.empleomax.com");
                    NetworkCredential Credentials = new NetworkCredential("informacion@empleomax.com", "123456Em@@");
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = Credentials;
                    smtp.Port = 8889;    
                    smtp.EnableSsl = false;
                    smtp.Send(mail2);
                    string lblMessage = "Mail Sent";

                }


                if (returnUrl == null || returnUrl == "") { returnUrl = ""; }
                return LocalRedirect(returnUrl);
            }
            else { Next_ = 0; mensajeUserName = "na";  }
            ViewBag.mensajeUserName = mensajeUserName;

            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE foto user  ++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           [HttpPost]
        
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Updateimage(IFormFile file)
        {
            var id              = _userManager.GetUserId(User);
            Guid id2            = new Guid(id);
            var mail            = _userManager.GetUserName(User);
            Guid idforos        = Guid.NewGuid();
            var fileName        = System.IO.Path.GetFileName(file.FileName); 
            string Namedocument = fileName;
            int sizeOfString    = Namedocument.Length;
            int sizeX = 0;
            string SubString    = "";
            if (sizeOfString > 10)
            {
                sizeX = sizeOfString - 10;
                int newVal      = Namedocument.Length - (Namedocument.Length - sizeX);
                SubString       = Namedocument.Substring(newVal);
            }
            else { SubString    = Namedocument; }
            Random rnd          = new Random();
            int newrnd          = rnd.Next(100000, 1325478855);
            var chars           = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars     = new char[8];
            var random          = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i]  = chars[random.Next(chars.Length)];
            }
            var finalString     = new String(stringChars);
            Namedocument        = finalString + newrnd + "_" + SubString;
            fileName            = Namedocument;
            string newnameimg   = fileName;

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            int subirimagenstatus   = 1;
            string uploadsFolder    = Path.Combine(webHostEnvironment.WebRootPath, "fotos");
            var fileName2           = Path.Combine(uploadsFolder, fileName);
            using (var localFile    = System.IO.File.OpenWrite(fileName2))
            using (var uploadedFile = file.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            if (subirimagenstatus == 1) 
            {
                UserInfo d = _context.userInfos.Where(s => s.IdUser.ToString() == id).FirstOrDefault();
                d.Foto = fileName;
                _context.SaveChanges();
            }

            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE NOMBRE DE IMAGEN  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("EditFoto")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Post(IFormFile file,string namef) 
        {
            var id                  = _userManager.GetUserId(User);
            var mail                = _userManager.GetUserName(User);
            var fileName            = System.IO.Path.GetFileName(file.FileName); 
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
            Random rnd              = new Random();
            int newrnd              = rnd.Next(100000, 1325478855);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            //=======================================================================================
            Namedocument = finalString + newrnd + "_" + SubString;
            fileName = Namedocument;
            string newnameimg = fileName;
            int subirimagenstatus = 0;

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
                subirimagenstatus = 1;
            }
            string uploadsFolder    = Path.Combine(webHostEnvironment.WebRootPath, "fotos");
            var fileName2           = Path.Combine(uploadsFolder, fileName);
            using (var localFile    = System.IO.File.OpenWrite(fileName2))
            using (var uploadedFile = file.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }


            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            string sql = $"update userInfos SET Foto = '{newnameimg}' WHERE IdUser = '{id}'";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.CommandType = CommandType.Text;
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
            return RedirectToAction("Candidatos", "Home");
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE NOMBRE DE IMAGEN  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("EditLogo")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Logo(IFormFile file, string namef) //, Guid Us, string Mi
        {
            var id              = _userManager.GetUserId(User);
            var mail            = _userManager.GetUserName(User);
            var fileName        = System.IO.Path.GetFileName(file.FileName); 
            string Namedocument = fileName;
            int sizeOfString    = Namedocument.Length;
            int sizeX           = 0;
            string SubString    = "";
            if (sizeOfString > 10)
            {
                sizeX           = sizeOfString - 10;
                int newVal      = Namedocument.Length - (Namedocument.Length - sizeX);
                SubString       = Namedocument.Substring(newVal);
            }
            else { SubString    = Namedocument; }
            //random number =========================================================================
            Random rnd          = new Random();
            int newrnd          = rnd.Next(100000, 1325478855);
            // random string ========================================================================
            var chars           = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars     = new char[8];
            var random          = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i]  = chars[random.Next(chars.Length)];
            }
            var finalString     = new String(stringChars);
            //=======================================================================================
            Namedocument        = finalString + newrnd + "_" + SubString;
            fileName            = Namedocument;
            string newnameimg   = fileName;
            int subirimagenstatus = 0;

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
                subirimagenstatus   = 1;
            }
            string uploadsFolder    = Path.Combine(webHostEnvironment.WebRootPath, "logo");
            var fileName2           = Path.Combine(uploadsFolder, fileName);
            using (var localFile    = System.IO.File.OpenWrite(fileName2))
            using (var uploadedFile = file.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"update empresaperfils SET Foto = '{newnameimg}' WHERE IdUser = '{id}'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return RedirectToAction("Empresas", "Home");
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE NOMBRE DE IMAGEN  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("Editimgforo")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Update_imagendeforo(IFormFile file, string namef, int hsdbhsha) //, Guid Us, string Mi
        {
            var id      = _userManager.GetUserId(User);
            var mail    = _userManager.GetUserName(User);
            int idforo  = hsdbhsha;
            if (idforo > 0) { idforo = hsdbhsha; } else { idforo = 0; }

            //======================================= NOBRE AL DOCUMENTO A SUBIR ==================================
            var fileName        = System.IO.Path.GetFileName(file.FileName); //=== N O M B R E   DEL  DOCUMENTO A SUBIR
            string Namedocument = fileName;
            int sizeOfString    = Namedocument.Length;
            int sizeX           = 0;
            string SubString    = "";
            if (sizeOfString > 10)
            {
                sizeX = sizeOfString - 10;
                int newVal  = Namedocument.Length - (Namedocument.Length - sizeX);
                SubString   = Namedocument.Substring(newVal);
            }
            else { SubString = Namedocument; }
            //random number =========================================================================
            Random rnd = new Random();
            int newrnd = rnd.Next(100000, 1325478855);
            // random string ========================================================================
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);
            //=======================================================================================
            Namedocument = finalString + newrnd + "_" + SubString;
            fileName = Namedocument;
            string newnameimg = fileName;
            int subirimagenstatus = 0;

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
                subirimagenstatus = 1;
            }
            string uploadsFolder    = Path.Combine(webHostEnvironment.WebRootPath, "foro");
            var fileName2           = Path.Combine(uploadsFolder, fileName);
            using (var localFile    = System.IO.File.OpenWrite(fileName2))
            using (var uploadedFile = file.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }


            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"update Foros SET Foto = '{newnameimg}' WHERE Id = '{idforo}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType     = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return RedirectToAction("Index", "Forosall");
        }

        //################################################### BLOG ##############################################################
        //################################################### BLOG ##############################################################
        //################################################### BLOG ##############################################################
        [HttpPost]
        [HttpGet]
        public IActionResult Blog(int? page, string buscar, string refe, int ns)
        {
            //USER ID
            int isaut = 0;
            Guid _IdUserPrincipal = new Guid("00000000-0000-0000-0000-000000000000");
            if (User.Identity.IsAuthenticated)
            {
                var id = _userManager.GetUserId(User);
                _IdUserPrincipal = new Guid(id);
                isaut = 1;
            }
            ViewBag.sPagereturnUrl = 1;
            var nPagereturnUrl = page;
            ViewBag.sPagereturnUrl = nPagereturnUrl;
            ViewBag.refe = refe;

            //Buscar
            string buscarx = buscar;
            string buscars = null;

            if (!String.IsNullOrEmpty(buscarx)) { buscars = $"and Foros.Titulo like '%{buscarx}%' OR Foros.Contenido like '%{buscarx}%'"; }
            else { buscars = ""; }
            List<ForoDto> todoempleoList = new List<ForoDto>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"select Foros.*,foroCategorias.* FROM Foros JOIN foroCategorias ON Foros.StatusForo = 0 And Foros.IdCategoria = foroCategorias.Id WHERE Foros.StatusForo < 20 {buscars} ORDER BY Foros.Publicado DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        ForoDto todoempleo      = new ForoDto();
                        todoempleo.Id           = Convert.ToInt64(dataReader["Id"]);
                        todoempleo.Titulo       = Convert.ToString(dataReader["Titulo"]).ToUpper();
                        todoempleo.Foto         = Convert.ToString(dataReader["Foto"]).ToUpper();
                        todoempleo.Contenido    = Convert.ToString(dataReader["Contenido"]);

                        string urltext = todoempleo.Contenido;
                                string cadenaConTags = urltext;
                                urltext = urltext.Replace(",", "");
                                urltext = urltext.Replace("<br>", "-_-");
                                urltext = urltext.Replace("<p>", ":::");
                                urltext = urltext.Replace("</p>", "~~");
                                urltext = urltext.Replace("www.", "http://");

                                String pattern;
                                pattern = @"(http:\/\/([\w.]+\/?)\S*)";
                                Regex re = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                                //urltext = re.Replace(urltext, "<a href=\"$1\" target=\"_blank\">$1</a>");
                                urltext = re.Replace(urltext, "<\"$1\">");
                                string cadenaSinTags = Regex.Replace(urltext, "<.*?>", string.Empty);

                                string cadenaLimpia = cadenaSinTags.Replace("<", "");
                                cadenaLimpia = cadenaLimpia.Replace(">", "");
                                cadenaLimpia = cadenaLimpia.ToString().Replace('"', ' ').Trim();

                                cadenaLimpia = cadenaLimpia.Replace("-_-", "<br>");
                                cadenaLimpia = cadenaLimpia.Replace(":::", "<p>");
                                cadenaLimpia = cadenaLimpia.Replace("~~", "</p>");
                        //=====================================================================================================
                        todoempleo.Contenido    = cadenaLimpia;
                        todoempleo.Autor        = Convert.ToString(dataReader["Autor"]).ToUpper();
                        todoempleo.StatusForo   = Convert.ToInt16(dataReader["StatusForo"]);
                        todoempleo.Publicado    = Convert.ToDateTime(dataReader["Publicado"]);
                        todoempleo.Categoria    = Convert.ToString(dataReader["Categoria"]).ToUpper();
                        Guid Idforotoguid       = Guid.Parse(dataReader["IdForo"].ToString());
                        todoempleo.IdForo       = Idforotoguid;
                        Int16 Next_Empleos      = 5;
                        Guid _IdUserEmpresa     = _IdUserPrincipal;
                        string mensajeUserName  = "0";
                        if (isaut == 1) { if (_context.ForoLikes.Any(u => u.IdUser.Equals(_IdUserPrincipal) && u.IdPost.Equals(todoempleo.Id))) { Next_Empleos = 0; mensajeUserName = "fe";  /*Follow existe*/} else { Next_Empleos = 5; mensajeUserName = "fn";  /*Follow NO existe*/} }
                        todoempleo.Likepost     = Next_Empleos;
                        ViewBag.nForoid         = todoempleo.Id;
                        ViewBag.iddelforo       = todoempleo.IdForo;
                        todoempleoList.Add(todoempleo);

                    }
                }
            }


            string mensaje_ns = "";
            if (ns == 1) { mensaje_ns = "Blog"; }
            ViewBag.ns          = ns;
            ViewBag.mensaje_ns  = mensaje_ns;
            var pageNumber      = page ?? 1; 
            int pageSize        = 3; 
            var onePageOfEmpleos = todoempleoList.ToPagedList(pageNumber, pageSize);

            return View(onePageOfEmpleos); 
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> LikePost(long LikeMe)
        {
            string mensajeUserName  = "na";
            int Next_               = 5;
            string checksql         = "";
            long Idpost_            = 0;
            Guid IdForo_            = new Guid("00000000-0000-0000-0000-000000000000");
            Guid IdUser_            = new Guid("00000000-0000-0000-0000-000000000000");
            if (User.Identity.IsAuthenticated)
            {
                var id              = _userManager.GetUserId(User);
                IdUser_             = new Guid(id);
            }

            if (Next_ == 5)
            {
                Foro d = _context.Foros.Where(s => s.Id == LikeMe).FirstOrDefault();
                if (d != null) { 
                    checksql    = "existe";
                    IdForo_     = d.IdForo;
                    Idpost_     = d.Id;
                    Next_       = 6;
                } else { checksql = "NO EXISTE"; }


            }

            if (Idpost_ > 0 && Next_ == 6)
            {
                ForoLike ForoLikes = new ForoLike
                {
                    IdPost  = Idpost_,
                    IdForo  = IdForo_,
                    IdUser  = IdUser_,
                    Like    = 1,
                    Status  = 1,
                    Fecha   = DateTime.Now,
                };
                _context.Add(ForoLikes);
                await _context.SaveChangesAsync();
                mensajeUserName = "ab"; 
            }
            else { Next_ = 0; mensajeUserName = "na";}


            ViewBag.mensajeUserName = mensajeUserName;
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MensajePost(int gasvdgavs, string mensaje_, string returnUrl, int RefPage, string titulo)
        {
            int nPagina     = RefPage;
            long Idpost_    = 0;
            if (nPagina > 0)
            { RefPage       = RefPage; }
            else { RefPage  = 1; }
            Guid IdForo_    = new Guid("00000000-0000-0000-0000-000000000000");
            Guid IdUser_    = new Guid("00000000-0000-0000-0000-000000000000");

            var id  = _userManager.GetUserId(User);
            IdUser_ = new Guid(id);

            int Next_ = 5;
            if (Next_ == 5)
            {
                Foro d = _context.Foros.Where(s => s.Id == gasvdgavs).FirstOrDefault();
                if (d != null)
                {
                    IdForo_ = d.IdForo;
                    Idpost_ = d.Id;
                    Next_   = 6;
                }


            }

            if (Idpost_ > 0 && Next_ == 6 && !String.IsNullOrEmpty(mensaje_))
            {
                ForoMsg Foromensajes = new ForoMsg
                {
                    IdForoInt           = Idpost_,
                    IdForo              = IdForo_,
                    IdUserPlataforma    = IdUser_,
                    Mensaje             = mensaje_,
                    StatusForoUser      = 0,
                    StatusForoAdmin     = 0,
                    PublicadoMsg        = DateTime.Now,
                };
                _context.Add(Foromensajes);
                await _context.SaveChangesAsync();
                return Redirect("~/Home/Blog/?page=" + RefPage);
            }
            else {
                ViewBag.ns = 1;
                return Redirect("~/Home/Blog/?page=" + RefPage); 
            }
        }


        //=====================================================================================================================

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
