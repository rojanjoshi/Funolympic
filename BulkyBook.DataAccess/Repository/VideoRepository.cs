using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class VideoRepository: Repository<Video>, IVideoRepository
    {
        private ApplicationDbContext _db;

        public VideoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Video obj)
        {
            var objFromDb = _db.videos.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Videolink = obj.Videolink;
                objFromDb.Name = obj.Name;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.ImageUrl = obj.ImageUrl;



            }
        }
    }

}
