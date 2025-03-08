using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoStudioApp.Database.DAL
{
    //Интерфейс для реализации классов
    //Используется паттерн для DAL Репозиторий
    interface IRepository<T> where T:class
    {
        List<T> GetAll();
        T GetByID(int ID);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
