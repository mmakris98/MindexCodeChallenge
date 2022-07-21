using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class DirectReportRepository : IDirectReportRepository
    {
        private readonly DirectReportContext _directReportContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public DirectReportRepository(ILogger<IEmployeeRepository> logger, DirectReportContext directReportContext)
        {
            _directReportContext = directReportContext;
            _logger = logger;
        }

        public DirectReport Add(DirectReport directReport)
        {
            _directReportContext.DirectReports.Add(directReport);
            _directReportContext.SaveChangesAsync();
            return directReport;
        }

        public List<DirectReport> GetById(string id)
        {
            //get list of direct reports for an employee
            var directReports = _directReportContext.DirectReports.Where(e => e.EmployeeId == id);
            List<DirectReport> reports = new List<DirectReport>();
            foreach (DirectReport directReport in directReports)
            {
                reports.Add(directReport);
            }
            return reports;
        }

        public Task SaveAsync()
        {
            return _directReportContext.SaveChangesAsync();
        }

        public DirectReport Remove(DirectReport directReport)
        {
            return _directReportContext.Remove(directReport).Entity;
        }
    }
}
