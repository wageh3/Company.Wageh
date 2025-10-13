using AutoMapper;
using Company.PL.Dto;
using Company.Wageh.BLL.Interfaces;
using Company.Wageh.DAL.Model;
using Company.Wageh.PL.Dto;
using Company.Wageh.PL.Helpers;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Wageh.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository; //momken A3mel comment 3lshan hstakhdem l Inject fy l view
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository ,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employee;
            if (SearchInput == null)
            {
                 employee = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employee = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

                return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var deps = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Departments"] = deps;
            return View();
        }

        public async Task<IActionResult> Create(CreateEmpDto dto)
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
                if (dto.Image is not null)
                {
                    dto.ImageName = DocumentSettings.UploadFile(dto.Image, "Images");
                }
                var employee = _mapper.Map<Employee>(dto);
                 await _unitOfWork.EmployeeRepository.AddAsync(employee);
                int Count = await _unitOfWork.CompleteAsync();
                if (Count > 0)
                {
                    TempData["Message"] = "Employee is Added Successfully !";
                    return RedirectToAction("Index");
                }
            }
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            var deps = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Departments"] = deps;
            if (id is null)
                return BadRequest("Invalid Id");
            Employee? emp = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (emp is null)
                return NotFound(new { StatusCode = 404, message = $"Department with id {id} is not Found!" });
            return View(emp);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var deps = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["Departments"] = deps;
            if (id is null) return BadRequest("Invalid id");
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
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
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmpDto Emp)
        {

            if (ModelState.IsValid)
            {
                if(Emp.ImageName is not null && Emp.Image is not null) 
                {
                    DocumentSettings.DeleteFile(Emp.ImageName, "Images");
                }
                if (Emp.Image is not null) 
                {
                    Emp.ImageName = DocumentSettings.UploadFile(Emp.Image,"Images");
                }
                //if (id != Emp.Id) return BadRequest();
                //var employee = new Employee()
                //{
                //    Id = id,
                //    Name = Emp.Name,
                //    Age = Emp.Age,
                //    Address = Emp.Address,
                //    CreateAt = Emp.CreateAt,
                //    HiringDate = Emp.HiringDate,
                //    Email = Emp.Email,
                //    IsActive = Emp.IsActive,
                //    IsDeleted = Emp.IsDeleted,
                //    DepartmentId= Emp.DepartmentId,
                //    Phone = Emp.Phone,
                //    Salary = Emp.Salary,
                //};
                var employee = _mapper.Map<Employee>(Emp);
                employee.Id = id; // set manually because it was ignored

                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(Emp);
        }

        public IActionResult Delete(Employee model)
        {
            Employee? Emp = _unitOfWork.EmployeeRepository.Get(model.Id);
            if (Emp is null) return BadRequest(); 
            _unitOfWork.EmployeeRepository.Delete(Emp);
            _unitOfWork.CompleteAsync();
            if (!string.IsNullOrEmpty(Emp.ImageName))
                DocumentSettings.DeleteFile(Emp.ImageName, "Images");
            return RedirectToAction("Index");
        }
    }
}
