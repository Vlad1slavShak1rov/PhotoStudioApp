using Microsoft.EntityFrameworkCore.Query.Internal;
using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
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
    /// Логика взаимодействия для EditBooking.xaml
    /// </summary>
    public partial class EditBooking : UserControl
    {
        private Booking _booking;
        private Worker photograph = null;
        private Worker visagiste = null;

        public event EventHandler Close;
        int sum = 0;
        public EditBooking(Booking booking)
        {
            InitializeComponent();
            _booking = booking;
            _ = InitData();
        }

        private async Task InitData()
        {
            ServiceApiService serviceApiService = new();
            AdditionalServiceApi additionalServiceApi = new();
            HallApiService hallApiService = new();
            WorkerApiService workerApiService = new();

            //Загружаем все студии
            HallBox.ItemsSource = await hallApiService.GetAll();
            //Показываем только описание
            HallBox.DisplayMemberPath = "Description";

            //Загружаем визажистов
            VisagisteBox.ItemsSource = await workerApiService.GetAllVisagiste();
            //Показываем только полное имя
            VisagisteBox.DisplayMemberPath = "FullName";
            
            //Загружаем фотографов
            PhotographBox.ItemsSource = await workerApiService.GetAllPhotograph();
            //Показываем только полное имя
            PhotographBox.DisplayMemberPath = "FullName";

            //Загружаем все основные услуги
            var serviceList = await serviceApiService.GetAll();
            foreach(var serv in serviceList)
            {
                ServiceBox.Items.Add(serv);
                //Показываем только название услуг
                ServiceBox.DisplayMemberPath = "ServiceName";
            }

            //Загружаем все дополнительные услуги 
            var addServList = await additionalServiceApi.GetAll();
            foreach(var addServ in addServList)
            {
                AddServiceBox.Items.Add(addServ);
                //Показываем только название услуг
                AddServiceBox.DisplayMemberPath = "ServiceName";
            }

            //Получаем все модели, связанные в таблице Booking
            if (_booking.PhotographID != null || _booking.VisagisteID != null)
            {
                photograph = await workerApiService.GetByPhotograph(_booking.PhotographID!.Value);
                visagiste = await workerApiService.GetByVisagiste(_booking.VisagisteID!.Value);
            }
            var hall = await hallApiService.GetById(_booking.HallID);
            var service = await serviceApiService.GetById(_booking.ServiceID);
            AdditionalService addService = null;

            int? addServiceID = _booking.AdditionalServicesID;
            if (addServiceID != null) addService = await additionalServiceApi.GetById(addServiceID!.Value);

            //Выводим текущие элементы выбранного бронирования
            PhotographBox.SelectedItem = photograph == null ? null : photograph;
            VisagisteBox.SelectedItem = visagiste == null ? null : visagiste;
            ServiceBox.SelectedItem = service;
            AddServiceBox.SelectedItem = addService;
            HallBox.SelectedItem = hall;
            sum = service.CostService + (int)addService.Cost;
            CostBox.Text = sum.ToString();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPhotograph = PhotographBox.SelectedItem as Worker;
            var selectedVisagiste = VisagisteBox.SelectedItem as Worker;
            var hall = HallBox.SelectedItem as Hall;
            var service = ServiceBox.SelectedItem as Services;
            var addService = AddServiceBox.SelectedItem as AdditionalService;
            var cost = sum;

            WorkerApiService workerApiService = new();

            photograph.IsOnBookin = false;
            visagiste.IsOnBookin = false;

            var dto = ConvertToDTO.ToWorkerDTO(photograph);
            await workerApiService.Update(dto);
            dto = ConvertToDTO.ToWorkerDTO(visagiste);
            await workerApiService.Update(dto);

            if (selectedPhotograph != null)
            {
                selectedPhotograph.IsOnBookin = true;
                dto = ConvertToDTO.ToWorkerDTO(selectedPhotograph);
                await workerApiService.Update(dto);
            }
            if (selectedVisagiste != null)
            {
                selectedVisagiste.IsOnBookin = true;
                dto = ConvertToDTO.ToWorkerDTO(selectedVisagiste);
                await workerApiService.Update(dto);
            }

            BookingApiService bookingApiService = new();

            _booking.PhotographID = selectedPhotograph == null ? null : selectedPhotograph.ID;
            _booking.VisagisteID = selectedVisagiste == null ? null : selectedVisagiste.ID;
            _booking.HallID = hall.ID;
            _booking.ServiceID = service.ID;
            _booking.AdditionalServicesID = addService.ID;
            _booking.CostServices = cost;

            var bookingDTO = ConvertToDTO.ToBookingDTO(_booking);

          
            await bookingApiService.Update(bookingDTO);
            Message.Success("Успешно!");

            Close?.Invoke(this,e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close?.Invoke(this, e);
        }

        private void AddServiceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int total = 0;

            // Учитываем стоимость зала
            if (HallBox.SelectedItem is Hall hall)
            {
                total += (int)hall.Cost;
            }

            // Учитываем основную услугу
            if (ServiceBox.SelectedItem is Services service)
            {
                total += service.CostService;
            }

            // Учитываем дополнительную услугу
            if (AddServiceBox.SelectedItem is AdditionalService additional)
            {
                total += (int)additional.Cost;
            }

            sum = total;
            CostBox.Text = sum.ToString();
        }
    }
}
