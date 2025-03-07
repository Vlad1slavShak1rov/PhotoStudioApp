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
    /// Логика взаимодействия для WorkerListView.xaml
    /// </summary>
    public partial class WorkerListView : UserControl
    {
        private AddWorkerView addWorkerView;
        private List<Worker> workerList;
        public WorkerListView()
        {
            InitializeComponent();
            InitData();
        }

        private void InitData()
        {
            if(workerList != null) workerList.Clear();
            using var context = new MyDBContext();
            RepositoryWorker repositoryWorker = new(context);
            workerList = repositoryWorker.GetAll();
            LoadChildren(workerList);
        }

        private void AddWorker_Click(object sender, RoutedEventArgs e)
        {
            MainScrollViewer.Visibility = Visibility.Collapsed;
            addWorkerView = new();

            addWorkerView.HorizontalAlignment = HorizontalAlignment.Center;
            addWorkerView.VerticalAlignment = VerticalAlignment.Center;
            addWorkerView.Width = 420;
            addWorkerView.Height = 450;

            addWorkerView.Close += AddWorkerView_Close;
            MainGrid.Children.Add(addWorkerView);
        }

        private void AddWorkerView_Close(object? sender, EventArgs e)
        {
            addWorkerView.Close -= AddWorkerView_Close;
            MainGrid.Children.Remove(addWorkerView);
            InitData();
            MainScrollViewer.Visibility = Visibility.Visible;
        }

        private void LoadChildren(List<Worker> workerList)
        {
            MainPanel.Children.Clear();
            if (workerList != null)
            {
                foreach (var worker in workerList)
                {
                    WorkerCards workerCards = new(worker);
                    MainPanel.Children.Add(workerCards);
                }
            }
           
        }

        private void FilteredComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = FilteredComboBox.SelectedItem as ComboBoxItem;
            string filter = selectedItem.Content.ToString();
            List<Worker> filtredList;
            switch (filter)
            {
                case "Фотографы":
                    filtredList = workerList.Where(wk => wk.Post == Enums.Post.Photograph).ToList();
                    break;
                case "Визажисты":
                    filtredList = workerList.Where(wk => wk.Post == Enums.Post.Visagiste).ToList();
                    break;
                default:
                    filtredList = workerList;
                    break;
            }

            LoadChildren(filtredList);
        }
    }
}
