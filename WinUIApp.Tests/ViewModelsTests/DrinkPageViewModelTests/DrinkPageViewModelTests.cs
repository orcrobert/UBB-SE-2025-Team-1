using Moq;
using System.Threading.Tasks;
using WinUIApp.Services;
using WinUIApp.Services.DummyServices;
using WinUIApp.ViewModels;
using Xunit;

namespace WinUIApp.Tests.ViewModels
{
    public class DrinkPageViewModelTests
    {
        private readonly Mock<IDrinkService> mockDrinkService;
        private readonly Mock<IUserService> mockUserService;

        public DrinkPageViewModelTests()
        {
            mockDrinkService = new Mock<IDrinkService>();
            mockUserService = new Mock<IUserService>();
        }

        [Fact]
        public async Task CheckIfInListAsync_DrinkInList_UpdatesStateAndText()
        {
            mockUserService.Setup(s => s.GetCurrentUserId()).Returns(1);
            mockDrinkService.Setup(s => s.IsDrinkInUserPersonalList(1, 100)).Returns(true);

            var viewModel = new DrinkPageViewModel(100, mockDrinkService.Object, mockUserService.Object)
            {
                UserId = 1
            };

            await viewModel.CheckIfInListAsync();

            Assert.Equal("\u2665", viewModel.ButtonText); // ♥
        }

        [Fact]
        public async Task CheckIfInListAsync_DrinkNotInList_UpdatesStateAndText()
        {
            mockUserService.Setup(s => s.GetCurrentUserId()).Returns(1);
            mockDrinkService.Setup(s => s.IsDrinkInUserPersonalList(1, 200)).Returns(false);

            var viewModel = new DrinkPageViewModel(200, mockDrinkService.Object, mockUserService.Object)
            {
                UserId = 1
            };

            await viewModel.CheckIfInListAsync();

            Assert.Equal("\U0001F5A4", viewModel.ButtonText); // 🖤
        }

        [Fact]
        public async Task AddRemoveFromListAsync_WhenNotInList_AddsDrink()
        {
            mockUserService.Setup(s => s.GetCurrentUserId()).Returns(2);
            mockDrinkService.Setup(s => s.IsDrinkInUserPersonalList(2, 300)).Returns(false);
            mockDrinkService.Setup(s => s.AddToUserPersonalDrinkList(2, 300)).Returns(true);

            var viewModel = new DrinkPageViewModel(300, mockDrinkService.Object, mockUserService.Object)
            {
                UserId = 2
            };

            await viewModel.CheckIfInListAsync();
            await viewModel.AddRemoveFromListAsync();

            Assert.Equal("\u2665", viewModel.ButtonText); // ♥
        }

        [Fact]
        public async Task AddRemoveFromListAsync_WhenInList_RemovesDrink()
        {
            mockUserService.Setup(s => s.GetCurrentUserId()).Returns(3);
            mockDrinkService.Setup(s => s.IsDrinkInUserPersonalList(3, 400)).Returns(true);
            mockDrinkService.Setup(s => s.DeleteFromUserPersonalDrinkList(3, 400)).Returns(true);

            var viewModel = new DrinkPageViewModel(400, mockDrinkService.Object, mockUserService.Object)
            {
                UserId = 3
            };

            await viewModel.CheckIfInListAsync();
            await viewModel.AddRemoveFromListAsync();

            Assert.Equal("\U0001F5A4", viewModel.ButtonText); // 🖤
        }

        [Fact]
        public async Task AddRemoveFromListAsync_InvalidIds_NoActionTaken()
        {
            var viewModel = new DrinkPageViewModel(mockDrinkService.Object, mockUserService.Object)
            {
                UserId = 0,
                DrinkId = 0
            };

            await viewModel.AddRemoveFromListAsync();

            Assert.Equal("\U0001F5A4", viewModel.ButtonText); // 🖤
        }
        [Fact]
        public async Task CheckIfInListAsync_ThrowsException_NoCrashAndDefaultText()
        {
            mockUserService.Setup(s => s.GetCurrentUserId()).Returns(1);
            mockDrinkService.Setup(s => s.IsDrinkInUserPersonalList(1, 100))
                            .Throws(new Exception("Service error"));

            var viewModel = new DrinkPageViewModel(100, mockDrinkService.Object, mockUserService.Object)
            {
                UserId = 1
            };

            await viewModel.CheckIfInListAsync();

            Assert.Equal("\U0001F5A4", viewModel.ButtonText); // 🖤
        }

        [Fact]
        public async Task AddRemoveFromListAsync_ThrowsException_NoCrashAndDefaultText()
        {
            mockUserService.Setup(s => s.GetCurrentUserId()).Returns(2);
            mockDrinkService.Setup(s => s.IsDrinkInUserPersonalList(2, 300)).Returns(false);
            mockDrinkService.Setup(s => s.AddToUserPersonalDrinkList(2, 300))
                            .Throws(new Exception("Service error"));

            var viewModel = new DrinkPageViewModel(300, mockDrinkService.Object, mockUserService.Object)
            {
                UserId = 2
            };

            await viewModel.AddRemoveFromListAsync();

            Assert.Equal("\U0001F5A4", viewModel.ButtonText); // 🖤
        }
    }
}
