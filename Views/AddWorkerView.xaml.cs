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
    /// Логика взаимодействия для AddWorkerView.xaml
    /// </summary>
    public partial class AddWorkerView : UserControl
    {
        //Событие закрытие окна
        public event EventHandler Close;
        public AddWorkerView()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close?.Invoke(this,e);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            try
            {
                //Проверка на то, являются ли TextBox непустыми 
                if (Validator.IsNotNullOrWhiteSpace(LoginBox.Text, PasswordBoxOne.Password, PasswordBoxTwo.Password, NameBox.Text, SecondNameBox.Text, LastNameBox.Text))
                {
                    if (PasswordBoxOne.Password == PasswordBoxTwo.Password)
                    {
                        if (PasswordBoxOne.Password.Count() >= 8)
                        {
                            UserApiService userApiService = new();
                            WorkerApiService workerApiService = new();

                            //Проверяем, существует ли такой пользователь в БД
                            var user = await userApiService.GetByLogin(LoginBox.Text);
                            if (user == null)
                            {
                                UserDTO userDTO = new()
                                {
                                    Login = LoginBox.Text,
                                    Password = PasswordBoxOne.Password,
                                    Role = Enums.Role.Worker
                                };
                                //Добавляем в БД
                                int userId = await userApiService.Create(userDTO);

                                Enum post;
                                //В зависимости от выбранного элемента в ComboBox присваиваем значение
                                post = PostComboBox.SelectedIndex == 0 ? Enums.Post.Photograph : Enums.Post.Visagiste;

                                WorkerDTO workerdto = new()
                                {
                                    Name = NameBox.Text,
                                    LastName = LastNameBox.Text,
                                    SecondName = SecondNameBox.Text,
                                    UserID = userId,
                                    Post = (Enums.Post)post
                                };

                                await workerApiService.Create(workerdto);
                                Message.Success("Успешно!");
                                Close?.Invoke(this, e);
                            }
                            else Message.Warning("Пользователь с таким ником уже зарегистрирован!");
                        }
                        else Message.Warning("Пароль должен быть длиной хотя бы 8 символов");
                    }
                    else Message.Warning("У вас разные пароли!");
                }
                else Message.Warning("У вас есть незаполненные поля!");
            } catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
            finally
            {
                BackButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
            }
        }

        private void LoginBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Validator.IsLetter(e.Text[0]) || Validator.IsSymbol(e.Text[0]);
        }
    }
}
