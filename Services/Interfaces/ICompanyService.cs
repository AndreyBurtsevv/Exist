using Exist.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exist.Services.Interfaces
{
    public interface ICompanyService
    {
        public Task<Company> Create(Company company);

        public Task<Company> AddGroup(int companyId, int groupId);

        public Task<Company> GetById(int id);

        public Task<List<Company>> GetAll();

        public Task<Company> Update(int id, Company company);

        public Task DeleteById(int id);

        public Task<Company> DeleteGroup(int companyId, int groupId);
    }
}
