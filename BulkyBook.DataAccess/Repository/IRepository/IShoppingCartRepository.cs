using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        double IncrementCount(ShoppingCart shoppingCart, double quantity);
        double DecrementCount(ShoppingCart shoppingCart, double quantity);
        void Update(ShoppingCart obj);
    }
}
