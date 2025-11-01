using Store.API.Domain.Models;
using Store.API.Domain.Services.Communication;

namespace Store.API.Domain.Services
{
    public interface ICategoryService
    {
         Task<IEnumerable<Category>> ListAsync();
         Task<Response<Category>> SaveAsync(Category category);
         Task<Response<Category>> UpdateAsync(int id, Category category);
         Task<Response<Category>> DeleteAsync(int id);
    }
}