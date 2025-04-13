namespace WinUIApp.Services.DummyServices
{
    public interface IUserService
    {
        int CurrentUserId { get; }

        int GetCurrentUserId();
    }
}