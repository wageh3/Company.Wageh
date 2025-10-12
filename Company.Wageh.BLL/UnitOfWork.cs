using Company.Wageh.BLL.Interfaces;
using Company.Wageh.BLL.Repositories;
using Company.Wageh.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Wageh.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDBContext _context;

        public IDepartmentRepository DepartmentRepository {  get; }

        public IEmployeeRepository EmployeeRepository { get; }

        public UnitOfWork(CompanyDBContext context)
        {
            _context = context;
            DepartmentRepository = new DepartmentRepository(_context);
            EmployeeRepository = new EmployeeRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
