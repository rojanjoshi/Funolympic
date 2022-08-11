using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private ApplicationDbContext _db;

        public CommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Comment obj)
        {
            var objFromDb = _db.Comments.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.CommentName = obj.CommentName;
                objFromDb.VideoId = obj.VideoId;
                objFromDb.ApplicationUserId = obj.ApplicationUserId;
                objFromDb.Date = obj.Date;
           

            }
        }
    }

}
