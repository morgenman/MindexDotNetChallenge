using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    // ReportingStructure model dynamically retrieves the number of reports for a given employee.
    public class ReportingStructure
    {
        public Employee Employee { get; set; }

        public ReportingStructure(Employee e) {
            Employee = e;
        }

        public int NumberOfReports => GetNumberOfReports(Employee);

        // Recursively counts the number of reports for a given employee.
        private int GetNumberOfReports(Employee employee)
        {
            // Null safe check for DirectReports.
            if ((employee?.DirectReports?.Count ?? 0) > 0)
            {
                var count = 0;
                foreach (Employee e in employee.DirectReports) count += GetNumberOfReports(e);
                return count + employee.DirectReports.Count;
            }
            // Base case
            return 0;
        }
    }
}
