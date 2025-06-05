using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using PhotoStudioApp.Windows;
using PhotoStudioApp.Service;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoStudioApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void SignUpButton_Click(object sender, RoutedEventArgs e)
    {
        RegistrationWindow registrationWindow = new();
        registrationWindow.Show();
        this.Close();
    }

    private async void SingInButton_Click(object sender, RoutedEventArgs e)
    {
        SingInButton.IsEnabled = false;
        SignUpButton.IsEnabled = false;
        LoginBox.IsEnabled = false;
        PasswordBox.IsEnabled = false;
        try
        {
            if (!string.IsNullOrEmpty(LoginBox.Text) && !string.IsNullOrEmpty(PasswordBox.Password))
            {
                string login = LoginBox.Text;
                string password = PasswordBox.Password;
                UserApiService userApiService = new();

                var user = await userApiService.GetByLogin(login);
                if (user != null)
                {
                    if (user.Password == password)
                    {
                        CustomerApiService customerApiService = new();
                        //Проверка, если роль пользователя равна роли рабочего
                        if ((Enums.Role)user.Role == Enums.Role.Worker)
                        {
                            WorkerApiService workerApiService = new();
                            var customer = await customerApiService.GetByUserId(user.ID);
                            if (customer != null)
                            {
                                Worker worker = await workerApiService.GetByUserId(user.ID);// проверяем зарегистрирован ли рабочий с таким ID

                                if (worker == null) //Если не зарегистрирован
                                {
                                    var workerDTO = new WorkerDTO()
                                    {
                                        Name = customer.Name,
                                        LastName = customer.LastName,
                                        SecondName = customer.SecondName,
                                        Post = Enums.Post.Photograph,
                                        UserID = user.ID
                                    };
                                    await workerApiService.Create(workerDTO);
                                    await customerApiService.DeleteById(customer.ID);
                                }
                            }
                        }
                        //Проверка, если роль пользователя равна роли Админа
                        else if ((Enums.Role)user.Role == Enums.Role.Admin)
                        {
                            var customer = await customerApiService.GetByUserId(user.ID);
                            if (customer != null) await customerApiService.DeleteById(customer.ID);
                        }

                        Message.Success("Успешно!");
                        MainWindow mainWindow = new(user);
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        Message.Warning("Неверный пароль");
                        return;
                    }
                }
                else
                {
                    Message.Warning("Такого пользователя не существует!");
                    return;
                }
            }
            else Message.Warning("У вас есть незаполненные поля!");
        }
        catch (Exception ex)
        {
            Message.Warning(ex.Message);
        }
        finally
        {
            SingInButton.IsEnabled = true;
            LoginBox.IsEnabled = true;
            PasswordBox.IsEnabled = true;
            SignUpButton.IsEnabled = false;
        }
    }
}