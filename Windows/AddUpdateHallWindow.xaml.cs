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
    /// Логика взаимодействия для AddUpdateHallWindow.xaml
    /// </summary>
    public partial class AddUpdateHallWindow : Window
    {
        private Hall Hall;
        public AddUpdateHallWindow(Hall hall = null)
        {
            InitializeComponent();
            Hall = hall;
            if(Hall != null)
            {
                InitTextBox();
                btRemove.Visibility = Visibility.Visible;
            }
        }
        private void InitTextBox()
        {
            tbName.Text = Hall.Description;
            tbCost.Text = ((int)Hall.Cost).ToString();
        }

        private void tbName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsLetter(e.Text[0]);
        }

        private void tbCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsNumber(e.Text[0]);
        }

        private async void btSave_Click(object sender, RoutedEventArgs e)
        {
            string desc = tbName.Text;
            if(!Validator.IsNotNullOrWhiteSpace(desc, tbCost.Text))
            {
                Message.Warning("У вас есть незаполненные поля!");
                return;
            }

            int curCost = int.Parse(tbCost.Text);

            HallApiService hallApiService = new();
            if(Hall == null)
            {
                HallDTO dtoHall = new() { Description = desc, Cost = curCost };
                await hallApiService.Create(dtoHall);
            }
            else
            {
                HallDTO dtoHall = new() {ID = Hall.ID,Cost = curCost, Description = desc };
                await hallApiService.Update(dtoHall);
            }

            Message.Success("Успешно!");
            this.DialogResult = true;
            this.Close();
        }

        private async void btRemove_Click(object sender, RoutedEventArgs e)
        {
            var res = Message.Question("Вы действительно хотите удалить данный зал?");
            if(res == MessageBoxResult.Yes)
            {
                HallApiService hallApiService = new();
                await hallApiService.DeleteById(Hall.ID);

                Message.Success("Успешно!");
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
