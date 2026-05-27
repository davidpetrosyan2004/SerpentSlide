using UnityEngine;

[CreateAssetMenu(fileName = "SnakeData", menuName = "ScriptableObjects/SnakeData", order = 1)]
public class SnakeData : ScriptableObject
{
    public enum CellType
    {
        E,
        H,
        B,
        T
    }

    public enum ColorType
    {
        None,
        Red,
        Blue,
        Green,
        Yellow,
        Orange,
    }
    [System.Serializable]
    public class Cell
    {
        public CellType type;
        public int indexBody;
        public ColorType color;
    }

    [System.Serializable]
    public class Row
    {
        public Cell[] column;

        public Row(int size)
        {
            column = new Cell[size];

            for (int i = 0; i < size; i++)
            {
                column[i] = new Cell
                {
                    type = CellType.E,
                    indexBody = -1,
                    color = ColorType.None,
                };
            }
        }

        public void ClearRow()
        {
            for (int i = 0; i < column.Length; i++)
            {
                column[i].type = CellType.E;
                column[i].indexBody = -1;
                column[i].color = ColorType.None;
            }
        }
    }

    public int columns;
    public int rows;
    public Row[] board;

    public void Clear()
    {
        if (board == null) return;

        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != null)
            {
                board[i].ClearRow();
            }
        }
    }

    public void CreateNewBoard()
    {
        if (columns <= 0 || rows <= 0) return;

        board = new Row[rows];

        for (int i = 0; i < rows; i++)
        {
            board[i] = new Row(columns);
        }
        Clear();
    }
}
