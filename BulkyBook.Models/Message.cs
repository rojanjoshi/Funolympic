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
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ?Subject { get; set; }

        [Required]
        [MaxLength(200)]
        public string ?MessageName { get; set; }
       

       public DateTime Date { get; set; } = DateTime.Now;   


        [Required]
        [Display(Name = "ApplicationUser")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ?ApplicationUser { get; set; }


       
    }
}
