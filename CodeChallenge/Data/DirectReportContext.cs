using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Data
{
    public class DirectReportContext : DbContext
    {
        public DirectReportContext(DbContextOptions<DirectReportContext> options) : base(options)
        {

        }

        public DbSet<DirectReport> DirectReports { get; set; }
    }
}
