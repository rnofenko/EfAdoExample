using System.ComponentModel.DataAnnotations;

namespace AdoExample.Models
{
    public class RiskGroup
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
    }
}