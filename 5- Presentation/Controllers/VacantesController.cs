using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace empleoswebMax.Controllers
{
    public class VacantesController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public VacantesController(IConfiguration configuration, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger         = logger;
            _userManager    = userManager;
            Configuration   = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        [HttpPost]
        [HttpGet]
        [Authorize]
        public IActionResult VerOferta(int nOferta, string sOferta)
        {
            string buscarx  = sOferta; 
            int xnumId      = nOferta; 
            List<Ofertas> todoempleoList    = new List<Ofertas>();
            string connectionString         = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql2 = $"Select empleo.*, empresaperfils.* from empleo join empresaperfils on empresaperfils.Id = empleo.Idnumempresa and empresaperfils.Idempresa = empleo.Idempresa WHERE empleo.Id = {xnumId}  And empleo.Job = '{buscarx}'"; /*{Dato_}*/
                SqlCommand command = new SqlCommand(sql2, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Ofertas todoempleo              = new Ofertas();
                        todoempleo.Id                   = Convert.ToInt64(dataReader["Id"]);
                        todoempleo.Job                  = dataReader.GetGuid(dataReader.GetOrdinal("Job"));
                        todoempleo.EmpresaCentro        = Convert.ToString(dataReader["EmpresaCentro"]).ToUpper();
                        todoempleo.PhoneNumber          = Convert.ToString(dataReader["PhoneNumber"]);
                        todoempleo.Email2               = Convert.ToString(dataReader["Email2"]);
                        todoempleo.RNC                  = Convert.ToString(dataReader["RNC"]);
                        todoempleo.Ciudadtrabajo        = Convert.ToString(dataReader["Ciudadtrabajo"]).ToUpper();
                        todoempleo.Titulotrabajo        = Convert.ToString(dataReader["Titulotrabajo"]).ToUpper();
                        todoempleo.dateadd              = Convert.ToDateTime(dataReader["dateadd"]);
                        todoempleo.TipoContrato         = Convert.ToInt16(dataReader["TipoContrato"]);
                        todoempleo.jornadahrs           = Convert.ToString(dataReader["jornadahrs"]);
                        todoempleo.diaslaborables       = Convert.ToString(dataReader["diaslaborables"]);
                        todoempleo.Descripciontrabajo   = Convert.ToString(dataReader["Descripciontrabajo"]);
                        var str                         = todoempleo.Descripciontrabajo;
                        var newstr                      = str.Replace("font-family", "");
                        var newstr2                     = newstr.Replace("color", "");
                        var newstr3                     = newstr2.Replace("font-size", "");
                        var newstr4                     = newstr3.Replace("background", "");
                        todoempleo.Descripciontrabajo   = newstr4;
                        todoempleo.Requisitostrabajo    = Convert.ToString(dataReader["Requisitostrabajo"]);
                        var _str2                       = todoempleo.Requisitostrabajo;
                        var _newstr                     = _str2.Replace("font-family", "");
                        var _newstr2                    = _newstr.Replace("color", "");
                        var _newstr3                    = _newstr2.Replace("font-size", "");
                        var _newstr4                    = _newstr3.Replace("background", "");
                        todoempleo.Requisitostrabajo    = _newstr4;
                        todoempleo.edadminima           = Convert.ToInt16(dataReader["edadminima"]);
                        todoempleo.edadmaxima           = Convert.ToInt16(dataReader["edadmaxima"]);
                        todoempleo.sexo                 = Convert.ToInt16(dataReader["sexo"]);
                        todoempleo.idiomas              = Convert.ToString(dataReader["idiomas"]);
                        todoempleo.Salario              = Convert.ToDouble(dataReader["Salario"]);
                        todoempleo.Salariohasta         = Convert.ToDouble(dataReader["Salariohasta"]);
                        todoempleo.Salariotratar        = Convert.ToBoolean(dataReader["Salariotratar"]);
                        todoempleo.publicosino          = Convert.ToBoolean(dataReader["publicosino"]);
                        todoempleo.Foto                 = Convert.ToString(dataReader["Foto"]);
                        ViewBag.Job                     = todoempleo.Job;
                        todoempleoList.Add(todoempleo);
                    }
                }
            }
            ViewBag.todoempleoList = todoempleoList;
            return View(todoempleoList);
        }

        public IActionResult Html()
        {
            string urltext = "";
            string cadenaConTags = urltext;
            urltext = urltext.Replace(",", "");
            urltext = urltext.Replace("<br>", "-_-");
            urltext = urltext.Replace("<p>", ":::");
            urltext = urltext.Replace("</p>", "~~");
            urltext = urltext.Replace("www.", "http://");

            String pattern;
            pattern = @"(http:\/\/([\w.]+\/?)\S*)";
            Regex re = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            urltext = re.Replace(urltext, "<\"$1\">");
            string cadenaSinTags = Regex.Replace(urltext, "<.*?>", string.Empty);

            string cadenaLimpia = cadenaSinTags.Replace("<", "");
            cadenaLimpia = cadenaLimpia.Replace(">", "");
            cadenaLimpia = cadenaLimpia.ToString().Replace('"', ' ').Trim();

            cadenaLimpia = cadenaLimpia.Replace("-_-", "<br>");
            cadenaLimpia = cadenaLimpia.Replace(":::","<p>");
            cadenaLimpia = cadenaLimpia.Replace("~~","</p>");
            ViewBag.con = cadenaConTags;
            ViewBag.sin = cadenaSinTags;
            ViewBag.sinhtml = cadenaLimpia;
            return View();

        }
    }
}