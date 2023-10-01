using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models
{
    // Compensation model stores the salary per employee and the effective date of the salary.
    public class Compensation
    {
        // This represents the EmployeeId and should always be in sync with the attached Employee object. 
        [Key]
        public String EmployeeId { get; set; }

        // Readme instructed to use employee as a field, rather than employeeId.
        public Employee Employee { get; set; }

        public double Salary { get; set; }

        // Backing Field
        private DateTime _EffectiveDate;

        // I chose to ignore the specific time for this as the name of the field is EffectiveDate. 
        public DateTime EffectiveDate
        {
            get => _EffectiveDate.Date;
            set => _EffectiveDate = value.Date;
        }
    }
}
