using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class CommentVm
    {
        public Comment? Comment { get; set; }
        [ValidateNever]
       
        public Video? Video { get; set; }
        public IEnumerable<Comment>? Commentlist { get; set; }
    



        
      

    }
}
