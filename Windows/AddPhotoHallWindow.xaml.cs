using System;
using Microsoft.Win32;
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
using System.Windows.Shapes;
using PhotoStudioApp.Model;
using PhotoStudioApp.Helper;
using PhotoStudioApp.Service;

namespace PhotoStudioApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddPhotoHallWindow.xaml
    /// </summary>
    public partial class AddPhotoHallWindow : Window
    {
        private HallPhoto hallPhoto;
        private int hallId;
        public AddPhotoHallWindow(int hallId, HallPhoto hallPhoto = null)
        {
            InitializeComponent();
            this.hallPhoto = hallPhoto;
            this.hallId = hallId;

            if(hallPhoto != null )
            {
                photo.Source = ImageConverter.ByteArrayToBitmapImage(hallPhoto.Image);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                photo.Source = bitmap;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = photo.Source as BitmapImage;
            var photoByte = ImageConverter.ImageToBytes(bitmapImage);
            
            HallPhotoService hallPhotoService = new HallPhotoService();
            var dtoPhotoHall = new HallPhotoDTO()
            {
                
                Image = photoByte,
                HallId = hallId,
            };

            if (hallPhoto == null)
                await hallPhotoService.Create(dtoPhotoHall);
            else
            {
                dtoPhotoHall.Id = hallPhoto.Id;
                await hallPhotoService.Update(dtoPhotoHall);
            }

            Message.Success("Успешно!");
            this.DialogResult = true;
            this.Close();
        }
    }
}
