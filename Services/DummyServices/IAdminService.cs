namespace WinUIApp.Services.DummyServices
{
    public interface IAdminService
    {
         abstract bool IsAdmin(int userId);
         abstract void SendNotificationFromUserToAdmin(int senderUserId, string userModificationRequestType, string userModificationRequestDetails);
    }
}