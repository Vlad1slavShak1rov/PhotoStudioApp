using PhotoStudioApp.Helper;
using PhotoStudioApp.Model;
using PhotoStudioApp.Views;
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
    /// Логика взаимодействия для CreateTableWindow.xaml
    /// </summary>
    public partial class CreateTableWindow : Window
    {
        private List<Booking> bookings;
        public CreateTableWindow(List<Booking> bookings)
        {
            InitializeComponent();
            this.bookings = bookings;
        }

        private void btCreateTable_Click(object sender, RoutedEventArgs e)
        {
            List<Booking> newBookings = bookings;
            if (spSelectTime.IsEnabled == true)
            {
                if(dpStart.SelectedDate == null || dpEnd.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату!");
                    return;
                }

                DateTime dateTimeStart = dpStart.SelectedDate.Value;
                DateTime dateTimeEnd = dpEnd.SelectedDate.Value;
                newBookings.Clear();

                foreach (var booking in bookings)
                {
                    if(dateTimeStart.Date >= booking.DateBooking.Date && booking.DateBooking.Date <= dateTimeEnd.Date)
                        newBookings.Add(booking);
                    
                }
            }
            CreateFile.CreateExcelTable(newBookings);
            MessageBox.Show("Успешно!");
            this.Close();
        }

        private void rbInAllTime_Checked(object sender, RoutedEventArgs e)
        {
            if (spSelectTime == null) return;
            spSelectTime.IsEnabled = false;
        }

        private void rbDuringTime_Checked(object sender, RoutedEventArgs e)
        {
            if (spSelectTime == null) return;
            spSelectTime.IsEnabled = true;
        }
    }
}
