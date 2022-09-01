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
        public Message ?Message { get; set; }
        public IEnumerable<Gallery>? Gallerylist { get; set; }

        public IEnumerable<Gallery>? IndexGallerylist { get; set; }
        public IEnumerable<Upcomming>? Upcomminglist { get; set; }
    }
}
