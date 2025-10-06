using Company.PL.Dto;
using Company.Wageh.BLL.Interfaces;
using Company.Wageh.DAL.Model;
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
        public IActionResult Details(int id)
        {
            Department dep = _departmentRepo.Get(id);
            return View(dep);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = _departmentRepo.Get(id);

            return View(department);
        }
        [HttpPost]
        public IActionResult Edit(Department dep)
        {

            if (ModelState.IsValid)
            {
                _departmentRepo.Update(dep);
                return RedirectToAction("Index");
            }
            return View(dep);
        }
        
        public IActionResult Delete(int id)
        {
            Department dep = _departmentRepo.Get(id);
            _departmentRepo.Delete(dep);
            return RedirectToAction("Index");
        }
    }
}
