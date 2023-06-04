using Battleships.Models;
using Battleships.Utils;
using Moq;

namespace Tests;

public class InputParserTests
{
    private const int DEFAULT_SHIP_LENGTH = 4;
    private readonly Mock<Action<string>> _setErrorMessageMock = new Mock<Action<string>>();

    [Fact]
    public void Parse_OnNullInput_SetsNullInputErrorMessage()
    {
        var inputParser = new InputParser(_setErrorMessageMock.Object);

        inputParser.Parse(null);

        _setErrorMessageMock.Verify(a => a.Invoke(ErrorMessages.NULL_INPUT), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("asc")]
    [InlineData("1")]
    [InlineData("1,")]
    [InlineData("1,1")]
    [InlineData("a123456")]
    [InlineData("1a23456")]
    [InlineData("1a")]
    [InlineData("14z")]
    [InlineData("$$")]
    public void Parse_OnInvalidInput_SetsInvalidInputErrorMessage(string input)
    {
        var inputParser = new InputParser(_setErrorMessageMock.Object);

        inputParser.Parse(input);

        _setErrorMessageMock.Verify(a => a.Invoke(ErrorMessages.INVALID_INPUT), Times.Once);
    }

    [Theory]
    [MemberData(nameof(InputTestData))]
    public void Parse_OnCorrectInput_ReturnsCoordinates(string input, Coordinates expectedResult)
    {
        var inputParser = new InputParser(_setErrorMessageMock.Object);

        var coordinates = inputParser.Parse(input);

        Assert.Equal(expectedResult, coordinates);
    }

    public static IEnumerable<object[]> InputTestData()
    {
        yield return new object[] { "a1", new Coordinates(0, 0) };
        yield return new object[] { "a2", new Coordinates(1, 0) };
        yield return new object[] { "b1", new Coordinates(0, 1) };
        yield return new object[] { "b2", new Coordinates(1, 1) };
        yield return new object[] { "z10", new Coordinates(9, 25) };
        yield return new object[] { "g15", new Coordinates(14, 6) };
    }
}
