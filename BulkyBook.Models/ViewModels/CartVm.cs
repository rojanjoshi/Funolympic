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
    public class CartVm
    {
        public ShoppingCart? ShoppingCart { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProductList { get; set; }
        [ValidateNever]

        public IEnumerable<ShoppingCart>? ListCart { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CustomerList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TransactionList { get; set; }



        public OrderHeader OrderHeader { get; set; }    
      

    }
}
