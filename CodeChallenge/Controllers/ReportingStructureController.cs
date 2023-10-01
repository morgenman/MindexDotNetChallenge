using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/reportingstructure")]
    public class ReportingStructureController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        // Included logger per existing logger pattern in EmployeeController
        public ReportingStructureController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        // Get a reporting structure by employee id
        [HttpGet("{id}", Name = "getReportingStructureById")]
        public IActionResult GetReportingStructureById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            // Will return null if id is not in repo
            var employee = _employeeService.GetById(id);

            // Return appropriate response on null
            if (employee == null)
                return NotFound();

            var reportStruct = new ReportingStructure(employee);

            return Ok(reportStruct);
        }
    }
}
