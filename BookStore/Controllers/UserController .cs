using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Business.Handlers;
using BookStore.Business.Interface;
using BookStore.Business.ViewModels;
using BookStore.Business;
using BookStore.Business.Filters;

namespace BookStore.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserBusiness userBuisness;
        public UserController(IUserBusiness userBuisness)
        {
            this.userBuisness = userBuisness;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                int status = await userBuisness.ValidateUser(loginModel);
                HttpContext.Session.SetString("userID", status.ToString());
                if (status>0)
                {
                    HttpContext.Session.SetString("userEmail", loginModel.Email);
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    ViewBag.Error = "Invalid Username or Password..";
                }
            }

            // If model validation fails, return to the login form with error messages
            return View(loginModel);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser( UserModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var status = await userBuisness.UserRegistration(user);
                    return Json(new { IsValid = true, message = "Registration successful!" });
                }
                catch (AppException ex)
                {
                    if (ex.ErrorCode == AppErrorCode.DuplicateName || ex.ErrorCode == AppErrorCode.DuplicateCode)
                    {
                        return Json(new { IsValid = false, message = ex.Message });
                    }
                    else
                    {
                        return Json(new { IsValid = false, message = "Error: " + ex.Message });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { IsValid = false, message = "Error saving data. " + ex.Message });
                }
            }
            else
            {
                // Model validation failed
                return Json(new { IsValid = false, message = "Please enter all Details Correctly" });
            }
        }

        [Route("Logout")]        /// 
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
    }
}
