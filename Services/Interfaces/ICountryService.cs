using Exist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Services.Interfaces
{
    public interface ICountryService
    {
        public Task<Country> Create(Country country);

        public Task<Country> GetById(int id);

        public Task<List<Country>> GetAll();

        public Task<Country> Update(int id, Country country);

        public Task DeleteById(int id);
    }
}
