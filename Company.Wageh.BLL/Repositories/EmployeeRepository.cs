using Company.Wageh.BLL.Interfaces;
using Company.Wageh.DAL.Data.Contexts;
using Company.Wageh.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Wageh.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        private readonly CompanyDBContext _context;

        public EmployeeRepository(CompanyDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetByNameAsync(string name)
        {
            return await _context.Employees.Include(E => E.Department).Where(e => e.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
