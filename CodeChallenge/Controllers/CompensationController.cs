﻿using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        // Included logger per existing logger pattern in EmployeeController
        public CompensationController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        // Get a the salary of an employee by employee id
        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var compensation = _employeeService.GetCompensationById(id);

            // Return appropriate response on null
            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }

        // Add salary amount to an employee by employee id
        [HttpPost]
        public IActionResult AddCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation add request for '{compensation?.Employee.FirstName} {compensation?.Employee.LastName}'");

            _employeeService.AddCompensation(compensation);

            return CreatedAtRoute("getCompensationById", new { id = compensation.Employee.EmployeeId }, compensation);
        }
    }
}
