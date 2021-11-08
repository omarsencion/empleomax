using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace empleoswebMax.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public IConfiguration Configuration { get; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            Configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl) 
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            string returnUrlnew = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    int nNewStatus = 0;
                    int nRealStatus = 0;
                    int _StatusGeneral = 0;
                    var xmail = Input.Email;
                    List<ApplicationUser> lasexperienciasList = new List<ApplicationUser>();
                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = $"SELECT Status,TypeUser,StatusGeneral from AspNetUsers WHERE UserName = '{xmail}'"; 
                        SqlCommand command = new SqlCommand(sql, connection);
                        using (SqlDataReader dataReader = command.ExecuteReader()) 
                        {
                            while (dataReader.Read())
                            {
                                ApplicationUser lasexperiencias = new ApplicationUser();
                                lasexperiencias.TypeUser        = Convert.ToInt32(dataReader["TypeUser"]);
                                lasexperiencias.Status          = Convert.ToInt16(dataReader["Status"]);
                                nNewStatus                      = Convert.ToInt32(dataReader["TypeUser"]);
                                _StatusGeneral                  = Convert.ToInt16(dataReader["StatusGeneral"]);
                                nRealStatus                     = lasexperiencias.Status;
                                lasexperienciasList.Add(lasexperiencias);

                                if(_StatusGeneral > 3)
                                {
                                    await _signInManager.SignOutAsync();
                                    _logger.LogInformation("User logged out.");
                                    return Redirect("~/Identity/Account/Login");
                                }
                            }
                        }
                    }
                    //------------------------------------------------------------------
                    
                    nNewStatus = nNewStatus;
                    if (nNewStatus == 69784 && nRealStatus == 69)
                    {
                        if (returnUrl == "/Home/Aplicar")
                        {
                            returnUrl = "~/Home/Aplicar";
                        }
                        else { returnUrl = "/Home/Empresas"; }
                        returnUrlnew = returnUrl;
                    }
                    else if (nNewStatus == 255485 && nRealStatus == 25)
                    {
                        if (returnUrl == "/Home/Aplicar")
                        {
                            returnUrl = "~/Home/Aplicar";
                        }
                        else { returnUrl = "~/Home/Candidatos"; }
                        returnUrlnew = returnUrl;
                    }
                    else
                    {
                        string url = "~/Aplicaciones/Logout";
                        return Redirect(url);
                    }

                    returnUrl = returnUrlnew;
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            return Page();
        }
    }
}
