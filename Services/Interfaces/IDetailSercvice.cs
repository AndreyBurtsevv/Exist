using Exist.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exist.Services.Interfaces
{
    public interface IDetailSercvice
    {
        public Task<Detail> Create(Detail detail);

        public Task<Detail> GetById(int id);

        public Task<List<Detail>> GetAll();

        public Task<Detail> Update(int id, Detail detail);

        public Task DeleteById(int id);
    }
}
