using Battleships.Models;
using Battleships.Utils;
using Moq;

namespace Tests;

public class GridTests
{
    private const int DEFAULT_SHIP_LENGTH = 4;
    private readonly Mock<Action<string>> _setErrorMessageMock = new Mock<Action<string>>();

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void PlaceShip_OnPlaceShip_IncreasesShipTilesCount(int shipLength)
    {
        var grid = new Grid(_setErrorMessageMock.Object);
        var initialCount = grid.Tiles.Cast<GridTile>().Count(t => t.HasShip);

        grid.PlaceShip(shipLength);

        Assert.Equal(initialCount + shipLength, grid.Tiles.Cast<GridTile>().Count(t => t.HasShip));
    }

    [Fact]
    public void MarkTile_OnInvalidRow_SetsRowErrorMessage()
    {
        var grid = new Grid(_setErrorMessageMock.Object);
        var coordinates = new Coordinates(grid.Size * 2, 0);

        grid.MarkTile(coordinates);

        _setErrorMessageMock.Verify(a => a.Invoke(ErrorMessages.INVALID_ROW), Times.Once);
    }

    [Fact]
    public void MarkTile_OnInvalidColumn_SetsColumnErrorMessage()
    {
        var grid = new Grid(_setErrorMessageMock.Object);
        var coordinates = new Coordinates(0, grid.Size * 2);

        grid.MarkTile(coordinates);

        _setErrorMessageMock.Verify(a => a.Invoke(ErrorMessages.INVALID_COLUMN), Times.Once);
    }

    [Fact]
    public void MarkTile_OnAlreadyHitTile_SetsAlreadyHitMessage()
    {
        var grid = new Grid(_setErrorMessageMock.Object);
        var coordinates = new Coordinates(0, 0);
        grid.Tiles[coordinates.Row, coordinates.Column].IsHit = true;

        grid.MarkTile(coordinates);

        _setErrorMessageMock.Verify(a => a.Invoke(ErrorMessages.TILE_ALREADY_HIT), Times.Once);
    }

    [Fact]
    public void MarkTile_OnCorrectCoordinates_MarksTileAsHit()
    {
        var grid = new Grid(_setErrorMessageMock.Object);
        var coordinates = new Coordinates(0, 0);

        grid.MarkTile(coordinates);

        Assert.True(grid.Tiles[coordinates.Row, coordinates.Column].IsHit);
    }

    [Fact]
    public void GetSafeAreaCoordinates_OnHorizontalShip_ReturnsCorrectSafeArea()
    {
        var start = new Coordinates(0, 0);
        var shipCoordinates = new List<Coordinates> { start, new(0, 1), new(0, 2) };
        var expectedSafeCoordinates = new List<Coordinates>
        {
            new(0, 0), new(0, 1), new(0, 2), new(0,3),
            new(1, 0), new(1, 1), new(1, 2), new(1,3)
        };
        var grid = new Grid(_setErrorMessageMock.Object);

        var safeCoordinates = grid.GetSafeAreaCoordinates(start, shipCoordinates.Count, true);

        Assert.Equal(expectedSafeCoordinates, safeCoordinates);
    }
}
