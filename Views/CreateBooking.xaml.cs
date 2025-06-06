using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.UniversalAccessibility.Drawing;
using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Enums;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PhotoStudioApp.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateBooking.xaml
    /// </summary>
    public partial class CreateBooking : UserControl
    {
        private User _user;
        private Customer _customer;
        int sum = 0;
        int bonusSum = 0;
        public event EventHandler CloseButton; //События для того, чтобы закрыть UserControl
        public CreateBooking(User user)
        {
            InitializeComponent();
            _user = user;

            _ = InitCustomer();
            _ = InitData();

            this.Loaded +=(e,s) => ServicesComboBox.SelectionChanged += AllCombobox_SelectionChanged;
            this.Loaded += (e, s) => AdditionalServicesCombobox.SelectionChanged += AllCombobox_SelectionChanged;
            this.Loaded += (e, s) => HallComboBox.SelectionChanged += AllCombobox_SelectionChanged;
        }

        private async Task InitCustomer()
        {
            CustomerApiService customerApiService = new();
            _customer = await customerApiService.GetByUserId(_user.ID);
            if (_customer.Balance == 0)
            {
                selectedBonusPay.IsEnabled = false;
                rbNoUse.IsEnabled = false;
            }
        }

        //Инициализирует ComboBox
        private async Task InitData() 
        {
            ServiceApiService serviceApiService = new();
            AdditionalServiceApi additionalServiceApi = new();
            HallApiService hallApiService = new();
            WorkerApiService workerApiService = new();

            var servicesList = await serviceApiService.GetAll();
            var additionalList = await additionalServiceApi.GetAll();
            var hallList = await hallApiService.GetAll();
            var workerList = await workerApiService.GetAll();

            InitComboBox<Services>(ServicesComboBox, servicesList);
            InitComboBox<AdditionalService>(AdditionalServicesCombobox, additionalList);
            InitComboBox<Hall>(HallComboBox, hallList);
            InitComboBox<Worker>(null, workerList);
        }

        //Мы используем обобщение чтобы метод мог принимать
        //списки разных типы
        private async void InitComboBox<T>(ComboBox comboBox, List<T> list)
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

        private async void AllCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int total = 0;
            int bonusTotal = 0;

            // Учитываем стоимость зала
            if (HallComboBox.SelectedItem is Hall hall)
            {
                total += (int)hall.Cost;
            }

            // Учитываем основную услугу
            if (ServicesComboBox.SelectedItem is Services service)
            {
                total += service.CostService;
                bonusTotal += (int)(service.BonusCost * 0.01);
            }

            // Учитываем дополнительную услугу
            if (AdditionalServicesCombobox.SelectedItem is AdditionalService additional)
            {
                total += (int)additional.Cost;
                bonusTotal += (int)(additional.BonusCost * 0.01);
            }

            sum = total;
            bonusSum = bonusTotal;
            AmountBlock.Text = $"Итого: {sum} р";
        }

        private async void PayButton_Click(object sender, RoutedEventArgs e)
        {
            BookingApiService bookingApiService = new();
            CustomerApiService customerApiService = new();
            PaymentsApiService paymentsApiService = new();
            HistoryPointsReceivedApi historyPointsReceivedApi = new();

            //Собираем данные с комбобоксов и преобразуем их в нужный тип
            var photograph = WorkerCombobox.SelectedItem as Worker;
            var visagiste = VisagisteCombobox.SelectedItem as Worker;
            var hall = HallComboBox.SelectedItem as Hall;
            var services = ServicesComboBox.SelectedItem as Services;
            if(DateBookingBox.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату для бронирования!");
                return;
            }
            var date = DateBookingBox.SelectedDate.Value;
            var cost = sum;

            if (date <= DateTime.Now.Date)
            {
                Message.Warning("Дата бронирования должна быть больше текущей!");
                return;
            }

            if (hall != null && services != null)
            {
                int? addServiceID = null;
                
                if (AdditionalServicesCombobox.SelectedItem is AdditionalService atributeService)
                {
                    addServiceID = atributeService.ID;
                }

                HistoryPointsReceivedDTO historyPointsReceived = new();

                if (tbCountPoints.IsEnabled)
                {
                    int bonus = int.Parse(tbCountPoints.Text);
                    _customer.Balance -= bonus;

                    historyPointsReceived = new()
                    {
                        CustomerID = _customer.ID,
                        Point = bonus,
                        Type = TypeAdmission.Потрачено,
                        Date = DateTime.Now.Date
                    };

                    await historyPointsReceivedApi.Create(historyPointsReceived);
                }

                historyPointsReceived = new()
                {
                    CustomerID = _customer.ID,
                    Point = bonusSum,
                    Type = TypeAdmission.Поступление,
                    Date = DateTime.Now.Date
                };
                await historyPointsReceivedApi.Create(historyPointsReceived);

                _customer.Balance += bonusSum;
                Message.Info($"Вам было начислено: {bonusSum} бонусов!");

                var customerDTO = ConvertToDTO.ToCustomerDTO( _customer );
                await customerApiService.Update(customerDTO);

                using var context = new MyDBContext();

                //Инициализации новой брони
                Booking booking = new()
                {
                    CustomerID = _customer.ID,
                    PhotographID = photograph == null ? null : photograph.ID,
                    VisagisteID = visagiste == null ? null : visagiste.ID,
                    HallID = hall.ID,
                    ServiceID = services.ID,
                    AdditionalServicesID = addServiceID,
                    CostServices = sum,
                    DateBooking = date.Date
                };

                var bookingDTO = ConvertToDTO.ToBookingDTO(booking);
                await bookingApiService.Create(bookingDTO);

                WorkerApiService workerApi = new();
                if (photograph != null)
                {
                    var dto = ConvertToDTO.ToWorkerDTO(photograph);
                    dto.IsOnBookin = true;
                    await workerApi.Update(dto);
                } 
                if(visagiste != null)
                {
                    var dto = ConvertToDTO.ToWorkerDTO(visagiste);
                    dto.IsOnBookin = true;
                    await workerApi.Update(dto);
                }


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

                var paymentDTO = ConvertToDTO.ToPaymentsDTO(paymentNew);
                await paymentsApiService.Create(paymentDTO);

                var mainService = ServicesComboBox.SelectedItem as Services;
                var additionalService = AdditionalServicesCombobox.SelectedItem as AdditionalService;
                CreateFile.CreatePdfReceipt(_customer, mainService, additionalService, booking, hall);

                Message.Success("Успешно!");
                CloseButton?.Invoke(this,e);
            }
        }

        private void rbNoUse_Checked(object sender, RoutedEventArgs e)
        {
            if (rbYesUse == null || rbNoUse == null) return;

            if (rbNoUse.IsChecked == true)
            {
                rbYesUse.IsChecked = false;
                tbCountPoints.IsEnabled = false;
                tbCountPoints.Text = "0";

                AllCombobox_SelectionChanged(null, null);
            }
        }

        private void rbYesUse_Checked(object sender, RoutedEventArgs e)
        {
            if (rbYesUse == null || rbNoUse == null) return;

            if (rbYesUse.IsChecked == true)
            {
                rbNoUse.IsChecked = false;
                tbCountPoints.IsEnabled = true;
            }
        }

        private void tbCountPoints_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true; 
            }
        }

        private async void tbCountPoints_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_customer == null) return;
            if (!int.TryParse(tbCountPoints.Text, out int bonus) || bonus == 0)
            {
                AllCombobox_SelectionChanged(null,null);
                return;
            }

            if (bonus > _customer.Balance)
            {
                MessageBox.Show("У вас нет такого количества баллов!");
                tbCountPoints.Text = string.Empty;
                return;
            }

            string currentAmount = AmountBlock.Text.Trim().Split(':')[1];
            currentAmount = currentAmount.Remove(currentAmount.Length - 1);
            // Извлечение текущей суммы
            if (!int.TryParse(currentAmount, out int amount))
            {
                MessageBox.Show("Ошибка в текущей сумме!");
                return;
            }

            // Вычисление новой суммы
            int newAmount = amount - bonus * 5;
            AmountBlock.Text = newAmount < 0 ? "Итого: 0 р" : "Итого:" + newAmount.ToString() + " р";
        }
    }
}
