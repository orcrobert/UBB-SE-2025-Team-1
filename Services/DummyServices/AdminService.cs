using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WinUIApp.Services.DummyServies
{
    class AdminService
    {
        private HashSet<int> adminUserIds = new HashSet<int> { 1, 42, 30, 10 };

        public bool IsAdmin(int userID)
        {
            return adminUserIds.Contains(userID);
        }

        public void SendNotification(int senderUserID, string title, string description)
        {
            Debug.WriteLine($"[Notification]\nFrom User: {senderUserID}\nTitle: {title}\nDescription: {description}");
        }
    }
}