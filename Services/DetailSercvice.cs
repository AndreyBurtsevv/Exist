using Exist.DB;
using Exist.Models;
using Exist.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Services
{
    public class DetailSercvice : IDetailSercvice
    {
        private readonly MyDbContext _dbContext;

        public DetailSercvice(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Detail> Create(Detail detail)
        {
            var group = _dbContext.Group.Include(x => x.Details).First(x => x.Id == detail.GroupId);
            var company = _dbContext.Company.Include(x => x.Details).First(x => x.Id == detail.CompanyId);

            _dbContext.Detail.Add(detail);

            group.Details.Add(detail);
            company.Details.Add(detail);

            await _dbContext.SaveChangesAsync();

            return detail;
        }

        public async Task DeleteById(int id)
        {
            var detail = _dbContext.Detail.First(x => x.Id == id);

            _dbContext.Detail.Remove(detail);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Detail>> GetAll()
        {
            return await _dbContext.Detail.Include(x => x.Company).Include(x => x.Group).ToListAsync();
        }

        public async Task<Detail> GetById(int id)
        {
            return _dbContext.Detail.Include(x => x.Company).Include(x => x.Group).First(x => x.Id == id);
        }

        public async Task<Detail> Update(int id, Detail detail)
        {
            var model = _dbContext.Detail.Include(x => x.Company).Include(x => x.Group).First(x => x.Id == id);

            if (string.IsNullOrEmpty(detail.Name))
            {
                model.Name = detail.Name;
            }

            if (string.IsNullOrEmpty(detail.Description))
            {
                model.Description = detail.Description;
            }

            if (detail.Price != 0)
            {
                model.Price = detail.Price;
            }

            if (detail.GroupId != 0)
            {
                model.Group = _dbContext.Group.First(x => x.Id == detail.GroupId);
            }

            if (detail.CompanyId != 0)
            {
                model.Company = _dbContext.Company.First(x => x.Id == detail.CompanyId);
            }

            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
