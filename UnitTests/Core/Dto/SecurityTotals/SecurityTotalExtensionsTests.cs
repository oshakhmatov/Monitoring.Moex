using Monitoring.Moex.Core.Dto.SecurityTotals;
using Monitoring.Moex.Core.Models;
using Xunit;

namespace UnitTests.Core.Dto.SecurityTotals
{
    public class SecurityTotalExtensionsTests
    {
        [Fact]
        public void AsShortDto_CorrectMap()
        {
            var model = new SecurityTotal()
            {
                Open = 1,
                Close = 2,
                OpenCloseDelta = 3,
                Security = new Security()
                {
                    ShortName = "name",
                },
            };

            var dto = model.AsShortDto();

            Assert.Equal(dto.SecurityName, model.Security.ShortName);
            Assert.Equal(dto.Open, model.Open);
            Assert.Equal(dto.Close, model.Close);
            Assert.Equal(dto.DeltaPercentage, model.OpenCloseDelta);
        }
    }
}
