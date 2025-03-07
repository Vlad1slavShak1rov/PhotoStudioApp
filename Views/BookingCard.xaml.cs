using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
    /// Логика взаимодействия для BookingCard.xaml
    /// </summary>
    public partial class BookingCard : UserControl
    {
        public BookingCard(Booking booking, bool IsAdmin)
        {
            InitializeComponent();
            InitData(booking, IsAdmin);
        }

        //Инициализация карточки с заказами текущего клиента
        private void InitData(Booking booking, bool IsAdmin)
        {
            BookingCardExpander.Header = $"Запись на {booking.DateBooking}";

            using var context = new MyDBContext();
            RepositoryWorker repositoryWorker = new(context);
            RepositoryHall repositoryHall = new(context);
            RepositoryAdditionalService repositoryAdditionalService = new(context);
            RepositoryServices repositoryServices = new(context);

            var photograph = repositoryWorker.GetByIDPhotograph(booking.PhotographID);
            var visagiste = repositoryWorker.GetByIDVisagiste(booking.VisagisteID);
            var hall = repositoryHall.GetByID(booking.HallID);
            var service = repositoryServices.GetByID(booking.ID);
            AdditionalService addService = null;

            int? addServiceID = booking.AdditionalServicesID;
            if(addServiceID != null) addService = repositoryAdditionalService.GetByID(addServiceID!.Value);

            PhotographLabel.Content = $"Фотограф: {photograph.Name} {photograph.LastName}";
            VisagisteLabel.Content = $"Визажист: {visagiste.Name} {visagiste.LastName}";
            ServiceLabel.Content = $"Услуга: {service.ServiceName}";

            if (addService != null) AddServiceLabel.Content = $"Дополнителньая услуга: {addService.ServiceName}";
            else AddServiceLabel.Content = $"Дополнителньая услуга: Не выбрана";

            HallLabel.Content = $"Холл: {hall.Description}";
            CostLabel.Content = $"Стоимость: {booking.CostServices}";

            //Показываем или прячем кнопки администратора
            AdminPanel.Visibility = IsAdmin ? Visibility.Visible: Visibility.Collapsed;
        }
    }
}
