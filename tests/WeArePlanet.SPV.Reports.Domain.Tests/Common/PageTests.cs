using WeArePlanet.SPV.Reports.Domain.Common;

namespace WeArePlanet.SPV.Reports.Domain.Tests.Common;

public class PageTests
{
    [Theory]
    [InlineData(20, 0, 0)]
    [InlineData(20, 1, 1)]
    [InlineData(20, 19, 1)]
    [InlineData(20, 20, 1)]
    [InlineData(20, 21, 2)]
    [InlineData(1, 20, 20)]
    [InlineData(20, 1000, 50)]
    public void TotalPages_MultipleScenarios_ReturnsExpected(int pageSize, int totalItems, int expectedTotalPages)
    {
        // Arrange
        var page = new Page<string>(Array.Empty<string>(), 1, pageSize, totalItems);

        // Act
        var result = page.TotalPages;

        // Assert
        result.Should().Be(expectedTotalPages);
    }
}
