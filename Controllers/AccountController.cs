using System.Drawing;
using System.Runtime.InteropServices;
using ClinicAppointmentSystemApp.Models;
using ClinicAppointmentSystemApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAppointmentSystemApp.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager , SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // Get the logged-in user
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        // Store the UserId as string in session storage
                        HttpContext.Session.SetString("UserId", user.Id);

                        // Get the roles of the user
                        var roles = await _userManager.GetRolesAsync(user);

                        // Store the first role in session storage
                        var userRole = roles.FirstOrDefault();
                        HttpContext.Session.SetString("UserRole", userRole ?? "Patient");

                        // Store user information in session
                        HttpContext.Session.SetString("UserFullName", $"{user.FirstName} {user.LastName}");

                        // Redirect based on the user's role
                        if (userRole == "Admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (userRole == "Doctor")
                        {
                            return RedirectToAction("GetDoctors", "Doctor");
                        }
                        else
                        {
                            return RedirectToAction("GetPatentAppointment", "Appointment");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User not found.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return View(model);
                }
            }

            return View(model);
        }
        #region login old

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Email or password is incorrect.");
        //            return View(model);
        //        }
        //    }

        //    return View(model);
        //}
        #endregion

        [HttpGet]
        public IActionResult Register()
        {   
            return View();
        }

        #region register old
        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        User users = new User
        //        {
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            Email = model.Email,    
        //            UserName = model.Email
        //        };

        //        var result = await _userManager.CreateAsync(users, model.Password);

        //        if (result.Succeeded) 
        //        {
        //            return RedirectToAction("Login" , "Account");
        //            // first action name and then controller name 
        //        }
        //        else
        //        {
        //            foreach(var error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error.Description);
        //            }

        //            return View(model);
        //        }

        //    }

        //    return View(model);
        //}

        #endregion

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User users = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Email
                };

                var result = await _userManager.CreateAsync(users, model.Password);

                if (result.Succeeded)
                {
                    // Assign the "Patient" role to the newly registered user
                    var roleResult = await _userManager.AddToRoleAsync(users, "Patient");

                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        // If role assignment fails, delete the user and show an error
                        await _userManager.DeleteAsync(users);
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                
                if(user == null)
                {
                    ModelState.AddModelError("","User with this email not exist");
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }

            }

            return View(model);
        }
        public IActionResult ChangePassword(string username) 
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail" ,"Account");
            }

            return View(new ChangePasswordViewModel { Email=username});
        }

        [HttpPost]

        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    // but we are not using this becuase we have to give the chance to set new password 
                    //var result = await _userManager.ChangePasswordAsync(model.Email , model.CurerntPassword , model.NewPassword )

                    var result = await _userManager.RemovePasswordAsync(user);

                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user , model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach(var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }

            }
            else
            {
                ModelState.AddModelError("","Someting went wrong , Try again later ..");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
           
            HttpContext.Session.Clear();

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index" , "Home");
        }
    }
}
