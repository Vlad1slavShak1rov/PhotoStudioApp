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
    public class RepositoryHall : IRepository<Hall>
    {
        //Контекст для работы с БД
        private readonly MyDBContext context;
        public RepositoryHall(MyDBContext context)
        {
            this.context = context;
        }
        //Получаем все помещения
        public List<Hall> GetAll() => context.Halls.ToList();
        //Получаем зал по ID
        public Hall GetByID(int id) => context.Halls.FirstOrDefault(add => add.ID == id);
        //Создаем поле
        public void Create(Hall entity)
        {
            try
            {
                context.Halls.Add(entity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Обовление поля
        public void Update(Hall entity)
        {
            try
            {
                context.Halls.Update(entity);
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
                Hall service = context.Halls.Find(id);
                context.Halls.Remove(service);
                context.SaveChanges();
            }catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
           
        }
    }
}
