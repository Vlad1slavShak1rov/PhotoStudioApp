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

        private async void SingUpButton_Click(object sender, RoutedEventArgs e)
        {
            SingUpButton.IsEnabled = false;
            SignUpButton.IsEnabled = false;
            try
            {
                if (Validator.IsNotNullOrWhiteSpace(NameBox.Text, SecondName.Text, LastName.Text, ContactBox.Text, LoginBox.Text, PasswordBoxOne.Password, PasswordBoxTwo.Password))
                {
                    if (PasswordBoxOne.Password == PasswordBoxTwo.Password)
                    {
                        if (PasswordBoxOne.Password.Length < 8)
                        {
                            Message.Warning("Пароль должен состоять хотя бы из 8 символов");
                            return;
                        }

                        UserApiService userApi = new();
                        CustomerApiService customerApi = new();

                        var user = await userApi.GetByLogin(LoginBox.Text);
                        if (user == null)
                        {
                            UserDTO userDTO = new()
                            {
                                Login = LoginBox.Text,
                                Password = PasswordBoxOne.Password,
                                Role = Enums.Role.Customer
                            };
                            var userId = await userApi.Create(userDTO);

                            CustomerDTO customerDTO = new()
                            {
                                Name = NameBox.Text,
                                UserID = userId,
                                SecondName = SecondName.Text,
                                LastName = LastName.Text,
                                Contact = ContactBox.Text
                            };

                            await customerApi.Create(customerDTO);

                            MainWindow mainWindow = new(userId);
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
            catch(Exception ex)
            {
                Message.Warning(ex.Message);
            }
            finally
            {
                SingUpButton.IsEnabled = true;
                SignUpButton.IsEnabled = true;
            }
        }

        private void StringCheck_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Validator.IsLetter(e.Text[0]) || Validator.IsSymbol(e.Text[0]);
        }
    }
}
