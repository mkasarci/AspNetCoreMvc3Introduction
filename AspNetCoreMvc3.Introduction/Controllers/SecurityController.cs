using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMvc3.Introduction.Identity;
using AspNetCoreMvc3.Introduction.Models.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMvc3.Introduction.Controllers
{
    public class SecurityController : Controller
    {
        private UserManager<AppIdentityUser> _userManager;
        SignInManager<AppIdentityUser> _signInManager;

        public SecurityController(SignInManager<AppIdentityUser> signInManager,
            UserManager<AppIdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) //It check the models state by models data anotations.
            {
                return View(loginViewModel);
            }

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Confirm your E-mail please.");
                    return View(loginViewModel);
                }
            }

            var result =
                await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, true);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Students");
            }

            ModelState.AddModelError(string.Empty, "Login Failed!");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            if (registerViewModel.Password != registerViewModel.ConfirmedPassword)
            {
                ModelState.AddModelError(string.Empty, "Register Failed!");
                return View();
            }

            var user = new AppIdentityUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                Age = registerViewModel.Age
            };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                var confirmationCode = _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callBackUrl = Url.Action("ConfirmEmail", "Security",
                    new { UserId = user.Id, Code = confirmationCode.Result });

                //Send Email to the user with this callback URL.

                return RedirectToAction("Index", "Students");
            }

            string errorMessage = string.Empty;
            foreach (var error in result.Errors)
            {
                errorMessage += $"{error.Description}{Environment.NewLine}";
            }


            ModelState.AddModelError(string.Empty, errorMessage);
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string UserId, string Code)
        {
            if (UserId == null || Code == null)
            {
                RedirectToAction("Login");
            }

            var user = await _userManager.FindByIdAsync(UserId);

            if (user is null)
            {
                throw new ApplicationException("Unable to find this user");
            }

            var result = await _userManager.ConfirmEmailAsync(user, Code);

            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }

            ModelState.AddModelError(string.Empty, "Invalid token");
            return View("Login");
        }

        public IActionResult ForgotPassword()
        {
            
            return View();
        }

        [HttpPost]
        [DataType(DataType.EmailAddress)]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "This is not a valid E-Mail address!");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "There is no user with this E-Mail address!");
                return View();
            }

            var passwordResetToken =  await _userManager.GeneratePasswordResetTokenAsync(user);

            var callBackUrl = Url.Action("ResetPassword", "Security",
                new { UserId = user.Id, Code = passwordResetToken });

            //Send email to user this callback URL.
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ResetPassword(string UserId, string Code)
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Code))
            {
                throw new ApplicationException("Invalid Token!");
            }

            AppIdentityUser user = await _userManager.FindByIdAsync(UserId);

            if (user is null)
            {
                throw new ApplicationException("Couldn't find the user.");
            }

            ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel
                {Code = Code, Email = user.Email};

            return View(resetPasswordViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            
            if (resetPasswordViewModel.Password != resetPasswordViewModel.ConfirmedPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords should be same!");
                return View();
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Code, resetPasswordViewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirm");
            }

            ModelState.AddModelError(string.Empty, "Reset password has been failed.");
            return View();
        }

        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
    }
}
