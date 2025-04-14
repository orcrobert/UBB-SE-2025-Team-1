// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Services.DummyServices
{
    /// <summary>
    /// Interface for managing user-related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        int CurrentUserId { get; }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <returns> User id. </returns>
        int GetCurrentUserId();
    }
}