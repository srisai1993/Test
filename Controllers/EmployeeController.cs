﻿using DotNet5Crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet5Crud.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly CompanyDBContext _context;

        public EmployeeController(CompanyDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        //AddOrEdit Get Method
        public async Task<IActionResult> AddOrEdit(int? employeeId)
        {
            
            ViewBag.PageName = employeeId == null ? "Create Employee" : "Edit Employee";
            ViewBag.IsEdit = employeeId == null ? false : true;
            if (employeeId == null)
            {
                return View();
            }
            else
            {
                var employee = await _context.Employees.FindAsync(employeeId);

                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }        
        }

        //AddOrEdit Post Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int employeeId, [Bind("EmployeeId,Name,Designation,Address,Salary,JoiningDate")]
        Employee employeeData)
        {
            bool IsEmployeeExist = false;

            Employee employee = await _context.Employees.FindAsync(employeeId);

            if (employee != null)
            {
                IsEmployeeExist = true;
            }
            else
            {
                employee = new Employee();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employee.Name = employeeData.Name;
                    employee.Designation = employeeData.Designation;
                    employee.Address = employeeData.Address;
                    employee.Salary = employeeData.Salary;
                    employee.JoiningDate = employeeData.JoiningDate;

                   if(IsEmployeeExist)
                    {
                        _context.Update(employee);
                    }
                    else
                    {
                        _context.Add(employee);
                    }                   
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeData);
        }

        // Employee Details
        public async Task<IActionResult> Details(int? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // GET: Employees/Delete/1
        public async Task<IActionResult> Delete(int? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
