using Core.EMS.Entities;
using Core.Utils.Interfaces;
using System.Threading.Tasks;

namespace Core.EMS.Interfaces
{
    public interface IProductService : IBaseService<Product>
    {
        Task<Product> GetProductByIDAsync(int id);
        Task<Product> UpdateProduct(Product entity);
    }
}