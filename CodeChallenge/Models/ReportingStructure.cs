using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }

        public ReportingStructure(Employee e) {
            Employee = e;
        }

        public int NumberOfReports => GetNumberOfReports(Employee);

        private int GetNumberOfReports(Employee employee)
        {
            if ((employee?.DirectReports?.Count ?? 0) > 0)
            {
                var count = 0;
                foreach (Employee e in employee.DirectReports)
                {
                    count += GetNumberOfReports(e);
                }
                return count + employee.DirectReports.Count;
            }
            return 0;
        }
    }
}
