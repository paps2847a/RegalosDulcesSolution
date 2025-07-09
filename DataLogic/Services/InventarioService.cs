using DataLogic.Repository;
using DataLogic.Services.Base;
using DataModel.Models;
using DataModel.Tables;
using DataPersistance;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataLogic.Services
{
    public interface IInventarioService : IBaseContract<Inventario>
    {
        Task<bool> AddRange(IEnumerable<Inventario> it);
        Task<bool> UpdRange(IEnumerable<Inventario> it);
        Task<bool> DeleteRange(IEnumerable<Inventario> it);
        Task<Inventario?> GetById(int id);
        Task<int> Count(Expression<Func<Inventario, bool>>? predicado = null);
        Task<bool> UpdNoTraking(Expression<Func<Inventario, bool>> pred, Expression<Func<SetPropertyCalls<Inventario>, SetPropertyCalls<Inventario>>> toChange);
        Task<bool> DeleteNoTracking(Expression<Func<Inventario, bool>> pred);
    }

    public class InventarioService : IInventarioService
    {
        private readonly InventarioRepository _svc;
        public InventarioService(DataContext data)
        {
            _svc = new InventarioRepository(data);
        }

        public async Task<Inventario> Add(Inventario it) => await _svc.AddAsync(it);

        public async Task<bool> AddRange(IEnumerable<Inventario> it) => await _svc.AddRangeAsync(it) > 0;

        public async Task<Inventario> Upd(Inventario it) => await _svc.UpdateAsync(it);

        public async Task<bool> UpdRange(IEnumerable<Inventario> it) => await _svc.UpdateRangeAsync(it);

        public async Task<bool> UpdNoTraking(Expression<Func<Inventario, bool>> pred, Expression<Func<SetPropertyCalls<Inventario>, SetPropertyCalls<Inventario>>> toChange)
        {
            return await _svc.UpdateAsyncNoTracker(pred, toChange);
        }

        public async Task<bool> Del(Inventario it) => await _svc.DeleteAsync(it);

        public async Task<bool> DeleteRange(IEnumerable<Inventario> it) => await _svc.DeleteRangeAsync(it);

        public async Task<bool> DeleteNoTracking(Expression<Func<Inventario, bool>> pred)
        {
            return await _svc.DeleteAsyncNoTracker(pred);
        }

        public async Task<Inventario?> GetById(int id) => await _svc.FirsOrDefaultAsync(x => x.IdCat == id);

        public async Task<int> Count(Expression<Func<Inventario, bool>>? predicado = null)
        {
            if (predicado is null)
                await _svc.CountAsync();

            return await _svc.CountAsync(predicado!);
        }

        public async Task<IEnumerable<Inventario>> GetAll(Filter filter)
        {
            if(filter.parameters.Count == 0)
                return await _svc.GetAllAsync();

            string _op = filter.parameters.ContainsKey("op") ? filter.parameters["op"].ToString() : "-";
            string _val = filter.parameters.ContainsKey("data") ? filter.parameters["val"].ToString() : "-";

            return Enumerable.Empty<Inventario>();
        }

    }
}
