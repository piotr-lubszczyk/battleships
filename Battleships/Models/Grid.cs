using Battleships.Utils;

namespace Battleships.Models
{
    public class Grid
    {
        public int Size { get; }
        public GridTile[,] Tiles;
        private Action<string> _setErrorMessage;

        public Grid(Action<string> setErrorMessage, int size = 10)
        {
            Size = size;
            Tiles = new GridTile[Size, Size];
            _setErrorMessage = setErrorMessage;

            for (var i = 0; i < Size; i++)
                for (var j = 0; j < Size; j++)
                    Tiles[i, j] = new GridTile();
        }

        public void MarkTile(Coordinates coordinates)
        {
            if (coordinates.Row >= Size)
            {
                _setErrorMessage(ErrorMessages.INVALID_ROW);
                return;
            }

            if (coordinates.Column >= Size)
            {
                _setErrorMessage(ErrorMessages.INVALID_COLUMN);
                return;
            }

            var tile = Tiles[coordinates.Row, coordinates.Column];
            if (tile.IsHit)
            {
                _setErrorMessage(ErrorMessages.TILE_ALREADY_HIT);
                return;
            }

            tile.IsHit = true;
        }


        public Ship PlaceShip(int size)
        {
            var rand = new Random();

            while (true)
            {
                var isHorizontal = rand.NextDouble() >= 0.5;

                var startLimit = Size - size + 1;
                var start = new Coordinates(rand.Next(0, !isHorizontal ? startLimit : Size),
                    rand.Next(0, isHorizontal ? startLimit : Size));

                var safeAreaCoordinates = GetSafeAreaCoordinates(start, size, isHorizontal);
                var safeAreaTiles = safeAreaCoordinates.Select(c => Tiles[c.Row, c.Column]).ToList();

                if (safeAreaTiles.Any(t => t.HasShip))
                    continue;

                var shipTiles = isHorizontal ?
                    Enumerable.Range(start.Column, size).Select(t => Tiles[start.Row, t]).ToList() :
                    Enumerable.Range(start.Row, size).Select(t => Tiles[t, start.Column]).ToList();

                foreach (var tile in shipTiles)
                    tile.HasShip = true;

                return new Ship(shipTiles);
            }
        }

        public IList<Coordinates> GetSafeAreaCoordinates(Coordinates start, int size,
            bool isHorizontal)
        {
            var safeArea = new List<Coordinates>();
            var safeLength = Enumerable.Range((isHorizontal ? start.Column : start.Row) - 1, size + 2).ToList();

            var shipAxis = isHorizontal ? start.Row : start.Column;
            for (var i = shipAxis - 1; i <= shipAxis + 1; i++)
                safeArea.AddRange(isHorizontal ?
                    safeLength.Select(c => new Coordinates(i, c)) :
                    safeLength.Select(c => new Coordinates(c, i)));

            var trimmedSafeArea = safeArea
                .Where(c => c.Row >= 0 && c.Row < Size && c.Column >= 0 && c.Column < Size);

            return trimmedSafeArea.ToList();
        }
    }
}
