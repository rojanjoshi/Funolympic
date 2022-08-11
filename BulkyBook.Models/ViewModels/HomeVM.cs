using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
    public class HomeVM
    {
       
        public IEnumerable<Video> ?Videolist{ get; set; }
        public IEnumerable<Category> ? Categorylist { get; set; }
    }
}
