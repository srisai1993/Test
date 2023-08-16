﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DotNet5Crud.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { set;get; }
        public string Address { get; set; } 
        public string Designation { get; set; }
        public decimal Salary { get; set; }
        public DateTime JoiningDate { get; set; }
    }
}
