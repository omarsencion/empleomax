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


using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace empleoswebMax.Controllers
{
    public class RelationsController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<RelationsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RelationsController(ApplicationDbContext context, IConfiguration configuration, ILogger<RelationsController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            Configuration = configuration;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(string _MailGuest, int am, int se)
        {
            //USUARIO PRINCIPAL
            int _TypeUserPrincipal  = 0;
            int _status             = 0;
            string id               = _userManager.GetUserId(User);
            Guid id2                = new Guid(id);
            var mail                = _userManager.GetUserName(User);
            ApplicationUser d       = _context.Users.Where(s => s.Id == id).First();
            _TypeUserPrincipal      = d.TypeUser;
            // USUARIO INVITADO
            string _IdUserGuest     = "00000000-0000-0000-0000-000000000000";
            string _MailGuest2      = _MailGuest;
            int ValidarMailGuest    = 0;
            int _TypeUserGuest      = 0;
            int _seguidor           = 0;
            int _seguidorStatus     = 0;
            int _amigo              = 0;
            int _amigoStatus        = 0;
            int validarsiamigoexiste = 0;
            int validarsiseguidorexiste = 0;

            if (se == 1) { _seguidor = 2; _seguidorStatus = 1; } else { _seguidor = 0; _seguidorStatus = 0; }
            if (am == 1) { _amigo = 2; _amigoStatus = 1; } else { _amigo = 0; _amigoStatus = 0; }
            if (_seguidor == 0 && _amigo == 0)
            { 
                if (!String.IsNullOrEmpty(_MailGuest2)) { ValidarMailGuest = 1; }
                if (ValidarMailGuest == 1)
                {
                    string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                    if (Regex.IsMatch(_MailGuest2, expression))
                    {
                        if (Regex.Replace(_MailGuest2, expression, string.Empty).Length == 0)
                        { ValidarMailGuest = 2; _status = 1; }
                    }
                    else
                    {
                        ValidarMailGuest = 0;
                        ViewBag.ErrorMail = "Ingrese un Email valido";
                        return View();
                    }
                }

                if (ValidarMailGuest == 2)
                {
                    ApplicationUser f   = _context.Users.Where(i => i.UserName == _MailGuest).First();
                    _IdUserGuest        = f.Id;
                    _MailGuest          = f.UserName;
                    _TypeUserGuest      = f.TypeUser;
                    _status             = 1;

                    var a = _context.friendsall.Where(b => b.IdUserPrincipal == id2 && b.MailGuest == _MailGuest2);
                    foreach (var items in a)
                    {
                        validarsiamigoexiste = items.amigo;
                        validarsiseguidorexiste = items.seguidor;
                    }

                }

                Friends friendsall      = new Friends
                {
                    IdUserPrincipal     = new Guid(id),
                    MailPrincipal       = mail,
                    TypeUserPrincipal   = _TypeUserPrincipal,
                    IdUserGuest         = new Guid(_IdUserGuest),
                    MailGuest           = _MailGuest,
                    TypeUserGuest       = _TypeUserGuest,
                    status              = _status,
                    fechaSolicitud      = DateTime.Now,
                    amigo               = _amigo,
                    amigoStatus         = _amigoStatus,
                    amigoStatusFecha    = DateTime.Now,
                    seguidor            = _seguidor, 
                    seguidorStatus      = 0, 
                    seguidorStatusFecha = DateTime.Now,
                    solicitudEnviada    = 0,  
                    solicitudRecibida   = 0
                };

                if (ModelState.IsValid)
                {
                    _context.Add(friendsall);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Candidatos", "Home");
                }
                else
                {
                    return View(friendsall);
                }
            }
            return View();

        }
    }
}