
using Microsoft.EntityFrameworkCore;
using ServiceTracker.Models;

namespace ServiceTracker.Services
{
    public interface IIdentityService
    {
        Task<Employee> GetByKerb(string kerb);
    }

    public class IdentityService : IIdentityService
    {
        private readonly ServiceTrackerContext _context;

        public IdentityService(ServiceTrackerContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByKerb(string kerb)
        {
            var model = await _context.Employees.Where(e => e.KerberosId == kerb).FirstOrDefaultAsync();
            return model;
        }
    }
}