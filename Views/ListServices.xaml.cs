using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
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
    /// Логика взаимодействия для ListServices.xaml
    /// </summary>
    public partial class ListServices : UserControl
    {
        private List<Services> servicesList = new();
        private List<AdditionalService> addServicesList = new();
        private bool isAdmin;
        public ListServices(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            _ = InitData();
           
        }

        private async Task InitData()
        {
            StackPanelServices.Children.Clear();
            if (servicesList != null && addServicesList != null)
            {
                servicesList.Clear();
                addServicesList.Clear();
            }

            ServiceApiService serviceApiService = new();
            AdditionalServiceApi additionalService = new();

            servicesList = await serviceApiService.GetAll();
            addServicesList = await additionalService.GetAll();

            await LoadSerivec();
            await LoadAddSerivec();
        }
        //Загрузка Основных услуг
        private async Task LoadSerivec()
        {
            foreach (var service in servicesList)
            {
                ServiceCardControl serviceCardControl = new(service, isAdmin, MainGrid);
                serviceCardControl.Margin = new Thickness(0, 5, 0, 0);
                serviceCardControl.Update += ServiceCardControl_Update;
                StackPanelServices.Children.Add(serviceCardControl);
            }
        }
        //Загрузка Дополнительных услуг
        private async Task LoadAddSerivec()
        {
            foreach (var addService in addServicesList)
            {
                ServiceCardControl serviceCardControl = new(addService, isAdmin, MainGrid);
                serviceCardControl.Margin = new Thickness(0, 5, 0, 0);
                serviceCardControl.Update += ServiceCardControl_Update;
                StackPanelServices.Children.Add(serviceCardControl);
            }
        }
        //Фильтрация
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                var selectedItem = comboBox.SelectedItem as ComboBoxItem; 
                if (selectedItem != null)
                {
                    TextBlock textBlock = selectedItem.Content as TextBlock;
                    if (textBlock != null)
                    {
                        string selectedString = textBlock.Text;
                        FiltersOn(selectedString);
                    }
                }
            }
        }
        private async void FiltersOn(string filter)
        {
            if(StackPanelServices != null)
            {
                switch (filter)
                {
                    case "Все":
                        StackPanelServices.Children.Clear();
                        await LoadSerivec();
                        await LoadAddSerivec();
                        break;
                    case "Основные услуги":
                        StackPanelServices.Children.Clear();
                        await LoadSerivec();
                        break;
                    case "Доп. услуги":
                        StackPanelServices.Children.Clear();
                        await LoadAddSerivec();
                        break;
                }
            }
        }

        //Поиск услуги по запросу поисковой строки
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilteretCombobox.SelectedIndex = 0;
            string searched = SearchTextBox.Text.ToLower();

            StackPanelServices.Children.Clear();
            foreach (var service in servicesList)
            {
                if (service.ServiceName.ToLower().Contains(searched))
                {
                    ServiceCardControl serviceCardControl = new(service, isAdmin, MainGrid);
                    serviceCardControl.Margin = new Thickness(0, 5, 0, 0);
                    serviceCardControl.Update += ServiceCardControl_Update;
                    StackPanelServices.Children.Add(serviceCardControl);
                }
              
            }
            foreach (var addService in addServicesList)
            {
                if (addService.ServiceName.ToLower().Contains(searched))
                {
                    ServiceCardControl serviceCardControl = new(addService, isAdmin, MainGrid);
                    serviceCardControl.Margin = new Thickness(0, 5, 0, 0);
                    serviceCardControl.Update += ServiceCardControl_Update;
                    StackPanelServices.Children.Add(serviceCardControl);
                }

            }
        }

        private async void ServiceCardControl_Update(object? sender, EventArgs e)
        {
            await InitData();
        }
    }
}
