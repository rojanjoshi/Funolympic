using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class MessageRepository: Repository<Message>, IMessageRepository
    {
        private ApplicationDbContext _db;

        public MessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Message obj)
        {
            var objFromDb = _db.Messages.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.MessageName = obj.MessageName;
                objFromDb.Subject = obj.Subject;
                objFromDb.Date = obj.Date;
                objFromDb.ApplicationUserId = obj.ApplicationUserId;
           

            }
        }
    }

}
