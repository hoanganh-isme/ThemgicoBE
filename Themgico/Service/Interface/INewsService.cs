
using Themgico.DTO;
using Themgico.DTO.News;
using Themgico.DTO.Product;

namespace Themgico.Service.Interface
{
    public interface INewsService
    {
        public Task<ResultDTO<List<NewsDTO>>> GetAllNews();
        public Task<ResultDTO<NewsDTO>> GetNewsById(int id);
        public Task<ResultDTO<NewsDTO>> CreateNews(NewsDTO newsDTO);
        public Task<ResultDTO<NewsDTO>> DeleteNews(int id);
        public Task<ResultDTO<NewsDTO>> UpdateNews(NewsDTO newsDTO);
    }
}
