using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace empleoswebMax.Controllers
{
    public class MaintenanceController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<MaintenanceController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;



        public MaintenanceController(IConfiguration configuration, ILogger<MaintenanceController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _logger         = logger;
            _userManager    = userManager;
            Configuration   = configuration;
            _context        = context;
        }

        public IActionResult Index()
        {
            return View();
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
            var id              = _userManager.GetUserId(User);
            string un_          = _userManager.GetUserName(User);
            string newpass      = NewPassword;
            string old          = OldPassword;
            int next            = 0;
            var user            = await _userManager.FindByNameAsync(un_);
            var password        = await _userManager.CheckPasswordAsync(user, old);

            if (password) { ViewBag.pwd = "si"; next = 1; } else { ViewBag.pwd = "no"; return RedirectToAction("UpdForm", "Maintenance");}
            if (user == null) { ViewBag.msg     = "nulo"; return RedirectToAction("UpdForm", "Maintenance"); }
            var newPassword                     = _userManager.PasswordHasher.HashPassword(user, newpass);
            user.PasswordHash                   = newPassword;
            //string nns = user.PasswordHash;
            if(next == 1) { 
            var res = await _userManager.UpdateAsync(user);
                if (res.Succeeded) { ViewBag.msg = "cambio ok"; return RedirectToAction("Logout", "Aplicaciones"); }
                else { ViewBag.msg = "no cambiado"; return RedirectToAction("UpdForm", "Maintenance"); }
            }

            return View();
        }

    }
}