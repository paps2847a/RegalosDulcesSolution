using DataLogic.Repository;
using DataLogic.Services.Base;
using DataModel.Models;
using DataModel.Tables;
using DataPersistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataLogic.Services
{
    public interface IRecordatorioService : IBaseContract<Recordatorio>
    {
        Task<bool> AddRange(IEnumerable<Recordatorio> it);
        Task<bool> UpdRange(IEnumerable<Recordatorio> it);
        Task<bool> DeleteRange(IEnumerable<Recordatorio> it);
        Task<Recordatorio?> GetById(int id);
        Task<int> Count(Expression<Func<Recordatorio, bool>>? predicado = null);
        Task<bool> UpdNoTracking(Expression<Func<Recordatorio, bool>> pred, Expression<Func<SetPropertyCalls<Recordatorio>, SetPropertyCalls<Recordatorio>>> toChange);
        Task<bool> DeleteNoTracking(Expression<Func<Recordatorio, bool>> pred);
    }

    public class RecordatorioService : IRecordatorioService
    {
        private readonly RecordatorioRepository _svc;
        public RecordatorioService(DataContext data)
        {
            _svc = new RecordatorioRepository(data);
        }

        public async Task<Recordatorio> Add(Recordatorio it) => await _svc.AddAsync(it);

        public async Task<bool> AddRange(IEnumerable<Recordatorio> it) => await _svc.AddRangeAsync(it) > 0;

        public async Task<Recordatorio> Upd(Recordatorio it) => await _svc.UpdateAsync(it);

        public async Task<bool> UpdRange(IEnumerable<Recordatorio> it) => await _svc.UpdateRangeAsync(it);

        public async Task<bool> UpdNoTracking(Expression<Func<Recordatorio, bool>> pred, Expression<Func<SetPropertyCalls<Recordatorio>, SetPropertyCalls<Recordatorio>>> toChange)
        {
            return await _svc.UpdateAsyncNoTracker(pred, toChange);
        }

        public async Task<bool> Del(Recordatorio it) => await _svc.DeleteAsync(it);

        public async Task<bool> DeleteRange(IEnumerable<Recordatorio> it) => await _svc.DeleteRangeAsync(it);

        public async Task<bool> DeleteNoTracking(Expression<Func<Recordatorio, bool>> pred)
        {
            return await _svc.DeleteAsyncNoTracker(pred);
        }

        public async Task<Recordatorio?> GetById(int id) => await _svc.FirsOrDefaultAsync(x => x.IdRecord == id, include: x => x.Include(x => x.Mensaje));

        public async Task<int> Count(Expression<Func<Recordatorio, bool>>? predicado = null)
        {
            if (predicado is null)
                return await _svc.CountAsync();

            return await _svc.CountAsync(predicado!);
        }

        public async Task<IEnumerable<Recordatorio>> GetAll(Filter filter)
        {
            if (filter.parameters.Count == 0)
                return await _svc.GetAllAsync(include: x => x.Include(x => x.Mensaje));

            string _op = filter.parameters.ContainsKey("op") ? filter.parameters["op"].ToString() : "-";
            string _val = filter.parameters.ContainsKey("data") ? filter.parameters["val"].ToString() : "-";

            return Enumerable.Empty<Recordatorio>();
        }

    }
}
