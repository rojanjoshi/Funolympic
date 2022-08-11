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
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CommentName { get; set; }


        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }


        public int VideoId { get; set; }
        [ForeignKey("VideoId")]
        [ValidateNever]
        public Video Video { get; set; }

        
        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public IEnumerable<Comment>? Commentlist { get; set; }





    }
}
