using Exist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<Group> Create(Group group);

        public Task<Group> AddCompany(int groupId, int companyId);

        public Task<Group> GetById(int id);

        public Task<List<Group>> GetAll();

        public Task<Group> Update(int id, Group group);

        public Task DeleteById(int id);

        public Task<Group> DeleteCompany(int groupId, int companyId);
    }
}
