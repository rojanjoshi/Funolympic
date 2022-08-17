using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
       
        IApplicationUserRepository ApplicationUser {  get; }
        IProductRepository Product { get; }
        ICustomerRepository Customer { get; }
        ITransactionRepository Transaction { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }

        ICategoryRepository Category { get; }
        IVideoRepository Video { get; }
        ICommentRepository Comment { get; }
        IMessageRepository Message { get; }

        void Save();
    }
}
