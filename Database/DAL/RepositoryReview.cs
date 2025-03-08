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
    public class RepositoryReview:IRepository<Review>
    {
        //Контекст для работы с бД
        private readonly MyDBContext context;
        public RepositoryReview(MyDBContext context)
        {
            this.context = context; 
        }
        //Получаем все отзывы
        public List<Review> GetAll() => context.Reviews.ToList();
        //Получаем отзыв по ID
        public Review GetByID(int id) => context.Reviews.FirstOrDefault(add => add.ID == id);
        //Получаем отзыв по ID брони
        public Review GetByBookingID(int id) => context.Reviews.FirstOrDefault(add => add.BookingID == id);
        //Получаем отзыв по ID клиента
        public Review GetByCustomer(int id) => context.Reviews.FirstOrDefault(add => add.CustomerID == id);
        //Создание нового поля
        public void Create(Review entity)
        {
            try
            {
                context.Reviews.Add(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Обновление поля
        public void Update(Review entity)
        {
            try
            {
                context.Reviews.Update(entity);
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
                Review service = context.Reviews.Find(id);
                context.Reviews.Remove(service);
                context.SaveChanges();
            }catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
           
        }
    }
}
