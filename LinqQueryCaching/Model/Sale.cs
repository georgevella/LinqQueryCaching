using System.ComponentModel.DataAnnotations;

namespace Experiments.LinqQueryCaching.Model
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        public Customer Customer { get; set; }

        public string Item { get; set; }
        public int Value { get; set; }

        public Sale()
        {
            
        }

        public Sale(int id, string item, int value)
        {
            Id = id;
            Item = item;
            Value = value;
        }
    }
}