using Company.Wageh.BLL.Interfaces;
using Company.Wageh.DAL.Data.Contexts;
using Company.Wageh.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Wageh.BLL.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyDBContext _Context;
        public DepartmentRepository(CompanyDBContext context) 
        {
            _Context = context;
        }
         public IEnumerable<Employee> GetAll()
                {
                    throw new NotImplementedException();
                }
        public int Add(Department model)
        {
            _Context.Departments.Add(model);
            return _Context.SaveChanges();
        }

        public int Delete(Department model)
        {
            _Context.Departments.Remove(model);
            return _Context.SaveChanges();
        }

        public Department? Get(int id)
        {
            return _Context.Departments.Find(id);
        }

        public IEnumerable<Department> GetAll()
        {
            return _Context.Departments.ToList();
        }

        public int Update(Department model)
        {
           var dept = _Context.Departments.Find(model.Id);
            if (dept == null) 
            {
                return 0;
            }
            dept.Name = model.Name;
            dept.Code = model.Code;
            dept.CreateAt = model.CreateAt;

            return _Context.SaveChanges();

        }
    }
}
