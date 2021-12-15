using Monitoring.Moex.Core.Models;
using Xunit;

namespace UnitTests.Core.Models
{
    public class SecurityTotalTests
    {
        [Fact]
        public void IsNotEmpty_HasAllRequiredFields_ReturnsTrue()
        {
            var cut = new SecurityTotal()
            {
                SecurityId = "id",
                Close = 1,
                Open = 2,
                High = 3,
                Low = 4
            };

            var result = cut.IsNotEmpty();

            Assert.True(result);
        }

        [Fact]
        public void IsNotEmpty_SecurityIdIsEmpty_ReturnsFalse()
        {
            var cut = new SecurityTotal()
            {
                SecurityId = "",
                Close = 1,
                Open = 2,
                High = 3,
                Low = 4
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }

        [Fact]
        public void IsNotEmpty_SecurityIdIsNull_ReturnsFalse()
        {
            var cut = new SecurityTotal()
            {
                Close = 1,
                Open = 2,
                High = 3,
                Low = 4
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }

        [Fact]
        public void IsNotEmpty_CloseIsNull_ReturnsFalse()
        {
            var cut = new SecurityTotal()
            {
                SecurityId = "id",
                Open = 2,
                High = 3,
                Low = 4
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }

        [Fact]
        public void IsNotEmpty_OpenIsNull_ReturnsFalse()
        {
            var cut = new SecurityTotal()
            {
                SecurityId = "id",
                Close = 1,
                High = 3,
                Low = 4
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }

        [Fact]
        public void IsNotEmpty_HighIsNull_ReturnsFalse()
        {
            var cut = new SecurityTotal()
            {
                SecurityId = "id",
                Close = 1,
                Open = 2,
                Low = 4
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }

        [Fact]
        public void IsNotEmpty_LowIsNull_ReturnsFalse()
        {
            var cut = new SecurityTotal()
            {
                SecurityId = "id",
                Close = 1,
                Open = 2,
                High = 3
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }
    }
}
