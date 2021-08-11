using Exist.DB;
using Exist.Models;
using Exist.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Services
{
    public class CountryService : ICountryService
    {
        private readonly MyDbContext _dbContext;

        public CountryService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Country> Create(Country country)
        {
            await _dbContext.Country.AddAsync(country);

            await _dbContext.SaveChangesAsync();

            return country;
        }

        public async Task DeleteById(int id)
        {
            var model = _dbContext.Country.First(x => x.Id == id);

            _dbContext.Country.Remove(model);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Country>> GetAll()
        {
            return _dbContext.Country.Include(x => x.Companys).ToList();
        }

        public async Task<Country> GetById(int id)
        {
            return _dbContext.Country.Include(x => x.Companys).First(x => x.Id == id);
        }

        public async Task<Country> Update(int id, Country country)
        {
            var model = _dbContext.Country.Include(x => x.Companys).First(x => x.Id == id);

            if (!string.IsNullOrEmpty(country.Name))
            {
                model.Name = country.Name;
            }

            await _dbContext.SaveChangesAsync();

            return model;
        }
    }
}
