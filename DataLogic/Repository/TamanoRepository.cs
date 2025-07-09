using DataModel.Tables;
using DataPersistance;
using LibreriaGenericaPropia.Generic;

namespace DataLogic.Repository
{
    public class TamanoRepository : GenericCode<Tamano>
    {
        public TamanoRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
