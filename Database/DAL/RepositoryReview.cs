using PhotoStudioApp.Database.DBContext;
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
        private readonly MyDBContext context;
        public RepositoryReview(MyDBContext context)
        {
            this.context = context; 
        }
        public List<Review> GetAll() => context.Reviews.ToList();
        public Review GetByID(int id) => context.Reviews.FirstOrDefault(add => add.ID == id);
        public Review GetByBookingID(int id) => context.Reviews.FirstOrDefault(add => add.BookingID == id);
        public Review GetByCustomer(int id) => context.Reviews.FirstOrDefault(add => add.CustomerID == id);
        public void Create(Review entity)
        {
            context.Reviews.Add(entity);
            context.SaveChanges();
        }
        public void Update(Review entity)
        {
            context.Reviews.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Review service = context.Reviews.Find(id);
            context.Reviews.Remove(service);
            context.SaveChanges();
        }
    }
}
