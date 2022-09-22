using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(String id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Compensation GetByEmployeeId(string id);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}