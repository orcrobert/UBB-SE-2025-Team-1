using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUIApp.Services.DummyServies
{
    class AdminService
    {
        private List<int> adminUserIds = new List<int> { 1, 42 };

        public bool IsAdmin(int userID)
        {
            return adminUserIds.Contains(userID);
        }

        public void SendNotification(int senderUserID, string title, string description)
        {
            Console.WriteLine($"[Notification]\nFrom User: {senderUserID}\nTitle: {title}\nDescription: {description}");
        }
    }
}
