using PhotoStudioApp.Model;
using PhotoStudioApp.Database.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Database.DAL
{
    public class RepositoryServices : IRepository<Services>
    {
        private readonly MyDBContext context;
        public RepositoryServices(MyDBContext context)
        {
            this.context = context;
        }
        public List<Services> GetAll() => context.Services.ToList();
        public Services GetByID(int id) => context.Services.FirstOrDefault(add => add.ID == id);
        public Services GetByName(string name) => context.Services.FirstOrDefault(ser => ser.ServiceName == name);
        public void Create(Services entity)
        {
            context.Services.Add(entity);
            context.SaveChanges();
        }
        public void Update(Services entity)
        {
            context.Services.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Services service = context.Services.Find(id);
            context.Services.Remove(service);
            context.SaveChanges();
        }
    }
}
