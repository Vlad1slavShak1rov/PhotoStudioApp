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
    /// Логика взаимодействия для EditWorker.xaml
    /// </summary>
    public partial class EditWorker : UserControl
    {
        private Worker _currentWorker;
        private User _currentUser;
        public event EventHandler Close;
        public EditWorker(Worker worker)
        {
            InitializeComponent();
            _currentWorker = worker;
            _ = InitData();
        }

        private async Task InitData()
        {
            UserApiService userApiService = new();
            //Получаем пользователя по ID
            _currentUser = await userApiService.GetById(_currentWorker.UserID);

            SecondNameBox.Text = _currentWorker.SecondName;
            NameBox.Text = _currentWorker.Name;
            LastNameBox.Text = _currentWorker.LastName;
            LoginBox.Text = _currentUser.Login;
            PasswordBox.Text = _currentUser.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //Проверяем не пустые ли TextBox
            if (Validator.IsNotNullOrWhiteSpace(SecondNameBox.Text, NameBox.Text, LastNameBox.Text, LoginBox.Text, PasswordBox.Text))
            {
                Update();
            }
            else Message.Warning("У вас есть незаполненные поля!");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            if (SecondNameBox.Text != _currentWorker.SecondName && 
            NameBox.Text != _currentWorker.Name &&
            LastNameBox.Text != _currentWorker.LastName &&
            LoginBox.Text != _currentUser.Login &&
            PasswordBox.Text != _currentUser.Password)
            {
                var result = Message.Question("У вас есть измененные данные. Сохранить?");
                if (result == MessageBoxResult.Yes) Update();
            }
            Close?.Invoke(this, e);
        }

        private async void Update()
        {
            UserApiService userApiService = new();
            WorkerApiService workerApiService = new();

            //Меняем свойства у существующих моделей
            _currentUser.Login = LoginBox.Text;
            _currentUser.Password = PasswordBox.Text;

            _currentWorker.SecondName = SecondNameBox.Text;
            _currentWorker.Name = NameBox.Text;
            _currentWorker.LastName = LastNameBox.Text;

            //Конвертируем в DTO
            var dtoUser = ConvertToDTO.ToUserDTO(_currentUser);
            var dtoWorker = ConvertToDTO.ToWorkerDTO(_currentWorker);

            //Обновляем
            await userApiService.Update(dtoUser);
            await workerApiService.Update(dtoWorker);
            Message.Success("Успешно!");

            //Вызываем событие
            Close?.Invoke(this, null);
        }

        private void SecondNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Validator.IsLetter(e.Text[0]);
        }
    }
}
