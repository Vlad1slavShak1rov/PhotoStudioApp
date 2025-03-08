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
    public class RepositoryWorker : IRepository<Worker>
    {
        //Котнекст для работы с БД
        private readonly MyDBContext context;
        public RepositoryWorker(MyDBContext context)
        {
            this.context = context;
        }
        //Получаем всех сотрудников
        public List<Worker> GetAll() => context.Workers.ToList();
        //Получаем всех сотрудников фотографов
        public List<Worker> GetAllPhotograph() => context.Workers.Where(w => w.Post == Enums.Post.Photograph).ToList();
       //Получаем всех сотрудников визажистов
        public List<Worker> GetAllVisagiste() => context.Workers.Where(w => w.Post == Enums.Post.Visagiste).ToList();
        //Получаем сотрудника по ID
        public Worker GetByID(int id) => context.Workers.FirstOrDefault(add => add.ID == id);
        //Получаем сотрудника по ID пользователя
        public Worker GetByUserID(int id) => context.Workers.FirstOrDefault(add => add.UserID == id);
        //Получаем фотографа по ID 
        public Worker GetByIDPhotograph(int id) => context.Workers.FirstOrDefault(ph => ph.ID == id && (Enums.Post)ph.Post == Enums.Post.Photograph);
        //Получаем визажиста по ID 
        public Worker GetByIDVisagiste(int id) => context.Workers.FirstOrDefault(ph => ph.ID == id && (Enums.Post)ph.Post == Enums.Post.Visagiste);
        public void Create(Worker entity)
        {
            try
            {
                context.Workers.Add(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Обновление поля
        public void Update(Worker entity)
        {
            try
            {
                context.Workers.Update(entity);
                context.SaveChanges();
            }catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
         
        }
        //Удаление поля
        public void Delete(int id)
        {
            try
            {
                Worker service = context.Workers.Find(id);
                context.Workers.Remove(service);
                context.SaveChanges();
            }catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
           
        }
    }
}
