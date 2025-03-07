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
    /// Логика взаимодействия для WorkerListView.xaml
    /// </summary>
    public partial class WorkerListView : UserControl
    {
        public WorkerListView()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            using var context = new MyDBContext();
            RepositoryWorker repositoryWorker = new(context);
            var workerList = repositoryWorker.GetAll();

            foreach(var worker in workerList)
            {
                WorkerCards workerCards = new(worker);
                MainPanel.Children.Add(workerCards);
            }
        }
    }
}
