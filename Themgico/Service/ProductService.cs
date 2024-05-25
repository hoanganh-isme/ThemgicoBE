using Microsoft.EntityFrameworkCore;
using Themgico.DTO;
using Themgico.DTO.Product;
using Themgico.Entities;
using Themgico.Service.Interface;

namespace Themgico.Service
{
    public class ProductService : IProductService
    {
        private readonly ThemgicoContext _context;
        public ProductService(ThemgicoContext context) 
        {
            _context = context;
        }
        public async Task<ResultDTO<List<ProductDTO>>> GetAllProducts()
        {
            try
            {
                // Lấy tất cả sản phẩm từ cơ sở dữ liệu
                var products = await _context.Products.ToListAsync();

                // Chuyển đổi danh sách sản phẩm sang DTO
                var productDTOs = products.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Image = p.Image,
                    Status = p.Status,
                    CategoryId = p.CategoryId,
                    //Thêm các trường khác nếu cần thiết
                }).ToList();

                // Trả về kết quả thành công cùng với danh sách sản phẩm DTO
                return ResultDTO<List<ProductDTO>>.Success(productDTOs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Trả về kết quả thất bại nếu có lỗi xảy ra
                return ResultDTO<List<ProductDTO>>.Fail("Failed to fetch products from the database");
            }
        }

    }
}
