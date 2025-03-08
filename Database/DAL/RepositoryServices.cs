using PhotoStudioApp.Model;
using PhotoStudioApp.Database.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoStudioApp.Helper;

namespace PhotoStudioApp.Database.DAL
{
    public class RepositoryServices : IRepository<Services>
    {
        //Контекст для работы с БД
        private readonly MyDBContext context;
        public RepositoryServices(MyDBContext context)
        {
            this.context = context;
        }
        //Получаем все записи об услуги
        public List<Services> GetAll() => context.Services.ToList();
        //Получаем услугу по его ID
        public Services GetByID(int id) => context.Services.FirstOrDefault(add => add.ID == id);
        //Получаем услугу по названию
        public Services GetByName(string name) => context.Services.FirstOrDefault(ser => ser.ServiceName == name);
        //Создание нового поля
        public void Create(Services entity)
        {
            try
            {
                context.Services.Add(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
           
        }
        //Обновление поля
        public void Update(Services entity)
        {
            try
            {
                context.Services.Update(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Удалениен поля
        public void Delete(int id)
        {
            try
            {
                Services service = context.Services.Find(id);
                context.Services.Remove(service);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
    }
}
