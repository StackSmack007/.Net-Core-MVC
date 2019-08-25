using System.ComponentModel.DataAnnotations;

namespace ChushkaApp.DTOS.Products
{
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Required, MaxLength(1024)]
        public string Description { get; set; }
        public string Type { get; set; }
    }
}