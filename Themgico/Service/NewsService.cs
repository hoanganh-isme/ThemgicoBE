using Microsoft.EntityFrameworkCore;
using Themgico.DTO;
using Themgico.DTO.News;
using Themgico.DTO.Product;
using Themgico.Entities;
using Themgico.Service.Interface;

namespace Themgico.Service
{
    public class NewsService : INewsService
    {
        private readonly ThemgicoContext _context;
        public NewsService(ThemgicoContext context)
        {
            _context = context;
        }
        public async Task<ResultDTO<List<NewsDTO>>> GetAllNews()
        {
            try
            {
                // Lấy tất cả sản phẩm từ cơ sở dữ liệu
                var news = await _context.News.ToListAsync();

                // Chuyển đổi danh sách sản phẩm sang DTO
                var newsDTOs = news.Select(p => new NewsDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Author = p.Author,
                    Image = p.Image,
                    Status = p.Status,
                    CreateDate = p.CreateDate,
                    //Thêm các trường khác nếu cần thiết
                }).ToList();

                // Trả về kết quả thành công cùng với danh sách sản phẩm DTO
                return ResultDTO<List<NewsDTO>>.Success(newsDTOs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Trả về kết quả thất bại nếu có lỗi xảy ra
                return ResultDTO<List<NewsDTO>>.Fail("Failed to fetch news from the database");
            }
        }
        public async Task<ResultDTO<NewsDTO>> GetNewsById(int id)
        {
            try
            {
                var news = await _context.News.FindAsync(id);
                if (news == null)
                {
                    return ResultDTO<NewsDTO>.Fail("News not found.");
                }

                var newsDTO = new NewsDTO
                {
                    Id = news.Id,
                    Title = news.Title,
                    Content = news.Content,
                    CreateDate = news.CreateDate,
                    Author = news.Author,
                    Image = news.Image,
                    Status = news.Status
                };

                return ResultDTO<NewsDTO>.Success(newsDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<NewsDTO>.Fail("Service is not available.");
            }
        }
        public async Task<ResultDTO<NewsDTO>> CreateNews(NewsDTO newsDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(newsDTO.Title))
                    return ResultDTO<NewsDTO>.Fail("Title is required.");

                if (string.IsNullOrEmpty(newsDTO.Content))
                    return ResultDTO<NewsDTO>.Fail("Content is required.");

                if (string.IsNullOrEmpty(newsDTO.Author))
                    return ResultDTO<NewsDTO>.Fail("Author is required.");

                var news = new News
                {
                    Title = newsDTO.Title,
                    Content = newsDTO.Content,
                    CreateDate = DateTime.Now,
                    Author = newsDTO.Author,
                    Image = newsDTO.Image,
                    Status = newsDTO.Status
                };

                _context.News.Add(news);
                await _context.SaveChangesAsync();

                newsDTO.Id = news.Id;
                newsDTO.CreateDate = news.CreateDate;

                return ResultDTO<NewsDTO>.Success(newsDTO, "News created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<NewsDTO>.Fail("Service is not available.");
            }
        }

        public async Task<ResultDTO<NewsDTO>> DeleteNews(int id)
        {
            try
            {
                var news = await _context.News.FindAsync(id);
                if (news == null)
                {
                    return ResultDTO<NewsDTO>.Fail("News not found.");
                }

                _context.News.Remove(news);
                await _context.SaveChangesAsync();

                return ResultDTO<NewsDTO>.Success(null, "News deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<NewsDTO>.Fail("Service is not available.");
            }
        }

        public async Task<ResultDTO<NewsDTO>> UpdateNews(NewsDTO newsDTO)
        {
            try
            {
                var news = await _context.News.FindAsync(newsDTO.Id);
                if (news == null)
                {
                    return ResultDTO<NewsDTO>.Fail("News not found.");
                }

                if (string.IsNullOrEmpty(newsDTO.Title))
                    return ResultDTO<NewsDTO>.Fail("Title is required.");

                if (string.IsNullOrEmpty(newsDTO.Content))
                    return ResultDTO<NewsDTO>.Fail("Content is required.");

                if (string.IsNullOrEmpty(newsDTO.Author))
                    return ResultDTO<NewsDTO>.Fail("Author is required.");

                news.Title = newsDTO.Title;
                news.Content = newsDTO.Content;
                news.CreateDate = DateTime.Now;
                news.Author = newsDTO.Author;
                news.Image = newsDTO.Image;
                news.Status = newsDTO.Status;

                _context.News.Update(news);
                await _context.SaveChangesAsync();

                return ResultDTO<NewsDTO>.Success(newsDTO, "News updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ResultDTO<NewsDTO>.Fail("Service is not available.");
            }
        }
    }
}

