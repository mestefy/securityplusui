using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityPlusUI.Model;
using SecurityPlusUI.View;

namespace SecurityPlusUI.Services
{
    public class NotificationService
    {
        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void DisplayMessage(string format, params object[] options)
        {
            Console.WriteLine(format, options);
        }

        public static void DisplayException(Exception exception)
        {
            Console.WriteLine("\t Error: {0}", exception.Message);
        }

        public static void DisplayWindows(IContext context)
        {
            var notificationWindow = new NotificationWindow(context);
            notificationWindow.ShowDialog();
        }
    }
}
