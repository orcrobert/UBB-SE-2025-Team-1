// <copyright file="IAdminService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services.DummyServices
{
    /// <summary>
    /// Interface for managing admin-related operations.
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Checks if a user is an admin.
        /// </summary>
        /// <param name="userId"> User id. </param>
        /// <returns> True, if admin, false otherwise. </returns>
        abstract bool IsAdmin(int userId);

        /// <summary>
        /// Sends a notification from a user to an admin.
        /// </summary>
        /// <param name="senderUserId"> User id. </param>
        /// <param name="userModificationRequestType"> Request type. </param>
        /// <param name="userModificationRequestDetails"> Request details. </param>
        abstract void SendNotificationFromUserToAdmin(int senderUserId, string userModificationRequestType, string userModificationRequestDetails);
    }
}