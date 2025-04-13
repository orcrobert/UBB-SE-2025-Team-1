namespace WinUIApp.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var expected = 4;
            var a = 2;
            var b = 2;
            // Act
            var result = a + b;
            // Assert
            Assert.Equal(expected, result);
        }
    }
}