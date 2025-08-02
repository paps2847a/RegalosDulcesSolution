using DataLogic.Repository;
using DataLogic.Services.Base;
using DataModel.Models;
using DataModel.Tables;
using DataPersistance;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLogic.Services
{
    public interface IWsGroupService : IBaseContract<WsGroup>
    {
        Task<bool> AddRange(IEnumerable<WsGroup> it);
        Task<bool> UpdRange(IEnumerable<WsGroup> it);
        Task<bool> DeleteRange(IEnumerable<WsGroup> it);
        Task<WsGroup?> GetById(int id);
        Task<int> Count(Expression<Func<WsGroup, bool>>? predicado = null);
        Task<bool> UpdNoTracking(Expression<Func<WsGroup, bool>> pred, Expression<Func<SetPropertyCalls<WsGroup>, SetPropertyCalls<WsGroup>>> toChange);
        Task<bool> DeleteNoTracking(Expression<Func<WsGroup, bool>> pred);
        Task<bool> AddGroupsPayLoad(IEnumerable<Root> GroupsPayLoad);
    }

    public class WsGroupService : IWsGroupService
    {
        private readonly WsGroupRepository _svc;
        public WsGroupService(DataContext data)
        {
            _svc = new WsGroupRepository(data);
        }

        public async Task<WsGroup> Add(WsGroup it) => await _svc.AddAsync(it);

        public async Task<bool> AddGroupsPayLoad(IEnumerable<Root> GroupsPayLoad)
        {
            var groups = GroupsPayLoad
                .Where(x => !string.IsNullOrEmpty(x.id._serialized) && x.name != null)
                .Select(x => new WsGroup()
                {
                    IdWsGrp = x.id._serialized,
                    GrpNam = x.name.ToString() ?? ""
                });

            return await _svc.AddRangeAsync(groups) > 0;
        }

        public async Task<bool> AddRange(IEnumerable<WsGroup> it) => await _svc.AddRangeAsync(it) > 0;

        public async Task<WsGroup> Upd(WsGroup it) => await _svc.UpdateAsync(it);

        public async Task<bool> UpdRange(IEnumerable<WsGroup> it) => await _svc.UpdateRangeAsync(it);

        public async Task<bool> UpdNoTracking(Expression<Func<WsGroup, bool>> pred, Expression<Func<SetPropertyCalls<WsGroup>, SetPropertyCalls<WsGroup>>> toChange)
        {
            return await _svc.UpdateAsyncNoTracker(pred, toChange);
        }

        public async Task<bool> Del(WsGroup it) => await _svc.DeleteAsync(it);

        public async Task<bool> DeleteRange(IEnumerable<WsGroup> it) => await _svc.DeleteRangeAsync(it);

        public async Task<bool> DeleteNoTracking(Expression<Func<WsGroup, bool>> pred)
        {
            return await _svc.DeleteAsyncNoTracker(pred);
        }

        public async Task<WsGroup?> GetById(int id) => await _svc.FirsOrDefaultAsync(x => x.IdGrp == id);

        public async Task<int> Count(Expression<Func<WsGroup, bool>>? predicado = null)
        {
            if (predicado is null)
                return await _svc.CountAsync();

            return await _svc.CountAsync(predicado!);
        }

        public async Task<IEnumerable<WsGroup>> GetAll(Filter filter)
        {
            if(filter.parameters.Count == 0)
                return await _svc.GetAllAsync();

            string _op = filter.parameters.ContainsKey("op") ? filter.parameters["op"].ToString() : "-";
            string _val = filter.parameters.ContainsKey("data") ? filter.parameters["val"].ToString() : "-";

            return Enumerable.Empty<WsGroup>();
        }

    }
}
