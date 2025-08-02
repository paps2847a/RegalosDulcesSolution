using DataModel.Tables;
using DataPersistance;
using LibreriaGenericaPropia.Generic;

namespace DataLogic.Repository
{
    public class WsGroupRepository : GenericCode<WsGroup>
    {
        public WsGroupRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
