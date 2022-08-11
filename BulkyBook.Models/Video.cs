using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string ?Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string ?Videolink { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category ?Category { get; set; }

        [ValidateNever]
        public string ?ImageUrl { get; set; }

    }
}
