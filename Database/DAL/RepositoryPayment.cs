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
    public class RepositoryPayment : IRepository<Payment>
    {
        //Контекст для работы с БД
        private readonly MyDBContext context;
        public RepositoryPayment(MyDBContext context)
        {
            this.context = context;
        }
        //Получаем всю информацию об оплате
        public List<Payment> GetAll() => context.Payments.ToList();
        //Получаем оплату по ID
        public Payment GetByID(int id) => context.Payments.FirstOrDefault(add => add.ID == id);
        //Создание нового поля 
        public void Create(Payment entity)
        {
            try
            {
                context.Payments.Add(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }

        //Обновление поля
        public void Update(Payment entity)
        {
            try
            {
                context.Payments.Update(entity);
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
                Payment service = context.Payments.Find(id);
                context.Payments.Remove(service);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
    }
}
