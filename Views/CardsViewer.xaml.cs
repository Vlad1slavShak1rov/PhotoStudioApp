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
    /// Логика взаимодействия для CardsViewer.xaml
    /// </summary>
    public partial class CardsViewer : UserControl
    {
        public CardsViewer(Review review)
        {
            InitializeComponent();
            InitData(review);
        }

        private void InitData(Review review)
        {
            using var context = new MyDBContext();
            RepositoryCustomer repositoryCustomer = new(context);
            var customer = repositoryCustomer.GetByID(review.CustomerID);
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
