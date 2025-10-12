using Company.Wageh.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Wageh.BLL.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        T? Get(int id);
        Task AddAsync(T model);
        void Update(T model);
        void Delete(T model);
    }
}
