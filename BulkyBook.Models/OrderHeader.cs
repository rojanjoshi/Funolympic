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
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; } 

        
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [ValidateNever]
        public Customer Customer { get; set; }


        public int TransactionId { get; set; } 
        [ForeignKey("TransactionId")]
        [ValidateNever]
        public Transaction Transaction { get; set; }


        public string Cashier { get; set; }
        
        public double Total { get; set; }
        [Range(0, 100, ErrorMessage = "Please enter a value between 1 and 100")]
        public double ?Dpercent { get; set; }

        [Range(0, 1000000, ErrorMessage = "Please enter a value between 1 and 1000000")]
        public double ?Discount { get; set; }
        
        public double Netamount { get; set; }
        [Range(0, 1000000, ErrorMessage = "Please enter a value between 1 and 1000000")]
        public double ?Tendor { get; set; }
        
        public double ?Change { get; set; }
    }
}
