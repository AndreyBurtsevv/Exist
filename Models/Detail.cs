using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Models
{
    public class Detail
    {
        [Key]
        public int Id { get; set; }

        [Range(0.99, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string Name { get; set; }    

        public string Description { get; set; }

        [Required]
        public int GroupId { get; set; }

        public Group Group { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
