using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Model;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(c => c.NationalPark).FirstOrDefault(d => d.Id == trailId);

        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(c => c.NationalPark).OrderBy(p=>p.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value= _db.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(a => a.Id == id);
        }

        public bool Save()
        {
            //if(_db.SaveChanges()>=0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
#pragma warning disable IDE0075 // Simplify conditional expression
            return _db.SaveChanges() >= 0 ? true : false;
#pragma warning restore IDE0075 // Simplify conditional expression
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npid)
        {
            return _db.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npid).ToList();
        }
    }
}
