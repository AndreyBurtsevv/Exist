using Exist.DB;
using Exist.Models;
using Exist.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly MyDbContext _dbContext;

        public CompanyService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Company> AddGroup(int companyId, int groupId)
        {
            var company = _dbContext.Company.Include(x => x.Country).Include(x => x.Groups).First(x => x.Id == companyId);
            var group = _dbContext.Group.First(x => x.Id == groupId);

            company.Groups.Add(group);

            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task<Company> Create(Company company)
        {
            company.Country = _dbContext.Country.First(x => x.Id == company.CountryId);
            await _dbContext.Company.AddAsync(company);
            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task DeleteById(int id)
        {
            var model = _dbContext.Company.First(x => x.Id == id);

            _dbContext.Company.Remove(model);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Company> DeleteGroup(int companyId, int groupId)
        {
            var company = _dbContext.Company.Include(x => x.Country).Include(x => x.Groups).First(x => x.Id == companyId);
            var group = _dbContext.Group.First(x => x.Id == groupId);

            company.Groups.Remove(group);

            await _dbContext.SaveChangesAsync();

            return company;
        }

        public async Task<List<Company>> GetAll()
        {
            return _dbContext.Company.Include(x => x.Country).Include(x => x.Groups).ToList();
        }

        public async Task<Company> GetById(int id)
        {
            return _dbContext.Company.Include(x => x.Country).Include(x => x.Groups).First(x => x.Id == id);
        }

        public async Task<Company> Update(int id, Company company)
        {
            var model = _dbContext.Company.Include(x => x.Country).Include(x => x.Groups).First(x => x.Id == id);

            if (!string.IsNullOrEmpty(company.Name))
            {
                model.Name = company.Name;
            }

            if (!string.IsNullOrEmpty(company.Description))
            {
                model.Description = company.Description;
            }

            if (company.CountryId != 0)
            {
                var country = _dbContext.Country.First(x => x.Id == company.CountryId);

                model.Country = country;
                model.CountryId = country.Id;
            }

            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
