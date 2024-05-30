using Microsoft.EntityFrameworkCore;
using Themgico.DTO;
using Themgico.DTO.Category;
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

        public async Task<ResultDTO<ProductDTO>> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    return ResultDTO<ProductDTO>.Fail("Product not found", 404);
                }

                // Ensure proper type conversion
                bool status;
                if (!bool.TryParse(product.Status.ToString(), out status))
                {
                    return ResultDTO<ProductDTO>.Fail("Invalid status value", 400);
                }

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Image = product.Image,
                    Status = status,
                    Category = new CategoryDTO
                    {
                        CategoryId = product.Category.CategoryId,
                        Name = product.Category.Name
                    }
                };

                return ResultDTO<ProductDTO>.Success(productDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<ProductDTO>.Fail("Service is not available");
            }
        }


        public async Task<ResultDTO<CreateProductDTO>> CreateProduct(CreateProductDTO productDTO)
        {
            if (string.IsNullOrEmpty(productDTO.Name))
            {
                return ResultDTO<CreateProductDTO>.Fail("Name is required.");
            }

            if (string.IsNullOrEmpty(productDTO.Description))
            {
                return ResultDTO<CreateProductDTO>.Fail("Description is required.");
            }

            if (productDTO.Price == null || productDTO.Price < 0)
            {
                return ResultDTO<CreateProductDTO>.Fail("Price should be greater than or equal to 0.");
            }

            if (productDTO.CategoryId == null || !productDTO.CategoryId.Any())
            {
                return ResultDTO<CreateProductDTO>.Fail("Category ID is required.");
            }

            if (string.IsNullOrEmpty(productDTO.Image))
            {
                return ResultDTO<CreateProductDTO>.Fail("Image is required.");
            }

            if (productDTO.Status == null)
            {
                return ResultDTO<CreateProductDTO>.Fail("Status is required.");
            }

            try
            {
                var product = new Product
                {
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    CategoryId = productDTO.CategoryId.FirstOrDefault(), // Assuming single category for simplicity
                    Image = productDTO.Image,
                    Status = productDTO.Status
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                productDTO.Id = product.Id;

                return ResultDTO<CreateProductDTO>.Success(productDTO, "Product created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<CreateProductDTO>.Fail("Service is not available");
            }
        }
        public async Task<ResultDTO<ProductDTO>> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return ResultDTO<ProductDTO>.Fail("Product not found", 404);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                var productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Image = product.Image,
                    Status = product.Status
                };

                return ResultDTO<ProductDTO>.Success(productDTO, "Product deleted successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<ProductDTO>.Fail("Service is not available");
            }
        }
        public async Task<ResultDTO<ProductDTO>> UpdateProduct(UpdateProductDTO productDTO)
        {
            try
            {
                var product = await _context.Products.FindAsync(productDTO.Id);

                if (product == null)
                {
                    return ResultDTO<ProductDTO>.Fail("Product not found", 404);
                }

                // Update the product properties if they are not null
                product.Name = productDTO.Name ?? product.Name;
                product.Description = productDTO.Description ?? product.Description;
                product.Price = productDTO.Price ?? product.Price;

                if (productDTO.CategoryId != null && productDTO.CategoryId.Any())
                {
                    product.CategoryId = productDTO.CategoryId.First();
                }

                product.Image = productDTO.Image ?? product.Image;
                product.Status = productDTO.Status ?? product.Status;

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                // Map updated product to ProductDTO
                var updatedProductDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Image = product.Image,
                    Status = product.Status
                };

                return ResultDTO<ProductDTO>.Success(updatedProductDTO, "Product updated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<ProductDTO>.Fail("Service is not available", 500);
            }
        }



    }
}
