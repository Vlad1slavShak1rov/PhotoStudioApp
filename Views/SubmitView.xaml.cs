using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
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
            IntiData();
            currentUser = user;
        }

        //Логика реализации загрузки данных. 
        //В нее входит загрузка комбобокса, где хранятся бронирования данного пользователя
        private void IntiData()
        {
            MyServiceBox.Items.Clear();

            using var context = new MyDBContext();
            RepositoryBooking repositoryBooking = new(context);
            RepositoryReview repositoryReview = new(context);

            var reviewsList = repositoryReview.GetAll();
            var bookingList = repositoryBooking.GetAll();

            foreach(var booking in bookingList)
            {
                //Если время брони меньше, чем текущее и отзыва нету в таблице Отзывы,
                //То добавляем Item добавляем в MyServiceBox
                if (booking.DateBooking <= DateTime.Now && repositoryReview.GetByBookingID(booking.ID) == null)
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
        private void SubmiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validator.IsNotNullOrWhiteSpace(MyServiceBox.Items.ToString()))
            {
                using var context = new MyDBContext();
                RepositoryReview repositoryReview = new(context);
                RepositoryCustomer repositoryCustomer = new(context);
                RepositoryBooking repositoryBooking = new(context);

                var customer = repositoryCustomer.GetByUserID(currentUser.ID);
                if(customer != null)
                {
                    var booling = MyServiceBox.SelectedItem as Booking;
                    Review review = new()
                    {
                        BookingID = booling.ID,
                        CustomerID = customer.ID,
                        Rating = rate,
                        ReviewText = ReviewTextBox.Text,
                        ReviewDate = DateTime.Now
                    };

                    repositoryReview.Create(review);
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
    }
}
