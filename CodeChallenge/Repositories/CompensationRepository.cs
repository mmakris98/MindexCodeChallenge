﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, IEmployeeRepository employeeRepository, CompensationContext compensationContext)
        {
            _employeeRepository = employeeRepository;
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            //check if employee exists
            var employee = _employeeRepository.GetById(compensation.Employee.EmployeeId); 
            if (employee == null)
            {
                _employeeRepository.Add(compensation.Employee);
            }
            compensation.CompensationId = compensation.Employee.EmployeeId;
            //check if compensation exists
            var comp = GetById(compensation.CompensationId);
            if(comp == null)
            {
                _compensationContext.Compensations.Add(compensation);
            }            
            
            return compensation;
        }

        public Compensation GetById(string id)
        {
            return _compensationContext.Compensations.SingleOrDefault(e => e.CompensationId == id);
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
