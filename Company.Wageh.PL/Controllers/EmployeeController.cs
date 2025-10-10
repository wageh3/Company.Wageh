using AutoMapper;
using Company.PL.Dto;
using Company.Wageh.BLL.Interfaces;
using Company.Wageh.DAL.Model;
using Company.Wageh.PL.Dto;
using Humanizer;
using Microsoft.AspNetCore.Mvc;

namespace Company.Wageh.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeRepository employeeRepository ,
            IDepartmentRepository departmentRepository,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employee;
            if (SearchInput == null)
            {
                 employee = _employeeRepository.GetAll();
            }
            else
            {
                employee = _employeeRepository.GetByName(SearchInput);
            }

                return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var deps = _departmentRepository.GetAll();
            ViewData["Departments"]=deps;
            return View();
        }

        public IActionResult Create(CreateEmpDto dto)
        {
            if (ModelState.IsValid)
            {
                //Manual Mapping
                //var employee = new Employee()
                //{
                //    Name = dto.Name,
                //    Age = dto.Age,
                //    Email = dto.Email,
                //    Address = dto.Address,
                //    Phone = dto.Phone,
                //    Salary = dto.Salary,
                //    IsDeleted = dto.IsDeleted,
                //    IsActive = dto.IsActive,
                //    DepartmentId = dto.DepartmentId,
                //    HiringDate = dto.HiringDate,
                //    CreateAt = dto.CreateAt
                //};

                //Auto Mapping
                var employee = _mapper.Map<Employee>(dto);
                int Count = _employeeRepository.Add(employee);
                if (Count > 0)
                {
                    TempData["Message"] = "Employee is Added Successfully !";
                    return RedirectToAction("Index");
                }
            }
            return View(dto);
        }

        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            var deps = _departmentRepository.GetAll();
            ViewData["Departments"] = deps;
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
            var deps = _departmentRepository.GetAll();
            ViewData["Departments"] = deps;
            if (id is null) return BadRequest("Invalid id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null)
                return NotFound(new { StatusCode = 404, message = $"Employee with id {id} is not Found!" });
            //var employeeDto = new CreateEmpDto()
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Address = employee.Address,
            //    CreateAt = employee.CreateAt,
            //    HiringDate = employee.HiringDate,
            //    Email = employee.Email,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    DepartmentId = employee.DepartmentId,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //};
            var employeeDto = _mapper.Map<CreateEmpDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // btmna3 ayy Request gay mn Tool khargya (msmo7 bs b l Requests l btegy mn l web)
        public IActionResult Edit([FromRoute] int id, CreateEmpDto Emp)
        {

            if (ModelState.IsValid)
            {
                //if (id != Emp.Id) return BadRequest();
                var employee = new Employee()
                {
                    Id = id,
                    Name = Emp.Name,
                    Age = Emp.Age,
                    Address = Emp.Address,
                    CreateAt = Emp.CreateAt,
                    HiringDate = Emp.HiringDate,
                    Email = Emp.Email,
                    IsActive = Emp.IsActive,
                    IsDeleted = Emp.IsDeleted,
                    DepartmentId= Emp.DepartmentId,
                    Phone = Emp.Phone,
                    Salary = Emp.Salary,
                };
                var count = _employeeRepository.Update(employee);

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
