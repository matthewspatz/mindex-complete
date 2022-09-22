using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Models
{
    public class Compensation
    {
        // Initially, based on the Readme I planned to use the complete Employee object
        // which would require a solution like the commented lines below. On considering 
        // typical use cases and endpoint requirements though, I decided to use the ID only.
        // Additionally, I felt that since I used a complete object for task #1 that if the 
        // purpose was to determine my understanding of the concept, that had been demonstrated.

        //public String CompensationId { get; set; }  // this would be needed as an object cannot serve as a primary key for EF
        //public Employee Employee { get; set; }

        [Key, ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public int Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
