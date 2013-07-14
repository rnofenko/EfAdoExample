using System;
using System.Collections.Generic;

namespace AdoExample.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int GroupId { get; set; }
        public RiskGroup Group { get; set; }

        public DateTime Changed { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}
