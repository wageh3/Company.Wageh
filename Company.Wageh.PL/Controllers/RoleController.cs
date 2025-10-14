using Company.Wageh.DAL.Model;
using Company.Wageh.PL.Dto;
using Company.Wageh.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Wageh.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
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
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                if(role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name
                    };

                    var result = await _roleManager.CreateAsync(role);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError("", "This Role Is Already Exist !!");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

            if (id is null)
                return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(new { StatusCode = 404, message = $"User with id {id} is not Found!" });
            var dto = new RoleToReturn()
            {
                Id = id,
                Name = role.Name
            };
            return View(ViewName, dto);
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
    }
}
