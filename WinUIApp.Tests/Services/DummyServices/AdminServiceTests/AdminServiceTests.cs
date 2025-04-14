using WinUIApp.Services.DummyServices;
using Xunit;

namespace WinUIApp.Tests.Services.DummyServices.AdminServiceTests
{
    public class AdminServiceTests
    {
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            _adminService = new AdminService();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(30)]
        [InlineData(42)]
        public void IsAdmin_KnownAdminIds_ReturnsTrue(int adminId)
        {
            // Act
            var result = _adminService.IsAdmin(adminId);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(99)]
        [InlineData(-1)]
        [InlineData(1234)]
        public void IsAdmin_UnknownIds_ReturnsFalse(int userId)
        {
            // Act
            var result = _adminService.IsAdmin(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SendNotificationFromUserToAdmin_ShouldNotThrow()
        {
            // Act & Assert
            var exception = Record.Exception(() =>
                _adminService.SendNotificationFromUserToAdmin(1, "Update", "Please update my name.")
            );

            Assert.Null(exception);
        }
    }
}
