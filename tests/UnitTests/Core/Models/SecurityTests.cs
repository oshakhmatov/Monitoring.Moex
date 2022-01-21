using Monitoring.Moex.Core.Models;
using Xunit;

namespace UnitTests.Core.Models
{
    public class SecurityTests
    {
        [Fact]
        public void IsNotEmpty_AllFieldsAreFilled_ReturnsTrue()
        {
            var cut = new Security()
            {
                SecurityId = "id",
                ShortName = "name"
            };

            var result = cut.IsNotEmpty();

            Assert.True(result);
        }

        [Fact]
        public void IsNotEmpty_SecurityIdIsNull_ReturnsFalse()
        {
            var cut = new Security()
            {
                ShortName = "name"
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }

        [Fact]
        public void IsNotEmpty_ShortNameIsNull_ReturnsFalse()
        {
            var cut = new Security()
            {
                SecurityId = "id"
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }

        [Fact]
        public void IsNotEmpty_SecurityIdIsEmpty_ReturnsFalse()
        {
            var cut = new Security()
            {
                SecurityId = "",
                ShortName = "name"
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }

        [Fact]
        public void IsNotEmpty_ShortNameIsEmpty_ReturnsFalse()
        {
            var cut = new Security()
            {
                SecurityId = "id",
                ShortName = ""
            };

            var result = cut.IsNotEmpty();

            Assert.False(result);
        }
    }
}
