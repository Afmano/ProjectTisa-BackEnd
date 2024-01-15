using Microsoft.AspNetCore.Mvc;

namespace ProjectPop.EF.Interfaces
{
    public interface ICrud<T>
    {
        ActionResult<IEnumerable<T>> Get();
        ActionResult<T> Get(int id);
        ActionResult Create(T item);
        ActionResult Update(T item);
        ActionResult<T> Delete(int id);
    }
}
