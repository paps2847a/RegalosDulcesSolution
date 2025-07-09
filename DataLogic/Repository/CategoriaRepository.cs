using DataModel.Tables;
using DataPersistance;
using LibreriaGenericaPropia.Generic;

namespace DataLogic.Repository
{
    public class CategoriaRepository : GenericCode<Categoria>
    {
        public CategoriaRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}
