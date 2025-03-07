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
    /// Логика взаимодействия для ServiceCardControl.xaml
    /// </summary>
    public partial class ServiceCardControl : UserControl
    {
        private Grid _mainGrid;
        private EditServiceView editServiceView;
        public event EventHandler Update;
        public ServiceCardControl(Services services, bool isAdmin, Grid grid)
        {
            InitializeComponent();
            _mainGrid = grid;
            InitData(services, isAdmin);
        }

        public ServiceCardControl(AdditionalService additionalService, bool isAdmin, Grid grid)
        {
            InitializeComponent();
            InitData(additionalService, isAdmin);
        }

        private void InitData(Services services, bool isAdmin)
        {
            ServiceCardExpander.Header = services.ServiceName;
            DescriptionLabel.Text = services.Description;
            CostLabel.Content = "Стоимость: " + services.CostService;
            TypeOfServiceLabel.Content = "Тип услуги: Основная";

            //Проверка, администратор ли сейчас пользуется или нет
            AdminButtons.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }
        private void InitData(AdditionalService services, bool isAdmin)
        {
            ServiceCardExpander.Header = services.ServiceName;
            DescriptionLabel.Text = services.Description;
            CostLabel.Content = "Стоимость: " + services.Cost;
            TypeOfServiceLabel.Content = "Тип услуги: Дополнительная";

            //Проверка, администратор ли сейчас пользуется или нет
            AdminButtons.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            using var context = new MyDBContext();
            RepositoryAdditionalService repositoryAdditionalService = new(context);
            RepositoryServices repositoryServices = new(context);

            var service = repositoryServices.GetByName(ServiceCardExpander.Header.ToString());
            if(service != null)
            {
                EditService(service);
            }
            else
            {
                var addService = repositoryAdditionalService.GetByName(ServiceCardExpander.Header.ToString());
                EditService(addService);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditService(Services services)
        {
            editServiceView = new(services);

            editServiceView.HorizontalAlignment = HorizontalAlignment.Center;
            editServiceView.VerticalAlignment = VerticalAlignment.Top;
            editServiceView.Margin = new Thickness(0,10,0, 0);

            editServiceView.Close += EditServiceView_Close;
            _mainGrid.Children.Add(editServiceView);
        }

        private void EditService(AdditionalService services)
        {
            editServiceView = new(services);

            editServiceView.HorizontalAlignment = HorizontalAlignment.Center;
            editServiceView.VerticalAlignment = VerticalAlignment.Top;
            editServiceView.Margin = new Thickness(0, 10, 0, 0);

            editServiceView.Close += EditServiceView_Close;
            _mainGrid.Children.Add(editServiceView);
        }

        private void EditServiceView_Close(object? sender, EventArgs e)
        {
            _mainGrid.Children.Remove(editServiceView);
            editServiceView.Close -= EditServiceView_Close;
            editServiceView = null;

            Update?.Invoke(this,e);
        }
    }
}
