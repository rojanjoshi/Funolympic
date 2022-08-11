using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository: IRepository<OrderDetail>
    {
        double IncrementCount(OrderDetail orderDetail, double quantity);
        double DecrementCount(OrderDetail orderDetail, double quantity);
        void Update(OrderDetail obj);
    }
}
