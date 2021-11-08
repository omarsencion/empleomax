using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core.Result;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Domain.Entity;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.UI.Web.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly IPlanServiceService _planService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscriptionController(IPlanServiceService planService, ISubscriptionService subscriptionService, UserManager<ApplicationUser> userManager)
        {
            _planService = planService;
            _subscriptionService = subscriptionService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            ServiceResultList<PlanServiceSubscriptionDto> result = new ServiceResultList<PlanServiceSubscriptionDto>();
            var user = await _userManager.GetUserAsync(User);

            if (user.TypeUser == 255485)
                result = await _planService.GetAll(Category.Candidate);
            
            if (user.TypeUser == 69784)
                result = await _planService.GetAll(Category.Employee);

            
            return View(result.Data);
        }
    }
}
