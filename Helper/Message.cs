using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoStudioApp.Helper
{
    public static class Message
    {
        public static void Warning(string message) => MessageBox.Show(message, "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
        public static void Success(string message) => MessageBox.Show(message, "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
        public static void Info(string message) => MessageBox.Show(message, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        public static MessageBoxResult Question(string message) => MessageBox.Show(message, "Вопрос", MessageBoxButton.YesNo);
    }
}
