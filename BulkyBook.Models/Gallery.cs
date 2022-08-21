﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Gallery
    {
        [Key]
        public int Id { get; set; }
   
 

        [ValidateNever]
        public string ?ImageUrl { get; set; }


    }
}
