using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
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
        private Booking _currentBooking;
        private Services mainService;
        private AdditionalService additionalService;
        private Customer _customer;
        private Hall _hall;

        private Grid _mainGrid;
        private EditBooking editBooking;
        public event EventHandler Update;
        public BookingCard(Booking booking, bool IsAdmin, Grid main)
        {
            InitializeComponent();
            _currentBooking = booking;
            _mainGrid = main;
            InitData(booking, IsAdmin);
        }

        //Инициализация карточки с заказами текущего клиента
        private async void InitData(Booking booking, bool IsAdmin)
        {
            BookingCardExpander.Header = $"Запись на {booking.DateBooking}";

            WorkerApiService workerApiService = new();
            HallApiService hallApiService = new();
            AdditionalServiceApi additionalServiceApi = new();
            ServiceApiService serviceApiService = new();
            CustomerApiService customerApiService = new();

            _customer = await customerApiService.GetById(booking.CustomerID);

            Worker photograph = null, visagiste = null;
            if (booking.PhotographID ==null || booking.VisagisteID == null)
            {
                photograph = await workerApiService.GetByPhotograph(booking.PhotographID!.Value);
                visagiste = await workerApiService.GetByVisagiste(booking.VisagisteID!.Value);
            }
          
            _hall = await hallApiService.GetById(booking.HallID);
            mainService = await serviceApiService.GetById(booking.ServiceID);

            int? addServiceID = booking.AdditionalServicesID;
            if(addServiceID != null) additionalService = await additionalServiceApi.GetById(addServiceID!.Value);

            string photographText = photograph == null ? "Без фотографа" : $"{photograph.Name} {photograph.LastName}";
            string visagisteText = visagiste == null ? "Без визажиста" : $"{visagiste.Name} {visagiste.LastName}";

            PhotographLabel.Content = $"Фотограф: {photographText}";
            VisagisteLabel.Content = $"Визажист: {visagisteText}";
            ServiceLabel.Content = $"Услуга: {mainService.ServiceName}";

            if (additionalService != null) AddServiceLabel.Content = $"Дополнителньая услуга: {additionalService.ServiceName}";
            else AddServiceLabel.Content = $"Дополнителньая услуга: Не выбрана";

            HallLabel.Content = $"Холл: {_hall.Description}";
            CostLabel.Content = $"Стоимость: {booking.CostServices}";

            //Показываем или прячем кнопки администратора
            AdminPanel.Visibility = IsAdmin ? Visibility.Visible: Visibility.Collapsed;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            editBooking = new(_currentBooking);

            editBooking.HorizontalAlignment = HorizontalAlignment.Center;
            editBooking.VerticalAlignment = VerticalAlignment.Top;
            editBooking.Margin = new Thickness(0, 10, 0, 0);

            editBooking.Close += EditBooking_Close;
            _mainGrid.Children.Add(editBooking);
        }

        private void EditBooking_Close(object? sender, EventArgs e)
        {
            _mainGrid.Children.Remove(editBooking);
            editBooking.Close -= EditBooking_Close;
            editBooking = null;
            Update?.Invoke(this, e);
        }

        private void btCreateReceipt_Click(object sender, RoutedEventArgs e)
        {
            CreateFile.CreatePdfReceipt(_customer, mainService, additionalService, _currentBooking, _hall);
        }
    }
}
