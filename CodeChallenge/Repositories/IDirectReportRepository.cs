using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IDirectReportRepository
    {
        List<DirectReport> GetById(String id);
        DirectReport Add(DirectReport directReport);
        //DirectReport Remove(DirectReport directReport);
        Task SaveAsync();
    }
}