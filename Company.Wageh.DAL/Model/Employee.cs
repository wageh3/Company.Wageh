﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Wageh.DAL.Model
{
    public class Employee : BaseEntity
    {
        
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public decimal Salary { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime HiringDate { get; set; }
        public DateTime CreateAt { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        public string? ImageName { get; set; } 

    }
}
