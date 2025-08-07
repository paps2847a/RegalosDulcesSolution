using DataModel.Tables;
using DataPersistance;
using LibreriaGenericaPropia.Generic;

namespace DataLogic.Repository
{
    public class RecordatorioRepository : GenericCode<Recordatorio>
    {
        public RecordatorioRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
