using Company.PL.Dto;
using Company.Wageh.BLL.Interfaces;
using Company.Wageh.DAL.Model;
using Company.Wageh.PL.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Company.Wageh.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            var employee = _employeeRepository.GetAll();
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Create(CreateEmpDto dto)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    Email = dto.Email,
                    Address = dto.Address,
                    Phone = dto.Phone,
                    Salary = dto.Salary,
                    IsDeleted = dto.IsDeleted,
                    IsActive = dto.IsActive,
                    HiringDate = dto.HiringDate,
                    CreateAt = dto.CreateAt
                };
                int Count = _employeeRepository.Add(employee);
                if (Count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(dto);
        }

        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest("Invalid Id");
            Employee? emp = _employeeRepository.Get(id.Value);
            if (emp is null)
                return NotFound(new { StatusCode = 404, message = $"Department with id {id} is not Found!" });
            return View(emp);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // btmna3 ayy Request gay mn Tool khargya (msmo7 bs b l Requests l btegy mn l web)
        public IActionResult Edit([FromRoute] int id, Employee Emp)
        {

            if (ModelState.IsValid)
            {
                if (id != Emp.Id) return BadRequest();

                var count = _employeeRepository.Update(Emp);

                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(Emp);
        }

        public IActionResult Delete(Employee model)
        {
            Employee? Emp = _employeeRepository.Get(model.Id);
            if (Emp is null) return BadRequest();
            _employeeRepository.Delete(Emp);
            return RedirectToAction("Index");
        }
    }
}
