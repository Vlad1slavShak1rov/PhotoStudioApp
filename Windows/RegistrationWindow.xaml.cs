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
using System.Windows.Shapes;
using Validator = PhotoStudioApp.Helper.Validator;

namespace PhotoStudioApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new();
            loginWindow.Show();
            this.Close();
        }

        private void SingUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validator.IsNotNullOrWhiteSpace(NameBox.Text, SecondName.Text, LastName.Text, ContactBox.Text, LoginBox.Text, PasswordBoxOne.Password, PasswordBoxTwo.Password))
            {
                if(PasswordBoxOne.Password == PasswordBoxTwo.Password)
                {
                    if (PasswordBoxOne.Password.Length < 8)
                    {
                        Message.Warning("Пароль должен состоять хотя бы из 8 символов");
                        return;
                    }

                    using var context = new MyDBContext();
                    RepositoryUser repositoryUser = new(context);
                    RepositoryCustomer repositoryCustomer = new(context);

                    var user = repositoryUser.GetByLogin(LoginBox.Text);
                    if(user == null)
                    {
                        user = new()
                        {
                            Login = LoginBox.Text,
                            Password = PasswordBoxOne.Password,
                            Role = Enums.Role.Customer
                        };
                        repositoryUser.Create(user);

                        Customer customer = new()
                        {
                            Name = NameBox.Text,
                            UserID = user.ID,
                            SecondName = SecondName.Text,
                            LastName = LastName.Text,
                            Contact = ContactBox.Text
                        };
                        repositoryCustomer.Create(customer);
                        MainWindow mainWindow = new(user);
                        mainWindow.Show();
                        this.Close();
                        Message.Success("Успешно!");
                    }
                    else Message.Warning("Пользователь с таким логином уже зарегистрировался!");
                }
                else Message.Warning("У вас введены разные пароли!");
            }
            else Message.Warning("У вас есть незаполненные поля!");
        }
    }
}
