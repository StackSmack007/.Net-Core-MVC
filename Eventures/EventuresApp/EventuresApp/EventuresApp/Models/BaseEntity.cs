namespace EventuresApp.Models
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}