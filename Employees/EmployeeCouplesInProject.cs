using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employees
{
    public class EmployeeCouplesInProject
    {
        public int FirstEmpId { get; set; }
        public int SecondEmpId { get; set; }
        public int ProjectId { get; set; }
        public TimeSpan TimeInProject { get; set; }
    }
}
