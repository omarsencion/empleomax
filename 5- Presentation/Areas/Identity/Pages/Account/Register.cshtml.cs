using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace empleoswebMax.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public int tipou { get; set; }
        public int nStatus { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public int TypeUser { get; set; }
            [Required(ErrorMessage = "Ingrese sus apellidos")]
            [Display(Name = "Apellidos")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Ingrese sus nombres")]
            [Display(Name = "Nombres")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Sexo")]
            public bool Sexo { get; set; }
            public int Status { get; set; }
            public int tipou { get; set; }

            //=================================
            public DateTime DateAdd { get; set; }
            public int TypeAdd { get; set; }
            public int StatusGeneral { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(int uhcbw87hhbdgtyy, string returnUrl = null)
        {
            tipou = uhcbw87hhbdgtyy;
            ViewData["tipou"] = tipou;
            if (tipou == 69784)
            {
                returnUrl = "/Home/Empresas";
            }
            else if (tipou == 255485)
            {
                returnUrl = "/Home/Candidatos";
            }
            else
            {
                string url = "/Home/Select";
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                int nNewStatus = Input.TypeUser;

                if (nNewStatus == 69784)
                {
                    returnUrl = "/Home/Empresas";
                }
                else if (nNewStatus == 255485)
                {
                    returnUrl = "/Home/Candidatos";
                }
                else
                {
                    string url = "/Home/Select";
                    return Redirect(url);
                }
                string nNewStatus2 = nNewStatus.ToString();
                string xStatus = nNewStatus2.Substring(0, 2);
                nStatus = Convert.ToInt32(xStatus);
                DateTime dDateAdd = DateTime.Now;
                int nTypeAdd = 1;
                int nStatusGeneral = 1;
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, TypeUser = Input.TypeUser, FirstName = Input.FirstName, LastName = Input.LastName, Sexo = Input.Sexo, Status = nStatus, DateAdd = dDateAdd, TypeAdd = nTypeAdd, StatusGeneral = nStatusGeneral };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Code);
                    string ErrorRegistro = error.Code;
                    if (ErrorRegistro == "DuplicateUserName")
                    {
                        ViewData["erroremail"] = "Utilice otro @email";
                        ViewData["tipou"] = nNewStatus;
                    }
                    else
                    {
                        ViewData["erroremail"] = "";
                        ViewData["tipou"] = nNewStatus;
                    }
                    string url = "/Home/Select/?" + "e=" + ErrorRegistro;
                }
            }

            return Page();
        }
    }
}
