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
    /// Логика взаимодействия для HistoryBonusView.xaml
    /// </summary>
    public partial class HistoryBonusView : UserControl
    {
        public event EventHandler BackClick;

        public HistoryBonusView(Customer customer)
        {
            InitializeComponent();
            _ = InitData(customer);
        }

        private async Task InitData(Customer customer)
        {
            HistoryPointsReceivedApi pointsReceivedApi = new HistoryPointsReceivedApi();
            var operationHistory = await pointsReceivedApi.GetByCustomerId(customer.ID);
            lvHistory.ItemsSource = operationHistory;
            lvHistory.DisplayMemberPath = "ShowInformation";
        }
        
        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            BackClick?.Invoke(this, e);
        }
    }
}
