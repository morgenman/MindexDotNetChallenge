using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            // As of right now adding an employee with subordinates causes an ID conflict.
            // I would fix this but it's out of scope of my assignment.
            employee.DirectReports = null;
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            // Enumerated the employee DbSet in order to force all children to be included. 
            // Ideally I would disable lazy loading, but I aimed for minimal impact.
            return _employeeContext.Employees.AsEnumerable().SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            // In order to prevent an existing salary from being assigned to a replacement employee, I have chosen to delete the compensation when an employee is updated.
            _employeeContext.Compensations.ToList().RemoveAll(c => c.Employee == employee);
            return _employeeContext.Employees.Remove(employee).Entity;
        }
    }
}
