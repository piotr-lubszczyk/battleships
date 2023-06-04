using System.Text.RegularExpressions;
using Battleships.Models;

namespace Battleships.Utils;

public class InputParser
{
    private const string INPUT_REGEX = "^[a-zA-Z][1-9][0-9]?$";
    private Action<string> _setErrorMessage;

    public InputParser(Action<string> setErrorMessage)
    {
        _setErrorMessage = setErrorMessage;
    }

    public Coordinates? Parse(string? input)
    {
        if (input == null)
        {
            _setErrorMessage(ErrorMessages.NULL_INPUT);
            return null;
        }

        var trimmed = input.Trim().ToLower();

        var regex = new Regex(INPUT_REGEX);
        var match = regex.Match(trimmed);
        if (!match.Success)
        {
            _setErrorMessage(ErrorMessages.INVALID_INPUT);
            return null;
        }

        var inputNumber = string.Join(string.Empty, trimmed.Skip(1).Take(trimmed.Length - 1));
        var row = int.Parse(inputNumber) - 1;
        var column = Array.IndexOf(Alphabet.CharArray, trimmed.First());

        return new Coordinates(row, column);
    }
}
