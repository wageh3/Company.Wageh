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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(CompanyDBContext context) : base(context) 
        {
            
        }
    }
}
