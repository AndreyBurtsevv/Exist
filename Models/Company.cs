using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        public IList<Group> Groups { get; set; }

        public IList<Detail> Details { get; set; }
    }
}
