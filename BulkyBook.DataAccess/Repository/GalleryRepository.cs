using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class GalleryRepository : Repository<Gallery>, IGalleryRepository
    {
        private ApplicationDbContext _db;

        public GalleryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Gallery obj)
        {
            var objFromDb = _db.Galleries.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
               
                objFromDb.ImageUrl = obj.ImageUrl;
                
           

            }
        }
    }

}
