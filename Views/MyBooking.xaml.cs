using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
using PhotoStudioApp.Windows;
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
        private BookingCard bookingCard;
        private List<Booking> bookings;
        public MyBooking(User user)
        {
            InitializeComponent();

            if ((Enums.Role)user.Role == Enums.Role.Customer)
                _ = InitData(user);
            else if ((Enums.Role)user.Role == Enums.Role.Worker)
                _ = InitDataWorker(user);
            else
                _ = InitAll();
        }
        
        private async Task InitData(User user)
        {
            CustomerApiService customerApiService = new();
            BookingApiService bookingApiService =new();
            ReviewApiService reviewApiService = new();

            var customer = await customerApiService.GetByUserId(user.ID);
            bookings = await bookingApiService.GetAllByCustomer(customer.ID);

            foreach(var booking in bookings)
            {
                //Показываем MessageBox если время бронирования меньше, чем текущее время
                if (booking.DateBooking < DateTime.Now && await reviewApiService.GetByBookingID(booking.ID) == null)
                {
                    Message.Info($"Запись {booking.DateBooking.Date} без отзыва\nПожалуйста, оставьте свои впечатления о фотоссесии!");
                }

                bookingCard = new(booking, false, MainGrid);
                bookingCard.Update += BookingCard_Update;
                MainPanel.Children.Add(bookingCard);
            }
        }

        //Метод загружает заказы конкретного рабочего
        private async Task InitDataWorker(User user)
        {
            WorkerApiService workerApiService = new();
            BookingApiService bookingApiService = new();
            ReviewApiService reviewApiService = new();


            var worker = await workerApiService.GetByUserId(user.ID);
            if(worker != null)
            {
                List<Booking> bookingList;
                if ((Enums.Post)worker.Post == Enums.Post.Photograph)
                {
                    bookingList = await bookingApiService.GetAllByPhotograph(worker.ID);
                }
                else
                {
                    bookingList = await bookingApiService.GetAllByVisagiste(worker.ID);
                }

                foreach (var booking in bookingList)
                {
                    bookingCard = new(booking, false, MainGrid);
                    bookingCard.Update += BookingCard_Update;
                    MainPanel.Children.Add(bookingCard);
                }
            }
        }

        //Обновление записей для администратора
        private async void BookingCard_Update(object? sender, EventArgs e)
        {
            await InitAll();
        }

        //Загрузка всего списка бронирования для администратора
        private async Task InitAll()
        {
            btCreateTable.Visibility = Visibility.Visible;
            MainPanel.Children.Clear();

            BookingApiService bookingApiService = new();
            bookings = await bookingApiService.GetAll();

            foreach (var booking in bookings)
            {
                bookingCard = new(booking, true, MainGrid);
                bookingCard.Update += BookingCard_Update;
                MainPanel.Children.Add(bookingCard);
            }
        }

        private void btCreateTable_Click(object sender, RoutedEventArgs e)
        {
            CreateTableWindow createTableWindow = new(bookings);
            createTableWindow.ShowDialog();
        }
    }
}
