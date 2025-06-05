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
    /// Логика взаимодействия для CardsViewer.xaml
    /// </summary>
    public partial class CardsViewer : UserControl
    {
        public CardsViewer(Review review)
        {
            InitializeComponent();
            _ = InitData(review);
        }

        private async Task InitData(Review review)
        {
            CustomerApiService customerApiService = new();
            
            var customer = await customerApiService.GetById(review.CustomerID);
            if(review != null)
            {
                NameLabel.Content = customer.Name + ' ' + customer.SecondName;
                ViewsBlock.Text = review.ReviewText;
                RateLabel.Content = $"Оценка: {review.Rating}";
                DatePublicateLabel.Content = review.ReviewDate.ToString("dd.MM.yyyy");
            }
        }
    }
}
