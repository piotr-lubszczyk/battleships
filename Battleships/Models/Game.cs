using Battleships.Utils;
using Battleships.Views;

namespace Battleships.Models;

public class Game
{
    public bool IsFinished => _ships.All(s => s.IsDestroyed);

    private readonly Grid _grid;
    private readonly int[] _shipSizes = { 4, 4, 5 };
    private readonly IList<Ship> _ships = new List<Ship>();
    private readonly BoardView _boardView;
    private string? _errorMessage;
    private readonly Action<string>? _setErrorMessage;
    private readonly InputParser _inputParser;

    public Game()
    {
        _setErrorMessage = (string message) => _errorMessage = message;

        _grid = new Grid(_setErrorMessage);
        _boardView = new BoardView(_grid, _ships);
        _inputParser = new InputParser(_setErrorMessage);

        foreach (var size in _shipSizes)
        {
            var ship = _grid.PlaceShip(size);
            _ships.Add(ship);
        }

        _boardView.Draw();
    }


    public void Play()
    {
        _errorMessage = null;

        var input = Console.ReadLine();
        var parsedInput = _inputParser.Parse(input);

        if (parsedInput != null)
            _grid.MarkTile(parsedInput);

        _boardView.Draw(_errorMessage);
    }

    public bool AskForContinue()
    {
        _boardView.AskForContinue();

        while (true)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.N)
                return false;
            else if (key.Key == ConsoleKey.Y)
                return true;
        }
    }
}
