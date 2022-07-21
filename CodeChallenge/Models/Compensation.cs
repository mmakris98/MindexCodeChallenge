using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public String CompensationId { get; set; }
        public Employee Employee { get; set; }
        public float Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
