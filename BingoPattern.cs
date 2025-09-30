public class BingoPattern
{
    public string Name { get; }
    public string[] Layout { get; }

    public BingoPattern(string name, string[] layout)
    {
        Name = name;
        Layout = layout;
    }

    public bool IsInPattern(int row, int col)
    {
        return Layout[row][col] == 'X';
    }

    public bool IsComplete(bool[,] clicked)
    {
        for (int row = 0; row < Layout.Length; row++)
        {
            for (int col = 0; col < Layout[row].Length; col++)
            {
                if (Layout[row][col] == 'X' && !clicked[row, col])
                {
                    return false;
                }
            }
        }
        return true;
    }
}