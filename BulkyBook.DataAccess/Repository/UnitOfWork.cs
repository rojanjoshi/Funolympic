using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
     
            ApplicationUser = new ApplicationUserRepository(_db);
            Product = new ProductRepository(_db);
            Customer = new CustomerRepository(_db);
            Transaction = new TransactionRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);

            Category = new CategoryRepository(_db);
            Video = new VideoRepository(_db);
            Comment = new CommentRepository(_db);
            Message = new MessageRepository(_db);
            Gallery = new GalleryRepository(_db);
        }


        public ICommentRepository Comment { get; private set; }
        public IVideoRepository Video { get; private set; }
        public IApplicationUserRepository ApplicationUser {  get; private set; }
        public IProductRepository Product { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public ITransactionRepository Transaction { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

        public ICategoryRepository Category { get; private set; }
        public IMessageRepository Message { get; private set; }
        public IGalleryRepository Gallery { get; private set; }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
