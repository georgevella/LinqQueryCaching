using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Experiments.LinqQueryCaching.Model
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public List<Sale> Sales { get; set; }  = new List<Sale>();

        public Customer(int id, string name, string surname)
        {
            Id = id;
            Name = name;
            Surname = surname;
        }

        public Customer()
        {
            
        }
    }
}