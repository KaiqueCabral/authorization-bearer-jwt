using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationProject.Interfaces;

public interface IRepository<T>
{
    Task Add(T obj);
    IEnumerable<T> GetAll();
    T GetById(int id);
    Task Update(T obj);
    Task Delete(T id);
}
