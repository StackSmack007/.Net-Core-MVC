using System.ComponentModel.DataAnnotations;

namespace ChushkaApp.DTOS.Home
{
    public class outputProductDTO
    {
        public int Id { get; set; }
        [Required, MaxLength(128)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Required, MaxLength(1024)]
        public string Description { get; set; }
        public string DescriptionShortened => Description.Length > 50 ? Description.Substring(0, 50) + "..." : Description;
    }
}
