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
    /// Логика взаимодействия для WorkerCards.xaml
    /// </summary>
    public partial class WorkerCards : UserControl
    {
        private Worker _currentWorker;
        private EditWorker _editWorker;

        //Событие для обновления списка рабочих
        public event EventHandler Update;
        //Grid для добавления EditWorker
        private Grid _mainGrid;
        public WorkerCards(Worker worker, Grid grid )
        {
            InitializeComponent();
            _currentWorker = worker;
            _mainGrid = grid;
            _= InitData();
        }

        private async Task InitData()
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

            BookingApiService bookingApiService = new();

            // Получаем список бронирований в зависимости от роли работника
            var bookingList = (Enums.Post)_currentWorker.Post == Enums.Post.Photograph ?
                await bookingApiService.GetAllByPhotograph(_currentWorker.ID) : await bookingApiService.GetAllByVisagiste(_currentWorker.ID);

            AllBookingCB.ItemsSource = bookingList;
            // Указываем путь к отображаемому свойству
            AllBookingCB.DisplayMemberPath = "GetNameBooking";
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _editWorker = new(_currentWorker);
            _editWorker.HorizontalAlignment = HorizontalAlignment.Center;
            _editWorker.VerticalAlignment = VerticalAlignment.Top;
            _editWorker.Margin = new Thickness(0,10,0,0);
            _editWorker.Close += _editWorker_Close;
            _mainGrid.Children.Add(_editWorker);

        }

        //удаляем окно и высвобождаем память
        private void _editWorker_Close(object? sender, EventArgs e)
        {
            _mainGrid.Children.Remove(_editWorker);
            _editWorker.Close -= _editWorker_Close;
            _editWorker = null;
            Update?.Invoke(this, e);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            BookingApiService bookingApiService = new();
            WorkerApiService workerApiService = new();
            UserApiService userApiService = new();

            //Получаем бронирование в зависисомсти от роли сотрудника
            var booking = _currentWorker.Post == Enums.Post.Photograph ? await bookingApiService.GetByPhotographId(_currentWorker.ID) : await bookingApiService.GetByVisagisteID(_currentWorker.ID);

            //Если работник не закреплен за бронью
            if (booking == null)
            {
                var result = Message.Question("Вы уверены, что хотите удалить сотрудника?");
                if(result == MessageBoxResult.Yes)
                {
                    await workerApiService.DeleteById(_currentWorker.ID);
                    await userApiService.DeleteById(_currentWorker.UserID);
                    Message.Success("Успешно!");
                    Update?.Invoke(this,e);
                }
            }
            else Message.Warning("Невозможно удалить сотрудника, т.к. он закреплен на бронью");

        }
    }
}
