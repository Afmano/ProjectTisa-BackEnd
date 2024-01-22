using Microsoft.AspNetCore.Mvc;

namespace ProjectPop.EF.Interfaces
{
    /// <summary>
    /// Interface for standart controllers with CRUD operations.
    /// </summary>
    /// <typeparam name="T">Type of model used to CRUD.</typeparam>
    public interface ICrud<T>
    {
        Task<ActionResult<IEnumerable<T>>> Get();
        Task<ActionResult<T>> Get(int id);
        Task<ActionResult> Create(T item);
        Task<ActionResult> Update(T item);
        Task<ActionResult<T>> Delete(int id);
    }
}
