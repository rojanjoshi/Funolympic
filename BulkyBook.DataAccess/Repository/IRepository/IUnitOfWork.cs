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


        ICategoryRepository Category { get; }
        IVideoRepository Video { get; }
        ICommentRepository Comment { get; }
        IMessageRepository Message { get; }
        IGalleryRepository Gallery { get; }
        IUpcommingRepository Upcomming { get; }

        void Save();
    }
}
