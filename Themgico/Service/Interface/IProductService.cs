using Themgico.DTO;
using Themgico.DTO.Product;

namespace Themgico.Service.Interface
{
    public interface IProductService
    {
        public Task<ResultDTO<List<ProductDTO>>> GetAllProducts();
    }
}
