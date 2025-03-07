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
        public ServiceCardControl(Services services, bool isAdmin)
        {
            InitializeComponent();
            InitData(services, isAdmin);
        }

        public ServiceCardControl(AdditionalService additionalService, bool isAdmin)
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
    }
}
