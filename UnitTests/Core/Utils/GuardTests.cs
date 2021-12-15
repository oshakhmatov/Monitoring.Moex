using Monitoring.Moex.Core.Utils;
using System;
using Xunit;

namespace UnitTests.Core.Utils
{
    public class GuardTests
    {
        [Fact]
        public void NotNull_SimpleObject_NothingHappen()
        {
            Guard.NotNull("simple", "name");
        }

        [Fact]
        public void NotNull_Null_ThrowsArgNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Guard.NotNull(null, "name"));
        }

        [Fact]
        public void NotNull_Null_ThrowsArgNullExceptionAndCorrectParamName()
        {
            var paramName = "name";
            Assert.Throws<ArgumentNullException>(paramName, () => Guard.NotNull(null, paramName));
        }
    }
}
