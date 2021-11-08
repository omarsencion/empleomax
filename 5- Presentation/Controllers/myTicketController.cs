using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;



using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
namespace empleoswebMax.Controllers
{
    public class myTicketController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<AplicacionesController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;



        public myTicketController(SignInManager<ApplicationUser> signInManager, IConfiguration configuration, ILogger<AplicacionesController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger         = logger;
            Configuration   = configuration;
            _userManager    = userManager;
            _signInManager  = signInManager;
            _context        = context;

        }

        [HttpPost]
        [HttpGet]
        [Authorize]      
        public IActionResult Index()
        {

            //+++++++++++++++++++++++++++++++++++++++ VER TICKETS +++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
            var id                      = _userManager.GetUserId(User);
            string UserTo_              = "";
            List<TicketDto> UserxList   = new List<TicketDto>();
            string connectionString     = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                string sql2 = $"Select * From Tickets where From_ = '{id}' ORDER BY Id DESC";
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
                        string To_x         = Userx.To_.ToString();
                                    if (To_x != null)
                                    {
                                        var d = _context.Users.Where(s => s.Id.Equals(To_x)).First();
                                        UserTo_ = d.FirstName + " " + d.LastName;
                                    }
                        Userx.NameTo_       = UserTo_;
                        Userx.FechaTicket   = Convert.ToDateTime(dataReader["FechaTicket"]);
                        ViewBag.IdUser      = Userx.Id;
                        UserxList.Add(Userx);
                    }
                }
            }
            ViewBag.UserxList = UserxList;
            return View();

        }

        public IActionResult TicketPost()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TicketPost(string Categoria, string Titulo, string Mensaje)
        {
            var id      = _userManager.GetUserId(User);
            Guid id2    = new Guid(id);

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string Categoria_a      = Categoria;
                string Titulo_a         = Titulo;
                DateTime FechaTicket    = DateTime.Now;
                string Mensaje_a        = Mensaje;
                Guid From__             = id2;
                Guid To_                = new Guid("a797849e-f052-4615-afcb-fa3aaa62e384");
                Int16 StatusTicket      = 0;

                        // random string ========================================================================
                        Random rnd      = new Random();
                        int newrnd      = rnd.Next(200000, 13278855);
                        var chars       = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        var stringChars = new char[8];
                        var random      = new Random();
                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }
                        var finalString = new String(stringChars);
                string incrementar = finalString + newrnd;

                string sql = $"Insert Into Tickets (TicketNumber, Categoria, Titulo, Mensaje, From_, To_, StatusTicket, FechaTicket) Values ('{incrementar}', '{Categoria_a}', '{Titulo_a}', '{Mensaje_a}', '{From__}', '{To_}', {StatusTicket}, '{FechaTicket}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return RedirectToAction("Index", "myTicket");
            }

            return View();
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS CATEGORIAS  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//  

        public IActionResult TicketUpdate()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TicketUpdate(string Categoria, string Mensaje)
        {
            var id = _userManager.GetUserId(User);
            Guid id2 = new Guid(id);
            //Guid Tracking_ = new Guid();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string Categoria_a      = Categoria;
                string NewMensaje       = Mensaje;
                DateTime FechaResponse  = DateTime.Now;
                string Mensaje_a        = Mensaje;
                Int16 StatusResponse    = 0;
                int isaut2              = 1;
                int userexiste2         = 0;
                string fromDB           = "";
                string ToDB             = "";
                string paraGuid         = "";
                if (isaut2 == 1)
                {
                    if (_context.Tickets.Any(u => u.TicketNumber.Equals(Categoria_a)))
                    { userexiste2   = 1; }
                    else { userexiste2 = 0; }

                    if (userexiste2 == 1)
                    {
                        Ticket u    = _context.Tickets.Where(s => s.TicketNumber.Equals(Categoria_a)).First(); //LINQ
                        fromDB      = u.From_.ToString();
                        ToDB        = u.To_.ToString();
                    }
                }

                if (id.Equals(fromDB)) { paraGuid = ToDB; }
                else { paraGuid = fromDB; }
                string sql = $"Insert Into TicketResponses (TicketNumber, Respuesta, From_, To_, StatusResponse, FechaResponse) Values ('{Categoria_a}', '{NewMensaje}', '{id}', '{paraGuid}', {StatusResponse}, '{FechaResponse}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    Ticket d = _context.Tickets.Where(s => s.TicketNumber.Equals(Categoria_a)).First();
                    d.StatusTicket = 0;
                    _context.SaveChanges();
                }

                return RedirectToAction("Index", "myTicket");
            }

            return View();
        }



        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//
        //+++++++++++++++++++++++++++++++++++++++ UPDATE STATUS CATEGORIAS  +++++++++++++++++++++++++++++++++++++++*++++++++//
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//  

        public IActionResult TicketUpdateadm()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TicketUpdateadm(string Categoria, string Mensaje)
        {
            var id = _userManager.GetUserId(User);
            Guid id2 = new Guid(id);
            //Guid Tracking_ = new Guid();

            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Categoria_a          = Categoria;
                string NewMensaje           = Mensaje;
                DateTime FechaResponse      = DateTime.Now;
                string Mensaje_a            = Mensaje;
                Int16 StatusResponse        = 0;
                int isaut2                  = 1;
                int userexiste2             = 0;
                string fromDB               = "";
                string ToDB                 = "";
                string paraGuid             = "";
                if (isaut2 == 1)
                {
                    if (_context.Tickets.Any(u => u.TicketNumber.Equals(Categoria_a)))
                    { userexiste2 = 1; }
                    else { userexiste2 = 0; }

                    if (userexiste2 == 1)
                    {
                        Ticket u    = _context.Tickets.Where(s => s.TicketNumber.Equals(Categoria_a)).First(); //LINQ
                        fromDB      = u.From_.ToString();
                        ToDB        = u.To_.ToString();
                    }
                }
                if (id.Equals(fromDB)) { paraGuid = ToDB; }
                else { paraGuid = fromDB; }
                string sql = $"Insert Into TicketResponses (TicketNumber, Respuesta, From_, To_, StatusResponse, FechaResponse) Values ('{Categoria_a}', '{NewMensaje}', '{id}', '{paraGuid}', {StatusResponse}, '{FechaResponse}')";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    Ticket d = _context.Tickets.Where(s => s.TicketNumber.Equals(Categoria_a)).First();
                    d.StatusTicket = 0;
                    _context.SaveChanges();
                }

                return RedirectToAction("Main", "Admin");
            }

            return View();
        }
    }
}