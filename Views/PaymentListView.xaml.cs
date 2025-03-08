using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
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
    /// Логика взаимодействия для PaymentListView.xaml
    /// </summary>
    public partial class PaymentListView : UserControl
    {
        public PaymentListView()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            using var context = new MyDBContext();
            RepositoryPayment repositoryPayment = new(context);
            var paymentList = repositoryPayment.GetAll();

            paymentList = paymentList.OrderBy(pay => pay.PaymentDate).ToList();
            foreach (var pay in paymentList)
            {
                paymentCards paymentCards = new(pay);
                MainPanel.Children.Add(paymentCards);
            }
        }
    }
}
