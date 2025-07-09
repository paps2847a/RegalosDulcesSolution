using DataModel.Tables;
using DataPersistance;
using LibreriaGenericaPropia.Generic;

namespace DataLogic.Repository
{
    public class InventarioRepository : GenericCode<Inventario>
    {
        public InventarioRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
