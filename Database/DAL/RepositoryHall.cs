using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Database.DAL
{
    public class RepositoryHall : IRepository<Hall>
    {
        private readonly MyDBContext context;
        public RepositoryHall(MyDBContext context)
        {
            this.context = context;
        }
        public List<Hall> GetAll() => context.Halls.ToList();
        public Hall GetByID(int id) => context.Halls.FirstOrDefault(add => add.ID == id);
        public void Create(Hall entity)
        {
            context.Halls.Add(entity);
            context.SaveChanges();
        }
        public void Update(Hall entity)
        {
            context.Halls.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Hall service = context.Halls.Find(id);
            context.Halls.Remove(service);
            context.SaveChanges();
        }
    }
}
