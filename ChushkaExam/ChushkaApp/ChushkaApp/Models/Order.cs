namespace ChushkaApp.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Order : BaseEntity<int>
    {
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public DateTime OrderedOn  { get; set; }

                [Required]
        public string ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public virtual ChushkaUser Client { get; set; }
    }
}