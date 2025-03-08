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

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //Проверка на то, являются ли TextBox непустыми 
            if (Validator.IsNotNullOrWhiteSpace(LoginBox.Text, PasswordBoxOne.Password, PasswordBoxTwo.Password, NameBox.Text, SecondNameBox.Text, LastNameBox.Text))
            {
                if (PasswordBoxOne.Password == PasswordBoxTwo.Password)
                {
                    if (PasswordBoxOne.Password.Count() >= 8)
                    {
                        using var context = new MyDBContext();
                        RepositoryUser repositoryUser = new(context);
                        RepositoryWorker repositoryWorker = new(context);

                        //Проверяем, существует ли такой пользователь в БД
                        var user = repositoryUser.GetByLogin(LoginBox.Text);
                        if (user == null)
                        {
                            user = new()
                            {
                                Login = LoginBox.Text,
                                Password = PasswordBoxOne.Password,
                                Role = Enums.Role.Worker
                            };
                            //Добавляем в БД
                            repositoryUser.Create(user);

                            Enum post;
                            //В зависимости от выбранного элемента в ComboBox присваиваем значение
                            post = PostComboBox.SelectedIndex == 0 ? Enums.Post.Photograph : Enums.Post.Visagiste;

                            Worker worker = new()
                            {
                                Name = NameBox.Text,
                                LastName = LastNameBox.Text,
                                SecondName = SecondNameBox.Text,
                                UserID = user.ID,
                                Post = (Enums.Post)post
                            };

                            repositoryWorker.Create(worker);
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
        }
    }
}
