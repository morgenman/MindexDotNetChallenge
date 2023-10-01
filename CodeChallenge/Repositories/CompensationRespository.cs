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
    public class CompensationRespository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRespository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.Id = compensation.Employee.EmployeeId;
            // Only return compensation if the employee exists in the database.
            if(_employeeContext.Employees.Contains(compensation.Employee)) _employeeContext.Compensations.Add(compensation);
            if(compensation.EffectiveDate == DateTime.MinValue) compensation.EffectiveDate = DateTime.Now.Date;
            else return null;
            return compensation;
        }

        public Compensation GetById(string id)
        {
            // Enumerated the employee DbSet in order to force all descendant employees to be included in query. 
            // Ideally I would disable lazy loading, but I didn't see an easy way to do that. 
            return _employeeContext.Compensations.AsEnumerable().SingleOrDefault(e => e.Id == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
