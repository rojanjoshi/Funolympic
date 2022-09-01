using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UpcommingRepository : Repository<Upcomming>, IUpcommingRepository
    {
        private ApplicationDbContext _db;

        public UpcommingRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Upcomming obj)
        {
            var objFromDb = _db.Upcommings.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Date = obj.Date;
                objFromDb.Name = obj.Name;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.ImageUrl = obj.ImageUrl;



            }
        }
    }

}
