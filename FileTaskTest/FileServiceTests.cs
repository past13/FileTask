using Moq;
using FileTask.Services;

namespace FileTaskTest;

public class FileServiceTests
{
    [Fact]
    public async Task ValidateAccountsAsync_ValidFile_ReturnsEmptyInvalidLines()
    {
        // Arrange
        var stream = GenerateStreamFromString("John 4113901p\nJane 4113902p\n");

        var mockMeasureTimeCallback = new Mock<Action<TimeSpan, int>>();

        var fileService = new FileService();

        // Act
        var invalidLines = await fileService.ValidateAccountsAsync(stream, mockMeasureTimeCallback.Object);

        // Assert
        Assert.Empty(invalidLines);
        mockMeasureTimeCallback.Verify(m => m(It.IsAny<TimeSpan>(), It.IsAny<int>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ValidateAccountsAsync_InvalidFormat_ReturnsThreeDifferentInvalidLines()
    {
        // Arrange
        var stream = GenerateStreamFromString("michael 3113902\nThomas 32999921\nXAEA-12 8293982\n");
        var mockMeasureTimeCallback = new Mock<Action<TimeSpan, int>>();

        var fileService = new FileService();

        // Act
        var invalidLines = await fileService.ValidateAccountsAsync(stream, mockMeasureTimeCallback.Object);

        // Assert
        Assert.Equal(3, invalidLines.Count);
        Assert.Contains("Account name - not valid for line 1 'michael 3113902'", invalidLines);
        Assert.Contains("Account number - not valid for line 2 'Thomas 32999921'", invalidLines);
        Assert.Contains("Account name, account number - not valid for line 3 'XAEA-12 8293982'", invalidLines);
        
        mockMeasureTimeCallback.Verify(m => m(It.IsAny<TimeSpan>(), It.IsAny<int>()), Times.Exactly(3));
    }

    private static Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}