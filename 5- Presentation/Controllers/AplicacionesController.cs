using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using EmpleosWebMax.Infrastructure.Core;
using System.Net;
using System.Net.Mail;

namespace empleoswebMax.Controllers
{
    public class AplicacionesController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<AplicacionesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;



        public AplicacionesController(SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<AplicacionesController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger = logger; 
            Configuration = configuration; 
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ I-N-D-E-X  A-P-L-I-C-A-R  A  E-M-P-L-E-O +++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult Index(int Id, string Oportunidad)
        {
            var id      = _userManager.GetUserId(User);
            var mail    = _userManager.GetUserName(User);
            if (Id > 0) { }
            else {
                return RedirectToAction("Empresas", "Home");
            }

            List<CandidatosList_> UserxList = new List<CandidatosList_>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select empleoAdds.*,empleo.*,AspNetUsers.* from empleoAdds join empleo ON empleo.Job = empleoAdds.Job And empleo.Id = empleoAdds.IdJob And empleo.IdUser = '{id}' join AspNetUsers On AspNetUsers.Id = empleoAdds.IdUser And AspNetUsers.TypeUser = 255485 where empleoAdds.IdJob = {Id} And empleoAdds.Job = '{Oportunidad}'";// And empleoAdds.IdJob = {nIdJob}
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        CandidatosList_ Userx   = new CandidatosList_();
                        Userx.IdJob             = Convert.ToInt64(dataReader["IdJob"]);
                        Userx.IdUser            = dataReader.GetGuid(dataReader.GetOrdinal("IdUser"));
                        Userx.Job               = dataReader.GetGuid(dataReader.GetOrdinal("Job"));
                        Userx.FirstName         = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName          = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Titulotrabajo     = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        Userx.Tracking          = Convert.ToString(dataReader["Tracking"]).ToUpper();
                        Userx.IdEmpresa         = dataReader.GetGuid(dataReader.GetOrdinal("IdEmpresa"));
                        ViewBag.IdUser          = Userx.Id;
                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;
            return View();

        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ I-N-D-E-X  A-P-L-I-C-A-R  A  E-M-P-L-E-O +++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult VerAplicaciones(int Id, string Oportunidad)
        {
            var id      = _userManager.GetUserId(User);
            var mail    = _userManager.GetUserName(User);

            //---------VER ---------------------------------------------------------
            List<CandidatosList_> UserxList = new List<CandidatosList_>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select empleoAdds.*,empleo.*,AspNetUsers.* from empleoAdds join empleo ON empleo.Job = empleoAdds.Job And empleo.Id = empleoAdds.IdJob And empleo.IdUser = '{id}' join AspNetUsers On AspNetUsers.Id = empleoAdds.IdUser And AspNetUsers.TypeUser = 255485 where empleoAdds.status = 1";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        CandidatosList_ Userx   = new CandidatosList_();
                        Userx.IdJob             = Convert.ToInt64(dataReader["IdJob"]);
                        Userx.IdUser            = dataReader.GetGuid(dataReader.GetOrdinal("IdUser"));
                        Userx.Job               = dataReader.GetGuid(dataReader.GetOrdinal("Job"));
                        Userx.FirstName         = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName          = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Titulotrabajo     = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        Userx.Tracking          = Convert.ToString(dataReader["Tracking"]).ToUpper();
                        Userx.IdEmpresa         = dataReader.GetGuid(dataReader.GetOrdinal("IdEmpresa"));
                        ViewBag.IdUser          = Userx.Id;
                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;
            return View();

        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ A-P-L-I-C-A-R  A  E-M-P-L-E-O YA EXISTE+++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        public IActionResult Existe(int reft, string returnUrl, int npage)
        {
            if (reft == null || reft == 0)
            {
                TempData["_reft"] = 0;
            }
            else if (reft == 1)
            {
                TempData["_reft"] = 1;
            }
            else { TempData["_reft"] = 0; }


            int _npage = 0;
            _npage = npage;
            if (_npage == null || _npage == 0)
            { _npage = 1; }
            else if (_npage > 0) { _npage = _npage; }
            else { _npage = 1; }
            ViewBag.npage = _npage;


            return View();
        }

        [HttpGet]
        public IActionResult Aplicaste(int reft, string returnUrl, int npage)
        {
            int referencia = 0;
            referencia = reft;
            if (referencia == null || referencia == 0)
            {
                ViewBag._reft = 0; 
            }
            else if (referencia == 1)
            {
                ViewBag._reft = 1;
            }
            else { ViewBag._reft = 0; }

            //page
            int _npage = 0;
            _npage = npage;
            if (_npage == null || _npage == 0)
            { _npage = 1; }
            else if (_npage > 0) { _npage = _npage; }
            else { _npage = 1; }

            ViewBag.npage = _npage;

            return View();
        }



        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ A-P-L-I-C-A-R  A  E-M-P-L-E-O ++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult Aplicar(int Id, string Oportunidad, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            string returnUrlnew = returnUrl;

            ViewBag.nnId = Id;
            ViewBag.Oportunidad = Oportunidad;
            var id          = _userManager.GetUserId(User);
            var mail        = _userManager.GetUserName(User);
            ViewBag.userId      = id;
            ViewBag.mail        = mail;
            DateTime dateadd    = DateTime.Now;

            ViewBag.Existe = 0;
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
                        ApplicationUser2 Userx  = new ApplicationUser2();
                        Userx.Id                = Convert.ToString(dataReader["Id"]);
                        Userx.FirstName         = Convert.ToString(dataReader["FirstName"]).ToUpper();
                        Userx.LastName          = Convert.ToString(dataReader["LastName"]).ToUpper();
                        Userx.Email             = Convert.ToString(dataReader["Email"]);
                        Userx.TypeUser          = Convert.ToInt32(dataReader["TypeUser"]);
                        ViewBag.IdUser          = Userx.Id;
                        ViewBag.Existe          = 1;
                        UserxList.Add(Userx);
                    }
                }
            }
            if(ViewBag.Existe == 0) {
                returnUrl = returnUrlnew;
                _logger.LogInformation("User no existe.");
                return LocalRedirect(returnUrl);
            }

            //0000000000000000000000000000 VER sI YA APLICO 0000000000000000000000000000000000000000
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
            //0000000000000000000000000000000 FIN DE VER SI YA SE APLICO 00000000000000000000000000000

            ViewBag.UserxList = UserxList; //============= username

            //****************************************
            if (ViewBag.Existe == 1)
            {
                return RedirectToAction("Existe", "Aplicaciones");
            }
            //****************************************
            if (ViewBag.nnId == 0 || ViewBag.nnId == null)
            {
                return RedirectToAction("Empleos", "Home");
            }

                        //#################################### insert aplicar #############################################
                        Boolean status_ = true;
                        string tracking_ = "nuevo";
                        //string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            string sql = $"Insert Into empleoAdds (IdJob,Job,IdUser,status,dateadd,Tracking,TrackingAdd) Values ({ViewBag.nnId},'{ViewBag.Oportunidad}','{ViewBag.IdUser}','{status_}','{dateadd}','{tracking_}','{dateadd}')";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.CommandType = CommandType.Text;

                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                            return RedirectToAction("VerAplicaciones", "Home");
                        }
                        //#################################### insert aplicar #############################################
        } 


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ CV DE CANDIDATOS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        public IActionResult CurriculumDetalle(Guid fgdgdfgttfdrd, int Id, Guid Oportunidad, Guid hdwdewttweggew)
        {
            var id          = fgdgdfgttfdrd; 
            var mail        = _userManager.GetUserName(User);
            ViewBag.userId  = id;
            ViewBag.mail    = mail;

            ViewBag.abcId   = Id;
            ViewBag.abcOp   = Oportunidad;
            Guid hdwdewttweggew2 = hdwdewttweggew;

            List<ApplicationUser2> UserxList = new List<ApplicationUser2>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM AspNetUsers WHERE TypeUser = 255485 and Id = '{id}'";
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
                        ViewBag.gId             = Userx.Id;
                        ViewBag.eMail           = Userx.Email;
                        ViewBag.elCandidatoX    = Userx.FirstName + " " + Userx.LastName;
                        UserxList.Add(Userx);
                    }
                }
            }
            //---------OBTENER DATOS ADICIONALES DEL CANDIDATO---------------------------------------------------------
            List<UserInfo> CandidatoList = new List<UserInfo>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM userInfos WHERE IdUser = '{ViewBag.gId}'"; 
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        UserInfo candidato = new UserInfo();
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
            //------------------------------------------------------------------EXPERIENCIAS DEL CANDIDATO---------------------------------------------------------
            List<Experiencias> lasexperienciasList = new List<Experiencias>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = $"SELECT experiencias.Id,experiencias.Empresa,experiencias.Posicion,experiencias.FuncionesRol,experiencias.Aportes,experiencias.desde,experiencias.hasta FROM experiencias INNER JOIN AspNetUsers ON AspNetUsers.TypeUser = 255485 and AspNetUsers.Id = '{ViewBag.gId}' and experiencias.IdUser = '{ViewBag.gId}' and experiencias.email = '{ViewBag.eMail}'"; /*SELECT * FROM experiencias WHERE TypeUser = 255485 and IdUser = '{id}' and email = '{mail}'*/
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

            //-------------------------------------------------------------OBTENER EDUCACION DEL CANDIDATO---------------------------------------------------------
            List<Educacion> EducacionList = new List<Educacion>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM educacion WHERE IdUser = '{ViewBag.gId}' and email = '{ViewBag.eMail}'"; 
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
            //--------------------------------------------------OBTENER REFERENCIAS---------------------------------------------------------
            List<Referencias> ReferenciaList = new List<Referencias>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM referencias WHERE IdUser = '{ViewBag.gId}' and email = '{ViewBag.eMail}'"; /*{Dato_}*/
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Referencias reference   = new Referencias();
                        reference.Id            = Convert.ToInt64(dataReader["Id"]);
                        reference.Persona       = Convert.ToString(dataReader["Persona"]).ToUpper();
                        reference.PhoneNumber   = Convert.ToString(dataReader["PhoneNumber"]);
                        reference.Email2        = Convert.ToString(dataReader["Email2"]);
                        reference.Empresa       = Convert.ToString(dataReader["Empresa"]).ToUpper();
                        reference.Parentezco    = Convert.ToString(dataReader["Parentezco"]);
                        ReferenciaList.Add(reference);
                    }
                }
            }


            //######################################## REGISTRAR VISITAS ##################################################################
            //#################################### insert aplicar #############################################
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int IdEmppleoAdd_       = ViewBag.abcId;
                string Tracking_        = "CV Visto";
                DateTime TrackingAdd_   = DateTime.Now;
                Guid Job_               = ViewBag.abcOp;
                Guid IdUserCandidato_   = ViewBag.userId;
                Guid IdUserEmpresa_     = hdwdewttweggew2;
                int Vistopor_           = 69784;
                string Msg_             = "";
                string Tracking_title_  = "Curriculum visto";
                int IdReferencia_       = 0;
                Guid From_x             = IdUserEmpresa_;
                Guid To_x               = IdUserCandidato_;
                int StatusTracking_     = 1;
                string elCandidato_ = ViewBag.elCandidatoX;

                string sql = $"Insert Into empleoAddTrackings (IdEmppleoAdd, Tracking, TrackingAdd, Job, IdUserCandidato, IdUserEmpresa, Vistopor, Msg, Tracking_title, IdReferencia, From_, To_, StatusTracking, elCandidato) Values ({IdEmppleoAdd_},'{Tracking_}','{TrackingAdd_}', '{Job_}', '{IdUserCandidato_}', '{IdUserEmpresa_}', {Vistopor_}, '{Msg_}', '{Tracking_title_}',{IdReferencia_}, '{From_x}', '{To_x}', {StatusTracking_}, '{elCandidato_}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            //#############################################################################################################################
            //-----------------------------------------------------------------------------------------------------------------------------
            ViewBag.UserxList = UserxList;
            ViewBag.CandidatoList = CandidatoList;
            ViewBag.lasexperienciasList = lasexperienciasList;
            ViewBag.EducacionList = EducacionList;
            ViewBag.ReferenciaList = ReferenciaList;
            return View();

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ I-N-D-E-X  M E N S A J E S  E-M-P-L-E-O ++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpGet]
        [HttpPost]
        [Authorize]
        public IActionResult Mensajes(int nOferta, string sOferta)
        {
            var id = _userManager.GetUserId(User);

            string buscarx = sOferta;
            string buscars = null;
            Int64 nI = nOferta;
            if (!String.IsNullOrEmpty(buscarx) || nI > 0) { buscars = $"And empleoAddTrackings.IdEmppleoAdd = {nI} and empleoAddTrackings.Job = '{buscarx}'"; }
            else { buscars = ""; }

            List<MSGs> Empleox = new List<MSGs>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT empleoAddTrackings.*,empleo.Titulotrabajo, empleo.publicosino, empleo.status, empresaperfils.EmpresaCentro FROM empleoAddTrackings JOIN empleo ON empleoAddTrackings.IdEmppleoAdd = empleo.Id And empleoAddTrackings.Job = empleo.Job JOIN empresaperfils ON empresaperfils.IdEmpresa = empleo.Idempresa WHERE empleoAddTrackings.To_ = '{id}' {buscars} order by Id DESC";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        MSGs Userx = new MSGs();
                        Userx.Id                        = Convert.ToInt64(dataReader["Id"]);
                        Userx.IdEmppleoAdd              = Convert.ToInt64(dataReader["IdEmppleoAdd"]);
                        Userx.Tracking                  = Convert.ToString(dataReader["Tracking"]).ToUpper();
                        Userx.TrackingAdd               = Convert.ToDateTime(dataReader["TrackingAdd"]);
                        Guid JOBguid                    = Guid.Parse(dataReader["Job"].ToString());
                        Userx.Job                       = JOBguid;
                        Guid IdUserCandidatoguid        = Guid.Parse(dataReader["IdUserCandidato"].ToString()); 
                        Userx.IdUserCandidato           = IdUserCandidatoguid;
                        Guid IdUserEmpresaguid          = Guid.Parse(dataReader["IdUserEmpresa"].ToString());
                        Userx.IdUserEmpresa             = IdUserEmpresaguid;
                        Userx.Vistopor                  = Convert.ToInt64(dataReader["Vistopor"]);
                        Userx.Msg                       = Convert.ToString(dataReader["Msg"]).ToUpper();
                        Userx.Tracking_title            = Convert.ToString(dataReader["Tracking_title"]).ToUpper();
                        Userx.IdReferencia              = Convert.ToInt64(dataReader["IdReferencia"]);
                        Userx.Titulotrabajo             = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        Userx.publicosino               = Convert.ToBoolean(dataReader["publicosino"]);
                        Userx.status                    = Convert.ToBoolean(dataReader["status"]);
                        Userx.EmpresaCentro             = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        Guid From_guid                  = Guid.Parse(dataReader["From_"].ToString());
                        Userx.From_                     = From_guid;
                        Guid To_guid                    = Guid.Parse(dataReader["To_"].ToString());
                        Userx.To_                       = To_guid;
                        Userx.StatusTracking            = Convert.ToInt32(dataReader["StatusTracking"]);
                        Empleox.Add(Userx);
                    }
                }
            }
            ViewBag.MsgList = Empleox;
            return View();
        }


        [HttpGet]
        [HttpPost]
        [Authorize]
        public IActionResult EnviarMensajeU(int Id, Guid Oportunidad, Guid hdwdewttweggew, string titulo, int sdhggwe, string dfgfdgd2244, string sdhtrtggwe)
        {
            var id = _userManager.GetUserId(User);
            ViewBag.abcId = Id;
            ViewBag.abcOp = Oportunidad;

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int IdEmppleoAdd_           = ViewBag.abcId;
                string Tracking_            = "Re:" + sdhtrtggwe;
                DateTime TrackingAdd_       = DateTime.Now;
                Guid Job_                   = ViewBag.abcOp;
                Guid IdUserCandidato_       = new Guid(id);
                Guid IdUserEmpresa_         = hdwdewttweggew;
                int Vistopor_               = 255485;
                string Msg_                 = dfgfdgd2244;
                string newOdd               = Msg_.Replace(',', ' ');
                            string newOdd1  = newOdd.Replace('"', ' ');
                            string newOdd2  = newOdd1.Replace((Char)39, ' ');
                Msg_                        = newOdd2;
                string Tracking_title_      = titulo;
                int IdReferencia_           = sdhggwe;
                Guid From_x                 = IdUserCandidato_;
                Guid To_x                   = IdUserEmpresa_;
                int StatusTracking_         = 1;

                string sql = $"Insert Into empleoAddTrackings (IdEmppleoAdd, Tracking, TrackingAdd, Job, IdUserCandidato, IdUserEmpresa, Vistopor, Msg, Tracking_title, IdReferencia, From_, To_, StatusTracking) Values ({IdEmppleoAdd_},'{Tracking_}','{TrackingAdd_}', '{Job_}', '{IdUserCandidato_}', '{IdUserEmpresa_}', {Vistopor_}, '{Msg_}', '{Tracking_title_}',{IdReferencia_}, '{From_x}', '{To_x}', {StatusTracking_})";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return RedirectToAction("Mensajes", "Aplicaciones");
            }
            return View();
        }

        //=============================================== MENSAJES EMPRESA ===========================================================
        //============================================================================================================================
        //============================================================================================================================
        [HttpGet]
        [HttpPost]
        [Authorize]
        public IActionResult EnviarMensajeE(int Id, Guid Oportunidad, Guid hdwdewttweggew, string titulo, int sdhggwe, string dfgfdgd2244, string sdhtrtggwe, Guid Idfscd)
        {
            var id = _userManager.GetUserId(User);
            ViewBag.abcId = Id;
            ViewBag.abcOp = Oportunidad;

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int IdEmppleoAdd_           = ViewBag.abcId;
                string Tracking_            = "Re:" + sdhtrtggwe;
                DateTime TrackingAdd_       = DateTime.Now;
                Guid Job_                   = ViewBag.abcOp;
                Guid IdUserCandidato_       = Idfscd;
                Guid IdUserEmpresa_         = hdwdewttweggew;
                int Vistopor_               = 69784;
                string Msg_                 = dfgfdgd2244;
                string newOdd               = Msg_.Replace(',', ' ');
                string newOdd1              = newOdd.Replace('"', ' ');
                string newOdd2              = newOdd1.Replace((Char)39, ' ');
                Msg_                        = newOdd2;
                string Tracking_title_      = titulo;
                int IdReferencia_           = sdhggwe;
                Guid From_x                 = IdUserEmpresa_;
                Guid To_x                   = IdUserCandidato_;
                int StatusTracking_ = 1;

                string sql = $"Insert Into empleoAddTrackings (IdEmppleoAdd, Tracking, TrackingAdd, Job, IdUserCandidato, IdUserEmpresa, Vistopor, Msg, Tracking_title, IdReferencia, From_, To_, StatusTracking) Values ({IdEmppleoAdd_},'{Tracking_}','{TrackingAdd_}', '{Job_}', '{IdUserCandidato_}', '{IdUserEmpresa_}', {Vistopor_}, '{Msg_}', '{Tracking_title_}',{IdReferencia_}, '{From_x}', '{To_x}', {StatusTracking_})";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return RedirectToAction("eMensajes", "Aplicaciones");
            }
            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ EMPRESAS -->>>> M E N S A J E S  E-M-P-L-E-O ++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpGet]
        [HttpPost]
        [Authorize]
        public IActionResult eMensajes()
        {
            var id = _userManager.GetUserId(User);
            List<Empresaperfil> empresaList = new List<Empresaperfil>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM empresaperfils WHERE  IdUser = '{id}'";
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Empresaperfil Userx = new Empresaperfil();
                        Guid IdUserEmpresaguid = Guid.Parse(dataReader["IdEmpresa"].ToString());
                        Userx.Idempresa = IdUserEmpresaguid;
                        ViewBag.IdEmpresa = Userx.Idempresa;
                        empresaList.Add(Userx);
                    }
                }
            }
            //########################################################################33---------------------------------------------------------------------------------------------------------------------------------
            List<MSGs> Empleox              = new List<MSGs>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT empleoAddTrackings.*,empleo.Titulotrabajo, empleo.publicosino, empleo.status, empresaperfils.EmpresaCentro FROM empleoAddTrackings JOIN empleo ON empleoAddTrackings.IdEmppleoAdd = empleo.Id AND empleo.IdUser = '{id}' And empleoAddTrackings.Job = empleo.Job JOIN empresaperfils ON empresaperfils.IdUser = empleo.IdUser AND empresaperfils.IdEmpresa = empleo.IdEmpresa  WHERE empleoAddTrackings.To_ = '{ViewBag.IdEmpresa}' order by empleoAddTrackings.Id DESC";

                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        MSGs Userx = new MSGs();
                        Userx.Id                    = Convert.ToInt64(dataReader["Id"]);
                        Userx.IdEmppleoAdd          = Convert.ToInt64(dataReader["IdEmppleoAdd"]);
                        Userx.Tracking              = Convert.ToString(dataReader["Tracking"]).ToUpper();
                        Userx.TrackingAdd           = Convert.ToDateTime(dataReader["TrackingAdd"]);
                        Guid JOBguid                = Guid.Parse(dataReader["Job"].ToString());
                        Userx.Job                   = JOBguid;
                        Guid IdUserCandidatoguid    = Guid.Parse(dataReader["IdUserCandidato"].ToString());
                        Userx.IdUserCandidato       = IdUserCandidatoguid;
                        string usuariocandidato     = IdUserCandidatoguid.ToString();
                        Guid IdUserEmpresaguid      = Guid.Parse(dataReader["IdUserEmpresa"].ToString());
                        Userx.IdUserEmpresa         = IdUserEmpresaguid;
                        Userx.Vistopor              = Convert.ToInt64(dataReader["Vistopor"]);
                        Userx.Msg                   = Convert.ToString(dataReader["Msg"]).ToUpper();
                        Userx.Tracking_title        = Convert.ToString(dataReader["Tracking_title"]).ToUpper();
                        Userx.IdReferencia          = Convert.ToInt64(dataReader["IdReferencia"]);
                        Userx.Titulotrabajo         = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        Userx.publicosino           = Convert.ToBoolean(dataReader["publicosino"]);
                        Userx.status                = Convert.ToBoolean(dataReader["status"]);
                        Userx.EmpresaCentro         = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        Guid From_guid              = Guid.Parse(dataReader["From_"].ToString());
                        Userx.From_                 = From_guid;
                        Guid To_guid                = Guid.Parse(dataReader["To_"].ToString());
                        Userx.To_                   = To_guid;
                        Userx.StatusTracking        = Convert.ToInt32(dataReader["StatusTracking"]);
                        string elcandidatoA = "";
                        int userexiste = 0;
                        if (_context.Users.Any(u => u.Id.Equals(usuariocandidato)))
                        { userexiste = 1; }
                        else { userexiste = 0; }

                        if (userexiste == 1)
                        {
                            ApplicationUser u = _context.Users.Where(s => s.Id.Equals(usuariocandidato)).FirstOrDefault(); //LINQ
                            elcandidatoA = u.FirstName + " " + u.LastName;
                        }
                        Userx.elCandidato = elcandidatoA; // Convert.ToString(dataReader["elCandidato"]).ToUpper();
                        Empleox.Add(Userx);
                    }
                }
            }
            ViewBag.MsgList = Empleox;
            return View();
        }

        //########################################################################33---------------------------------------------------------------------------------------------------------------------------------
        public IActionResult EmailSend(int id, string Oportunidad, string title, int RefPage)
        {
            ViewBag.id_abc = id;
            ViewBag.id2_abc = Oportunidad;
            ViewBag.titulo = title;
            ViewBag.RefPage = RefPage;
            return View();
        }

        public async Task<IActionResult> EmailSend2(EmailSendDto email, string from_, string to_, string asunto_, string mensaje_, int Id, string Oportunidad, string returnUrl, int RefPage, string titulo)
        {
            ViewBag.id_abc = Id;
            ViewBag.id2_abc = Oportunidad;

            int nPagina = 1;
            nPagina = RefPage;
            if (nPagina == null || nPagina == 0 || nPagina < 0)
            { nPagina = 1; }
            else if (nPagina > 0)
            { nPagina = nPagina; }
            else { nPagina = 1; }
            ViewBag.nPagina = nPagina;

            returnUrl = returnUrl ?? Url.Content("~/");
            string returnUrlnew     = returnUrl;
            string TituloEmpleo     = titulo;
            string TextoEnCompartir = "Oportunidades de trabajo en EmpleoMax";
            string urlShare         = "https://www.empleomax.com/Home/Empleos?buscar" + @TituloEmpleo;
            //====================================================================================

            from_ = from_;
            to_ = to_;
            asunto_ = asunto_;
            mensaje_ = mensaje_;
            ViewBag.titulo = titulo;
            if(!String.IsNullOrEmpty(from_) || !String.IsNullOrEmpty(to_)) { 
            var lafechaes = DateTime.Now;
                //create the mail message 
                MailMessage mail = new MailMessage();

                //set the addresses 
                mail.From = new MailAddress("informacion@empleomax.com");
                mail.To.Add(to_);

                //set the content 
                mail.Subject = $"{asunto_}";
                mail.IsBodyHtml = true;
                string htmlBody = "";

                htmlBody = "" +
                    "<p style = 'line - height: 150 %; margin - top: 0; margin - bottom: 0' > Hola! </ p >" +
                    $"<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>{from_} ha compartido contigo esta oportuidad laboral</p>" +
                    $"<BR>{mensaje_}<br>" +
                    "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>Haz clic y visita ya esta oportunidad!</p>" +
                    $"<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'><a href='{urlShare}'>Clic aqui en este link</a></p>" +
                    "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>&nbsp;</p>" +
                    "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>Cordialmente.</p>" +
                    "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>&nbsp;</p>" +
                    "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>El equipo de Empleos Max.</p>" +
                    "<p style='line-height: 150%; margin-top: 0; margin-bottom: 0'>informacion@empleomax.com</p>" +
                    $"{lafechaes}";

                mail.Body = htmlBody;
                //send the message 
                SmtpClient smtp = new SmtpClient("mail.empleomax.com");
                NetworkCredential Credentials = new NetworkCredential("informacion@empleomax.com", "123456Em@@");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = Credentials;
                smtp.Port = 8889;    
                smtp.EnableSsl = false;
                smtp.Send(mail);
                string lblMessage = "Mail Sent";
                return Redirect("~/Home/Empleos/?page=" + RefPage + "&c=" + "dice" + "&refe=" + Oportunidad);

                //===================================================================================
            }
            else {
                return Redirect("~/Home/Empleos/?page=" + RefPage + "&c=" + 2 + "&refe=" + Oportunidad+"&ns=1");
            }

        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ CV DE CANDIDATOS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        public IActionResult CurriculumVitae(Guid hdwdewttweggew)//Guid fgdgdfgttfdrd, int Id, Guid Oportunidad, 
        {
            int userexiste = 0;
            if (_context.Users.Any(u => u.Id.Equals(hdwdewttweggew.ToString())))
            { userexiste = 1; }
            else { userexiste = 0; }

            string id = hdwdewttweggew.ToString("D");
            string mail = "";
            int existeuser = 0;
            if(userexiste == 1) { 
                    ApplicationUser x = _context.Users.Where(s => s.Id == id).First();
                     mail       =    x.UserName;
                    existeuser  = 1;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            if(existeuser == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.userId = id;
            ViewBag.mail = mail;

            //---------OBTENER DATOS DEL CANDIDATO ---------------------------------------------------------
            List<ApplicationUser2> UserxList = new List<ApplicationUser2>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM AspNetUsers WHERE TypeUser = 255485 and Id = '{id}'";
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
                        ViewBag.gId             = Userx.Id;
                        ViewBag.eMail           = Userx.Email;
                        ViewBag.elCandidatoX    = Userx.FirstName + " " + Userx.LastName;
                        UserxList.Add(Userx);
                    }
                }
            }

            //---------OBTENER DATOS ADICIONALES DEL CANDIDATO---------------------------------------------------------
            List<UserInfo> CandidatoList = new List<UserInfo>();
            //string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"SELECT * FROM userInfos WHERE IdUser = '{id}'"; /*{Dato_}*/
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

            //------------------------------------------------------------------EXPERIENCIAS DEL CANDIDATO---------------------------------------------------------
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

            //-------------------------------------------------------------OBTENER EDUCACION DEL CANDIDATO---------------------------------------------------------
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

            //--------------------------------------------------OBTENER REFERENCIAS---------------------------------------------------------
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

            //#############################################################################################################################
            //-----------------------------------------------------------------------------------------------------------------------------
            ViewBag.UserxList               = UserxList;
            ViewBag.CandidatoList           = CandidatoList;
            ViewBag.lasexperienciasList     = lasexperienciasList;
            ViewBag.EducacionList           = EducacionList;
            ViewBag.ReferenciaList          = ReferenciaList;
            return View();

            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        }
    }
}

