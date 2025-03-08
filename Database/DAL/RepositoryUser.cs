using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
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
        //Контекст для работы с БД
        private readonly MyDBContext context;
        public RepositoryUser(MyDBContext context)
        {
            this.context = context;
        }
        //Получение всех пользовательей
        public List<User> GetAll() => context.Users.ToList();
        //Получение пользователя по ID
        public User GetByID(int id) => context.Users.FirstOrDefault(add => add.ID == id);
        //Получение по его Логину
        public User GetByLogin(string login)
        {
            User user = context.Users.FirstOrDefault(us => us.Login == login);
            return user;
        }
        //СОздание поля
        public void Create(User entity)
        {
            try
            {
                context.Users.Add(entity);
                context.SaveChanges();
            }
           catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Обновление поля
        public void Update(User entity)
        {
            try
            {
                context.Users.Update(entity);
                context.SaveChanges();
            }
           catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Удаление поля
        public void Delete(int id)
        {
            try
            {
                User service = context.Users.Find(id);
                context.Users.Remove(service);
                context.SaveChanges();
            }catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
           
        }
    }
}
