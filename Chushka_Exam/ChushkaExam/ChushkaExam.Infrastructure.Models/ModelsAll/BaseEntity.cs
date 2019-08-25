namespace ChushkaExam.Infrastructure.Models.ModelsAll
{
    using System.ComponentModel.DataAnnotations;
    public abstract class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}