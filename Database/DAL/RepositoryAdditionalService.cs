using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Database.DAL
{
    public class RepositoryAdditionalService:IRepository<AdditionalService>
    {
        //Контекст для базы данных
        private readonly MyDBContext context;
        public RepositoryAdditionalService(MyDBContext context)
        {
            this.context = context;
        }
        //Получаем все записи
        public List<AdditionalService> GetAll() => context.AdditionalServices.ToList();
        //Получаем по ID
        public AdditionalService GetByID(int id) => context.AdditionalServices.FirstOrDefault(add => add.ID == id);
        //Получаем по Имени
        public AdditionalService GetByName(string name) => context.AdditionalServices.FirstOrDefault(addSer => addSer.ServiceName == name);
        //Созздание нового поля
        public void Create(AdditionalService entity)
        {
            try
            {
                context.AdditionalServices.Add(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Обновление поля
        public void Update(AdditionalService entity)
        {
            try
            {
                context.AdditionalServices.Update(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Удаление записи
        public void Delete(int id)
        {
            try
            {
                AdditionalService service = context.AdditionalServices.Find(id);
                context.AdditionalServices.Remove(service);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
    }
}
