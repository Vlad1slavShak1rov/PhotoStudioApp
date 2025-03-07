using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoStudioApp.Views
{
    /// <summary>
    /// Логика взаимодействия для MyBooking.xaml
    /// </summary>
    public partial class MyBooking : UserControl
    {
        public MyBooking(User user)
        {
            InitializeComponent();

            if ((Enums.Role)user.Role == Enums.Role.Customer)
                InitData(user);
            else if ((Enums.Role)user.Role == Enums.Role.Worker)
                InitDataWorker(user);
            else
                InitAll();
        }
        
        private void InitData(User user)
        {
            using var context = new MyDBContext();

            RepositoryCustomer repositoryCustomer = new(context);
            RepositoryBooking repositoryBooking = new(context);
            RepositoryReview repositoryReview = new(context);

            var customer = repositoryCustomer.GetByUserID(user.ID);
            var bookingList = repositoryBooking.GetAllByCustomer(customer.ID);

            foreach(var booking in bookingList)
            {
                //Показываем MessageBox если время бронирования меньше, чем текущее время
                if (booking.DateBooking < DateTime.Now && repositoryReview.GetByBookingID(booking.ID) == null)
                {
                    Message.Info($"Запись {booking.DateBooking.Date} без отзыва\nПожалуйста, оставьте свои впечатления о фотоссесии!");
                }

                BookingCard bookingCard = new(booking, false, MainGrid);
                MainPanel.Children.Add(bookingCard);
            }
        }

        //Метод загружает заказы конкретного рабочего
        private void InitDataWorker(User user)
        {
            using var context = new MyDBContext();

            RepositoryWorker repositoryWorker= new(context);
            RepositoryBooking repositoryBooking = new(context);
            RepositoryReview repositoryReview = new(context);

            var worker = repositoryWorker.GetByUserID(user.ID);
            if(worker != null)
            {
                List<Booking> bookingList;
                if ((Enums.Post)worker.Post == Enums.Post.Photograph)
                {
                    bookingList = repositoryBooking.GetAllByPhotograph(worker.ID);
                }
                else
                {
                    bookingList = repositoryBooking.GetAllByVisagiste(worker.ID);
                }

                foreach (var booking in bookingList)
                {
                    BookingCard bookingCard = new(booking, false, MainGrid);
                    MainPanel.Children.Add(bookingCard);
                }
            }
        }

        //Загрузка всего списка бронирования для администратора
        private void InitAll()
        {
            using var context = new MyDBContext();
            RepositoryBooking repositoryBooking = new(context);
            var bookingList = repositoryBooking.GetAll();

            foreach (var booking in bookingList)
            {
                BookingCard bookingCard = new(booking, true, MainGrid);
                MainPanel.Children.Add(bookingCard);
            }
        }
    }
}
