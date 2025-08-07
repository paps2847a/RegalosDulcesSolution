using DataModel.Tables;
using DataPersistance;
using LibreriaGenericaPropia.Generic;

namespace DataLogic.Repository
{
    public class MensajeRepository : GenericCode<Mensaje>
    {
        public MensajeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
