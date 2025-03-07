using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Database.DAL
{
    public class RepositoryUser: IRepository<User>
    {
        private readonly MyDBContext context;
        public RepositoryUser(MyDBContext context)
        {
            this.context = context;
        }
        public List<User> GetAll() => context.Users.ToList();
        public User GetByID(int id) => context.Users.FirstOrDefault(add => add.ID == id);
        public User GetByLogin(string login)
        {
            User user = context.Users.FirstOrDefault(us => us.Login == login);
            return user;
        }
        public void Create(User entity)
        {
            context.Users.Add(entity);
            context.SaveChanges();
        }
        public void Update(User entity)
        {
            context.Users.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            User service = context.Users.Find(id);
            context.Users.Remove(service);
            context.SaveChanges();
        }
    }
}
