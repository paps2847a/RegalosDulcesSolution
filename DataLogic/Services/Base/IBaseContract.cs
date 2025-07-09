using DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Services.Base
{
    public interface IBaseContract<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Filter filter);
        Task<T> Add(T it);
        Task<T> Upd(T it);
        Task<bool> Del(T it);
    }
}
