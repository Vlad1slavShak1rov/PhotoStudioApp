using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Enums;
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
    /// Логика взаимодействия для EditServiceView.xaml
    /// </summary>
    public partial class EditServiceView : UserControl
    {
        private Services _services;
        private AdditionalService _addServices;

        public event EventHandler Close;
        public EditServiceView(Services services)
        {
            InitializeComponent();
            _services = services;
            InitData(_services);

        }
        public EditServiceView(AdditionalService additionalService)
        {
            InitializeComponent();
            _addServices = additionalService;
            InitData(_addServices);

        }
        private void InitData(Services services)
        {
            ServiceNameBox.Text = services.ServiceName;
            DescriptionBox.Text = services.Description;
            CostBox.Text = services.CostService.ToString();
        }
        private void InitData(AdditionalService additionalService)
        {
            ServiceNameBox.Text = additionalService.ServiceName;
            DescriptionBox.Text = additionalService.Description;
            CostBox.Text = additionalService.Cost.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close?.Invoke(this,e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Validator.IsNotNullOrWhiteSpace(ServiceNameBox.Text, DescriptionBox.Text, CostBox.Text))
            {
                if (_services == null)
                {
                    if (!decimal.TryParse(CostBox.Text,out decimal cost))
                    {
                        Message.Warning("Вы ввели некорректную цену!");
                        return;
                    }
                    _addServices.ServiceName = ServiceNameBox.Text;
                    _addServices.Description = DescriptionBox.Text;
                    _addServices.Cost = cost;

                    using var context = new MyDBContext();
                    RepositoryAdditionalService repositoryAdditionalService = new(context);
                    repositoryAdditionalService.Update(_addServices);
                }
                else
                {
                    if (!int.TryParse(CostBox.Text, out int cost))
                    {
                        Message.Warning("Вы ввели некорректную цену!");
                        return;
                    }
                    _services.ServiceName = ServiceNameBox.Text;
                    _services.Description = DescriptionBox.Text;
                    _services.CostService = cost;

                    using var context = new MyDBContext();
                    RepositoryServices repositoryServices = new(context);
                    repositoryServices.Update(_services);
                }
                Message.Success("Успешно!");
                Close?.Invoke(this,e);
            }
            else Message.Warning("У вас есть незаполненные поля!");
        }
    }
}
