using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        
        [MaxLength(20)]
        public string Address { get; set; }
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }
        [MaxLength(20)]
        public string Email { get; set; }
    }
}
