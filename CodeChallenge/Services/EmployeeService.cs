using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompensationRepository _compensationRepository;
        private readonly IDirectReportRepository _directReportRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository, ICompensationRepository compensationRepository, IDirectReportRepository directReportRepository)
        {
            _employeeRepository = employeeRepository;
            _compensationRepository = compensationRepository;
            _directReportRepository = directReportRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                if (_employeeRepository.GetById(employee.EmployeeId) == null)
                    _employeeRepository.Add(employee);
                if (employee.DirectReports != null)
                {
                    foreach (var emp in employee.DirectReports)
                    {
                        var id = emp.EmployeeId;
                        if (_employeeRepository.GetById(id) == null)
                        {
                            var newEmp = _employeeRepository.Add(emp);
                            id = newEmp.EmployeeId;
                        }
                        _directReportRepository.Add(new DirectReport() { EmployeeId = employee.EmployeeId, DirectReportId = id });
                    }
                }      
                _employeeRepository.SaveAsync().Wait();
                _directReportRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                var employee = _employeeRepository.GetById(id);
                if(employee != null)
                    employee.DirectReports = _directReportRepository.GetById(id).Select(x => { return _employeeRepository.GetById(x.DirectReportId); }).ToList();
                return employee;
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        //Reporting Structure
        public ReportingStructure GetReportingStructure(string id)
        {
            var employee = GetById(id);
            var reportingStructure = new ReportingStructure() {Employee = employee ,NumberOfReports = 0};

            if (employee != null)
            {
                reportingStructure.NumberOfReports = GetNumberOfDirectReports(employee.EmployeeId); ;
            }
                
            return reportingStructure;          
        }

        public int GetNumberOfDirectReports(String id)
        {
            var numberOfReports = 0;
            var directReports = _directReportRepository.GetById(id);
            if(directReports != null)
                foreach (DirectReport report in directReports)
                {
                    numberOfReports++;
                    numberOfReports += GetNumberOfDirectReports(report.DirectReportId);
                }
            return numberOfReports;
        }
        
        //Compensation
        public Compensation CreateCompensation(Compensation compensation)
        {
            if (compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
                _employeeRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Compensation GetCompensationById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var compensation = _compensationRepository.GetById(id);
                compensation.Employee = _employeeRepository.GetById(id);
                return compensation;
            }

            return null;
        }
    }
}
