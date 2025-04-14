// <copyright file="AdminService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services.DummyServices
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Windows.System;

    /// <summary>
    /// Service for managing admin-related operations.
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly HashSet<int> adminUserIds = [1, 42, 30, 10];

        /// <summary>
        /// Checks if a user is an admin.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <returns> True, if admin, false otherwise. </returns>
        public bool IsAdmin(int userId)
        {
            return this.adminUserIds.Contains(userId);
        }

        /// <summary>
        /// Sends a notification from a user to an admin.
        /// </summary>
        /// <param name="senderUserId"> Sender user id. </param>
        /// <param name="userModificationRequestType"> Request type. </param>
        /// <param name="userModificationRequestDetails"> Request Details. </param>
        public void SendNotificationFromUserToAdmin(int senderUserId, string userModificationRequestType, string userModificationRequestDetails)
        {
            Debug.WriteLine($"[Notification]\nFrom User: {senderUserId}\nTitle: {userModificationRequestType}\nDescription: {userModificationRequestDetails}");
        }
    }
}