using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.System;

namespace WinUIApp.Services.DummyServies
{
    public class AdminService
    {
        // Mock admin user IDs for development/testing purposes.
        private static readonly HashSet<int> adminUserIds = new HashSet<int> { 1, 42, 30, 10 };


        public bool IsAdmin(int userId)
        {
            return adminUserIds.Contains(userId);
        }

        public void SendNotificationFromUserToAdmin(int senderUserId, string userModificationRequestType, string userModificationRequestDetails)
        {
            Debug.WriteLine($"[Notification]\nFrom User: {senderUserId}\nTitle: {userModificationRequestType}\nDescription: {userModificationRequestDetails}");

        }
    }
}