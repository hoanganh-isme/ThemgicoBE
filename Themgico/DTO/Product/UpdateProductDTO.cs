namespace Themgico.DTO.Product
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public List<int> CategoryId { get; set; }
        public string? Image { get; set; }
        public bool? Status { get; set; }
    }
}
