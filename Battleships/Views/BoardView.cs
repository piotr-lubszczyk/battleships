using System.Text;
using Battleships.Models;
using Battleships.Utils;

namespace Battleships.Views;

public class BoardView
{
    private readonly Grid _grid;
    private readonly IEnumerable<Ship> _ships;
    private readonly string _columnHeaders;


    public BoardView(Grid grid, IEnumerable<Ship> ships)
    {
        _grid = grid;
        _ships = ships;
        _columnHeaders =
                $"  |{string.Join('|', Alphabet.CharArray.Take(_grid.Size).Select(c => c.ToString().ToUpper()))}|";
    }

    public void Draw(string? errorMessage = null)
    {
        Console.Clear();

        Console.WriteLine("Ships");
        var groupedShips = _ships.GroupBy(s => s.Size).OrderBy(g => g.Key);
        Console.WriteLine(string.Join(", ", groupedShips.Select(g => $"{g.Count(s => !s.IsDestroyed)}x{g.Key}")));

        DrawGrid();

        if (!string.IsNullOrEmpty(errorMessage))
            Console.WriteLine(errorMessage);
    }

    public void AskForContinue()
    {
        Console.WriteLine("You won! Want to play again? Press y/n to select");
    }

    private void DrawGrid()
    {
        Console.WriteLine("Board");
        Console.WriteLine(_columnHeaders);

        for (var row = 0; row < _grid.Size; row++)
        {
            var builder = new StringBuilder();
            var rowNr = row + 1 >= 10 ? $"{row + 1}" : $" {row + 1}";

            builder.Append($"{rowNr}|");

            var rowTiles = Enumerable
                .Range(0, _grid.Size)
                .Select(c => GetSymbol(_grid.Tiles[row, c]))
                .ToList();

            builder.Append(string.Join('|', rowTiles));
            builder.Append('|');

            Console.WriteLine(builder.ToString());
        }
    }

    private char GetSymbol(GridTile tile) => tile.IsHit ? tile.HasShip ? 'x' : 'o' : ' ';
}
