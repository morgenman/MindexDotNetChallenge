using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public String Id { get; set; }
        public Employee Employee { get; set; }

        public double Salary { get; set; }

        // Backing Field
        private DateTime _EffectiveDate;
        public DateTime EffectiveDate
        {
            get => _EffectiveDate.Date;
            set => _EffectiveDate = value.Date;
        }
    }
}
