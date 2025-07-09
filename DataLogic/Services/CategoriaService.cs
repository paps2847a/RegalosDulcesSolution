using DataLogic.Repository;
using DataLogic.Services.Base;
using DataModel.Models;
using DataModel.Tables;
using DataPersistance;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataLogic.Services
{
    public interface ICategoriaService : IBaseContract<Categoria>
    {
        Task<bool> AddRange(IEnumerable<Categoria> it);
        Task<bool> UpdRange(IEnumerable<Categoria> it);
        Task<bool> DeleteRange(IEnumerable<Categoria> it);
        Task<Categoria?> GetById(int id);
        Task<int> Count(Expression<Func<Categoria, bool>>? predicado = null);
        Task<bool> UpdNoTraking(Filter filter);
        Task<bool> DeleteNoTracking(Filter pred);
    }

    public class CategoriaService : ICategoriaService
    {
        private readonly CategoriaRepository _svc;
        public CategoriaService(DataContext data)
        {
            _svc = new CategoriaRepository(data);
        }

        public async Task<Categoria> Add(Categoria it) => await _svc.AddAsync(it);

        public async Task<bool> AddRange(IEnumerable<Categoria> it) => await _svc.AddRangeAsync(it) > 0;

        public async Task<Categoria> Upd(Categoria it) => await _svc.UpdateAsync(it);

        public async Task<bool> UpdRange(IEnumerable<Categoria> it) => await _svc.UpdateRangeAsync(it);

        public async Task<bool> UpdNoTraking(Filter filter)
        {
            //HACER COMO GETALL pero con UpdateAsyncNoTracker


            return false;
        }

        public async Task<bool> Del(Categoria it) => await _svc.DeleteAsync(it);

        public async Task<bool> DeleteRange(IEnumerable<Categoria> it) => await _svc.DeleteRangeAsync(it);

        public async Task<bool> DeleteNoTracking(Filter pred)
        {
            //HACER COMO GETALL pero con DeleteAsyncNoTracker

            return false;
        }

        public async Task<Categoria?> GetById(int id) => await _svc.FirsOrDefaultAsync(x => x.IdCat == id);

        public async Task<int> Count(Expression<Func<Categoria, bool>>? predicado = null)
        {
            if (predicado is null)
                return await _svc.CountAsync();

            return await _svc.CountAsync(predicado!);
        }

        public async Task<IEnumerable<Categoria>> GetAll(Filter filter)
        {
            if(filter.parameters.Count == 0)
                return await _svc.GetAllAsync();

            string _op = filter.parameters.ContainsKey("op") ? filter.parameters["op"].ToString() : "-";
            string _val = filter.parameters.ContainsKey("data") ? filter.parameters["val"].ToString() : "-";

            return Enumerable.Empty<Categoria>();
        }

    }
}
