using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mail;
namespace empleoswebMax.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null )
                {

                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                string nmail        = user.UserName;
                string CandiName    = user.FirstName + " " + user.LastName;
                string urlShare     = $"{HtmlEncoder.Default.Encode(callbackUrl)}";
                string asunto_      = "Cambio de Contraseña www.empleomax.com";
                string to_          = $"{nmail}"; 


                if (nmail != null)
                {
                    MailMessage mail2 = new MailMessage();
                    mail2.From = new MailAddress("informacion@empleomax.com");
                    mail2.To.Add(to_);
                    mail2.Subject = $"{asunto_}";
                    mail2.IsBodyHtml = true;
                    string htmlBody = "";
                    var lafechaes = DateTime.Now;

                    htmlBody = "" +


                $"Hola, {CandiName} <br> A continuación tienes un enlace para cambiar tu email: <a href='{urlShare}'> Click Aquí</a> y serás redirigido a en nuestro portal www.EmpleoMAX.com <br><br>" +
                $"En caso de no reconocer esta solitud de cambio, por favor informenos de inmediato  a los siguientes contáctos." +

                $"<br><br>Saludos,<br><br>" +
                $"Ing.Claudio D. Medina M. M.B.A. <br>" +
                $"Director General <br>" +
                $"www.ProcesOptimo.com│www.empleoMAX.com <br>" +
                $"Cel. 809 - 913 - 5288 │ 829 - 876 - 2285 <br>" +
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

                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

            }

            return Page();
        }
    }
}
