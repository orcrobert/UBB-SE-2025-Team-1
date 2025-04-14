// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services.DummyServices
{
    /// <summary>
    /// Service for managing user-related operations.
    /// </summary>
    public class UserService : IUserService
    {
        private const int DefaultUserId = 1;

        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        public int CurrentUserId { get; } = DefaultUserId;

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <returns> User id. </returns>
        public int GetCurrentUserId()
        {
            return this.CurrentUserId;
        }
    }
}