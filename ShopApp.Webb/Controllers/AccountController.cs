using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Webb.EmailServices;
using ShopApp.Webb.Extensions;
using ShopApp.Webb.Identity;
using ShopApp.Webb.Models;

namespace ShopApp.Webb.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model) {

            if (ModelState.IsValid == false) {
                return View(model);
            }

            var user = new ApplicationUser {
                FullName=model.FullName,
                UserName=model.UserName,
                Email=model.Email
            };

            var result = await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded) {
                // generate token
                /*
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var calbackUrl = Url.Action("ConfirmEmail", "Account",new {
                    userId=user.Id,
                    token=code
                });
                // send email

                EmailSender.SendEmail(model.Email, "Hesabınızı Doğrulayınız", $"Lütfen email hesabınızı onaylamak için linke <a href='http://localhost:50369{calbackUrl}'> tıklayınız.</a>");
                
                TempData.Put("message", new ResultMessage() {
                    Title="Hesap Onayı",
                    Message="E-posta adresinize gelen link ile hesabınızı onaylayınız",
                    Css="warning"
                });
                */
                return RedirectToAction("Login","Account");
            }

            ModelState.AddModelError("","şifre bir büyük harf bir de küçük harf içermelidir.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = null) {
            return View(new LoginModel() {
                ReturnUrl=ReturnUrl
            });
        }

        [HttpPost]
        public async  Task<IActionResult> Login(LoginModel model) {

            if (ModelState.IsValid == false) {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) {
                ModelState.AddModelError("","bu kullanıcı sistemde yok");
                return View(model);
            }
            /*
            if(!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı email ile onaylayınız");
                return View(model);
            }
            */
            var result = await _signInManager.PasswordSignInAsync(user.UserName,model.Password,false,false);

            if (result.Succeeded) {
                return Redirect(model.ReturnUrl ?? "~/");
            }

            ModelState.AddModelError("","email veya parola yanlış");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if(userId==null || token == null) {
                TempData.Put("message", new ResultMessage()
                {
                    Title="Hesap Onayı",
                    Message="Hesap onayı için bilgileriniz yanlış",
                    Css="danger"
                });

                return RedirectToAction("Index","Home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null) {

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded) {
                    TempData.Put("message", new ResultMessage()
                    {
                        Title = "Hesap Onayı",
                        Message = "Hesabınız başarıyla onaylanmıştır",
                        Css = "success"
                    });
                    return RedirectToAction("Login","Account");
                }
            }
            TempData.Put("message", new ResultMessage()
            {
                Title = "Hesap Onayı",
                Message = "Hesabınız onaylanamadı",
                Css = "danger"
            });
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email) == true) {

                TempData.Put("message", new ResultMessage()
                {
                    Title = "Şifre Sıfırla",
                    Message = "Bilgileriniz Hatalı",
                    Css = "danger"
                });
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null) {

                TempData.Put("message", new ResultMessage()
                {
                    Title = "Şifre Sıfırla",
                    Message = "E-posta adresi ile bir kullanıcı bulunamadı",
                    Css = "danger"
                });
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = Url.Action("ResetPassword","Account",new {
                token=code
            });

            EmailSender.SendEmail(Email,"Şifrenizi Sıfırlayın",$"Parolanızı yenilemek için linke <a href='http://localhost:50369{callbackUrl}'>tıklayınız</a>");

            TempData.Put("message", new ResultMessage()
            {
                Title = "Şifre Sıfırla",
                Message = "Parola yenilemek için hesabınıza mail gönderildi",
                Css = "warning"
            });

            return RedirectToAction("Login","Account");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token) {

            if(token == null) {
                return RedirectToAction("Index","Home");
            }

            var model = new ResetPasswordModel() {
                Token=token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model) {
            if (ModelState.IsValid == false) {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) {
                return RedirectToAction("Index","Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded) {

                return RedirectToAction("Login","Account");
            }

            return View(model);
        }

        public IActionResult Accessdenied()
        {
            return View();
        }
    }
}