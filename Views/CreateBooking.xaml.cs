using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Enums;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Migrations;
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
    /// Логика взаимодействия для CreateBooking.xaml
    /// </summary>
    public partial class CreateBooking : UserControl
    {
        private User _user;
        int sum = 0;
        public event EventHandler CloseButton; //События для того, чтобы закрыть UserControl
        public CreateBooking(User user)
        {
            InitializeComponent();
            _user = user;
            InitData();
            this.Loaded +=(e,s) => ServicesComboBox.SelectionChanged += AllCombobox_SelectionChanged;
            this.Loaded += (e, s) => AdditionalServicesCombobox.SelectionChanged += AllCombobox_SelectionChanged;
        }

        //Инициализирует ComboBox
        private void InitData() 
        {
            using var context = new MyDBContext();

            RepositoryServices repositoryServices = new(context);
            RepositoryAdditionalService repositoryAdditionalService = new(context);
            RepositoryHall repositoryHall = new(context);
            RepositoryWorker repositoryWorker = new(context);

            var servicesList = repositoryServices.GetAll();
            var additionalList = repositoryAdditionalService.GetAll();
            var hallList = repositoryHall.GetAll();
            var workerList = repositoryWorker.GetAll();

            InitComboBox<Services>(ServicesComboBox, servicesList);
            InitComboBox<AdditionalService>(AdditionalServicesCombobox, additionalList);
            InitComboBox<Hall>(HallComboBox, hallList);
            InitComboBox<Worker>(null, workerList);
        }

        //Мы используем обобщение чтобы метод мог принимать
        //списки разных типо
        private void InitComboBox<T>(ComboBox comboBox, List<T> list)
        {

            foreach (var item in list)
            {
                if (item is Worker worker) // Если список с рабочими
                {
                    if ((Enums.Post)worker.Post == Enums.Post.Photograph) 
                    {
                        WorkerCombobox.Items.Add(worker);
                        WorkerCombobox.DisplayMemberPath = "FullName";
                    }
                    else
                    {
                        VisagisteCombobox.Items.Add(worker); // Сохраняем объект worker
                        VisagisteCombobox.DisplayMemberPath = "FullName";
                    }
                }
                else if (item is Services services)
                {
                    comboBox.Items.Add(services); // Добавляем сам объект Services
                    comboBox.DisplayMemberPath = "ServiceName";
                }
                else if (item is Hall hall)
                {
                    comboBox.Items.Add(hall); // Добавляем сам объект Hall
                    comboBox.DisplayMemberPath = "Description";
                }
                else if (item is AdditionalService additional)
                {
                    comboBox.Items.Add(additional); // Добавляем сам объект AdditionalService
                    comboBox.DisplayMemberPath = "ServiceName";
                }
            }
        }

        private void AllCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdditionalServicesCombobox.SelectedItem != null && ServicesComboBox.SelectedItem != null)
            {
                sum = 0;
                if (AdditionalServicesCombobox.SelectedIndex == 0) //если выбрана основная услуга
                {
                    if (ServicesComboBox.SelectedItem is Services service)
                    {
                        sum = service.CostService;
                    }
                }
                else if (ServicesComboBox.SelectedIndex == 0) //если выбрана только дополнительная услуга
                {
                    if (AdditionalServicesCombobox.SelectedItem is AdditionalService additional)
                    {
                        sum = (int)additional.Cost;
                    }
                }
                else //если выбрана основная услуга и дополнителньая услуга
                {
                    if (AdditionalServicesCombobox.SelectedItem is AdditionalService additional && ServicesComboBox.SelectedItem is Services service)
                    {
                        sum = (int)additional.Cost + service.CostService;
                    }
                }
                AmountBlock.Text = $"Итого: {sum} р";
            }
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            using var context = new MyDBContext();
            RepositoryBooking repositoryBooking = new(context);
            RepositoryCustomer repositoryCustomer = new(context);
            RepositoryPayment repositoryPayment = new(context);

            //Собираем данные с комбобоксов и преобразуем их в нужный тип
            var photograph = WorkerCombobox.SelectedItem as Worker;
            var visagiste = VisagisteCombobox.SelectedItem as Worker;
            var hall = HallComboBox.SelectedItem as Hall;
            var services = ServicesComboBox.SelectedItem as Services;
            var duration = DateBookingBox.Text;
            var cost = sum;

            if (DateTime.TryParse(duration,out DateTime date))
            {
                if(date <= DateTime.Now.Date)
                {
                    Message.Warning("Дата бронирования должна быть больше текущей!");
                    return;
                }
            }

            if (photograph != null && visagiste != null && hall != null && services != null)
            {
                int? addServiceID = null;
                var customer = repositoryCustomer.GetByUserID(_user.ID);
                if (AdditionalServicesCombobox.SelectedItem is AdditionalService atributeService)
                {
                    addServiceID = atributeService.ID;
                }

                //Инициализации новой брони
                Booking booking = new()
                {
                    CustomerID = customer.ID,
                    PhotographID = photograph.ID,
                    VisagisteID = visagiste.ID,
                    HallID = hall.ID,
                    ServiceID = services.ID,
                    AdditionalServicesID = addServiceID,
                    CostServices = sum,
                    DateBooking = date.Date
                };

                repositoryBooking.Create(booking);
                var payment = PaymentMethodBox.SelectedItem as ComboBoxItem;

                //Присваиваем значение method способ оплаты через Enum
                Enums.PaymentMethod method;
                if (payment.Content == "Наличными") method = PaymentMethod.Cash;
                else method = PaymentMethod.Cards;

                //Добавляем чек
                Payment paymentNew = new()
                {
                    BookingID = booking.ID,
                    Amount = (decimal)booking.CostServices,
                    PaymentMethod = method,
                    PaymentDate = DateTime.Now,
                };

                repositoryPayment.Create(paymentNew);

                Message.Success("Успешно!");

                CloseButton?.Invoke(this,e);
            }
        }
    }
}
