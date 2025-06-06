using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Validator = PhotoStudioApp.Helper.Validator;

namespace PhotoStudioApp.Views
{
    /// <summary>
    /// Логика взаимодействия для SubmitView.xaml
    /// </summary>
    public partial class SubmitView : UserControl
    {
        private int rate = 0;
        private User currentUser;
        public event EventHandler CloseControl;
        public SubmitView(User user)
        {
            InitializeComponent();
            _ = IntiData();
            currentUser = user;
        }

        //Логика реализации загрузки данных. 
        //В нее входит загрузка комбобокса, где хранятся бронирования данного пользователя
        private async Task IntiData()
        {
            MyServiceBox.Items.Clear();

            BookingApiService bookingApi = new();
            ReviewApiService reviewApi = new();

            var reviewsList = await reviewApi.GetAll();
            var bookingList = await bookingApi.GetAll();

            foreach(var booking in bookingList)
            {
                //Если время брони меньше, чем текущее и отзыва нету в таблице Отзывы,
                //То добавляем Item добавляем в MyServiceBox
                if (booking.DateBooking <= DateTime.Now && await reviewApi.GetByBookingID(booking.ID) == null)
                {
                    MyServiceBox.Items.Add(booking);
                    MyServiceBox.DisplayMemberPath = "GetNameBooking";
                }
            }
            if (MyServiceBox.Items.Count == 0) MyServiceBox.Items.Add("У вас нету услуг");
            MyServiceBox.SelectedIndex = 0;
        }

        //Нажатие на кнопку Отправить
        //Сохранение отзыва в БД
        private async void SubmiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validator.IsNotNullOrWhiteSpace(MyServiceBox.Items.ToString()))
            {
                ReviewApiService reviewApi = new();
                CustomerApiService customerApi = new();
                BookingApiService bookingApi = new();

                var customer = await customerApi.GetByUserId(currentUser.ID);
                if(customer != null)
                {
                    var booking = MyServiceBox.SelectedItem as Booking;
                    Review review = new()
                    {
                        BookingID = booking.ID,
                        CustomerID = customer.ID,
                        Rating = rate,
                        ReviewText = ReviewTextBox.Text,
                        ReviewDate = DateTime.Now
                    };

                    var reviewDTO = ConvertToDTO.ToReviewDTO(review);

                    await reviewApi.Create(reviewDTO);
                    Message.Success("Отзыв добавлен!");
                    CloseControl?.Invoke(this, e);
                }

            }
            else Message.Warning("Вы не выбрали услугу!");
        }

        private void CheckCheckedButton(RadioButton radioButton)
        {
            if (radioButton.IsChecked == true)
            {
                switch (radioButton.Content.ToString())
                {
                    case "1":
                        rate = 1;
                        break;
                    case "2":
                        rate = 2;
                        break;
                    case "3":
                        rate = 3;
                        break;
                    case "4":
                        rate = 4;
                        break;
                    default:
                        rate = 5; 
                        break;
                }
            }
        }

        private void Rate_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            CheckCheckedButton(radioButton);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            CloseControl?.Invoke(this, e);
        }

        private void ReviewTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Validator.IsLetter(e.Text[0]) || !Validator.IsDigit(e.Text[0]);
        }
    }
}
