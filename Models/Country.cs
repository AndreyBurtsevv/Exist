using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exist.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IList<Company> Companys { get; set; }
    }
}
