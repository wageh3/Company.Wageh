﻿using Company.Wageh.BLL.Interfaces;
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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDBContext _context;

        public GenericRepository(CompanyDBContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll()
        {
            if(typeof(T)==typeof(Employee))
            {
                return (IEnumerable<T>) _context.Employees.Include(E=>E.Department).ToList();
            }
            return _context.Set<T>().ToList();
        }

        public T? Get(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Add(T model)
        {
            _context.Set<T>().Add(model);
        }

        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }

    }
}
