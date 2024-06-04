using Themgico.DTO.Product;
using Themgico.DTO;
using Themgico.DTO.Category;

namespace Themgico.Service.Interface
{
    public interface ICategoryService
    {
        public Task<ResultDTO<List<CategoryDTO>>> GetAllCategory();
        public Task<ResultDTO<CategoryDTO>> GetCategoryById(int id);
        public Task<ResultDTO<CategoryDTO>> CreateCategory(CategoryDTO productDTO);
        public Task<ResultDTO<CategoryDTO>> DeleteCategory(int id);
        public Task<ResultDTO<CategoryDTO>> UpdateCategory(CategoryDTO productDTO);
    }
}
