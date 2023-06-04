namespace Battleships.Utils
{
    public static class Alphabet
    {
        public static char[] CharArray => Enumerable.Range('a', 26).Select(Convert.ToChar).ToArray();
    }
}
