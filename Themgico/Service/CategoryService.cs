using Microsoft.EntityFrameworkCore;
using Themgico.DTO;
using Themgico.DTO.Category;
using Themgico.Entities;
using Themgico.Service.Interface;

namespace Themgico.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ThemgicoContext _context;

        public CategoryService(ThemgicoContext context)
        {
            _context = context;
        }
        public async Task<ResultDTO<CategoryDTO>> CreateCategory(CategoryDTO categoryDTO)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(categoryDTO.Name))
                {
                    return ResultDTO<CategoryDTO>.Fail("Name is required.");
                }

                if (string.IsNullOrEmpty(categoryDTO.CategoryDescription))
                {
                    return ResultDTO<CategoryDTO>.Fail("Category Description is required.");
                }

                // Create new category
                var category = new Category
                {
                    Name = categoryDTO.Name,
                    CategoryDescription = categoryDTO.CategoryDescription
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                categoryDTO.CategoryId = category.CategoryId;

                return ResultDTO<CategoryDTO>.Success(categoryDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<CategoryDTO>.Fail("Failed to create category.");
            }
        }


        public async Task<ResultDTO<CategoryDTO>> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    return ResultDTO<CategoryDTO>.Fail("Category not found");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return ResultDTO<CategoryDTO>.Success(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<CategoryDTO>.Fail("Failed to delete category");
            }
        }

        public async Task<ResultDTO<List<CategoryDTO>>> GetAllCategory()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();

                var categoryDTOs = categories.Select(c => new CategoryDTO
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    CategoryDescription = c.CategoryDescription
                }).ToList();

                return ResultDTO<List<CategoryDTO>>.Success(categoryDTOs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<List<CategoryDTO>>.Fail("Failed to fetch categories");
            }
        }

        public async Task<ResultDTO<CategoryDTO>> GetCategoryById(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    return ResultDTO<CategoryDTO>.Fail("Category not found");
                }

                var categoryDTO = new CategoryDTO
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    CategoryDescription = category.CategoryDescription
                };

                return ResultDTO<CategoryDTO>.Success(categoryDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<CategoryDTO>.Fail("Failed to fetch category");
            }
        }

        public async Task<ResultDTO<CategoryDTO>> UpdateCategory(CategoryDTO categoryDTO)
        {
            try
            {
                // Validation
                if (categoryDTO.CategoryId <= 0)
                {
                    return ResultDTO<CategoryDTO>.Fail("Invalid Category ID.");
                }

                if (string.IsNullOrEmpty(categoryDTO.Name))
                {
                    return ResultDTO<CategoryDTO>.Fail("Name is required.");
                }

                if (string.IsNullOrEmpty(categoryDTO.CategoryDescription))
                {
                    return ResultDTO<CategoryDTO>.Fail("Category Description is required.");
                }

                var category = await _context.Categories.FindAsync(categoryDTO.CategoryId);

                if (category == null)
                {
                    return ResultDTO<CategoryDTO>.Fail("Category not found.");
                }

                category.Name = categoryDTO.Name;
                category.CategoryDescription = categoryDTO.CategoryDescription;

                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                return ResultDTO<CategoryDTO>.Success(categoryDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<CategoryDTO>.Fail("Failed to update category.");
            }
        }

    }
}

