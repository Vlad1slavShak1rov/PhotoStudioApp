using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Database.DAL
{
    public class RepositoryCustomer : IRepository<Customer>
    {
        private readonly MyDBContext context;
        public RepositoryCustomer(MyDBContext context)
        {
            this.context = context;
        }
        public List<Customer> GetAll() => context.Customers.ToList();
        public Customer GetByID(int id) => context.Customers.FirstOrDefault(add => add.ID == id);
        public Customer GetByUserID(int id) => context.Customers.FirstOrDefault(add => add.UserID == id);
        public void Create(Customer entity)
        {
            context.Customers.Add(entity);
            context.SaveChanges();
        }
        public void Update(Customer entity)
        {
            context.Customers.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            Customer service = context.Customers.Find(id);
            context.Customers.Remove(service);
            context.SaveChanges();
        }
    }
}
