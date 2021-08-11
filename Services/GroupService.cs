using Exist.DB;
using Exist.Models;
using Exist.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Services
{
    public class GroupService : IGroupService
    {
        private readonly MyDbContext _dbContext;

        public GroupService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Group> Create(Group group)
        {
            _dbContext.Group.Add(group);
            await _dbContext.SaveChangesAsync();
            return group;
        }

        public async Task<Group> AddCompany(int groupId, int companyId)
        {
            var group = _dbContext.Group.Include(x => x.Companys).Include(x => x.Details).First(x => x.Id == groupId);
            var company = _dbContext.Company.First(x => x.Id == companyId);

            group.Companys.Add(company);

            await _dbContext.SaveChangesAsync();

            return group;
        }

        public async Task DeleteById(int id)
        {
            var group = _dbContext.Group.First(x => x.Id == id);
            _dbContext.Group.Remove(group);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Group> DeleteCompany(int groupId, int companyId)
        {
            var group = _dbContext.Group.Include(x => x.Companys).Include(x => x.Details).First(x => x.Id == groupId);
            var company = _dbContext.Company.First(x => x.Id == companyId);

            group.Companys.Remove(company);

            await _dbContext.SaveChangesAsync();

            return group;
        }

        public async Task<List<Group>> GetAll()
        {
            return await _dbContext.Group.Include(x => x.Details).Include(x => x.Companys).ToListAsync();
        }

        public async Task<Group> GetById(int id)
        {
            return _dbContext.Group.Include(x => x.Details).Include(x => x.Companys).First(x => x.Id == id);
        }

        public async Task<Group> Update(int id, Group group)
        {
            var model = _dbContext.Group.Include(x => x.Details).Include(x => x.Companys).First(x => x.Id == id);

            if (!string.IsNullOrEmpty(group.Name))
            {
                model.Name = group.Name;
            }

            if (!string.IsNullOrEmpty(group.Description))
            {
                model.Description = group.Description;
            }

            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
