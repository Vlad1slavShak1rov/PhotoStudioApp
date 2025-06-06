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
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public event EventHandler CloseClick;
        private Worker _currentWorker;
        private User _currentUser;

        public SettingsView(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadData();
        }
        private async void LoadData()
        {
            await GetWorker();
            await InitData();
        }

        //Получаем сотрудника
        private async Task GetWorker()
        {
            WorkerApiService workerApiService = new();
            _currentWorker = await workerApiService.GetByUserId(_currentUser.ID);
        }

        //Инициализируем TextBox
        private async Task InitData()
        {
            SurnameTextBox.Text = _currentWorker.SecondName;
            NameTextBox.Text = _currentWorker.Name;
            LastNameBox.Text = _currentWorker.LastName;
            PostBox.Text = (Enums.Post)_currentWorker.Post == Enums.Post.Photograph ? "Фотограф" : "Визажист";
        }

        private async void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (SurnameTextBox.Text != _currentWorker.SecondName || NameTextBox.Text != _currentWorker.Name || LastNameBox.Text != _currentWorker.LastName
                && Validator.IsNotNullOrWhiteSpace(SurnameTextBox.Text, NameTextBox.Text, LastNameBox.Text))
            {
                var result = Message.Question("У вас измененные данные. Сохранить их?");
                if(result == MessageBoxResult.Yes) await SaveData();
            }
            CloseClick?.Invoke(this, e);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validator.IsNotNullOrWhiteSpace(SurnameTextBox.Text, NameTextBox.Text, LastNameBox.Text))
            {
                await SaveData();
                CloseClick?.Invoke(this, e);
            }
            else MessageBox.Show("У вас есть незаполненные поля!");
        }

        //Сохраняем изменения
        private async Task SaveData()
        {
            WorkerApiService workerApiService = new();

            _currentWorker.Name = NameTextBox.Text;
            _currentWorker.SecondName = SurnameTextBox.Text;
            _currentWorker.LastName = LastNameBox.Text;

            var workerDTO = ConvertToDTO.ToWorkerDTO(_currentWorker);

            await workerApiService.Update(workerDTO);
            Message.Success("Успешно!");
        }

        private void SurnameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Validator.IsLetter(e.Text[0]);
        }
    }
}
