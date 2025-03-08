using PhotoStudioApp.Database.DAL;
using PhotoStudioApp.Database.DBContext;
using PhotoStudioApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
    /// Логика взаимодействия для paymentCards.xaml
    /// </summary>
    public partial class paymentCards : UserControl
    {
        public paymentCards(Payment payment)
        {
            InitializeComponent();
            InitData(payment);
        }

        private void InitData(Payment payment)
        {
            datePaymentLabel.Content = $"Дата оплаты: {payment.PaymentDate}";
            string methodPay = payment.PaymentMethod == Enums.PaymentMethod.Cards ? "Наличными" : "Карта";
            methodPayment.Content = $"Метод оплаты: {methodPay}";
            costLabel.Content = $"Сумма {payment.Amount}";
        }
    }
}
