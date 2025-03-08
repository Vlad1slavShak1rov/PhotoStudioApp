using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoStudioApp.Database.DAL
{
    public class RepositoryCustomer : IRepository<Customer>
    {
        //Конктест с работой БД
        private readonly MyDBContext context;
        public RepositoryCustomer(MyDBContext context)
        {
            this.context = context;
        }
        //Получение всех клинтов
        public List<Customer> GetAll() => context.Customers.ToList();
        //Получение клиента по ID
        public Customer GetByID(int id) => context.Customers.FirstOrDefault(add => add.ID == id);
        //Получение клиента по ID User
        public Customer GetByUserID(int id) => context.Customers.FirstOrDefault(add => add.UserID == id);
        //Создание нового поля
        public void Create(Customer entity)
        {
            try
            {
                context.Customers.Add(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Обновление поля
        public void Update(Customer entity)
        {
            try
            {
                context.Customers.Update(entity);
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
                Customer service = context.Customers.Find(id);
                context.Customers.Remove(service);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
    }
}
