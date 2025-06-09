using PhotoStudioApp.Helper;
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
    /// Логика взаимодействия для HallsPhoto.xaml
    /// </summary>
    public partial class HallsPhoto : UserControl
    {
        private HallPhoto hallsPhoto;
        private Hall Hall;
        public event EventHandler Update;
        public HallsPhoto(Hall hall, HallPhoto hallsPhoto, bool isCustomer = false)
        {
            InitializeComponent();
            this.hallsPhoto = hallsPhoto;
            this.Hall = hall;

            if (!isCustomer) 
            {
                image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            }

            Initialize();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AddPhotoHallWindow addHallPhotoWindow = new(Hall.ID, hallsPhoto);
            var result = addHallPhotoWindow.ShowDialog();
            if (result == true)
            {
                Update?.Invoke(this, e);
            }
        }

        private void Initialize()
        {
            if (hallsPhoto != null) image.Source = ImageConverter.ByteArrayToBitmapImage(hallsPhoto.Image);
        }
    }
}
