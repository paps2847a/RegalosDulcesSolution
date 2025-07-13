using DataLogic.Repository;
using DataLogic.Services.Base;
using DataModel.Models;
using DataModel.Tables;
using DataPersistance;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataLogic.Services
{
    public interface ITamanoService : IBaseContract<Tamano>
    {
        Task<bool> AddRange(IEnumerable<Tamano> it);
        Task<bool> UpdRange(IEnumerable<Tamano> it);
        Task<bool> DeleteRange(IEnumerable<Tamano> it);
        Task<Tamano?> GetById(int id);
        Task<int> Count(Expression<Func<Tamano, bool>>? predicado = null);
        Task<bool> UpdNoTraking(Expression<Func<Tamano, bool>> pred, Expression<Func<SetPropertyCalls<Tamano>, SetPropertyCalls<Tamano>>> toChange);
        Task<bool> DeleteNoTracking(Expression<Func<Tamano, bool>> pred);
    }

    public class TamanoService : ITamanoService
    {
        private readonly TamanoRepository _svc;
        public TamanoService(DataContext data)
        {
            _svc = new TamanoRepository(data);
        }

        public async Task<Tamano> Add(Tamano it) => await _svc.AddAsync(it);

        public async Task<bool> AddRange(IEnumerable<Tamano> it) => await _svc.AddRangeAsync(it) > 0;

        public async Task<Tamano> Upd(Tamano it) => await _svc.UpdateAsync(it);

        public async Task<bool> UpdRange(IEnumerable<Tamano> it) => await _svc.UpdateRangeAsync(it);

        public async Task<bool> UpdNoTraking(Expression<Func<Tamano, bool>> pred, Expression<Func<SetPropertyCalls<Tamano>, SetPropertyCalls<Tamano>>> toChange)
        {
            return await _svc.UpdateAsyncNoTracker(pred, toChange);
        }

        public async Task<bool> Del(Tamano it) => await _svc.DeleteAsync(it);

        public async Task<bool> DeleteRange(IEnumerable<Tamano> it) => await _svc.DeleteRangeAsync(it);

        public async Task<bool> DeleteNoTracking(Expression<Func<Tamano, bool>> pred)
        {
            return await _svc.DeleteAsyncNoTracker(pred);
        }

        public async Task<Tamano?> GetById(int id) => await _svc.FirsOrDefaultAsync(x => x.IdTam == id);

        public async Task<int> Count(Expression<Func<Tamano, bool>>? predicado = null)
        {
            if (predicado is null)
                return await _svc.CountAsync();

            return await _svc.CountAsync(predicado!);
        }

        public async Task<IEnumerable<Tamano>> GetAll(Filter filter)
        {
            if(filter.parameters.Count == 0)
                return await _svc.GetAllAsync();

            string _op = filter.parameters.ContainsKey("op") ? filter.parameters["op"].ToString() : "-";
            string _val = filter.parameters.ContainsKey("data") ? filter.parameters["val"].ToString() : "-";

            return Enumerable.Empty<Tamano>();
        }

    }
}
