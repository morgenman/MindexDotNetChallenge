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
    // CompensationRepository stores Compensation records in the employee context
    public class CompensationRespository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRespository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        // Add a compensation record to the database
        public Compensation Add(Compensation compensation)
        {
            // Since the readme was ambiguous regarding whether employee parameter was an object or an id, I have chosen to support both.
            if (compensation.Employee == null && compensation.EmployeeId != null)
                compensation.Employee = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == compensation.EmployeeId);
            else if (compensation.Employee != null && compensation.EmployeeId == null)
                compensation.EmployeeId = compensation.Employee.EmployeeId;
            else if (compensation.Employee == null && compensation.EmployeeId == null)
                return null;

            // If the effective date is not set, set it to today.
            if (compensation.EffectiveDate == DateTime.MinValue) compensation.EffectiveDate = DateTime.Now.Date;

            // Only return compensation if the employee exists in the database.
            if (_employeeContext.Employees.Contains(compensation.Employee)) _employeeContext.Compensations.Add(compensation);
            else return null;

            return compensation;
        }

        // Get a compensation record by employee id
        public Compensation GetById(string id)
        {
            return _employeeContext.Compensations.SingleOrDefault(e => e.EmployeeId == id);
        }

        // Persist changes to the database
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
