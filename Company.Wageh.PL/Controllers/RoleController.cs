using Company.Wageh.DAL.Model;
using Company.Wageh.PL.Dto;
using Company.Wageh.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Wageh.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturn> role;
            if (SearchInput == null)
            {
                role = _roleManager.Roles.Select(R => new RoleToReturn()
                {
                    Id = R.Id,
                    Name = R.Name,
                   
                });
            }
            else
            {
                role = _roleManager.Roles.Select(R => new RoleToReturn()
                {
                    Id = R.Id,
                    Name = R.Name
                }).Where(U => U.Name.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(role);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturn model)
        {
            if(ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                if(role is null)
                {
                    role = new IdentityRole() 
                    {
                        Name = model.Name,
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError("", "This Role Is Already Exists");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

            if (id is null)
                return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if(role is null)
                 return NotFound(new { StatusCode = 404, message = $"Role with id {id} is not Found!" });
            var dto = new RoleToReturn()
            {
                Id = id,
                Name = role.Name
            };
            return View(ViewName,dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // btmna3 ayy Request gay mn Tool khargya (msmo7 bs b l Requests l btegy mn l web)
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturn model)
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation !");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                    return NotFound(new { StatusCode = 404, message = $"Role with id {id} is not Found!" });
                var roleresult = await _roleManager.FindByNameAsync(model.Name);
                if (roleresult is null)
                {
                    role.Name = model.Name;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError("","This Role Is Already Exist !!");
                
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturn model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Operation !");
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                    return NotFound(new { StatusCode = 404, message = $"Role with id {id} is not Found!" });
                var roleresult = await _roleManager.FindByNameAsync(model.Name);
                
                    role.Name = model.Name;

                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                
                ModelState.AddModelError("", "Invalid Operation !!");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleid)
        {
            var role = await _roleManager.FindByIdAsync(roleid);
            if (role is null)
                return NotFound();

            ViewData["RoleId"] = roleid;
            var usersInRole = new List<UserInRoleDto>();
            var users = await _userManager.Users.ToListAsync();
            if (users is not null)
            {
                foreach (var user in users)
                {
                    var userinrole = new UserInRoleDto()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                    };
                    if(await _userManager.IsInRoleAsync(user , role.Name))
                    {
                        userinrole.IsSelected = true;
                    }
                    else
                    {
                        userinrole.IsSelected = false;
                    }
                    usersInRole.Add(userinrole);
                }
            }
            else
            {
                return BadRequest();
            }
            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleid , List<UserInRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync (roleid);
            if(role is null)
                return NotFound();
            if (ModelState.IsValid)
            {
                foreach(var user in users)
                {
                    var appuser = await _userManager.FindByIdAsync(user.UserId);
                    if (appuser is not null) 
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appuser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appuser, role.Name);
                        }
                    }

                }
                return RedirectToAction("Edit",new {id = role.Id});
            }
            return View(users);
        }
    }
}
