using PhotoStudioApp.Database.DBContext;
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
        private readonly MyDBContext context;
        public RepositoryBooking(MyDBContext context)
        {
            this.context = context;
        }
        public List<Booking> GetAll() => context.Bookings.ToList();
        public List<Booking> GetAllByCustomer(int id) => context.Bookings.Where(bkg => bkg.CustomerID == id).ToList();
        public List<Booking> GetAllByPhotograph(int id) => context.Bookings.Where(bkg => bkg.PhotographID == id).ToList();
        public List<Booking> GetAllByVisagiste(int id) => context.Bookings.Where(bkg => bkg.VisagisteID == id).ToList();
        public Booking GetByID(int id) => context.Bookings.FirstOrDefault(add => add.ID == id);
        public Booking GetByPhotographID(int id) => context.Bookings.FirstOrDefault(add => add.PhotographID == id);
        public Booking GetByVisagisteID(int id) => context.Bookings.FirstOrDefault(add => add.VisagisteID == id);
        public Booking GetByServiceID(int id) => context.Bookings.FirstOrDefault(add => add.ServiceID == id);
        public Booking GetByAddServiceID(int id) => context.Bookings.FirstOrDefault(add => add.AdditionalServicesID == id);
        public void Create(Booking entity)
        {
            context.Bookings.Add(entity);
            context.SaveChanges();
        }
        public void Update(Booking entity)
        {
            context.Bookings.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Booking service = context.Bookings.Find(id);
            context.Bookings.Remove(service);
            context.SaveChanges();
        }
    }
}
