using PhotoStudioApp.Database.DBContext;
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
        private readonly MyDBContext context;
        public RepositoryPayment(MyDBContext context)
        {
            this.context = context;
        }
        public List<Payment> GetAll() => context.Payments.ToList();
        public Payment GetByID(int id) => context.Payments.FirstOrDefault(add => add.ID == id);
        public void Create(Payment entity)
        {
            context.Payments.Add(entity);
            context.SaveChanges();
        }
        public void Update(Payment entity)
        {
            context.Payments.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Payment service = context.Payments.Find(id);
            context.Payments.Remove(service);
            context.SaveChanges();
        }
    }
}
