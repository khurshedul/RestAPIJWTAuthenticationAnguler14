using Core.EMS.Entities;
using Core.EMS.Interfaces;
using Core.Utils.Interfaces;
using Core.Utils.Services;
using System.Threading.Tasks;

namespace Core.EMS.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IAsyncRepository<Product> _asyncRepositoryProduct;

        public ProductService(IAsyncRepository<Product> asyncRepositoryProduct) : base(asyncRepositoryProduct)
        {
            _asyncRepositoryProduct = asyncRepositoryProduct;
        }

        public async Task<Product> GetProductByIDAsync(int id)
        {
            return await _asyncRepositoryProduct.FirstOrDefaultAsync(s=>s.ID== id);
        }

        public async Task<Product> UpdateProduct(Product entity)
        { 
            await _asyncRepositoryProduct.UpdateAsync(entity);
            return entity;
        }

    }
}
