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
    /// Логика взаимодействия для HistoryBonusView.xaml
    /// </summary>
    public partial class HistoryBonusView : UserControl
    {
        public event EventHandler BackClick;

        public HistoryBonusView(Customer customer)
        {
            InitializeComponent();
            InitData(customer);
        }

        private void InitData(Customer customer)
        {
            using var context = new MyDBContext();
            var operationHistory = context.HistoryPoints.Where(h=>h.CustomerID == customer.ID).ToList();
            lvHistory.ItemsSource = operationHistory;
            lvHistory.DisplayMemberPath = "ShowInformation";
        }
        
        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            BackClick?.Invoke(this, e);
        }
    }
}
