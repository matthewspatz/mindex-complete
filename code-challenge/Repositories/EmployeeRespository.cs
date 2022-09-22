using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            // This fix for the DirectReports not loading works fine in this case 
            // but to actually cover any number of levels would need to be written recursively.
            // That poses performance issues potentially in EF though so without going to deep into 
            // the rabbit hole, this is what I implemented here.
            var employee = _employeeContext.Employees
                .Include(e => e.DirectReports)
                .ThenInclude(e => e.DirectReports)
                .ThenInclude(e => e.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == id);

            return employee;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        public Compensation Add(Compensation compensation)
        {
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetByEmployeeId(string id)
        {
            var compensation = _employeeContext.Compensations.SingleOrDefault(e => e.EmployeeId == id);

            return compensation;
        }
    }
}
