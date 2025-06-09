using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
using PhotoStudioApp.Windows;
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
    /// Логика взаимодействия для MyWorkfView.xaml
    /// </summary>
    public partial class MyWorkfView : UserControl
    {
        private Worker currentWorker;
        public MyWorkfView(Worker worker)
        {
            InitializeComponent();
            currentWorker = worker;
            _ = InitData();
        }

        private async Task InitData()
        {
            myWorkPanel.Children.Clear();
            WorkerPhotoWorks workerPhotoWorks = new();
            var works = await workerPhotoWorks.GetByWorkerId(currentWorker.ID);
            foreach(var work in works)
            {
                MyWorksCardView myWorksCardView = new(work);
                myWorkPanel.Children.Add(myWorksCardView);
                myWorksCardView.Update += MyWorksCardView_Update;
            }
        }

        private async void MyWorksCardView_Update(object? sender, EventArgs e)
        {
            await InitData();
        }

        private async void btAddMyWork_Click(object sender, RoutedEventArgs e)
        {
            AddNewPhoto addNewPhoto = new(currentWorker);
            var resutt = addNewPhoto.ShowDialog();
            if(resutt == true)
            {
                await InitData();
            }
        }
    }
}
