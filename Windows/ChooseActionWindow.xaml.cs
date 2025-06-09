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
    /// Логика взаимодействия для ChooseActionWindow.xaml
    /// </summary>
    public partial class ChooseActionWindow : Window
    {
        public bool IsCreateNew { get; private set; }
        public ChooseActionWindow()
        {
            InitializeComponent();
            btCreate.Content = "Создать";  
            btUpdate.Content = "Обновить"; 
        }
        private void btCreate_Click(object sender, RoutedEventArgs e)
        {
            IsCreateNew = true;
            DialogResult = true;
            Close();
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {
            IsCreateNew = false;
            DialogResult = true;
            Close();
        }
    }
}
