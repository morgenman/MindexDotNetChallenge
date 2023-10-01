using System;
using System.Net;
using System.Net.Http;
using System.Text;
using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateEmployee_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Complaints",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newEmployee = response.DeserializeContent<Employee>();
            Assert.IsNotNull(newEmployee.EmployeeId);
            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
            Assert.AreEqual(employee.Department, newEmployee.Department);
            Assert.AreEqual(employee.Position, newEmployee.Position);
        }

        [TestMethod]
        public void GetEmployeeById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var employee = response.DeserializeContent<Employee>();
            Assert.AreEqual(expectedFirstName, employee.FirstName);
            Assert.AreEqual(expectedLastName, employee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_Ok()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Department = "Engineering",
                FirstName = "Pete",
                LastName = "Best",
                Position = "Developer VI",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var putRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var putResponse = putRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
            var newEmployee = putResponse.DeserializeContent<Employee>();

            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_NotFound()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "Invalid_Id",
                Department = "Music",
                FirstName = "Sunny",
                LastName = "Bono",
                Position = "Singer/Song Writer",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }



        [TestMethod]
        public void GetReportingStructureById_Returns_Subordinate_Count()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedDirectReports = 4;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedDirectReports, reportingStructure.NumberOfReports);
        }

        [TestMethod]
        public void GetReportingStructureById_Returns_NotFound()
        {
            // Arrange
            var employeeId = "NotAnEmployeeId";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetReportingStructureById_Returns_Zero()
        {
            // Arrange
            var employeeId = "62c1084e-6e34-4630-93fd-9153afb65309";
            var expectedDirectReports = 0;

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(expectedDirectReports, reportingStructure.NumberOfReports);
        }

        // Getting compensation for an employee without compensation info set should return not found.
        [TestMethod]
        public void GetCompensationById_Returns_Not_Found()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        // Getting compensation for a non-employee.
        [TestMethod]
        public void GetCompensationById_For_Invalid_Employee_Returns_Not_Found()
        {
            // Arrange
            var employeeId = "NotAnEmployeeId";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddCompensation_Returns_Not_Found()
        {
            // Arrange
            var employeeId = "NotAnEmployeeId";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void AddCompensation_Returns_Compensation()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var salary = 100000;
            var getEmployee = _httpClient.GetAsync($"api/employee/{employeeId}");
            var responseEmployee = getEmployee.Result;
            Assert.AreEqual(HttpStatusCode.OK, responseEmployee.StatusCode); // This is not what we are testing, but this needs to be true for rest of test to work
            var employee = responseEmployee.DeserializeContent<Employee>();

            var compensation = new Compensation()
            {
                Employee = employee,
                Salary = salary,
                EffectiveDate = DateTime.Now,
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(salary, newCompensation.Salary);
            Assert.AreEqual(employeeId, newCompensation.Id);
            Assert.AreEqual(employeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(compensation.EffectiveDate.Date, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public void AddCompensation_Without_Date_Assigns_EffectiveDate()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var salary = 100000;
            var getEmployee = _httpClient.GetAsync($"api/employee/{employeeId}");
            var responseEmployee = getEmployee.Result;
            Assert.AreEqual(HttpStatusCode.OK, responseEmployee.StatusCode); // This is not what we are testing, but this needs to be true for rest of test to work
            var employee = responseEmployee.DeserializeContent<Employee>();

            var compensation = new Compensation()
            {
                Employee = employee,
                Salary = salary,
            };
            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(salary, newCompensation.Salary);
            Assert.AreEqual(employeeId, newCompensation.Id);
            Assert.AreEqual(employeeId, newCompensation.Employee.EmployeeId);
            Assert.AreEqual(DateTime.Now.Date, newCompensation.EffectiveDate);
        }

        // In order to prevent an existing salary from being assigned to a replacement employee, I have chosen to delete the compensation when an employee is updated.
        // This test ensures that compensation is removed after employee mutation.
        [TestMethod]
        public void MutateEmployee_Deletes_Compensation()
        {
            // Arrange (get employee, create compensation, mutate employee)
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var salary = 100000;
            var getEmployee = _httpClient.GetAsync($"api/employee/{employeeId}");
            var responseEmployee = getEmployee.Result;
            Assert.AreEqual(HttpStatusCode.OK, responseEmployee.StatusCode); // This is not what we are testing, but this needs to be true for rest of test to work
            var employee = responseEmployee.DeserializeContent<Employee>();

            var compensation = new Compensation()
            {
                Employee = employee,
                Salary = salary,
                EffectiveDate = DateTime.Now,
            };

            // The following block is necessary to populate the compensation repository
            var requestContentCompensation = new JsonSerialization().ToJson(compensation);
            var postCompensation = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContentCompensation, Encoding.UTF8, "application/json"));
            var responseCompensation = postCompensation.Result;
            Assert.AreEqual(HttpStatusCode.Created, responseCompensation.StatusCode); // This is not what we are testing, but this needs to be true for rest of test to work

            // MutateEmployee
            employee.FirstName = "NewFirstName";
            // Mutation isn't strictly necessary, put operations automatically remove & readd regaurdless. Note that doing so removes subordinates, see EmployeeRepository.cs:25
            var requestContent = new JsonSerialization().ToJson(employee);
            var putRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var putResponse = putRequestTask.Result;
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode); // This is not what we are testing, but this needs to be true for rest of test to work

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
