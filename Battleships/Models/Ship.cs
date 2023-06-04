namespace Battleships.Models
{
    public class Ship
    {
        public IList<GridTile> Tiles { get; } = new List<GridTile>();
        public int Size => Tiles.Count;
        public bool IsDestroyed => Tiles.All(t => t.IsHit);

        public Ship(IList<GridTile> tiles) => Tiles = tiles;
    }
}
