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
    /// Логика взаимодействия для SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public event EventHandler CloseClick;
        private Worker _currentWorker;


        public SettingsView(User user)
        {
            InitializeComponent();
            GetWorker(user);
            InitData();

        }

        private void GetWorker(User user)
        {
            using var context = new MyDBContext();
            RepositoryWorker repositoryWorker = new(context);
            var worker = repositoryWorker.GetByID(user.ID);
        }

        private void InitData()
        {
            SurnameTextBox.Text = _currentWorker.SecondName;
            NameTextBox.Text = _currentWorker.Name;
            LastNameBox.Text = _currentWorker.LastName;
            PostBox.Text = (Enums.Post)_currentWorker.Post == Enums.Post.Photograph ? "Фотограф" : "Визажист";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (SurnameTextBox.Text != _currentWorker.SecondName || NameTextBox.Text != _currentWorker.Name || LastNameBox.Text != _currentWorker.LastName
                && Validator.IsNotNullOrWhiteSpace(SurnameTextBox.Text, NameTextBox.Text, LastNameBox.Text))
            {
                var result = Message.Question("У вас измененные данные. Сохранить их?");
                if(result == MessageBoxResult.Yes) SaveData();
                CloseClick?.Invoke(this,e);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validator.IsNotNullOrWhiteSpace(SurnameTextBox.Text, NameTextBox.Text, LastNameBox.Text))
            {
                SaveData();
                Message.Success("Успешно!");
                CloseClick?.Invoke(this, e);
            }
            else MessageBox.Show("У вас есть незаполненные поля!");
        }

        private void SaveData()
        {
            using var context = new MyDBContext();
            RepositoryWorker repositoryWorker = new(context);

            _currentWorker = new()
            {
                Name = NameTextBox.Text,
                SecondName = SurnameTextBox.Text,
                LastName = LastNameBox.Text,
            };

            repositoryWorker.Update(_currentWorker);
        }
    }
}
