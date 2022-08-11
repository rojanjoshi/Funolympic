using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderDetailRepository: Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;

        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public double DecrementCount(OrderDetail orderDetail, double quantity)
        {
            orderDetail.Quantity -= quantity;
            return orderDetail.Quantity;
        }

        public double IncrementCount(OrderDetail orderDetail, double quantity)
        {
            orderDetail.Quantity += quantity;
            return orderDetail.Quantity;
        }

        public void Update(OrderDetail obj)
        {
            var objFromDb = _db.OrderDetails.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.OrderId = obj.OrderId;
                objFromDb.ProductId= obj.ProductId;
                objFromDb.Price = obj.Price;
                objFromDb.Quantity = obj.Quantity;
            


            }
        }
    }

}
