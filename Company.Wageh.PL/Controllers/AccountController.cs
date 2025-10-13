using Company.Wageh.DAL.Model;
using Company.Wageh.PL.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.Wageh.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }   
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                   user = await _userManager.FindByNameAsync(model.UserName);
                    if(user is null) 
                    {
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirsName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                   
                }
                ModelState.AddModelError("", "Invalid SignUp!!");
            }
            return View(model);
        }
        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if (ModelState.IsValid) 
            {
               var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null) 
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                       var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemmemberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                ModelState.AddModelError("","Invalid Login !");
            }
            return View();
        }
        #endregion

        #region SignOut

        #endregion
    }
}
