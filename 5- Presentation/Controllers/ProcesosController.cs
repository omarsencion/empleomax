using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using EmpleosWebMax.Infrastructure.Core;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.IO.Pipelines;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Common.Enum;

namespace empleoswebMax.Controllers
{
    public class ProcesosController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<ProcesosController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IPlanService _planService;

        public ProcesosController(IConfiguration configuration, ILogger<ProcesosController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, IWebHostEnvironment hostEnvironment, ISubscriptionService suscriptionServices, IPlanService planService)
        {
            Configuration       = configuration;
            _logger             = logger;
            _userManager        = userManager;
            _signInManager      = signInManager;
            _context            = context;
            webHostEnvironment  = hostEnvironment;
            _subscriptionService = suscriptionServices;
            _planService = planService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            ViewBag.fecha = DateTime.Now;
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ A-P-L-I-C-A-R  A  E-M-P-L-E-O ++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult Aplicando(int Id, Guid Oportunidad, string returnUrl, int RefPage)
        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            ViewBag._reft = RefPage;
            string  mailEmp     = "";
            string  TituloEmp   = "";
            string  IduserEmp   = ("00000000-0000-0000-0000-000000000000");
            string  idEmp       = ("00000000-0000-0000-0000-000000000000");
            int     TypeadEmp   = 0;
            string CandiName    = "";
            int enviaremail     = 0;
            ApplicationUser z = _context.Users.Where(s => s.Id == id).FirstOrDefault();
            if (z == null) { }
            else
            {
                CandiName = z.FirstName + " " + z.LastName;
            }

            if (Id > 0 && Oportunidad != Guid.Empty) {
                //datos emppleo
                Empleo u = _context.empleo.Where(s => s.Id.Equals(Id)).FirstOrDefault(); //LINQ
                if (u == null) { }else {
                    IduserEmp   = u.IdUser.ToString();
                    idEmp       = u.Idempresa.ToString();
                    TituloEmp   = u.Titulotrabajo;
                }
                int Existe = 3;

                if (_context.empleoAdds.Any(u => u.IdJob.Equals(Id) && u.IdUser.ToString().Equals(id)))
                { 
                    Existe = 1;
                    ViewBag.aplicando = "Aplicaste";
                    return View();
                }
                else { 
                    Existe = 0;

                }


                if (Existe == 0) { 
                ApplicationUser e = _context.Users.Where(s => s.Id == IduserEmp).FirstOrDefault();
                if (e == null) { }else {
                    TypeadEmp = e.TypeAdd;
                    mailEmp = e.UserName;
                }

                ViewBag.aplicando = "valido";
                EmpleoAdd empleoAdds = new EmpleoAdd
                {
                    IdJob           = Id,
                    Job             = Oportunidad,
                    IdUser          = new Guid(id),
                    status          = true,
                    dateadd         = DateTime.Now,
                    Tracking        = "Nuevo",
                    TrackingAdd     = DateTime.Now,
                    EmailCandidato  = mail,
                    EmailEmpresa    = mailEmp,
                    IdUserEmpresa   = new Guid(idEmp),
                    TituloEmpleo    = TituloEmp,
                    TypeAddEmpresa  = TypeadEmp,
                    statusSenMail   = 1,
                };
                _context.Add(empleoAdds);
                enviaremail = 1;
                _context.SaveChanges();
                if (TypeadEmp == 2 && enviaremail == 1 && Id > 0 && Oportunidad != Guid.Empty) { 
                    //**************************ENVIAR EMAIL Y REDIRECT **************************************************************************
                            string from_            = ViewBag.EmailCandidato;
                            string to_              = $"informacion@empleomax.com,{mailEmp}"; 
                            string asunto_          = $"{TituloEmp}";
                            string TituloEmpleo     = TituloEmp;
                            string TextoEnCompartir = "Oportunidades de trabajo en EmpleoMax";
                            string urlShare         = "https://www.empleomax.com/aplicaciones/curriculumvitae/?hdwdewttweggew=" + @id;
                            var lafechaes           = DateTime.Now;
                            MailMessage mail2       = new MailMessage();
                            mail2.From              = new MailAddress("informacion@empleomax.com"); 
                            mail2.To.Add(to_);
                            mail2.Subject = $"{asunto_}";
                            mail2.IsBodyHtml = true;
                            string htmlBody = "";
                            htmlBody = "" +
                                $"Hola, el Sr. / Sra. {CandiName} le comparte su CV a través de este enlace<a href='{urlShare}'> Curriculum.</a> en nuestro portal www.EmpleoMAX.com <br><br>" +
                                $"aprovechamos para invitarlos a crear su perfil en nuestro portal y, para ello le ofrecemos 6 meses gratuitos de nuestro servicio MAX OFERTA ideado para que usted pueda probar todas las ventajas de trabajar con nuestro equipo.<br><br>" +
                                $"Para cualquier información, no dude en contactarnos."+
                                $"<br><br>Saludos,<br><br>" +
                                $"Ing.Claudio D. Medina M. M.B.A. <br>" +
                                $"Director General <br>" +
                                $"www.ProcesOptimo.com│www.empleoMAX.com <br>" +
                                $"Cel. 809 - 913 - 5288 │ 829 - 876 - 2285 <br>" +
                                $"{lafechaes}";

                        mail2.Body = htmlBody;
                            SmtpClient smtp = new SmtpClient("mail.empleomax.com");
                            NetworkCredential Credentials = new NetworkCredential("informacion@empleomax.com", "123456Em@@");
                            smtp.UseDefaultCredentials  = false;
                            smtp.Credentials            = Credentials;
                            smtp.Port                   = 8889;    
                            smtp.EnableSsl              = false;
                            smtp.Send(mail2);
                            string lblMessage           = "Mail Sent";
                    }
                     ViewBag.aplicando = "Exito";
                }
            }
            else { ViewBag.aplicando = "Error"; }
            return View();
        }

        //email
        private async Task SendEmail(string toEmailAddress, string emailSubject, string emailMessage)
        {
            var msg = "ok";
            var message = new MailMessage();
            message.To.Add(toEmailAddress);

            message.Subject = emailSubject;
            message.Body = emailMessage;

            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.SendMailAsync(message);
            }
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ A-P-L-I-C-A-R  A  E-M-P-L-E-O ++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        //[Authorize]
        public IActionResult Aplicar(int Id, string Oportunidad, string returnUrl, int RefPage)
        {
            ViewBag.id_abc      = Id;
            ViewBag.id2_abc     = Oportunidad;
            var id              = _userManager.GetUserId(User);
            var mail            = _userManager.GetUserName(User);
            string Process      = "";
            int nPagina         = 1;
            nPagina             = RefPage;
            if (nPagina == null || nPagina == 0 || nPagina < 0)
            { nPagina           = 1; }
            else if (nPagina > 0)
            { nPagina           = nPagina; }
            else { nPagina      = 1; }
            ViewBag.nPagina     = nPagina;

            bool isAuthenticated = User.Identity.IsAuthenticated;
            int UserLogedIn     = 0;

            returnUrl           = returnUrl ?? Url.Content("~/");
            string returnUrlnew = returnUrl;

            ViewBag.nnId        = Id;
            ViewBag.Oportunidad = Oportunidad;
            ViewBag.userId      = id;
            ViewBag.mail        = mail;
            DateTime dateadd = DateTime.Now;

            if (isAuthenticated == true)
            {
                UserLogedIn = 1;
                Process = "Ok go";
                ViewBag.Existe = 0;
                ViewBag.NamesCandidato = "";
                List<ApplicationUser2> UserxList = new List<ApplicationUser2>();
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql2 = $"SELECT * FROM AspNetUsers WHERE TypeUser = 255485 And Id = '{id}' and UserName = '{mail}'";
                    SqlCommand command = new SqlCommand(sql2, connection);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            ApplicationUser2 Userx      = new ApplicationUser2();
                            Userx.Id                    = Convert.ToString(dataReader["Id"]);
                            Userx.FirstName             = Convert.ToString(dataReader["FirstName"]).ToUpper();
                            Userx.LastName              = Convert.ToString(dataReader["LastName"]).ToUpper();
                            Userx.Email                 = Convert.ToString(dataReader["UserName"]);
                            Userx.TypeUser              = Convert.ToInt32(dataReader["TypeUser"]);
                            ViewBag.IdUser              = Userx.Id;
                            ViewBag.Existe              = 1;
                            ViewBag.NamesCandidato      = Userx.FirstName + " " + Userx.LastName;
                            ViewBag.NameFirst           = Userx.FirstName;
                            ViewBag.EmailCandidato      = Userx.Email;
                            UserxList.Add(Userx);
                        }
                    }
                }

                if (ViewBag.Existe == 0)
                {
                    returnUrl = returnUrlnew;
                    _logger.LogInformation("User no existe.");
                    return LocalRedirect(returnUrl);
                }


                //================================== BUSCAR EMAIL DE EMPRESA PARA ENVIAR NOTIFICACION CUANDO ALGUIEN APLICA ========================================================
                ViewBag.statussendemail = 0;
                List<EmpleoAdd> UserEmpresaList = new List<EmpleoAdd>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql2 = $"select AspNetUsers.Id As IdUserEmpresa, AspNetUsers.UserName As EmailEmpresa, AspNetUsers.TypeAdd As TypeAddEmpresa, empleo.Titulotrabajo As TituloEmpleo from AspNetUsers JOIN empleo ON empleo.IdUser = AspNetUsers.Id  WHERE AspNetUsers.TypeUser = 69784 AND empleo.Id = {Id} AND empleo.Job = '{Oportunidad}'";
                    SqlCommand command = new SqlCommand(sql2, connection);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            EmpleoAdd Userx = new EmpleoAdd();
                            Guid gIdUserEmpresa     = Guid.Parse(dataReader["IdUserEmpresa"].ToString());
                            Userx.IdUserEmpresa     = gIdUserEmpresa;
                            Userx.EmailEmpresa      = Convert.ToString(dataReader["EmailEmpresa"]);
                            Userx.TituloEmpleo      = Convert.ToString(dataReader["TituloEmpleo"]).ToUpper();
                            Userx.TypeAddEmpresa    = Convert.ToInt32(dataReader["TypeAddEmpresa"]);

                            ViewBag.IdUserEmpresa   = Userx.IdUserEmpresa;
                            ViewBag.EmailEmpresa    = Userx.EmailEmpresa;
                            ViewBag.TituloEmpleo    = Userx.TituloEmpleo;
                            ViewBag.TypeAddEmpresa  = Userx.TypeAddEmpresa;
                            ViewBag.statussendemail = 1;
                            UserEmpresaList.Add(Userx);
                        }
                    }
                }

                if (ViewBag.Existe == 0)
                {
                    returnUrl = returnUrlnew;
                    _logger.LogInformation("User no existe.");
                    return LocalRedirect(returnUrl);
                }
                //==================================================================================================================================================================

                ViewBag.userId = id;
                ViewBag.Existe = 0; 
                List<EmpleoAdd> EmpleList = new List<EmpleoAdd>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql2 = $"SELECT * FROM empleoAdds WHERE IdUser = '{ViewBag.IdUser}' and IdJob = {ViewBag.nnId} and Job = '{ViewBag.Oportunidad}'";
                    SqlCommand command = new SqlCommand(sql2, connection);
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            EmpleoAdd Userx = new EmpleoAdd();
                            Userx.Id = Convert.ToInt64(dataReader["Id"]);
                            ViewBag.Existe = 1;
                            EmpleList.Add(Userx);
                        }
                    }
                }
                ViewBag.UserxList = UserxList; 
                if (ViewBag.Existe == 1)
                {
                    return RedirectToAction("Existe", "Aplicaciones");
                }

                if (ViewBag.nnId == 0 || ViewBag.nnId == null)
                {
                    return LocalRedirect(returnUrl);
                }

                int userexiste          = 0;
                Guid IdUserEmpresa_     = new Guid("00000000-0000-0000-0000-000000000000");
                string EmailEmpresa_    = "";
                long idemple            = ViewBag.nnId;
                string IdUserEmpresa_ss = "";
                if (idemple > 0)
                {
                    if (_context.empleo.Any(u => u.Id.Equals(idemple)))
                    { userexiste = 1; }
                    else { userexiste   = 0; }

                    if (userexiste == 1)
                    {
                        Empleo u            = _context.empleo.Where(s => s.Id.Equals(idemple)).First(); 
                        IdUserEmpresa_ss    = u.Idempresa.ToString();
                        IdUserEmpresa_      = u.Idempresa;
                        Empresaperfil b     = _context.empresaperfils.Where(s => s.Idempresa.Equals(IdUserEmpresa_)).First(); 
                        EmailEmpresa_       = b.email;
                    }
                }
                Boolean status_ = true;
                string tracking_ = "nuevo";

                //..............................................................................................................................
                ViewBag.IdUserEmpresa   = IdUserEmpresa_ss;
                ViewBag.EmailEmpresa    = EmailEmpresa_;
                ViewBag.TituloEmpleo    = ViewBag.TituloEmpleo;
                ViewBag.TypeAddEmpresa  = ViewBag.TypeAddEmpresa;
                ViewBag.statussendemail = 1;
                string EmailCandidato = mail;
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = $"Insert Into empleoAdds " +
                        $"(IdJob,Job,IdUser,status,dateadd,Tracking,TrackingAdd,IdUserEmpresa,EmailEmpresa,TituloEmpleo,TypeAddEmpresa,statusSenMail,EmailCandidato) " +
                        $"Values " +
                        $"({ViewBag.nnId},'{ViewBag.Oportunidad}','{ViewBag.IdUser}','{status_}','{dateadd}','{tracking_}','{dateadd}'," +
                        $"'{ViewBag.IdUserEmpresa}','{ViewBag.EmailEmpresa}','{ViewBag.TituloEmpleo}',{ViewBag.TypeAddEmpresa},{ViewBag.statussendemail},'{EmailCandidato}')";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    //**************************ENVIAR EMAIL Y REDIRECT **************************************************************************
                    ViewBag.TypeAddEmpresa  = ViewBag.TypeAddEmpresa;
                    ViewBag.NameFirst       = ViewBag.NameFirst;
                    string from_            = ViewBag.EmailCandidato;//"informacion@empleomax.com";
                    string to_              = "informacion@empleomax.com"; //ViewBag.EmailEmpresa;
                    string asunto_          = $"Hola, {ViewBag.NamesCandidato} le comparte su Curriculum";
                    string mensaje_         = ViewBag.TituloEmpleo;
                    ViewBag.titulo          = ViewBag.TituloEmpleo;
                    string TituloEmpleo     = ViewBag.TituloEmpleo;
                    string TextoEnCompartir = "Oportunidades de trabajo en EmpleoMax";
                    string urlShare = "https://www.empleomax.com/Home/Empleos?buscar" + @TituloEmpleo;
                    var lafechaes = DateTime.Now;

                    MailMessage mail2 = new MailMessage();

                    //set the addresses 
                    mail2.From = new MailAddress("informacion@empleomax.com"); //IMPORTANT: This must be same as your smtp authentication address.
                    mail2.To.Add(to_);

                    //set the content 
                    mail2.Subject = $"{asunto_}";
                    //mail.Body = "This is from system.net.mail using C sharp with smtp authentication.";
                    mail2.IsBodyHtml = true;
                    string htmlBody = "";

                    htmlBody = "" +
                    $"<p style = 'line - height: 150 %; margin - top: 0; margin - bottom: 0' > Hola! <b>{ViewBag.NamesCandidato}</b> le comparte que tiene su Curriculum Vitae en nuestro portal www.EmpleoMAX.com </p>" +
                            $"<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>{ViewBag.NameFirst} ha visto la oportuidad laboral:</p>" +
                            $"<b>{mensaje_}</b><br><br>" +
                            $"<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>aprovechamos para invitarlos a crear su perfil en nuestro portal y, para ello le ofrecemos 6 meses gratuitos de nuestro servicio MAX OFERTA ideado para que usted pueda probar todas las ventajas de trabajar con nuestro equipo.</p><br>" +
                            $"<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'><a href='{urlShare}'>Clic aqui en este link para ver el curriculum de {ViewBag.NamesCandidato}.</a></p><br><br>" +
                            "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'><br>Para cualquier información, no dude en contactarnos.&nbsp;</p>" +
                            "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>Cordialmente.</p>" +
                            "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>&nbsp;</p>" +
                            "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>El equipo de Empleos Max.</p>" +
                            "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>informacion@empleomax.com</p>" +
                            $"{lafechaes}";

                    mail2.Body                      = htmlBody;
                    SmtpClient smtp                 = new SmtpClient("mail.empleomax.com");
                    NetworkCredential Credentials   = new NetworkCredential("informacion@empleomax.com", "123456Em@@");
                    smtp.UseDefaultCredentials      = false;
                    smtp.Credentials                = Credentials;
                    smtp.Port                       = 8889;  
                    smtp.EnableSsl                  = false;
                    smtp.Send(mail2);
                    string lblMessage = "Mail Sent";
                    return RedirectToAction("Aplicaste", "Aplicaciones");
                }
            }
            else
            {
                UserLogedIn = 2;
                Process = "No go";
            }

            ViewBag.UserLogedInstring = Process;

                return View();

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login2(string returnUrl, int RefPage)
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login2(LoginViewModel user, int Id, string Oportunidad, int reft, string returnUrl, int npage)
        {
            ViewBag.reft3       = reft;
            ViewBag.returnUrl   = returnUrl;
            int _npage          = 0;
            _npage              = npage;
            if (_npage == null || _npage == 0)
            { _npage = 1; }
            else if (_npage > 0) { _npage = _npage; }
            else { _npage = 1; }

            ViewBag.nPagina = _npage;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    ViewBag.CheckUserType   = 0;
                    var id                  = _userManager.GetUserId(User);
                    var mail                = user.Email;
                    ViewBag.nnId            = Id;
                    ViewBag.Oportunidad     = Oportunidad;
                    ViewBag.iduser          = "00000000-0000-0000-0000-000000000000";

                    List<ApplicationUserDto> lasexperienciasList = new List<ApplicationUserDto>();
                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = $"SELECT Id,TypeUser from AspNetUsers WHERE UserName = '{mail}'"; 
                        SqlCommand command = new SqlCommand(sql, connection);
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                ApplicationUserDto lasexperiencias  = new ApplicationUserDto();
                                Guid IdUserCandidatoguid            = Guid.Parse(dataReader["Id"].ToString());
                                lasexperiencias.Id                  = IdUserCandidatoguid;
                                lasexperiencias.TypeUser            = Convert.ToInt32(dataReader["TypeUser"]);
                                ViewBag.iduser                      = lasexperiencias.Id;
                                ViewBag.CheckUserType               = lasexperiencias.TypeUser;
                                lasexperienciasList.Add(lasexperiencias);

                            }
                        }
                    }

                    ViewBag.Existe = 0; 
                    List<EmpleoAdd> EmpleList = new List<EmpleoAdd>();
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql2 = $"SELECT * FROM empleoAdds WHERE IdUser = '{ViewBag.iduser}' and IdJob = {ViewBag.nnId} and Job = '{ViewBag.Oportunidad}'";
                        SqlCommand command = new SqlCommand(sql2, connection);
                        using (SqlDataReader dataReader = command.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                EmpleoAdd Userx = new EmpleoAdd();
                                Userx.Id        = Convert.ToInt64(dataReader["Id"]);
                                ViewBag.Existe  = 1;
                                EmpleList.Add(Userx);
                            }
                        }
                    }

                    if (ViewBag.Existe == 1)
                    {
                        return RedirectToAction("Existe", "Aplicaciones", new { reft = ViewBag.reft3, returnUrl = ViewBag.returnUrl, npage = _npage });
                    }

                    if (ViewBag.nnId == 0 || ViewBag.nnId == null)
                    {
                        return RedirectToAction("Empleos", "Home");
                    }

                    if (ViewBag.CheckUserType == 255485)
                    {
                        Boolean status_ = true;
                        string tracking_ = "nuevo";
                        DateTime dateadd = DateTime.Now;
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            string sql = $"Insert Into empleoAdds (IdJob,Job,IdUser,status,dateadd,Tracking,TrackingAdd) Values ({ViewBag.nnId},'{ViewBag.Oportunidad}','{ViewBag.iduser}','{status_}','{dateadd}','{tracking_}','{dateadd}')";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.CommandType = CommandType.Text;
                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                            return RedirectToAction("Aplicaste", "Aplicaciones", new { reft = ViewBag.reft3, returnUrl = ViewBag.returnUrl, npage = _npage });
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.id_abc = Id;
                ViewBag.id2_abc = Oportunidad;

                ModelState.AddModelError(string.Empty, "ingrese un email y password validos");
                return View(user);
            }
            return View(user);
        }


        //+++++++++++++++++++++++++++++++++++++++ VER APLICACIONES +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult Amigos(int doc, string laoport)
        {
            var id = _userManager.GetUserId(User);
            List<FriendsDto> UserxList  = new List<FriendsDto>();
            string connectionString     = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string sql2 = $"Select * from friendsall WHERE status < 20 AND IdUserPrincipal = '{id}'  ORDER BY amigoStatusFecha DESC, TypeUserGuest DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    string FirsN = "";
                    string LastN = "";
                    string NamesGuest = "";
                    while (dataReader.Read())
                    {
                        FriendsDto Userx        = new FriendsDto();
                        Userx.Id = Convert.ToInt64(dataReader["Id"]);
                        Userx.IdUserGuest       = dataReader.GetGuid(dataReader.GetOrdinal("IdUserGuest"));
                        Userx.MailGuest         = Convert.ToString(dataReader["MailGuest"]).ToUpper();
                        Userx.Nameinvitado      = Convert.ToString(dataReader["Nameinvitado"]).ToUpper();
                        Userx.solicitudEnviada  = Convert.ToInt16(dataReader["solicitudEnviada"]);
                        Userx.amigoStatus       = Convert.ToInt16(dataReader["amigoStatus"]);
                        Userx.TypeUserGuest     = Convert.ToInt32(dataReader["TypeUserGuest"]);
                        Userx.amigoStatusFecha  = Convert.ToDateTime(dataReader["amigoStatusFecha"]);
                        Userx.status            = Convert.ToInt16(dataReader["status"]);
                        ApplicationUser u       = _context.Users.Where(s => s.UserName == Userx.MailGuest).FirstOrDefault(); //LINQ
                            if(Userx.TypeUserGuest == 11)
                            {
                                Userx.Nombres = Userx.Nameinvitado;
                            }
                            else { 
                                NamesGuest = u.FirstName + " " + u.LastName;
                            Userx.Nombres = NamesGuest;
                            }
                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;


            List<FriendsDto> UserxList2 = new List<FriendsDto>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string sql2 = $"Select * from friendsall WHERE  IdUserGuest = '{id}' AND status < 4  ORDER BY amigoStatusFecha DESC, TypeUserGuest DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    string FirsN = "";
                    string LastN = "";
                    string NamesGuest = "";
                    while (dataReader.Read())
                    {
                        FriendsDto Userx            = new FriendsDto();
                        Userx.Id                    = Convert.ToInt64(dataReader["Id"]);
                        Userx.IdUserPrincipal       = dataReader.GetGuid(dataReader.GetOrdinal("IdUserPrincipal"));
                        string Idprinci             = Convert.ToString(Userx.IdUserPrincipal);
                        Userx.MailPrincipal         = Convert.ToString(dataReader["MailPrincipal"]).ToUpper();
                        Userx.Nameinvitado          = Convert.ToString(dataReader["Nameinvitado"]).ToUpper();
                        Userx.solicitudEnviada      = Convert.ToInt16(dataReader["solicitudEnviada"]);
                        Userx.solicitudRecibida     = Convert.ToInt16(dataReader["solicitudRecibida"]);
                        Userx.amigoStatus           = Convert.ToInt16(dataReader["amigoStatus"]);
                        Userx.TypeUserGuest         = Convert.ToInt32(dataReader["TypeUserGuest"]);
                        Userx.TypeUserPrincipal     = Convert.ToInt32(dataReader["TypeUserPrincipal"]);
                        Userx.amigoStatusFecha      = Convert.ToDateTime(dataReader["amigoStatusFecha"]);
                        Userx.status                = Convert.ToInt16(dataReader["status"]);
                        ApplicationUser u           = _context.Users.Where(s => s.Id == Idprinci).FirstOrDefault(); 
                            if (Userx.TypeUserPrincipal == 11)
                            {
                                Userx.Nombres = "";
                            }
                            else if(Userx.TypeUserPrincipal == 255485 || Userx.TypeUserPrincipal == 69784)
                            {
                                NamesGuest = u.FirstName + " " + u.LastName;
                                Userx.Nombres = NamesGuest;
                            } else { Userx.Nombres = ""; }

                        UserxList2.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList2 = UserxList2;

            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        [HttpGet]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CheckGuest(string username)
        {
            string mensajeUserName  = "";
            bool b1                 = string.IsNullOrEmpty(username);
            int Next_               = 0; //
            string _MailGuest       = "";
            if (b1 == false)
            { 
                Next_ = 1; mensajeUserName = "nn";}
                    else 
            { Next_ = 0; mensajeUserName = "nv";}

            _MailGuest = username;
            if (Next_ == 1)
            {
                string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                if (Regex.IsMatch(_MailGuest, expression))
                {
                    if (Regex.Replace(_MailGuest, expression, string.Empty).Length == 0)
                    { Next_ = 2; mensajeUserName = "ec";}
                }
                else
                {
                    Next_ = 0; mensajeUserName = "ef";
                }
                
            }
            ViewBag.mensajeUserName = mensajeUserName;
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

        [ActionName("EditUsuario")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Update_Post2(Empleo empleo, string St, int gdfsd, string returnUrl) //, Guid Us, string Mi

        {
            var id      = _userManager.GetUserId(User);
            Guid id2    = new Guid(id);
            var mail    = _userManager.GetUserName(User);

            int status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; } 
                else if (St == "x") { status1 = 1; } 
                else if (St == "r") { status1 = 2; } 
                else { status1 = 0; }
            }

            if (status1 < 30 && gdfsd >=0) 
            {
                Friends d = _context.friendsall.Where(s => s.Id == gdfsd && s.IdUserPrincipal == id2).FirstOrDefault();
                d.status = status1;
                _context.SaveChanges();
            }
            return LocalRedirect(returnUrl);
        }

        [ActionName("EditUsuario2")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Update_Post3(Empleo empleo, string St, int gdfsd, string returnUrl) 

        {
            var id = _userManager.GetUserId(User);
            Guid id2 = new Guid(id);
            var mail = _userManager.GetUserName(User);
            int amigostatus_ = 0;

            int status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; } 
                else if (St == "x") { status1 = 1; } 
                else if (St == "xy") { status1 = 1; amigostatus_ = 1; }
                else if (St == "r") { status1 = 2; } 
                else { status1 = 0; }
            }

            if (status1 < 30 && gdfsd >= 0) 
            {
                Friends d = _context.friendsall.Where(s => s.Id == gdfsd && s.IdUserGuest == id2).FirstOrDefault();
                d.status = status1;
                if (St == "xy")
                {
                    d.amigoStatus = amigostatus_;
                }
                _context.SaveChanges();
            }
            return LocalRedirect(returnUrl);
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//  
        public IActionResult MyDoc()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyDoc(DocsDto model, string DocName, string DocNameTitle, IFormFile file) //RegisterViewModel model
        {
            var id = _userManager.GetUserId(User);
            Guid id2 = new Guid(id);
            int addEmpresa = 0;
            bool b1 = string.IsNullOrEmpty(DocNameTitle);
            if (b1 == false) { addEmpresa = 1; }

            //======================================= NOBRE AL DOCUMENTO A SUBIR ==================================
            var fileName = System.IO.Path.GetFileName(file.FileName); //=== N O M B R E   DEL  DOCUMENTO A SUBIR
            string Namedocument = fileName;
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
            //======================================== UPLOAD FILE ================================================

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "documentos");
            var fileName2 = Path.Combine(uploadsFolder, fileName);
            using (var localFile = System.IO.File.OpenWrite(fileName2))
            using (var uploadedFile = file.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            if (addEmpresa == 1)
            {
                //string uniqueFileName = UploadedFile(model);
                Docs UserDocs = new Docs
                {
                    IdUser          = id2,
                    DocName         = fileName,
                    DocNameTitle    = DocNameTitle,
                    Status          = 0,
                    Fecha           = DateTime.Now,
                };
                _context.Add(UserDocs);
                await _context.SaveChangesAsync();
                return RedirectToAction("VerDocumentos", "Procesos");

            }
            return View();

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ VER DOcumentos +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//        
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult VerDocumentos()
        {
            var id = _userManager.GetUserId(User);

            //---------APLICAR ---------------------------------------------------------
            List<Docs> UserxList = new List<Docs>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select * from UserDocs where IdUser = '{id}' AND Status < 20 order by Fecha DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Docs Userx = new Docs();
                        Userx.Id            = Convert.ToInt64(dataReader["Id"]);
                        Userx.IdUser        = dataReader.GetGuid(dataReader.GetOrdinal("IdUser"));
                        Userx.DocName       = Convert.ToString(dataReader["DocName"]).ToUpper();
                        Userx.DocNameTitle  = Convert.ToString(dataReader["DocNameTitle"]).ToUpper();
                        Userx.Status        = Convert.ToInt16(dataReader["Status"]);
                        Userx.Fecha         = Convert.ToDateTime(dataReader["Fecha"]);                       
                        ViewBag.IdUser      = Userx.Id;
                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS DOCUMENTOS  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("EditDocument")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Post(Docs docu, string St, long gdfsd) //, Guid Us, string Mi
        {
            var id      = _userManager.GetUserId(User);
            var mail    = _userManager.GetUserName(User);
            if (gdfsd < 0) { gdfsd = 0; }

            Int16 status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; } 
                else if (St == "x") { status1 = 0; } 
                else if (St == "r") { status1 = 1; } 
                else { status1 = 0; }
            }

            if (gdfsd > 0) //validar Id
            {
                Docs d = _context.UserDocs.Where(s => s.Id == gdfsd).FirstOrDefault();
                d.Status = status1;
                _context.SaveChanges();
            }

            return RedirectToAction("VerDocumentos", "Procesos");
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Descarga(DocsDto model, string DocName, string DocNameTitle) //RegisterViewModel model
        {
            string newDocDownload = DocName;
            bool b1 = string.IsNullOrEmpty(newDocDownload);
            if (b1 == false) { newDocDownload = DocName; } else { newDocDownload = "download.png"; }


            string uniqueFileName   = newDocDownload;
            string uploadsFolder    = Path.Combine(webHostEnvironment.WebRootPath, "documentos");
            string filePath         = Path.Combine(uploadsFolder, uniqueFileName);
            var path                = filePath;
            var memory              = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(path).ToLowerInvariant();

            return File(memory, GetMimeTypes()[ext], Path.GetFileName(path));
        }

          
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string> {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xls", "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxlmformats-officedocument.spreadsheetml.sheet"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"},
            {".csv", "text/csv"},
            };
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ BLOG COMENTARIOS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        public IActionResult PostComentarios(int Id, string Oportunidad, string returnUrl, int RefPage)
        {
            ViewBag.nForoid     = Id;
            ViewBag.iddelforo   = Oportunidad;

            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            List<ForoMsgDto> NEWMSGxList = new List<ForoMsgDto>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connectionM = new SqlConnection(connectionString))
            {
                connectionM.Open();
                string sql2M = $"SELECT * FROM Foromensajes WHERE IdForoint = {ViewBag.nForoid} and IdForo = '{ViewBag.iddelforo}' and StatusForoUser = 0 And StatusForoAdmin = 0 ORDER BY PublicadoMsg DESC";
                SqlCommand commandM = new SqlCommand(sql2M, connectionM);
                using (SqlDataReader dataReaderM = commandM.ExecuteReader())
                {
                    while (dataReaderM.Read())
                    {
                        ForoMsgDto MSGx = new ForoMsgDto();
                        MSGx.Mensaje = Convert.ToString(dataReaderM["Mensaje"]);
                        MSGx.PublicadoMsg = Convert.ToDateTime(dataReaderM["PublicadoMsg"]);
                        Guid Idforotoguid = Guid.Parse(dataReaderM["IdUserPlataforma"].ToString());
                        MSGx.IdUserPlataforma = Idforotoguid;
                        string idus = Convert.ToString(MSGx.IdUserPlataforma);
                        int isaut = 1;
                        int userexiste = 0;
                        string nameUser = "#";
                        if (isaut == 1)
                        {
                            if (_context.Users.Any(u => u.Id.Equals(Idforotoguid)))
                            { userexiste = 1; }
                            else { userexiste = 0; }

                            if (isaut == 1)
                            {
                                ApplicationUser u = _context.Users.Where(s => s.Id.Equals(idus)).FirstOrDefault(); //LINQ
                                nameUser = u.FirstName;
                            }
                        }
                        MSGx.UserMsg = nameUser;

                        NEWMSGxList.Add(MSGx);
                    }
                }
            }
            ViewBag.MSGList = NEWMSGxList;

            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("Ticketstatus")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Ticket(Docs docu, string St, long gdfsd) 
        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            if (gdfsd < 0) { gdfsd = 0; }

            Int16 status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; } 
                else if (St == "x") { status1 = 0; } 
                else if (St == "r") { status1 = 1; } 
                else { status1 = 0; }
            }

            if (gdfsd > 0) //validar Id
            {
                Ticket d = _context.Tickets.Where(s => s.Id == gdfsd).First();
                d.StatusTicket = status1;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "myTicket");
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ BLOG COMENTARIOS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        //[Authorize]
        public IActionResult TicketComentarios(int Id, string Oportunidad, string returnUrl, int RefPage)
        {
            var id                  = _userManager.GetUserId(User);
            ViewBag.nForoid         = Id;
            ViewBag.iddelforo       = Oportunidad;
            string nameUser2        = "";
            string paraquien        = "";
            string Ticketmensaje    = "";
            int checkstatus         = 0;
            Ticket d                = _context.Tickets.Where(s => s.TicketNumber.Equals(Oportunidad)).First();
            paraquien               = d.From_.ToString();
            Ticketmensaje           = d.Mensaje;
            checkstatus             = d.StatusTicket;


            if (id.Equals(paraquien) && checkstatus < 20)
            {
                d.StatusTicket = 1;
            }
            _context.SaveChanges();
            ViewBag.Ticketmensaje = Ticketmensaje;

            _context.SaveChanges();
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            List<TicketDto> NEWMSGxList = new List<TicketDto>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connectionM = new SqlConnection(connectionString))
            {
                string TicketNumber_ = Oportunidad;


                string NameFrom_x = nameUser2;
                connectionM.Open();
                string sql2M = $"SELECT * FROM TicketResponses Where TicketNumber = '{TicketNumber_}'  AND To_ = '{id}' OR From_ = '{id}'  ORDER BY Id DESC";
                SqlCommand commandM = new SqlCommand(sql2M, connectionM);
                using (SqlDataReader dataReaderM = commandM.ExecuteReader())
                {
                    while (dataReaderM.Read())
                    {
                        TicketDto MSGx          = new TicketDto();
                        MSGx.Respuesta          = Convert.ToString(dataReaderM["Respuesta"]);
                        MSGx.FechaResponse      = Convert.ToDateTime(dataReaderM["FechaResponse"]);
                        Guid From_              = Guid.Parse(dataReaderM["From_"].ToString());
                        string From_s           = From_.ToString();
                        Guid To_                = Guid.Parse(dataReaderM["To_"].ToString());
                        string To_s             = To_.ToString();


                        ApplicationUser u = _context.Users.Where(s => s.Id.Equals(From_s)).First(); //LINQ
                        NameFrom_x = u.FirstName + " " + u.LastName;
                        MSGx.NameFrom_ = NameFrom_x;
                        NEWMSGxList.Add(MSGx);
                    }
                }
            }
            ViewBag.MSGList = NEWMSGxList;

            return View();
        }


        //+++++++++++++++++++++++++++++++++++++++ BLOG COMENTARIOS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult TicketComentarioss(int Id, string Oportunidad, string returnUrl, int RefPage)
        {
            var id                  = _userManager.GetUserId(User);
            ViewBag.nForoid         = Id;
            ViewBag.iddelforo       = Oportunidad;
            string nameUser2        = "";
            string paraquien        = "";
            string Ticketmensaje    = "";
            int checkstatus         = 0;

            Ticket d                = _context.Tickets.Where(s => s.TicketNumber.Equals(Oportunidad)).First();
            paraquien               = d.To_.ToString();
            checkstatus             = d.StatusTicket;

            if (id.Equals(paraquien) && checkstatus < 20)
            {
                d.StatusTicket = 1;
                Ticketmensaje = d.Mensaje;
            }
            _context.SaveChanges();
            ViewBag.Ticketmensaje = Ticketmensaje;
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            List<TicketDto> NEWMSGxList = new List<TicketDto>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connectionM = new SqlConnection(connectionString))
            {
                string TicketNumber_ = Oportunidad;
                string NameFrom_x = nameUser2;
                connectionM.Open();
                string sql2M = $"SELECT * FROM TicketResponses Where TicketNumber = '{TicketNumber_}'  AND To_ = '{id}' OR From_ = '{id}'  ORDER BY Id DESC";
                SqlCommand commandM = new SqlCommand(sql2M, connectionM);
                using (SqlDataReader dataReaderM = commandM.ExecuteReader())
                {
                    while (dataReaderM.Read())
                    {
                        TicketDto MSGx      = new TicketDto();
                        MSGx.Respuesta      = Convert.ToString(dataReaderM["Respuesta"]);
                        MSGx.FechaResponse  = Convert.ToDateTime(dataReaderM["FechaResponse"]);
                        Guid From_          = Guid.Parse(dataReaderM["From_"].ToString());
                        string From_s       = From_.ToString();
                        Guid To_            = Guid.Parse(dataReaderM["To_"].ToString());
                        string To_s         = To_.ToString();
                        ApplicationUser u   = _context.Users.Where(s => s.Id.Equals(From_s)).First(); //LINQ
                        NameFrom_x          = u.FirstName + " " + u.LastName;
                        MSGx.NameFrom_      = NameFrom_x;
                        NEWMSGxList.Add(MSGx);
                    }
                }
            }
            ViewBag.MSGList = NEWMSGxList;
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS DOCUMENTOS  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           
        [HttpPost]
        [ActionName("Ticketstatusadm")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Ticketadm(Docs docu, string St, long gdfsd) //, Guid Us, string Mi
        {
            var id = _userManager.GetUserId(User);
            var mail = _userManager.GetUserName(User);
            if (gdfsd < 0) { gdfsd = 0; }

            Int16 status1 = 0;
            if (St != null)
            {
                if (St == "a") { status1 = 20; } //delete
                else if (St == "x") { status1 = 0; } //show
                else if (St == "r") { status1 = 1; } //hidden
                else { status1 = 0; }
            }

            if (gdfsd > 0) //validar Id
            {
                Ticket d = _context.Tickets.Where(s => s.Id == gdfsd).First();
                d.StatusTicket = status1;
                _context.SaveChanges();
            }

            return RedirectToAction("Main", "Admin");
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ REGISTRO USUARIOS Con emresas ya creadas  +++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//           [HttpPost]
        public IActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName        = model.Email,
                    Email           = model.Email,
                    FirstName       = model.FirstName,
                    LastName        = model.LastName,
                    Sexo            = model.Sexo,
                    TypeUser        = 255485,
                    Status          = 25,
                    StatusGeneral   = 1,
                    DateAdd         = DateTime.Now,
                    TypeAdd = 1,

                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var planDto = await _planService.GetPlanDefaultByCategory(Category.Candidate);
                    if (planDto.ResultKind == ResultKind.Success) 
                    {
                        var suscription = new SubscriptionDto
                        { 
                          ApplicationUserId = user.Id,
                          PlanId = planDto.Data.Id,
                          PlanName = planDto.Data.Name,
                          PlanPrice = planDto.Data.Price,
                          TotalPrice = planDto.Data.Price
                        };

                        await _subscriptionService.AddSubscription(suscription);
                    }
                    
                    return RedirectToAction("Candidatos", "Home");
                }

                foreach (var error in result.Errors)
                {
                    //ModelState.AddModelError("", error.Description);
                    ModelState.AddModelError(string.Empty, error.Code);
                    string ErrorRegistro = error.Code;
                    if (ErrorRegistro == "DuplicateUserName")
                    {
                        ViewBag.erroremail = "Utilice otro @email";
                        ViewBag.ns = 1;
                        //ViewData["tipou"] = nNewStatus;
                    }
                    else
                    {
                        ViewBag.erroremail = "";
                        ViewBag.ns = 0;
                        //ViewData["tipou"] = nNewStatus;
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ REGISTRO NEGOCIOS USUARIOS Con emresas ya creadas  ++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++// 
        public IActionResult BusinessRegister()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BusinessRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user            = new ApplicationUser
                {
                    UserName        = model.Email,
                    Email           = model.Email,
                    FirstName       = model.FirstName,
                    LastName        = model.LastName,
                    Sexo            = model.Sexo,
                    TypeUser        = 69784,
                    Status          = 69,
                    StatusGeneral   = 1,
                    DateAdd         = DateTime.Now,
                    TypeAdd         = 1,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var planDto = await _planService.GetPlanDefaultByCategory(Category.Employee);
                    if (planDto.ResultKind == ResultKind.Success)
                    {
                        var suscription = new SubscriptionDto
                        {
                            ApplicationUserId = user.Id,
                            PlanId = planDto.Data.Id,
                            PlanName = planDto.Data.Name,
                            PlanPrice = planDto.Data.Price,
                            TotalPrice = planDto.Data.Price
                        };

                        await _subscriptionService.AddSubscription(suscription);
                    }
                    return RedirectToAction("Empresas", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Code);
                    string ErrorRegistro = error.Code;
                    if (ErrorRegistro == "DuplicateUserName")
                    {
                        ViewBag.erroremail = "Utilice otro @email";
                        ViewBag.ns = 1;
                    }
                    else
                    {
                        ViewBag.erroremail = "";
                        ViewBag.ns = 0;
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//   

    }
}