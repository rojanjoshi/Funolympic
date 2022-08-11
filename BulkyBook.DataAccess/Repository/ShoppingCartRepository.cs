using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public double DecrementCount(ShoppingCart shoppingCart, double quantity)
        {
            shoppingCart.Quantity -= quantity;
            return shoppingCart.Quantity;
        }

        public double IncrementCount(ShoppingCart shoppingCart, double quantity)
        {
            shoppingCart.Quantity += quantity;
            return shoppingCart.Quantity;
        }

        public void Update(ShoppingCart obj)
        {
            var objFromDb = _db.ShoppingCarts.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.ProductId = obj.ProductId;
                objFromDb.ApplicationUserId = obj.ApplicationUserId;
                objFromDb.Quantity = obj.Quantity;
                objFromDb.Price = obj.Price;
                


            }
        }
    }

}
