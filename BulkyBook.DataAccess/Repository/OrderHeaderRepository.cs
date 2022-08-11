using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository: Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderHeader obj)
        {
            var objFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.CreatedDate = obj.CreatedDate;
                objFromDb.ApplicationUserId= obj.ApplicationUserId;
                objFromDb.CustomerId = obj.CustomerId;
                objFromDb.TransactionId = obj.TransactionId;
                objFromDb.Total = obj.Total;
                objFromDb.Discount = obj.Discount;
                objFromDb.Netamount = obj.Netamount;
                objFromDb.Tendor = obj.Tendor;
                objFromDb.Change = obj.Change;
                objFromDb.Dpercent = obj.Dpercent;

            }
        }
    }

}
