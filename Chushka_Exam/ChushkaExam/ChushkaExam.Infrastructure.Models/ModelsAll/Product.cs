namespace ChushkaExam.Infrastructure.Models.ModelsAll
{
    using System.ComponentModel.DataAnnotations;
    public class Product : BaseEntity<int>
    {
        [Required, MaxLength(128)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Required, MaxLength(1024)]
        public string Description { get; set; }

        public virtual ProductType Type {get;set;}
    }

    public enum ProductType
    {
        Food=1,
        Domestic=2,
        Health=3,
        Cosmetic=4,
        Other=5
    }
}