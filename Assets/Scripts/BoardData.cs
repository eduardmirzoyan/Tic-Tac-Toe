using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardData : ScriptableObject
{
    public Marker[,] grid;

    public Marker this[int i, int j]
    {
        get => grid[i, j];
        set => grid[i, j] = value;
    }

    public int Width => grid.GetLength(0);
    public int Height => grid.GetLength(1);

    public BoardData Copy()
    {
        BoardData copy = Instantiate(this);
        Marker[,] gridCopy = grid.Clone() as Marker[,];
        copy.grid = gridCopy;
        return copy;
    }
}
