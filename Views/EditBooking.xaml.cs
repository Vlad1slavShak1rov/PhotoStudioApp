using Microsoft.EntityFrameworkCore.Query.Internal;
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
    /// Логика взаимодействия для EditBooking.xaml
    /// </summary>
    public partial class EditBooking : UserControl
    {
        private Booking _booking;
        public event EventHandler Close;
        int sum = 0;
        public EditBooking(Booking booking)
        {
            InitializeComponent();
            _booking = booking;
            InitData();
        }

        private void InitData()
        {
            using var context = new MyDBContext();
            RepositoryWorker repositoryWorker = new(context);
            RepositoryHall repositoryHall = new(context);
            RepositoryAdditionalService repositoryAdditionalService = new(context);
            RepositoryServices repositoryServices = new(context);

            //Загружаем все студии
            HallBox.ItemsSource = repositoryHall.GetAll();
            //Показываем только описание
            HallBox.DisplayMemberPath = "Description";

            //Загружаем визажистов
            VisagisteBox.ItemsSource = repositoryWorker.GetAllVisagiste();
            //Показываем только полное имя
            VisagisteBox.DisplayMemberPath = "FullName";
            
            //Загружаем фотографов
            PhotographBox.ItemsSource = repositoryWorker.GetAllPhotograph();
            //Показываем только полное имя
            PhotographBox.DisplayMemberPath = "FullName";

            //Загружаем все основные услуги
            var serviceList = repositoryServices.GetAll();
            foreach(var serv in serviceList)
            {
                ServiceBox.Items.Add(serv);
                //Показываем только название услуг
                ServiceBox.DisplayMemberPath = "ServiceName";
            }

            //Загружаем все дополнительные услуги 
            var addServList = repositoryAdditionalService.GetAll();
            foreach(var addServ in addServList)
            {
                AddServiceBox.Items.Add(addServ);
                //Показываем только название услуг
                AddServiceBox.DisplayMemberPath = "ServiceName";
            }

            //Получаем все модели, связанные в таблице Booking
            var photograph = repositoryWorker.GetByIDPhotograph(_booking.PhotographID);
            var visagiste = repositoryWorker.GetByIDVisagiste(_booking.VisagisteID);
            var hall = repositoryHall.GetByID(_booking.HallID);
            var service = repositoryServices.GetByID(_booking.ServiceID);
            AdditionalService addService = null;

            int? addServiceID = _booking.AdditionalServicesID;
            if (addServiceID != null) addService = repositoryAdditionalService.GetByID(addServiceID!.Value);

            //Выводим текущие элементы выбранного бронирования
            PhotographBox.SelectedItem = photograph;
            VisagisteBox.SelectedItem = visagiste;
            ServiceBox.SelectedItem = service;
            AddServiceBox.SelectedItem = addService;
            HallBox.SelectedItem = hall;
            sum = service.CostService + (int)addService.Cost;
            CostBox.Text = sum.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var photograph = PhotographBox.SelectedItem as Worker;
            var visagiste = VisagisteBox.SelectedItem as Worker;
            var hall = HallBox.SelectedItem as Hall;
            var service = ServiceBox.SelectedItem as Services;
            var addService = AddServiceBox.SelectedItem as AdditionalService;
            var cost = sum;

            using var context = new MyDBContext();
            RepositoryBooking repositoryBooking = new(context);

            _booking.PhotographID = photograph.ID;
            _booking.VisagisteID = visagiste.ID;
            _booking.HallID = hall.ID;
            _booking.ServiceID = service.ID;
            _booking.AdditionalServicesID = addService.ID;
            _booking.CostServices = cost;

            repositoryBooking.Update(_booking);
            Message.Success("Успешно!");

            Close?.Invoke(this,e);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close?.Invoke(this, e);
        }

        private void AddServiceBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddServiceBox.SelectedItem != null && ServiceBox.SelectedItem != null)
            {
                sum = 0;
                if (AddServiceBox.SelectedIndex == 0) //если выбрана основная услуга
                {
                    if (ServiceBox.SelectedItem is Services service)
                    {
                        sum = service.CostService;
                    }
                }
                else if (AddServiceBox.SelectedIndex == 0) //если выбрана только дополнительная услуга
                {
                    if (AddServiceBox.SelectedItem is AdditionalService additional)
                    {
                        sum = (int)additional.Cost;
                    }
                }
                else //если выбрана основная услуга и дополнителньая услуга
                {
                    if (AddServiceBox.SelectedItem is AdditionalService additional && ServiceBox.SelectedItem is Services service)
                    {
                        sum = (int)additional.Cost + service.CostService;
                    }
                }
                if(CostBox != null)
                {
                    CostBox.Text = $"Итого: {sum} р";
                }
            }  
        }
    }
}
