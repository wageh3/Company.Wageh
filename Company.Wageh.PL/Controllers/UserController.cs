using Company.Wageh.DAL.Model;
using Company.Wageh.PL.Dto;
using Company.Wageh.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Wageh.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;
            if (SearchInput == null)
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U=>U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            
            if (id is null)
                return BadRequest("Invalid Id");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound(new { StatusCode = 404, message = $"User with id {id} is not Found!" });
            var dto = new UserToReturnDto()
            {
                Id = id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManager.GetRolesAsync(user).Result
            };
            return View(ViewName,dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // btmna3 ayy Request gay mn Tool khargya (msmo7 bs b l Requests l btegy mn l web)
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturnDto model)
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation !");
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    return NotFound(new { StatusCode = 404, message = $"User with id {id} is not Found!" });
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

               var result = await _userManager.UpdateAsync(user);
               if(result.Succeeded)
               {
                    return RedirectToAction("Index");
               }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute]string id ,UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation !");
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    return NotFound(new { StatusCode = 404, message = $"User with id {id} is not Found!" });
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}
