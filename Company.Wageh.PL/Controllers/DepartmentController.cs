using Company.PL.Dto;
using Company.Wageh.BLL.Interfaces;
using Company.Wageh.DAL.Model;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Company.Wageh.PL.Controllers
{
    
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepo;
        public DepartmentController (IDepartmentRepository departmentRepository)
        {
            _departmentRepo = departmentRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var Department = _departmentRepo.GetAll();

            return View(Department);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateDepDto dto)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Code = dto.Code,
                    Name = dto.Name,
                    CreateAt = dto.CreateAt,
                };
                int Count = _departmentRepo.Add(department);
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(dto);
        }

        [HttpGet]
        public IActionResult Details(int? id , string ViewName = "Details")
        {
            if(id is null) 
                return BadRequest("Invalid Id");
            Department? dep = _departmentRepo.Get(id.Value);
            if (dep is null) 
                return NotFound(new { StatusCode = 404, message = $"Department with id {id} is not Found!" });
            return View(dep);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest("Invalid Id");
            var dep = _departmentRepo.Get(id.Value);
            if (dep is null)
                return NotFound(new { StatusCode = 404, message = $"Department with id {id} is not Found!" });
            var department = new CreateDepDto()
            {
                Code = dep.Code,
                Name = dep.Name,
                CreateAt = dep.CreateAt,
            };
          
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // btmna3 ayy Request gay mn Tool khargya (msmo7 bs b l Requests l btegy mn l web)
        public IActionResult Edit([FromRoute]int id ,CreateDepDto dep)
        {
            
            if (ModelState.IsValid)
            {
                //if (id != dep.Id) return BadRequest();
                var department = new Department()
                {
                    Id = id,
                    Code = dep.Code,
                    Name = dep.Name,
                    CreateAt = dep.CreateAt,
                };
               
                var count = _departmentRepo.Update(department);

                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(dep);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Department model)
        {
            Department? dep = _departmentRepo.Get(model.Id);
            if (dep is null) return BadRequest();
            _departmentRepo.Delete(dep);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null)
            //    return BadRequest("Invalid Id");
            //Department? dep = _departmentRepo.Get(id.Value);
        //if (dep is null)
        //    return NotFound(new { StatusCode = 404, message = $"Department with id {id} is not Found!" });
            return Details(id,"Delete");
        }
    }
}
