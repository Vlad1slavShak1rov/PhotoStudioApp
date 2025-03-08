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
    public class RepositoryBooking : IRepository<Booking>
    {
        private readonly MyDBContext context; //Контекст для работы с БД
        public RepositoryBooking(MyDBContext context)
        {
            this.context = context;
        }
        //Получаем все брони
        public List<Booking> GetAll() => context.Bookings.ToList();
        //Получаем все брони по ID клиента
        public List<Booking> GetAllByCustomer(int id) => context.Bookings.Where(bkg => bkg.CustomerID == id).ToList();
        //Получаем брони фотографа по ID
        public List<Booking> GetAllByPhotograph(int id) => context.Bookings.Where(bkg => bkg.PhotographID == id).ToList();
        //Получаем брони визажиста по ID
        public List<Booking> GetAllByVisagiste(int id) => context.Bookings.Where(bkg => bkg.VisagisteID == id).ToList();
        //Получаем бронь по ID
        public Booking GetByID(int id) => context.Bookings.FirstOrDefault(add => add.ID == id);
        //Получаем одну брони фотографа
        public Booking GetByPhotographID(int id) => context.Bookings.FirstOrDefault(add => add.PhotographID == id);
        //Получаем одну брони визажиста
        public Booking GetByVisagisteID(int id) => context.Bookings.FirstOrDefault(add => add.VisagisteID == id);
        //Получаем бронь по ID основной услуге
        public Booking GetByServiceID(int id) => context.Bookings.FirstOrDefault(add => add.ServiceID == id);
        //Получаем бронь по ID дополнительной услуге
        public Booking GetByAddServiceID(int id) => context.Bookings.FirstOrDefault(add => add.AdditionalServicesID == id);
        //Создание нового поля
        public void Create(Booking entity)
        {
            try
            {
                context.Bookings.Add(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
        //Обновление поля
        public void Update(Booking entity)
        {
            try
            {
                context.Bookings.Update(entity);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
            
        }
        //Удааление поля
        public void Delete(int id)
        {
            try
            {
                Booking service = context.Bookings.Find(id);
                context.Bookings.Remove(service);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
        }
    }
}
