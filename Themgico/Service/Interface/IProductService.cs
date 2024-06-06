using Themgico.DTO;
using Themgico.DTO.Product;

namespace Themgico.Service.Interface
{
    public interface IProductService
    {
        public Task<ResultDTO<List<ProductDTO>>> GetAllProducts();
        public Task<ResultDTO<ProductDTO>> GetProductById(int id);
        public Task<ResultDTO<CreateProductDTO>> CreateProduct(CreateProductDTO productDTO);
        public Task<ResultDTO<ProductDTO>> DeleteProduct(int id);
        public Task<ResultDTO<ProductDTO>> UpdateProduct(UpdateProductDTO productDTO);
        public Task<ResultDTO<string>> UpdateProductStatus(int id);
    }
}
