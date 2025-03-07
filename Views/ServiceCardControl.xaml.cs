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
        public ServiceCardControl(Services services)
        {
            InitializeComponent();
            InitData(services);
        }

        public ServiceCardControl(AdditionalService additionalService)
        {
            InitializeComponent();
            InitData(additionalService);
        }

        private void InitData(Services services)
        {
            ServiceCardExpander.Header = services.ServiceName;
            DescriptionLabel.Text = services.Description;
            CostLabel.Content = "Стоимость: " + services.CostService;
            TypeOfServiceLabel.Content = "Тип услуги: Основная";
        }
        private void InitData(AdditionalService services)
        {
            ServiceCardExpander.Header = services.ServiceName;
            DescriptionLabel.Text = services.Description;
            CostLabel.Content = "Стоимость: " + services.Cost;
            TypeOfServiceLabel.Content = "Тип услуги: Дополнительная";
        }
    }
}
