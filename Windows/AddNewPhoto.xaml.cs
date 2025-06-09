using Microsoft.Win32;
using OfficeOpenXml.Style.Table;
using PdfSharp.Drawing;
using PhotoStudioApp.Helper;
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
using System.Windows.Shapes;

namespace PhotoStudioApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddNewPhoto.xaml
    /// </summary>
    public partial class AddNewPhoto : Window
    {
        private string imagePath;
        private Worker Worker;
        private bool update;
        ImagePhotographDTo imageDTO { get; set; }
        public AddNewPhoto(Worker worker, bool isUpdate = false, ImagePhotographDTo image = null)
        {
            InitializeComponent();
            update = isUpdate;
            Worker = worker;
            btRemove.Visibility = Visibility.Collapsed;
            if (update)
            {
                btRemove.Visibility = Visibility.Visible;
                imageDTO = image;
                imagePath = "sd";
                tbDescription.Foreground = Brushes.Black;
                InitData();
            }
        }
        private void InitData()
        {
            tbDescription.Text = imageDTO.Description;
            photo.Source = ImageConverter.ByteArrayToBitmapImage(imageDTO.MyWorks);
        }

        private void photo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.png"; 

            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
                BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                photo.Source = bitmap;
            }
        }

        private void tbDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbDescription.Text == "Комментарий к работе (необязательно)")
            {
                tbDescription.Text = "";
                tbDescription.Foreground = Brushes.Black;
            }
        }

        private void tbDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                tbDescription.Text = "Комментарий к работе (необязательно)";
                tbDescription.Foreground = Brushes.LightGray;
            }
        }

        private async void btSave_Click(object sender, RoutedEventArgs e)
        {
            if(imagePath == null)
            {
                Message.Warning("Вы не выбрали фото!");
                return;
            }
            string desc = tbDescription.Text;
            if(desc == "Комментарий к работе (необязательно)")
            {
                desc = string.Empty;
            }

            BitmapImage bitmapImage = photo.Source as BitmapImage;

            var image = ImageConverter.ImageToBytes(bitmapImage);

            var newImage = new ImagePhotographDTo()
            {
                Description = desc,
                MyWorks = image,
                WorkerId = Worker.ID
            };

            WorkerPhotoWorks workerApi = new();
            try
            {
                if (update)
                {
                    await workerApi.Update(imageDTO);
                }
                else
                    await workerApi.Create(newImage);
                
                    
            } catch (Exception ex)
            {
                Message.Warning(ex.Message);
                return;
            }
            Message.Success("Успешно!");
            this.DialogResult = true;
        }

        private async void btRemove_Click(object sender, RoutedEventArgs e)
        {
            var result = Message.Question("Вы уверены, что хотите удалит?");
            if(result == MessageBoxResult.Yes)
            {
                WorkerPhotoWorks workerApi = new();
                await workerApi.Delete(imageDTO.Id);
                this.DialogResult = true;
            }
        }
    }
}
