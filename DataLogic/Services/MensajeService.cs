using DataLogic.Repository;
using DataLogic.Services.Base;
using DataModel.Models;
using DataModel.Tables;
using DataPersistance;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataLogic.Services
{
    public interface IMensajeService : IBaseContract<Mensaje>
    {
        Task<bool> AddRange(IEnumerable<Mensaje> it);
        Task<bool> UpdRange(IEnumerable<Mensaje> it);
        Task<bool> DeleteRange(IEnumerable<Mensaje> it);
        Task<Mensaje?> GetById(int id);
        Task<int> Count(Expression<Func<Mensaje, bool>>? predicado = null);
        Task<bool> UpdNoTracking(Expression<Func<Mensaje, bool>> pred, Expression<Func<SetPropertyCalls<Mensaje>, SetPropertyCalls<Mensaje>>> toChange);
        Task<bool> DeleteNoTracking(Expression<Func<Mensaje, bool>> pred);
    }

    public class MensajeService : IMensajeService
    {
        private readonly MensajeRepository _svc;
        public MensajeService(DataContext data)
        {
            _svc = new MensajeRepository(data);
        }

        public async Task<Mensaje> Add(Mensaje it) => await _svc.AddAsync(it);

        public async Task<bool> AddRange(IEnumerable<Mensaje> it) => await _svc.AddRangeAsync(it) > 0;

        public async Task<Mensaje> Upd(Mensaje it) => await _svc.UpdateAsync(it);

        public async Task<bool> UpdRange(IEnumerable<Mensaje> it) => await _svc.UpdateRangeAsync(it);

        public async Task<bool> UpdNoTracking(Expression<Func<Mensaje, bool>> pred, Expression<Func<SetPropertyCalls<Mensaje>, SetPropertyCalls<Mensaje>>> toChange)
        {
            return await _svc.UpdateAsyncNoTracker(pred, toChange);
        }

        public async Task<bool> Del(Mensaje it) => await _svc.DeleteAsync(it);

        public async Task<bool> DeleteRange(IEnumerable<Mensaje> it) => await _svc.DeleteRangeAsync(it);

        public async Task<bool> DeleteNoTracking(Expression<Func<Mensaje, bool>> pred)
        {
            return await _svc.DeleteAsyncNoTracker(pred);
        }

        public async Task<Mensaje?> GetById(int id) => await _svc.FirsOrDefaultAsync(x => x.IdMsg == id);

        public async Task<int> Count(Expression<Func<Mensaje, bool>>? predicado = null)
        {
            if (predicado is null)
                return await _svc.CountAsync();

            return await _svc.CountAsync(predicado!);
        }

        public async Task<IEnumerable<Mensaje>> GetAll(Filter filter)
        {
            if (filter.parameters.Count == 0)
                return await _svc.GetAllAsync();

            string _op = filter.parameters.ContainsKey("op") ? filter.parameters["op"].ToString() : "-";
            string _val = filter.parameters.ContainsKey("data") ? filter.parameters["val"].ToString() : "-";

            return Enumerable.Empty<Mensaje>();
        }

    }
}
