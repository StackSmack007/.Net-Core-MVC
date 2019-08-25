using System.ComponentModel.DataAnnotations.Schema;

namespace ChushkaExam.Infrastructure.Models.ModelsAll
{
    public class Order : BaseEntity<int>
    {
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public virtual ChushkaUser Client { get; set; }
    }
}