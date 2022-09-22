using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            // In order for the return to match the schema in the readme, after fixing the Repository method, this
            // mapping code would be needed since without it, full JSON versions of the direct reports are returned instead  
            // of just their IDs. This might be desirable depending on the situation but does not match the spec so I leave 
            // it here commented but return the full type (not doing so breaks a unit test and, again, I wasnt sure how 
            // far down the rabbit hole to go.)

            //var returnObject = new
            //{
            //    EmployeeId = employee.EmployeeId,
            //    FirstName = employee.FirstName,
            //    LastName = employee.LastName,
            //    Position = employee.Position,
            //    Department = employee.Department,
            //    DirectReports = employee.DirectReports.Select(r => r.EmployeeId)
            //};

            //return Ok(returnObject);

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet("numberOfReports/{id}", Name = "getNumberOfReportsById")]
        public IActionResult GetNumberOfReportsById(String id)
        {
            _logger.LogDebug($"Received number of reports get request for '{id}'");

            var reportingStructure = _employeeService.GetReportingStructureById(id);
            if (reportingStructure == null)
                return NotFound();

            // Here I chose not to pare down the response since the readme called for the 
            // "fully filled out ReportingStructure for the specified employeeId".  If that 
            // wasn't the intention, the code above for the GET endpoint could easily be (re)used here
            return Ok(reportingStructure);
        }


        // Although depending on the eventual scope of this example were it a real world case I might 
        // have chosen to create a separate area for compensation (controller/service/repository/context) it seemed 
        // more practical to extend the employee domain instead.  Again in a real world situation, this 
        // would be easy to move in a subsequent iteration if there were more pieces to be added.
        [HttpPost("compensation")]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for EmployeeId '{compensation.EmployeeId}'");

            _employeeService.Create(compensation);

            return CreatedAtRoute("getCompensationById", new { id = compensation.EmployeeId }, compensation);
        }

        [HttpGet("compensation/{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received compensation get request for EmployeeId '{id}'");

            var compensation = _employeeService.GetCompensationByEmployeeId(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
