namespace WinUIApp.Services.DummyServices
{
    public interface IAdminService
    {
        static abstract bool IsAdmin(int userId);
        static abstract void SendNotificationFromUserToAdmin(int senderUserId, string userModificationRequestType, string userModificationRequestDetails);
    }
}