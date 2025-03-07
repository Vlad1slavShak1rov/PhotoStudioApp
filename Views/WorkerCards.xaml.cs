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
    /// Логика взаимодействия для WorkerCards.xaml
    /// </summary>
    public partial class WorkerCards : UserControl
    {
        private Worker _currentWorker;
        public WorkerCards(Worker worker)
        {
            InitializeComponent();
            _currentWorker = worker;
            InitData();
        }

        private void InitData()
        {
            string post = "";

            switch ((Enums.Post)_currentWorker.Post)
            {
                case Enums.Post.Visagiste:
                    post = "Визажист";
                    break;

                case Enums.Post.Photograph:
                    post = "Фотограф";
                    break;
            }
            WorkerExpander.Header = $"ФИО: {_currentWorker.SecondName} {_currentWorker.Name} {_currentWorker.LastName}     Должность: {post}";

            using var context = new MyDBContext();
            RepositoryBooking repositoryBooking = new(context);

            // Получаем список бронирований в зависимости от роли работника
            var bookingList = (Enums.Post)_currentWorker.Post == Enums.Post.Photograph ?
                repositoryBooking.GetAllByPhotograph(_currentWorker.ID) : repositoryBooking.GetAllByVisagiste(_currentWorker.ID);

            AllBookingCB.ItemsSource = bookingList;
            // Указываем путь к отображаемому свойству
            AllBookingCB.DisplayMemberPath = "GetNameBooking";
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
