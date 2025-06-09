using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using PhotoStudioApp.Service;
using PhotoStudioApp.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// Логика взаимодействия для MyWorksView.xaml
    /// </summary>
    public partial class MyWorksCardView : UserControl
    {
        private ImagePhotograph CurrentImageData { get; set; }
        private Worker currentWorker;
        public event EventHandler Update;
        public MyWorksCardView(ImagePhotograph imagePhotograph, bool isCustomer = false)
        {
            
            InitializeComponent();
            CurrentImageData = imagePhotograph;
            InitWorker();
            SetData(CurrentImageData.MyWorks, CurrentImageData.Description);

            if(!isCustomer)
            {
                this.MouseLeftButtonDown += MyWorksCardView_MouseLeftButtonDown;
            }
        }
        private async void InitWorker()
        {
            WorkerApiService workerApiService = new();
            currentWorker = await workerApiService.GetById(CurrentImageData.WorkerId);
        }
        private void MyWorksCardView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dto = new ImagePhotographDTo()
            {
                Id = CurrentImageData.Id,
                Description = CurrentImageData.Description,
                WorkerId = CurrentImageData.WorkerId,
                MyWorks = CurrentImageData.MyWorks,
            };
            AddNewPhoto addNewPhoto = new(currentWorker, true, dto);
            var resutt = addNewPhoto.ShowDialog();
            if (resutt == true)
            {
                Update?.Invoke(this,e);
            }
        }

        public void SetData(byte[] imageBytes, string description)
        {
            PhotoImage.Source = Helper.ImageConverter.ByteArrayToBitmapImage(imageBytes);
            DescriptionText.Text = string.IsNullOrWhiteSpace(description) ? "Без описания" : description;
        }
    }
}
