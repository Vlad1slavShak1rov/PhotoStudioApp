using Microsoft.EntityFrameworkCore.Query.Internal;
using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
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

            PhotographBox.Text = photograph.Name + " " + photograph.LastName;
            VisagisteBox.Text = visagiste.Name + " " + visagiste.LastName;
            HallBox.Text = hall.Description;

            ServiceBox.SelectedItem = service;
            AddServiceBox.SelectedItem = addService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

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
