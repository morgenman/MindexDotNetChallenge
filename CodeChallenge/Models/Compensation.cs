using Microsoft.EntityFrameworkCore;
using System;

namespace CodeChallenge.Models
{
    [Keyless]
    public class Compensation
    {
        public Employee Employee { get; set; }

        public double Salary { get; set; }

        public DateOnly EffectiveDate { get; set; }
    }
}
