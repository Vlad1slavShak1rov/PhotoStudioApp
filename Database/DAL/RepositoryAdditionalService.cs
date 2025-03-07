using PhotoStudioApp.Database.DBContext;
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
        private readonly MyDBContext context;
        public RepositoryAdditionalService(MyDBContext context)
        {
            this.context = context;
        }
        public List<AdditionalService> GetAll() => context.AdditionalServices.ToList();
        public AdditionalService GetByID(int id) => context.AdditionalServices.FirstOrDefault(add => add.ID == id);
        public void Create(AdditionalService entity)
        {
            context.AdditionalServices.Add(entity);
            context.SaveChanges();
        }
        public void Update(AdditionalService entity)
        {
            context.AdditionalServices.Update(entity);
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            AdditionalService service = context.AdditionalServices.Find(id);
            context.AdditionalServices.Remove(service);
            context.SaveChanges();
        }
    }
}
