using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Database.DAL
{
    public class RepositoryWorker : IRepository<Worker>
    {
        private readonly MyDBContext context;
        public RepositoryWorker(MyDBContext context)
        {
            this.context = context;
        }
        public List<Worker> GetAll() => context.Workers.ToList();
        public List<Worker> GetAllPhotograph() => context.Workers.Where(w => w.Post == Enums.Post.Photograph).ToList();
        public List<Worker> GetAllVisagiste() => context.Workers.Where(w => w.Post == Enums.Post.Visagiste).ToList();
        public Worker GetByID(int id) => context.Workers.FirstOrDefault(add => add.ID == id);
        public Worker GetByUserID(int id) => context.Workers.FirstOrDefault(add => add.UserID == id);
        public Worker GetByIDPhotograph(int id) => context.Workers.FirstOrDefault(ph => ph.ID == id && (Enums.Post)ph.Post == Enums.Post.Photograph);
        public Worker GetByIDVisagiste(int id) => context.Workers.FirstOrDefault(ph => ph.ID == id && (Enums.Post)ph.Post == Enums.Post.Visagiste);
        public void Create(Worker entity)
        {
            context.Workers.Add(entity);
            context.SaveChanges();
        }
        public void Update(Worker entity)
        {
            context.Workers.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Worker service = context.Workers.Find(id);
            context.Workers.Remove(service);
            context.SaveChanges();
        }
    }
}
