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
    /// Логика взаимодействия для ViewsList.xaml
    /// </summary>
    public partial class ViewsList : UserControl
    {
        private User currentUser;
        private SubmitView submitView;
        private List<Review> reviewList;
        public ViewsList(User user)
        {
            InitializeComponent();
            currentUser = user;
            InitData();
        }

        private void InitData()
        {
            if(reviewList != null) reviewList.Clear();
            MainPanel.Children.Clear();
            
            using var context = new MyDBContext();
            RepositoryReview repositoryReview = new(context);

            reviewList = repositoryReview.GetAll();
            LoadReviewCards(reviewList);
        }

        //Загружает UserControl с созданием отзыва
        private void MakeViewsButton_Click(object sender, RoutedEventArgs e)
        {
            MainScrollViewer.Visibility = Visibility.Collapsed;
            submitView = new(currentUser);
            submitView.Margin = new Thickness(0, 50, 0, 0);
            submitView.VerticalAlignment = VerticalAlignment.Top;
            submitView.HorizontalAlignment = HorizontalAlignment.Center;
            submitView.CloseControl += SubmitView_CloseControl;
            MainGrid.Children.Add(submitView);
        }

        //При закрытии UserControl с созданием отзыва
        //Отписываемся от события CloseControl
        //И освобождаем память submitView
        private void SubmitView_CloseControl(object? sender, EventArgs e)
        {
            InitData();
            FilteredComboBox.SelectedIndex = 0;

            MainScrollViewer.Visibility = Visibility.Visible;
            MainGrid.Children.Remove(submitView);
            submitView.CloseControl -= SubmitView_CloseControl;
            submitView = null;
        }

        private void LoadReviewCards(List<Review> reviewsList)
        {
            if(reviewList != null)
            {
                MainPanel.Children.Clear();
                foreach (var review in reviewList)
                {
                    CardsViewer cardsViewer = new(review);
                    cardsViewer.Margin = new Thickness(0, 10, 0, 0);
                    cardsViewer.Height = 50;
                    MainPanel.Children.Add(cardsViewer);
                }
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var selectedItem = comboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                switch (selectedItem.Content)
                {
                    case "Не сортировать":
                        // Не применяем сортировку
                        break;
                    case "Сначала новые":
                        SortReviewsByDateDescending();
                        break;
                    case "Сначала старые":
                        SortReviewsByDateAscending();
                        break;
                    case "С высокой оценкой":
                        SortReviewsByRatingDescending();
                        break;
                    case "С низкой оценкой":
                        SortReviewsByRatingAscending();
                        break;
                }

                // После сортировки перезагружаем карточки отзывов
                LoadReviewCards(reviewList);
            }
        }

        private void SortReviewsByDateDescending()
        {
            reviewList = reviewList.OrderByDescending(r => r.ReviewDate).ToList();
        }

        private void SortReviewsByDateAscending()
        {
            reviewList = reviewList.OrderBy(r => r.ReviewDate).ToList();
        }

        private void SortReviewsByRatingDescending()
        {
            reviewList = reviewList.OrderByDescending(r => r.Rating).ToList();
        }

        private void SortReviewsByRatingAscending()
        {
            reviewList = reviewList.OrderBy(r => r.Rating).ToList();
        }
    }
}
